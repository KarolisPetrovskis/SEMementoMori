using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MementoMori.Server.Controllers;
using MementoMori.Server.Interfaces;
using MementoMori.Server.DTOS;
using MementoMori.Server.Models;
using MementoMori.Server;

namespace MementoMori.Tests.UnitTests.ControllerTests
{
    public class DecksControllerTests
    {
        private readonly Mock<IDeckHelper> _mockDeckHelper;
        private readonly Mock<IAuthService> _mockAuthService;

        private readonly Mock<ICardService> _mockCardService;
        private readonly DecksController _controller;

        public DecksControllerTests()
        {
            _mockDeckHelper = new Mock<IDeckHelper>();
            _mockAuthService = new Mock<IAuthService>();
            _mockCardService = new Mock<ICardService>();

            _controller = new DecksController(_mockDeckHelper.Object, _mockAuthService.Object, _mockCardService.Object);

            var httpContext = new DefaultHttpContext();
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
        }

        [Fact]
        public async Task ViewAsync_ReturnsNotFound_WhenDeckNotExists()
        {
            var deckId = Guid.NewGuid();
            _mockDeckHelper
                .Setup(d => d.Filter(It.Is<Guid[]>(ids => ids.Contains(deckId)), null, null))
                .ReturnsAsync(new List<Deck>());

            var result = await _controller.ViewAsync(deckId);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task ViewAsync_ReturnsBadRequest_WhenGuidEmpty() 
        {
            var deckId = Guid.Empty;
            
            var result = await _controller.ViewAsync(deckId);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task ViewAsync_ReturnsDeckDTO_WhenDeckExists()
        {
            var deckId = Guid.NewGuid();
            var creatorId = Guid.NewGuid();
            var deck = new Deck
            {
                Id = deckId,
                Title = "Test Deck",
                Creator = new User { Id = creatorId, Username = "TestUser", Password = "Password" },
                CardCount = 1,
                Modified = DateOnly.FromDateTime(DateTime.UtcNow),
                Rating = 4.5,
                isPublic = true,
                Tags = new List<TagTypes> { TagTypes.Music, TagTypes.Mathematics },
                Description = "Test Description",
                Cards = new List<Card> { new Card { Id = Guid.NewGuid(), Question = "Q1", Answer = "A1" } }
            };
            _mockDeckHelper
                .Setup(d => d.Filter(It.Is<Guid[]>(ids => ids.Contains(deckId)), null, null))
                .ReturnsAsync([deck]);
            _mockAuthService
                .Setup(a => a.GetRequesterId(It.IsAny<HttpContext>()))
                .Returns(creatorId);

            var result = await _controller.ViewAsync(deckId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var deckDTO = Assert.IsType<DeckDTO>(okResult.Value);
            Assert.Equal(deckId, deckDTO.Id);
            Assert.Equal("TestUser", deckDTO.CreatorName);
            Assert.Equal(1, deckDTO.CardCount);
            Assert.True(deckDTO.IsOwner);
        }

        [Fact]
        public async Task EditorViewAsync_ReturnsBadRequest_WhenGuidEmpty()
        {
            var deckId = Guid.Empty;

            var result = await _controller.EditorViewAsync(deckId);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task EditorView_ReturnsNotFound_WhenDeckNotExists()
        {
            var deckId = Guid.NewGuid();
            _mockDeckHelper
                .Setup(d => d.Filter(It.Is<Guid[]>(ids => ids.Contains(deckId)), null, null))
                .ReturnsAsync([]);

            var result = await _controller.EditorViewAsync(deckId);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task EditorViewAsync_ReturnsDeckEditorDTO_WhenDeckExists()
        {
            var deckId = Guid.NewGuid();
            var deck = new Deck
            {
                Id = deckId,
                Title = "Editable Deck",
                isPublic = true,
                Modified = DateOnly.MaxValue,
                CardCount = 2,
                Description = "Editable Description",
                Cards = new List<Card>
            {
                new Card { Id = Guid.NewGuid(), Question = "Q1", Answer = "A1" },
                new Card { Id = Guid.NewGuid(), Question = "Q2", Answer = "A2" }
            }
            };

            _mockDeckHelper
                .Setup(d => d.Filter(It.Is<Guid[]>(ids => ids.Contains(deckId)), null, null))
                .ReturnsAsync([deck]);

            var result = await _controller.EditorViewAsync(deckId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var editorDTO = Assert.IsType<DeckEditorDTO>(okResult.Value);

            Assert.Equal(deckId, editorDTO.Id);
            Assert.True(editorDTO.isPublic);
            Assert.Equal(2, editorDTO.CardCount);
            Assert.Equal("Editable Description", editorDTO.Description);
            Assert.Equal("Q1", editorDTO.Cards?.First().Question);
        }

        [Fact]
        public void GetDueCards_ValidData_ReturnsOkResult()
        {
            // Arrange
            var deckId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var cards = new List<Card>
            {
                new Card { Id = Guid.NewGuid(), Question = "Q1", Description = "D1", Answer = "A1" },
                new Card { Id = Guid.NewGuid(), Question = "Q2", Description = "D2", Answer = "A2" }
            };

            _mockAuthService.Setup(a => a.GetRequesterId(It.IsAny<HttpContext>())).Returns(userId);
            _mockCardService.Setup(c => c.GetCardsForReview(deckId, userId)).Returns(cards);

            // Act
            var result = _controller.GetDueCards(deckId) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var returnedCards = result.Value as List<CardDTO>;
            Assert.NotNull(returnedCards);
            Assert.Equal(2, returnedCards.Count);
        }

        [Fact]
        public void GetDueCards_NoDueCards_ReturnsNotFound()
        {
            // Arrange
            var deckId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            _mockAuthService.Setup(a => a.GetRequesterId(It.IsAny<HttpContext>())).Returns(userId);
            _mockCardService.Setup(c => c.GetCardsForReview(deckId, userId)).Returns(new List<Card>());

            // Act
            var result = _controller.GetDueCards(deckId) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }
        [Fact]
        public void AddCardsToCollection_ValidData_ReturnsOkResult(){

            var deckId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            _mockAuthService.Setup(a => a.GetRequesterId(It.IsAny<HttpContext>())).Returns(userId);
            var result = _controller.AddCardsToCollection(deckId) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Deck successfully added to user's collection.", ((dynamic)result.Value).message);

            _mockCardService.Verify(c => c.AddCardsToCollection(userId, deckId), Times.Once);

        }
        [Fact]
        public void AddCardsToCollection_InvalidData_ReturnsBadRequest()
        {
            // Arrange
            var deckId = Guid.Empty;
            var userId = Guid.Empty;

            _mockAuthService.Setup(a => a.GetRequesterId(It.IsAny<HttpContext>())).Returns(userId);

            // Act
            var result = _controller.AddCardsToCollection(deckId) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Invalid deck or user ID.", ((dynamic)result.Value).message);
        }
                [Fact]
        public async Task UpdateCard_ValidData_ReturnsOkResult()
        {
            // Arrange
            var deckId = Guid.NewGuid();
            var cardId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var quality = 4;

            _mockAuthService.Setup(a => a.GetRequesterId(It.IsAny<HttpContext>())).Returns(userId);

            // Act
            var result = await _controller.UpdateCard(deckId, cardId, quality) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Card updated successfully", ((dynamic)result.Value).message);

            _mockCardService.Verify(c => c.UpdateSpacedRepetition(userId, deckId, cardId, quality), Times.Once);
        }

        [Fact]
        public async Task UpdateCard_InvalidData_ReturnsBadRequest()
        {
            // Arrange
            var deckId = Guid.Empty;
            var cardId = Guid.Empty;
            var userId = Guid.Empty;
            var quality = 4;

            _mockAuthService.Setup(a => a.GetRequesterId(It.IsAny<HttpContext>())).Returns(userId);

            // Act
            var result = await _controller.UpdateCard(deckId, cardId, quality) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Invalid deck, card, or user ID.", ((dynamic)result.Value).message);
        }

        [Fact]
        public async Task UpdateCard_CardNotFound_ReturnsNotFound()
        {
            // Arrange
            var deckId = Guid.NewGuid();
            var cardId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var quality = 4;

            _mockAuthService.Setup(a => a.GetRequesterId(It.IsAny<HttpContext>())).Returns(userId);
            _mockCardService.Setup(c => c.UpdateSpacedRepetition(userId, deckId, cardId, quality)).Throws<KeyNotFoundException>();

            // Act
            var result = await _controller.UpdateCard(deckId, cardId, quality) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }

    }

}