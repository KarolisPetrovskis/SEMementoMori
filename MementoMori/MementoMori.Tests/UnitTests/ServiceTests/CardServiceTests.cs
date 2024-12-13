using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using MementoMori.Server.Database;
using MementoMori.Server.Models;
using MementoMori.Server.Service;
using Moq;
using MementoMori.Server;

namespace MementoMori.Tests
{
    public class CardServiceTests
    {
        private readonly AppDbContext _context;
        private readonly Mock<ISpacedRepetition> _mockSpacedRepetition;
        private readonly CardService _service;

        public CardServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new AppDbContext(options);
            _mockSpacedRepetition = new Mock<ISpacedRepetition>();
            _service = new CardService(_context, _mockSpacedRepetition.Object);
        }

        [Fact]
        public void AddCardsToCollection_ValidDeck_AddsNewUserCards()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var deckId = Guid.NewGuid();
            var card1 = new Card { Id = Guid.NewGuid(), Answer = "", Question = "" };
            var card2 = new Card { Id = Guid.NewGuid(), Answer = "", Question = "" };

            var deck = new Deck { Id = deckId, Cards = new List<Card> { card1, card2 }, Title = "", isPublic = false,
            CardCount = 2, Modified = DateOnly.MaxValue};

            _context.Decks.Add(deck);
            _context.SaveChanges();

            // Act
            _service.AddCardsToCollection(userId, deckId);

            // Assert
            var userCards = _context.UserCards.Where(uc => uc.UserId == userId && uc.DeckId == deckId).ToList();
            Assert.Equal(2, userCards.Count);
            Assert.Contains(userCards, uc => uc.CardId == card1.Id);
            Assert.Contains(userCards, uc => uc.CardId == card2.Id);
        }

        [Fact]
        public void AddCardsToCollection_InvalidDeck_ThrowsException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var deckId = Guid.NewGuid();

            // Act & Assert
            Assert.Throws<Exception>(() => _service.AddCardsToCollection(userId, deckId));
        }

        [Fact]
        public async Task UpdateSpacedRepetition_ValidCard_UpdatesCardData()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var deckId = Guid.NewGuid();
            var cardId = Guid.NewGuid();
            var quality = 5;

            var userCard = new UserCardData
            {
                UserId = userId,
                DeckId = deckId,
                CardId = cardId,
                Interval = 1,
                Repetitions = 0,
                EaseFactor = 2.5,
                LastReviewed = DateTime.UtcNow
            };

            _context.UserCards.Add(userCard);
            _context.SaveChanges();

            // Act
            await _service.UpdateSpacedRepetition(userId, deckId, cardId, quality);

            // Assert
            _mockSpacedRepetition.Verify(s => s.UpdateCard(userCard, quality), Times.Once);
            var updatedCard = _context.UserCards.FirstOrDefault(uc => uc.UserId == userId && uc.DeckId == deckId && uc.CardId == cardId);
            Assert.NotNull(updatedCard);
            Assert.True(updatedCard.LastReviewed > DateTime.UtcNow.AddMinutes(-1));
        }

        [Fact]
        public async Task UpdateSpacedRepetition_InvalidCard_ThrowsException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var deckId = Guid.NewGuid();
            var cardId = Guid.NewGuid();
            var quality = 5;

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                await _service.UpdateSpacedRepetition(userId, deckId, cardId, quality));
        }

        [Fact]
        public void GetCardsForReview_ValidData_ReturnsCardsForReview()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var deckId = Guid.NewGuid();
            var cardId = Guid.NewGuid();
            var today = DateTime.Today;

            var userCard = new UserCardData
            {
                UserId = userId,
                DeckId = deckId,
                CardId = cardId,
                Interval = 1,
                LastReviewed = today.AddDays(-1)
            };

            var card = new Card { Id = cardId, Answer = "", Question = "" };

            _context.UserCards.Add(userCard);
            _context.Cards.Add(card);
            _context.SaveChanges();

            // Act
            var result = _service.GetCardsForReview(deckId, userId);

            // Assert
            Assert.Single(result);
            Assert.Equal(cardId, result.First().Id);
        }

        [Fact]
        public void GetCardsForReview_NoCards_ReturnsEmptyList()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var deckId = Guid.NewGuid();

            // Act
            var result = _service.GetCardsForReview(deckId, userId);

            // Assert
            Assert.Empty(result);
        }
    }
}
