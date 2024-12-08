﻿using MementoMori.Server;
using MementoMori.Server.Database;
using MementoMori.Server.DTOS;
using MementoMori.Server.Exceptions;
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
        public async void UpdateDeck_RemovesCards()
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

            await helper.UpdateDeckAsync(updatedDeckDTO, creatorId);

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
        public async Task CreateDeck_FailsWithArgumentException()
        {
            var context = CreateDbContext();
            var helper = new DeckHelper(context);

            var invalidDeckDTO = new EditedDeckDTO
            {
                Deck = new DeckEditableProperties
                {
                    isPublic = true,
                    Title = "   ",
                    Description = null,
                    Tags = new List<TagTypes> { TagTypes.Biology }
                },
                NewCards = new[]
                {
                    new Card { Question = "Question?", Answer = "Answer", DeckId = Guid.Empty }
                }
            };
            Guid requesterId = Guid.NewGuid();
            await Assert.ThrowsAsync<ArgumentException>(() => helper.CreateDeckAsync(invalidDeckDTO, requesterId));
        }
        [Fact]
        public async Task DeleteDeck_DeleteDeckSuccessfully()
        {
            var context = CreateDbContext();
            var helper = new DeckHelper(context);

            var deck = createTestDeck2();
            Guid requesterId = deck.CreatorId;
            context.Decks.Add(deck);
            await context.SaveChangesAsync();
            await helper.DeleteDeckAsync(deck.Id, requesterId);

            var deletedDeck = await context.Decks.FindAsync(deck.Id);
            Assert.Null(deletedDeck);

            var associatedCards = context.Cards.Where(c => c.DeckId == deck.Id).ToList();
            Assert.Empty(associatedCards);
                    
        }

        [Fact]
        public async Task DeleteDeck_ThrowsUnauthorizedEditingException_WhenUserIsNotCreator()
        {
            var context = CreateDbContext();
            var helper = new DeckHelper(context);

            var deck = createTestDeck2();
            Guid requesterId = Guid.NewGuid();
            context.Decks.Add(deck);
            await context.SaveChangesAsync();
            var exception = await Assert.ThrowsAsync<UnauthorizedEditingException>(() => helper.DeleteDeckAsync(deck.Id, requesterId));
            Assert.NotNull(exception); 
        }

        [Fact]
        public async Task DeleteDeck_KeyNotFoundException()
        {
            var context = CreateDbContext();
            var helper = new DeckHelper(context);

            Guid nonExistentDeckId = Guid.NewGuid();
            Guid requesterId = Guid.NewGuid();

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => helper.DeleteDeckAsync(nonExistentDeckId, requesterId));
            Assert.NotNull(exception);
        }

        [Fact]
        public async Task GetUserDecks_ReturnsUserDecks_WhenUserHasDecks()
        {
            var context = CreateDbContext();
            var helper = new DeckHelper(context);
            var userId = Guid.NewGuid();

            var decks = new List<Deck>
            {
                new Deck 
                { 
                    Id = Guid.NewGuid(), 
                    CreatorId = userId, 
                    Title = "Deck 1", 
                    isPublic = true,
                    Modified = DateOnly.FromDateTime(DateTime.Now),
                    CardCount = 0
                },
                new Deck 
                { 
                    Id = Guid.NewGuid(), 
                    CreatorId = userId, 
                    Title = "Deck 2", 
                    isPublic = false,
                    Modified = DateOnly.FromDateTime(DateTime.Now),
                    CardCount = 0
                }
            };

            context.Decks.AddRange(decks);
            await context.SaveChangesAsync();
            var userDecks = await helper.GetUserDecks(userId);

            Assert.NotNull(userDecks);
            Assert.Equal(2, userDecks.Length);
            Assert.Contains(userDecks, d => d.Title == "Deck 1");
            Assert.Contains(userDecks, d => d.Title == "Deck 2");
        }

        [Fact]
        public async Task GetUserDecks_ReturnsEmptyArray_WhenUserHasNoDecks()
        {
            var context = CreateDbContext();
            var helper = new DeckHelper(context);
            var userId = Guid.NewGuid();
            await context.SaveChangesAsync();
            var userDecks = await helper.GetUserDecks(userId);

            Assert.NotNull(userDecks);
            Assert.Empty(userDecks);
        }

        [Fact]
        public async Task GetUserDecks_ReturnsOnlyUserOwnedDecks()
        {
            var context = CreateDbContext();
            var helper = new DeckHelper(context);
            var userId = Guid.NewGuid();
            var otherUserId = Guid.NewGuid();

            var decks = new List<Deck>
            {
                new Deck 
                { 
                    Id = Guid.NewGuid(), 
                    CreatorId = userId, 
                    Title = "User's Deck", 
                    isPublic = false,
                    Modified = DateOnly.FromDateTime(DateTime.Now),
                    CardCount = 0
                },
                new Deck 
                { 
                    Id = Guid.NewGuid(), 
                    CreatorId = otherUserId, 
                    Title = "Other User's Deck", 
                    isPublic = true,
                    Modified = DateOnly.FromDateTime(DateTime.Now),
                    CardCount = 0
                }
            };

            context.Decks.AddRange(decks);
            await context.SaveChangesAsync();
            var userDecks = await helper.GetUserDecks(userId);

            Assert.NotNull(userDecks);
            Assert.Single(userDecks);
            Assert.Equal("User's Deck", userDecks[0].Title);
        }

        [Fact]
        public async Task GetUserDecks_ReturnsCorrectDTOProperties()
        {
            var context = CreateDbContext();
            var helper = new DeckHelper(context);
            var userId = Guid.NewGuid();

            var deck = new Deck 
            { 
                Id = Guid.NewGuid(), 
                CreatorId = userId, 
                Title = "Test Deck", 
                isPublic = true,
                Modified = DateOnly.FromDateTime(DateTime.Now),
                CardCount = 0
            };

            context.Decks.Add(deck);
            await context.SaveChangesAsync();
            var userDecks = await helper.GetUserDecks(userId);

            Assert.NotNull(userDecks);
            Assert.Single(userDecks);
            
            var userDeck = userDecks[0];

            Assert.Equal(deck.Id, userDeck.Id);
            Assert.Equal(deck.Title, userDeck.Title);
        }
    }
}
