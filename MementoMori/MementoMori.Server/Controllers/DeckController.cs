using Microsoft.AspNetCore.Mvc;
using MementoMori.Server.DTOS;
using MementoMori.Server.Extensions;

namespace MementoMori.Server.Controllers
{
    [ApiController]
    [Route("[controller]/{deckId}")]
    public class DecksController : ControllerBase
    {
        
        private readonly ICardFileReader _cardFileReader;


        public DecksController(ICardFileReader cardFileReader)
        {
   			_cardFileReader = cardFileReader;

        }

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
                Tags = Deck.TagsToString(),
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
                Tags = deck.TagsToString(),
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
            
			string serverDirectory = Directory.GetCurrentDirectory();
			string _filePath = Path.Combine(serverDirectory, "CardFile", deckId.ToString() + ".txt");

			var fileContent = _cardFileReader.ExtractCards(_filePath).AsQueryable();
            
            var Cards = fileContent.Select(Card => new CardDTO
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
