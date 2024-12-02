using MementoMori.Server;
using MementoMori.Server.Database;
using MementoMori.Server.DTOS;
using MementoMori.Server.Models;
using MementoMori.Server.Service;
using Microsoft.EntityFrameworkCore;

namespace MementoMori.Tests.UnitTests.ServiceTests
{
    public class DeckHelperTests
    {
        private AppDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async Task Filter_ReturnsEmptyList_WhenNoMatches()
        {
            var context = CreateDbContext();
            var helper = new DeckHelper(context);
            var deck1 = new Deck { Id = Guid.NewGuid(), Title = "Deck1", isPublic = true, CardCount = 1, Modified = DateOnly.MaxValue, };
            context.Decks.Add(deck1);
            context.SaveChanges();

            var result = await helper.Filter(selectedTags: new[] { "Mathematics" });

            Assert.Empty(result);
        }

        [Fact]
        public void UpdateDeck_UpdatesDeckDetails()
        {
            var context = CreateDbContext();
            var helper = new DeckHelper(context);
            var creatorId = Guid.NewGuid();
            var deck = new Deck { Id = Guid.NewGuid(), Title = "Old Title", isPublic = true, CardCount = 1, Modified = DateOnly.MaxValue, CreatorId = creatorId };
            context.Decks.Add(deck);
            context.SaveChanges();
            var updatedDeckDTO = new EditedDeckDTO
            {
                Deck = new DeckEditableProperties
                {
                    Id = deck.Id,
                    Title = "New Title",
                    isPublic = false
                }
            };

            helper.UpdateDeckAsync(updatedDeckDTO, creatorId);

            var updatedDeck = context.Decks.First();
            Assert.Equal("New Title", updatedDeck.Title);
            Assert.False(updatedDeck.isPublic);
        }

        [Fact]
        public void UpdateDeck_AddsNewCards()
        {
            var context = CreateDbContext();
            var helper = new DeckHelper(context);
            var creatorId = Guid.NewGuid();
            var deck = new Deck { Id = Guid.NewGuid(), Title = "Deck1", isPublic = true, CardCount = 1, Modified = DateOnly.MaxValue, CreatorId = creatorId };
            context.Decks.Add(deck);
            context.SaveChanges();
            var newCard = new Card { Id = Guid.NewGuid(), Question = "New Question", Answer = "New Answer" };
            var updatedDeckDTO = new EditedDeckDTO
            {
                Deck = new DeckEditableProperties { Id = deck.Id, isPublic = true, Title = "test" },
                NewCards = new[] { newCard }
            };

            helper.UpdateDeckAsync(updatedDeckDTO, creatorId);

            var addedCard = context.Cards.First();
            Assert.Equal(newCard.Question, addedCard.Question);
            Assert.Equal(deck.Id, addedCard.DeckId);
        }

        [Fact]
        public void UpdateDeck_RemovesCards()
        {
            var context = CreateDbContext();
            var helper = new DeckHelper(context);
            var creatorId = Guid.NewGuid();
            var deck = new Deck { Id = Guid.NewGuid(), Title = "Deck1", isPublic = true, CardCount = 1, Modified = DateOnly.MaxValue, CreatorId = creatorId };
            var card = new Card { Id = Guid.NewGuid(), Question = "Question1", Answer = "Answer1", DeckId = deck.Id };
            context.Decks.Add(deck);
            context.Cards.Add(card);
            context.SaveChanges();
            var updatedDeckDTO = new EditedDeckDTO
            {
                Deck = new DeckEditableProperties { Id = deck.Id, isPublic = true, Title = "Title" },
                RemovedCards = new[] { card.Id }
            };

            helper.UpdateDeckAsync(updatedDeckDTO, creatorId);

            Assert.Empty(context.Cards);
        }

        [Fact]
        public async Task CreateDeck()
        {
            var context = CreateDbContext();
            var helper = new DeckHelper(context);
            var deck = new {
                    isPublic = true,
                    Title = " Test deck",
                    Description = "",
                    Tags = new List<TagTypes> ([TagTypes.Beginner, TagTypes.Biology]),     
                };
                            CardEditableProperties[] cards = new CardEditableProperties[]
            {
                new CardEditableProperties
                {
                    Question = "What is the capital of France?",
                    Description = "Geography question",
                    Answer = "Paris",
                    lastInterval = null,
                    nextShow = null,
                },
                new CardEditableProperties
                {
                    Question = "What is 2 + 2?",
                    Description = "Simple math question",
                    Answer = "4",
                    lastInterval = null,
                    nextShow = null,
                },
                new CardEditableProperties
                {
                    Question = "Who wrote 'To Kill a Mockingbird'?",
                    Description = "Literature question",
                    Answer = "Harper Lee",
                    lastInterval = null, // No previous interval
                    nextShow = null // No scheduled review
                }
            };

            //await helper.CreateDeck()    
        }
    }
}
