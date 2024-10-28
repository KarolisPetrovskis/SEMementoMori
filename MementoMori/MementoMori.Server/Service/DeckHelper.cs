using MementoMori.Server.Database;
using MementoMori.Server.Extensions;
using MementoMori.Server.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MementoMori.Server.Service
{
    public class DeckHelper
    {
        private readonly AppDbContext _context;

        public DeckHelper(AppDbContext context) 
        {
            _context = context;
        }
        public List<Deck> Filter(Guid[]? ids = null, string? titleSubstring  = null, string[]? selectedTags = null)
        {
            var Decks = _context.Decks.Include(deck => deck.Cards).Where(deck => deck.isPublic);

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
    }
}
