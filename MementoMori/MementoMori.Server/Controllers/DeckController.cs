using Microsoft.AspNetCore.Mvc;
using MementoMori.Server.DTOS;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace MementoMori.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class DecksController : ControllerBase
    {

        [HttpGet("{deckId}/cards")]
        public IActionResult GetDeck(Guid deckId)
        {

            if (deckId == Guid.Empty)
            {
                return BadRequest(new { errorCode = ErrorCode.InvalidInput, message = "Invalid deck ID." });
            }
            
            var deck = TestDeck.Decks.FirstOrDefault(deck => deck.Id == deckId);

            if (deck == null)
                return NotFound("Deck not found.");

            var Cards = deck.Cards.Select(Card => new CardDTO
            {
                Id = Card.Id,
                Question = Card.Question,
                Description = Card.Description,
                Answer = Card.Answer,

            }).ToList();

            return Ok(Cards);

            
        }

    }
}
