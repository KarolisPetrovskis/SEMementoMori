using MementoMori.Server.Database;
using MementoMori.Server.Migrations;
using MementoMori.Server.Models;
using MementoMori.Server.Service;
using Microsoft.EntityFrameworkCore;

namespace MementoMori.Server
{
    public class CardService : ICardService
    {
        private readonly AppDbContext _context;
        private readonly ISpacedRepetition _spacedRepetitionService;
        private readonly DeckHelper _deckHelper;

        public CardService(AppDbContext context, ISpacedRepetition spacedRepetitionService, DeckHelper deckHelper)
        {
            _context = context;
            _spacedRepetitionService = spacedRepetitionService;
            _deckHelper = deckHelper;

        }
        public void AddCardsToCollection(Guid userId, Guid deckId)
        {
            var deck = _context.Decks
                .Include(d => d.Cards)
                .FirstOrDefault(d => d.Id == deckId);

            if (deck == null)
            {
                throw new Exception("Deck not found.");
            }

            foreach (var card in deck.Cards)
            {
                var existingUserCard = _context.UserCards
                    .FirstOrDefault(uc => uc.UserId == userId && uc.DeckId == deckId && uc.CardId == card.Id);

                if (existingUserCard == null)
                {
                    var userCardData = new UserCardData
                    {
                        UserId = userId,
                        DeckId = deckId,
                        CardId = card.Id,
                        Interval = 1,
                        Repetitions = 0,
                        EaseFactor = 2.5,
                        LastReviewed = DateTime.Now
                    };

                    _context.UserCards.Add(userCardData);
                }
            }

            _context.SaveChanges();
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
