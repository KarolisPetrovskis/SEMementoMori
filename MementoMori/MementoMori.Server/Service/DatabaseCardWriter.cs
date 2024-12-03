using MementoMori.Server.Database;
using MementoMori.Server.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace MementoMori.Server.Service
{
    public class DatabaseCardWriter : IDatabaseCardWriter
    {
        private readonly AppDbContext _context;
        public DatabaseCardWriter(AppDbContext context)
        {
            _context = context;
        }

        public void AddCard(string question, string text, Guid deckId)
        {
            Deck deck = _context.Decks.SingleOrDefault(c => c.Id == deckId);

            var newCard = new Card
            {
                Id = Guid.NewGuid(),
                Question = question,
                Description = "NULL",
                Answer = text,
            };
            if (deck != null)
            {
                deck.Cards.Add(newCard);

                _context.Decks.Add(deck);

                _context.SaveChanges();
            }
        }

        public void UpdateCardData(Guid cardId, string? question = null, string? description = null, string? answer = null, int? lastInterval = null, DateOnly? nextShow = null)
        {
            var card = _context.Cards.SingleOrDefault(c => c.Id == cardId);
            if (card != null) 
            {
                if (question != null)
                {
                    card.Question = question;
                }
                if (description != null)
                {
                    card.Description = description;
                }
                if (answer != null)
                {
                    card.Answer = answer;
                }
                _context.SaveChanges();
            }
        }

    }
}
