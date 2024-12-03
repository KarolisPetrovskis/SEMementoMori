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

        public EditedDeckDTO createTestDeck1()
        {
            var deck = new DeckEditableProperties{
                    isPublic = true,
                    Title = "Test deck",
                    Description = null,
                    Tags = new List<TagTypes> ([TagTypes.Beginner, TagTypes.Biology]),     
                };
            Card[] cards =
            [
                new Card
                {
                    DeckId = Guid.Empty,
                    Question = "What is the capital of France?",
                    Description = "Geography question",
                    Answer = "Paris",
                    lastInterval = null,
                    nextShow = null,
                },
                new Card
                {
                    DeckId = Guid.Empty,
                    Question = "What is 2 + 2?",
                    Description = "Simple math question",
                    Answer = "4",
                    lastInterval = null,
                    nextShow = null,
                },
                new Card
                {
                    DeckId = Guid.Empty,
                    Question = "Who wrote 'To Kill a Mockingbird'?",
                    Description = "Literature question",
                    Answer = "Harper Lee",
                    lastInterval = null,
                    nextShow = null,
                }
            ];
            var editedDeck = new EditedDeckDTO{
                Deck = deck,
                Cards = null,
                NewCards = cards,
                RemovedCards = null,
            };
            return editedDeck;
        }
        public Deck createTestDeck2()
        {
            var deckId = Guid.NewGuid();
            var deck = new Deck
            {
                Id = deckId,
                CreatorId = Guid.NewGuid(),
                isPublic = true,
                Title = " Test Deck",
                Description = "This is a test deck.",
                Tags = new List<TagTypes> { TagTypes.Biology },
                Modified = DateOnly.FromDateTime(DateTime.Now),
                CardCount = 2,
                Cards = new List<Card>
                {
                    new Card
                    {
                        Id = Guid.NewGuid(),
                        DeckId = deckId,
                        Question = "What is the capital of France?",
                        Answer = "Paris",
                        Description = "Geography question",
                        lastInterval = null,
                        nextShow = null,
                    },
                    new Card
                    {
                        Id = Guid.NewGuid(),
                        DeckId = deckId,
                        Question = "What is 2 + 2?",
                        Answer = "4",
                        Description = "Math question",
                        lastInterval = null,
                        nextShow = null
                    }
                }
            };
            return deck;
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
        public async Task CreateDeck_CreatesNewDeckSuccessfully()
        {
            var context = CreateDbContext();
            var helper = new DeckHelper(context);

            var deck = createTestDeck1();

            Guid requesterId = Guid.NewGuid();

            Guid savedDeckId = await helper.CreateDeckAsync(deck, requesterId);

            var savedDeck = context.Decks.Include(d => d.Cards).FirstOrDefault(d => d.Id == savedDeckId);

            Assert.NotNull(savedDeck);
            Assert.Equal(requesterId, savedDeck.CreatorId);
            Assert.Equal(deck.Deck.Title, savedDeck.Title);
            Assert.Null(savedDeck.Description);
            Assert.True(savedDeck.isPublic);
            Assert.Equal(deck.Deck.Tags, savedDeck.Tags);
            Assert.NotNull(savedDeck.Cards);
            Assert.Equal(deck.NewCards?.Length, savedDeck.Cards.Count);
        }
        [Fact]
        public async Task DeleteDeck_DeleteDeckSuccessfully()
        {
            var context = CreateDbContext();
            var helper = new DeckHelper(context);

            var deck = createTestDeck2();

            context.Decks.Add(deck);
            await context.SaveChangesAsync();
            await helper.DeleteDeckAsync(deck.Id);

            var deletedDeck = await context.Decks.FindAsync(deck.Id);
            Assert.Null(deletedDeck);

            var associatedCards = context.Cards.Where(c => c.DeckId == deck.Id).ToList();
            Assert.Empty(associatedCards);
                    
        }

    }
}
