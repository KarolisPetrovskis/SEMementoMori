using MementoMori.Server.Extensions;

namespace MementoMori.Server.Service
{
    public class DeckHelper
    {
        public List<Deck> Filter(Guid[]? ids = null, string? titleSubstring  = null, string[]? selectedTags = null)
        {
            var Decks = TestDeck.Decks;
            var filteredDecks = Decks.ToList().Where(deck => deck.isPublic);
            if (ids != null)
            {
                filteredDecks = TestDeck.Decks.Where(deck => ids.Contains(deck.Id));
            }
            if (!string.IsNullOrEmpty(titleSubstring))
            {
                filteredDecks = filteredDecks.Where(deck => deck.Title.Contains(titleSubstring, StringComparison.OrdinalIgnoreCase));
            }
            if (selectedTags != null && selectedTags.Length != 0)
            {
                filteredDecks = filteredDecks.Where(deck => deck.Tags != null && selectedTags.All(tag => deck.TagsToString().Contains(tag)));
            }
            return filteredDecks.ToList();
        }
    }
}
