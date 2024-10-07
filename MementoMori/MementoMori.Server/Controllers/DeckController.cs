using Microsoft.AspNetCore.Mvc;
using MementoMori.Server.DTOS;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace MementoMori.Server.Controllers
{

    namespace MementoMori.Server.Controllers
    {
        [ApiController]
        [Route("[controller]")]

        public class DecksController : ControllerBase
        {

            [HttpGet]
            public IActionResult GetDecks()
            {
                return Ok(TestDeck.Decks);

            }

            [HttpGet("{deckId}/cards")]
            public IActionResult GetDeck(Guid deckId)
            {
                var deck = TestDeck.Decks.FirstOrDefault(deck => deck.Id == deckId);
                var Cards = deck.Cards.Select(Card => new CardDTO
                {
                    Id = Card.Id,
                    Question = Card.Question,
                    Description = Card.Description,
                    Answer = Card.Answer,

                }).ToList();

                if (deck == null)
                    return NotFound("Deck not found.");

                return Ok(Cards);
            }

        }
    }
}
