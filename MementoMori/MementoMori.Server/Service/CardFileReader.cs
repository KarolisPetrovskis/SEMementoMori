using MementoMori.Server.Database;
using Microsoft.EntityFrameworkCore;
namespace MementoMori.Server
{
    public class CardFileReader : ICardFileReader
    {

        private readonly AppDbContext _context;
        public CardFileReader(AppDbContext context)
        {
            _context = context;
        }

        public Card[] ExtractCards(Guid deckId)
        {
            Card[] cards = _context.Cards
                .Where(card => EF.Property<Guid>(card, "DeckId") == deckId)
                .ToArray();
            return cards;

        }
    }
}