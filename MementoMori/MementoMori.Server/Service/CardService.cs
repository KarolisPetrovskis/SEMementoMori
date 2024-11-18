using MementoMori.Server.Database;
using MementoMori.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace MementoMori.Server.Service
{
    public class CardService : ICardService
    {
        private readonly AppDbContext _context;
        private readonly ISpacedRepetition _spacedRepetitionService;
        //private readonly DeckHelper _deckHelper;

        public CardService(AppDbContext context, ISpacedRepetition spacedRepetitionService)
        {
            _context = context;
            _spacedRepetitionService = spacedRepetitionService;
            //_deckHelper = deckHelper;

        }
        public void AddCardsToCollection(Guid userId, Guid deckId)
        {
            // Fetch the deck with its cards from the database
            var deck = _context.Decks
                .Include(d => d.Cards)
                .FirstOrDefault(d => d.Id == deckId);

            if (deck == null)
            {
                throw new Exception("Deck not found.");
            }

            // Avoid duplicate database queries by loading all existing user cards for the deck
            var existingUserCards = _context.UserCards
                .Where(uc => uc.UserId == userId && uc.DeckId == deckId)
                .Select(uc => uc.CardId)
                .ToHashSet();

            var newUserCards = new List<UserCardData>();

            foreach (var card in deck.Cards)
            {
                // Skip adding cards that already exist in the user's collection
                if (!existingUserCards.Contains(card.Id))
                {
                    newUserCards.Add(new UserCardData
                    {
                        UserId = userId,
                        DeckId = deckId,
                        CardId = card.Id,
                        Interval = 1,
                        Repetitions = 0,
                        EaseFactor = 2.5,
                        LastReviewed = DateTime.UtcNow // Use UTC to avoid time zone issues
                    });
                }
            }

            // Add all new cards in bulk for efficiency
            if (newUserCards.Any())
            {
                _context.UserCards.AddRange(newUserCards);
                _context.SaveChanges(); // Save changes once to reduce database calls
            }
        }


        public void UpdateSpacedRepetition(Guid userId, Guid deckId, Guid cardId, int quality)
        {
            var userCardData =_context.UserCards
            .FirstOrDefault(uc => uc.UserId == userId && uc.DeckId == deckId && uc.CardId == cardId);

            if (userCardData == null)
            {
                throw new KeyNotFoundException("Card data not found for the specified user, deck, and card.");
            }

            _spacedRepetitionService.UpdateCard(userCardData, quality);
            userCardData.LastReviewed = DateTime.Now;

            _context.UserCards.Update(userCardData);
            _context.SaveChanges();
        }
        public List<Card> GetCardsForReview(Guid deckId)
        {
            //need to check with userId also
            var cardsForReview = _context.UserCards
            .Where(uc => uc.DeckId == deckId && uc.LastReviewed.AddDays(uc.Interval) <= DateTime.Today)
            .Select(uc => uc.CardId)
            .ToList();

            return _context.Cards
                .Where(c => cardsForReview.Contains(c.Id))
                .ToList();
        }
    }
}
