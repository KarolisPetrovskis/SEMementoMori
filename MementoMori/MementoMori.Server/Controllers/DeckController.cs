using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace MementoMori.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]


    public class DeckBrowserController : ControllerBase
    {

        [HttpGet]
        public IActionResult GetDecks()
        {
            return Ok(TestDeck.Decks);

        }

        [HttpGet("{deckId}")]
        public IActionResult GetDeck(Guid deckId) 
        {
            var deck = TestDeck.Decks.FirstOrDefaul(deck  => deck.Id == deckId);

            if (deck == null)
                return NotFound("Deck not found.");

            return Ok(deck);
        }
        
        //Create deck

        //Get the next card of a deck by priority (don't know if it should be in the controller)
}
