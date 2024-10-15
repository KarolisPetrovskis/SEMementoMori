using MementoMori.Server.DTOS;
using Microsoft.AspNetCore.Mvc;

namespace MementoMori.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DeckBrowserController : ControllerBase
    {

        [HttpGet("getDecks")]
        public ActionResult<DeckBrowserDTO> getDecks([FromQuery] string[] selectedTags, string? searchString)
        {
            var Decks = TestDeck.Decks;

            var filteredDecks = Decks.ToList().Where(deck => deck.isPublic);

            if (selectedTags.Length != 0)
            {
                filteredDecks = filteredDecks.Where(deck => deck.Tags != null && selectedTags.All(tag => deck.Tags.Contains(tag)));
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                filteredDecks = filteredDecks.Where(deck => deck.Title.Contains(searchString, StringComparison.OrdinalIgnoreCase));
            }

            var filteredDecksList = filteredDecks.ToList();
            filteredDecksList.Sort();

            var result = filteredDecksList.Select(deck => new DeckBrowserDTO
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
