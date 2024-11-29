using MementoMori.Server.Database;
using MementoMori.Server.DTOS;
using MementoMori.Server.Exceptions;
using MementoMori.Server.Extensions;
using MementoMori.Server.Interfaces;
using MementoMori.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace MementoMori.Server.Service
{
    public class DeckHelper : IDeckHelper
    {
        private readonly AppDbContext _context;

        public DeckHelper(AppDbContext context)
        {
            _context = context;
        }
        public List<Deck> Filter(Guid[]? ids = null, string? titleSubstring = null, string[]? selectedTags = null)
        {
            var Decks = _context.Decks.Include(deck => deck.Cards).Include(deck => deck.Creator).Where(deck => deck.isPublic);

            if (ids != null && ids.Length > 0)
            {
                Decks = Decks.Where(deck => ids.Contains(deck.Id));
            }

            if (!string.IsNullOrEmpty(titleSubstring))
            {
                Decks = Decks.Where(deck => EF.Functions.ILike(deck.Title, $"%{titleSubstring}%"));
            }

            if (selectedTags != null && selectedTags.Length != 0)
            {
                TagTypes[] selectedTagEnums;
                try
                {
                    selectedTagEnums = selectedTags.Select(tag => Enum.Parse<TagTypes>(tag)).ToArray();
                }
                catch (Exception)
                {
                    // This state should not be reachable by using only UI
                    return new List<Deck>();
                }

                Decks = Decks.Where(deck => deck.Tags != null && !(selectedTagEnums.Except(deck.Tags).Any()));
            }

            return Decks.ToList();
        }

        public void UpdateDeck(EditedDeckDTO editedDeckDTO, Guid requesterId)
        {
            try
            {
                _context.SecureUpdate<Deck, DeckEditableProperties>(editedDeckDTO.Deck, requesterId);
                if (editedDeckDTO.Cards != null)
                {
                    foreach (CardEditableProperties card in editedDeckDTO.Cards)
                    {
                        _context.SecureUpdate<Card, CardEditableProperties>(card, editedDeckDTO.Deck.Id);
                    }
                }
                if (editedDeckDTO.NewCards != null)
                {
                    foreach (Card card in editedDeckDTO.NewCards)
                    {
                        card.DeckId = editedDeckDTO.Deck.Id;
                        _context.Add(card);
                    }
                }
                if (editedDeckDTO.RemovedCards != null)
                {
                    foreach (Guid cardId in editedDeckDTO.RemovedCards)
                    {
                        _context.Remove<Card>(cardId);
                    }
                }
                _context.SaveChanges();
            }
            catch (UnauthorizedEditingException ex)
            {
                LogError(editedDeckDTO.Deck.Id, requesterId, ex);
                throw;
            }
        }
        public Guid CreateDeck (EditedDeckDTO createDeck, Guid requesterId)
        {
            Guid newDeckGuid = Guid.NewGuid();
            Deck newDeck = new()
            {
                Id = newDeckGuid,
                CreatorId = requesterId,
                isPublic = createDeck.Deck.isPublic,
                Title = createDeck.Deck.Title,
                Description = createDeck.Deck.Title,
                Tags = createDeck.Deck.Tags,
                Rating = 0,
                RatingCount = 0,
                Modified = DateOnly.FromDateTime(DateTime.Now),
                Cards = createDeck.NewCards?.ToList(),
                CardCount = 0,
            };
            _context.Decks.Add(newDeck);
            _context.SaveChanges();
            return newDeckGuid;
        }
        public void DeleteDeck(Guid deckId)
        {
            var deck = _context.Decks.Include(d => d.Cards).FirstOrDefault(d => d.Id == deckId);
            if (deck != null)
            {
                _context.Decks.Remove(deck);
                _context.SaveChanges();
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }
        private void LogError(Guid deckId, Guid requesterId, Exception exception)
        {
            string logFilePath = "error_log.txt";
            string logEntry = $"Timestamp: {DateTime.UtcNow}\nDeckId: {deckId}\nRequesterId: {requesterId}\nError: {exception.Message}\n---\n";

            try
            {
                File.AppendAllText(logFilePath, logEntry);
            }
            catch (Exception logEx)
            {
                Console.WriteLine($"Failed to log error: {logEx.Message}");
            }
        }
    }
}
