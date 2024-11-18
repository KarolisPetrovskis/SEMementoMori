using MementoMori.Server.Database;
using MementoMori.Server.DTOS;
using MementoMori.Server.Exceptions;
using MementoMori.Server.Extensions;
using MementoMori.Server.Interfaces;
using MementoMori.Server.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
