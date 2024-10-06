using Microsoft.AspNetCore.Mvc;

namespace MementoMori.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DeckBrowserController : ControllerBase
    {

        [HttpGet("getDecks")]
        public ActionResult<DeckBrowserDeck> GetDecks([FromQuery] string[] selectedTags, string? searchString)
        {
            var Decks = TestDeck.Decks;

            var filteredDecks = Decks.AsQueryable().Where(deck => deck.isPublic);

            if (selectedTags.Length != 0)
            {
                filteredDecks = filteredDecks.Where(deck => deck.Tags != null && selectedTags.All(tag => deck.Tags.Contains(tag)));
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                filteredDecks = filteredDecks.Where(deck => deck.Title.Contains(searchString, StringComparison.OrdinalIgnoreCase));
            }

            filteredDecks = filteredDecks.Order().Reverse();

            var result = filteredDecks.Select(deck => new DeckBrowserDeck
            {
                Id = deck.Id,
                Title = deck.Title,
                Rating = deck.Rating,
                Modified = deck.Modified,
                Cards = deck.CardCount,
                Tags = deck.Tags
            }).ToArray();

            return Ok(result);
        }
    }
}
