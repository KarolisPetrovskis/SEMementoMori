using Microsoft.AspNetCore.Mvc;
using MementoMori.Server.DTOS;

namespace MementoMori.Server.Controllers
{
    [ApiController]
    [Route("[controller]/{deckId}")]
    public class DecksController : ControllerBase
    {
        [HttpGet("deck")]
        public IActionResult View(Guid deckId) {

            if (deckId == Guid.Empty)
            {
                return BadRequest(new { errorCode = ErrorCode.InvalidInput, message = "Invalid deck ID." });
            }

            var Deck = TestDeck.Decks.FirstOrDefault(deck => deck.Id == deckId);

            if (Deck == null)
                return NotFound("Deck not found.");

            var DeckDTO = new DeckDTO
            {
                Id = Deck.Id,
                creatorId = Deck.creatorId,
                CardCount = Deck.CardCount,
                Modified = Deck.Modified,
                Rating = Deck.Rating,
                Tags = Deck.Tags,
                Title = Deck.Title,
                Description = Deck.Description
                
            };
            return Ok(DeckDTO);

        }

        [HttpGet("EditorView")]
        public IActionResult EditorView(Guid deckId) 
        {

            if (deckId == Guid.Empty)
            {
                return BadRequest(new { errorCode = ErrorCode.InvalidInput, message = "Invalid deck ID." });
            }

            var deck = TestDeck.Decks.FirstOrDefault(deck => deck.Id == deckId);

            if (deck == null)
                return NotFound("Deck not found.");

            var DTO = new DeckEditorDTO
            {
                Id = deck.Id,
                isPublic = deck.isPublic,
                CardCount = deck.CardCount,
                Description = deck.Description,
                Tags = deck.Tags,
                Title = deck.Title,
                Cards = deck.Cards.Select(Card => new CardDTO
                {
                    Id = Card.Id,
                    Question = Card.Question,
                    Description = Card.Description,
                    Answer = Card.Answer,

                }).ToArray()
            };
            return Ok(DTO);
        }

        [HttpGet("cards")]
        public IActionResult GetCards(Guid deckId)
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
