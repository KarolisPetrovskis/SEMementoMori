﻿using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MementoMori.Server.Controllers;
using MementoMori.Server.Interfaces;
using MementoMori.Server.DTOS;
using MementoMori.Server.Models;
using MementoMori.Server;
using MementoMori.Server.Exceptions;

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
                .Setup(d => d.Filter(It.Is<Guid[]>(ids => ids.Contains(deckId)), null, null, null))
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
                Creator = new User { Id = creatorId, Username = "TestUser", Password = "Password", HeaderColor = "white" },
                CardCount = 1,
                Modified = DateOnly.FromDateTime(DateTime.UtcNow),
                Rating = 4.5,
                isPublic = true,
                Tags = new List<TagTypes> { TagTypes.Music, TagTypes.Mathematics },
                Description = "Test Description",
                Cards = new List<Card> { new Card { Id = Guid.NewGuid(), Question = "Q1", Answer = "A1" } }
            };
            _mockDeckHelper
                .Setup(d => d.Filter(It.Is<Guid[]>(ids => ids.Contains(deckId)), null, null, It.Is<Guid?>(id => id == creatorId)))
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
                    .Setup(d => d.Filter(It.Is<Guid[]>(ids => ids.Contains(deckId)), null, null, null))
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
                    .Setup(d => d.Filter(It.Is<Guid[]>(ids => ids.Contains(deckId)), null, null, null))
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
        public async Task CreateDeck_Unautherized()
        {
            _mockAuthService
                .Setup(auth => auth.GetRequesterId(It.IsAny<HttpContext>()))
                .Returns((Guid?)null);

            var deckId = Guid.NewGuid();
            var deck = new EditedDeckDTO
            {
                Deck = new DeckEditableProperties
                {
                    Id = deckId,
                    isPublic = true,
                    Title = "Test Deck",
                    Description = "Test Description",
                    Tags = new List<TagTypes> { TagTypes.Music, TagTypes.Mathematics },
                },
                NewCards = [
                    new Card{
                    DeckId = deckId,
                    Question = "Q1",
                    Answer = "A1"},
                        new Card{
                    DeckId = deckId,
                    Question = "Q2",
                    Answer = "A2"}
                ]
            };
            var result = await _controller.CreateDeck(deck);
            Assert.IsType<UnauthorizedResult>(result.Result);
        }
        [Fact]
        public async Task CreateDeck_ValidRequest_ReturnsOk()
        {
            var requesterId = Guid.NewGuid();

            _mockAuthService
                .Setup(auth => auth.GetRequesterId(It.IsAny<HttpContext>()))
                .Returns(requesterId);

            var newDeckId = Guid.NewGuid();
            _mockDeckHelper
                .Setup(helper => helper.CreateDeckAsync(It.IsAny<EditedDeckDTO>(), requesterId))
                .ReturnsAsync(newDeckId);

            var deck = new EditedDeckDTO
            {
                Deck = new DeckEditableProperties
                {
                    Id = newDeckId,
                    isPublic = true,
                    Title = "Test Deck",
                    Description = "Test Description",
                    Tags = new List<TagTypes> { TagTypes.Music, TagTypes.Mathematics },
                },
                NewCards = [
                    new Card{
                    DeckId = newDeckId,
                    Question = "Q1",
                    Answer = "A1"},
                        new Card{
                    DeckId = newDeckId,
                    Question = "Q2",
                    Answer = "A2"}
                ]
            };

            var result = await _controller.CreateDeck(deck);

            Assert.IsType<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(newDeckId, okResult.Value);
        }

        [Fact]
        public async Task CreateDeck_InternalServerError()
        {
            var requesterId = Guid.NewGuid();

            _mockAuthService
                .Setup(auth => auth.GetRequesterId(It.IsAny<HttpContext>()))
                .Returns(requesterId);

            _mockDeckHelper
                .Setup(helper => helper.CreateDeckAsync(It.IsAny<EditedDeckDTO>(), requesterId))
                .ThrowsAsync(new Exception());

            var newDeckId = Guid.NewGuid();
            var deck = new EditedDeckDTO
            {
                Deck = new DeckEditableProperties
                {
                    Id = newDeckId,
                    isPublic = true,
                    Title = "Test Deck",
                    Description = "Test Description",
                    Tags = new List<TagTypes> { TagTypes.Music, TagTypes.Mathematics },
                },
                NewCards = [
                    new Card{
                    DeckId = newDeckId,
                    Question = "Q1",
                    Answer = "A1"},
                        new Card{
                    DeckId = newDeckId,
                    Question = "Q2",
                    Answer = "A2"}
                ]
            };

            var result = await _controller.CreateDeck(deck);
            Assert.IsType<StatusCodeResult>(result.Result);
            var statusCodeResult = result.Result as StatusCodeResult;
            Assert.NotNull(statusCodeResult);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
        [Fact]
        public async Task DeleteDeck_RequestPasses_ReturnsOk()
        {
            var requesterId = Guid.NewGuid();
            _mockAuthService
                .Setup(auth => auth.GetRequesterId(It.IsAny<HttpContext>()))
                .Returns((Guid)requesterId);
            var deckId = Guid.NewGuid();
            _mockDeckHelper
                .Setup(helper => helper.DeleteDeckAsync(deckId, requesterId))
                .Returns(Task.CompletedTask);
            var result = await _controller.DeleteDeck(deckId);
            Assert.IsType<OkResult>(result);
            _mockDeckHelper.Verify(helper => helper.DeleteDeckAsync(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Once);

        }
        [Fact]
        public async Task DeleteDeck_RequesterIdNull_ReturnsUnauthorized()
        {
            var deckId = Guid.NewGuid();
            _mockAuthService
                .Setup(auth => auth.GetRequesterId(It.IsAny<HttpContext>()))
                .Returns((Guid?)null);
            var result = await _controller.DeleteDeck(deckId);
            Assert.IsType<UnauthorizedResult>(result);
            _mockDeckHelper.Verify(helper => helper.DeleteDeckAsync(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
        }
        [Fact]
        public async Task DeleteDeck_ExceptionThrown_ReturnsStatusCode500()
        {
            var requesterId = Guid.NewGuid();
            _mockAuthService
                .Setup(auth => auth.GetRequesterId(It.IsAny<HttpContext>()))
                .Returns(requesterId);
            var deckId = Guid.NewGuid();
            _mockDeckHelper
                .Setup(helper => helper.DeleteDeckAsync(deckId, requesterId))
                .ThrowsAsync(new Exception());

            var result = await _controller.DeleteDeck(deckId);
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
        [Fact]
        public async Task EditDeck_ValidRequest_ReturnsOk()
        {
            var editedDeckDTO = new EditedDeckDTO
            {
                Deck = new DeckEditableProperties
                {
                    Id = Guid.NewGuid(),
                    isPublic = true,
                    Title = "Updated Title",
                    Description = "Updated Description",
                    Tags = new List<TagTypes> { TagTypes.Mathematics }
                }
            };

            var requesterId = Guid.NewGuid();
            _mockAuthService.Setup(auth => auth.GetRequesterId(It.IsAny<HttpContext>()))
                                .Returns(requesterId);

            var result = await _controller.EditDeck(editedDeckDTO);

            _mockDeckHelper.Verify(helper => helper.UpdateDeckAsync(editedDeckDTO, requesterId), Times.Once);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task EditDeck_MissingRequesterId_ReturnsUnauthorized()
        {
            var editedDeckDTO = new EditedDeckDTO
            {
                Deck = new DeckEditableProperties
                {
                    Id = Guid.NewGuid(),
                    isPublic = true,
                    Title = "Updated Title",
                    Description = "Updated Description",
                    Tags = new List<TagTypes> { TagTypes.Mathematics }
                }
            };

            _mockAuthService.Setup(auth => auth.GetRequesterId(It.IsAny<HttpContext>()))
                                .Returns((Guid?)null);

            var result = await _controller.EditDeck(editedDeckDTO);

            _mockDeckHelper.Verify(helper => helper.UpdateDeckAsync(It.IsAny<EditedDeckDTO>(), It.IsAny<Guid>()), Times.Never);
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task EditDeck_UnauthorizedEditingException_ReturnsUnauthorized()
        {
            var editedDeckDTO = new EditedDeckDTO
            {
                Deck = new DeckEditableProperties
                {
                    Id = Guid.NewGuid(),
                    isPublic = true,
                    Title = "Updated Title",
                    Description = "Updated Description",
                    Tags = new List<TagTypes> { TagTypes.Mathematics }
                }
            };

            var requesterId = Guid.NewGuid();
            _mockAuthService.Setup(auth => auth.GetRequesterId(It.IsAny<HttpContext>()))
                            .Returns(requesterId);

            _mockDeckHelper.Setup(helper => helper.UpdateDeckAsync(editedDeckDTO, requesterId))
                .ThrowsAsync(new UnauthorizedEditingException());

            var result = await _controller.EditDeck(editedDeckDTO);
            _mockDeckHelper.Verify(helper => helper.UpdateDeckAsync(editedDeckDTO, requesterId), Times.Once);
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public void AddCardsToCollection_ValidData_ReturnsOkResult(){

            var deckId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var deck = new Deck
            {
                Id = deckId,
                Title = "Test Deck",
                Creator = new User { Id = userId, Username = "TestUser", Password = "Password", HeaderColor = "blue" },
                CardCount = 1,
                Modified = DateOnly.FromDateTime(DateTime.UtcNow),
                Rating = 4.5,
                isPublic = true,
                Tags = new List<TagTypes> { TagTypes.Music, TagTypes.Mathematics },
                Description = "Test Description",
                RatingCount = 1,
                Cards = new List<Card> { new Card { Id = Guid.NewGuid(), Question = "Q1", Answer = "A1" } }
            };

            _mockAuthService.Setup(a => a.GetRequesterId(It.IsAny<HttpContext>())).Returns(userId);
            var result = _controller.AddCardsToCollection(deckId);
            Assert.IsType<OkResult>(result);
        }
        [Fact]
        public void AddCardsToCollection_InvalidData_ReturnsBadRequest()
        {
            var deckId = Guid.Empty;
            var userId = Guid.Empty;

            _mockAuthService.Setup(a => a.GetRequesterId(It.IsAny<HttpContext>())).Returns(userId);

            var result = _controller.AddCardsToCollection(deckId) as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }
        [Fact]
        public async Task UpdateCard_ValidData_ReturnsOkResult()
        {
            var deckId = Guid.NewGuid();
            var cardId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var quality = 4;

            _mockAuthService.Setup(a => a.GetRequesterId(It.IsAny<HttpContext>())).Returns(userId);

            var result = await _controller.UpdateCard(deckId, cardId, quality);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task UpdateCard_InvalidData_ReturnsBadRequest()
        {
            var deckId = Guid.Empty;
            var cardId = Guid.Empty;
            var userId = Guid.Empty;
            var quality = 4;

            _mockAuthService.Setup(a => a.GetRequesterId(It.IsAny<HttpContext>())).Returns(userId);

            var result = await _controller.UpdateCard(deckId, cardId, quality) as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task UpdateCard_CardNotFound_ReturnsNotFound()
        {
            var deckId = Guid.NewGuid();
            var cardId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var quality = 4;

            _mockAuthService.Setup(a => a.GetRequesterId(It.IsAny<HttpContext>())).Returns(userId);
            _mockCardService.Setup(c => c.UpdateSpacedRepetition(userId, deckId, cardId, quality)).Throws<KeyNotFoundException>();

            var result = await _controller.UpdateCard(deckId, cardId, quality) as NotFoundObjectResult;

            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }
    }
}
