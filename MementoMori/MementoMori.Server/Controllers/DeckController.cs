using Microsoft.AspNetCore.Mvc;
using MementoMori.Server.DTOS;

namespace MementoMori.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class DecksController : ControllerBase
    {
        
        private readonly ICardFileReader _cardFileReader;


        public DecksController(ICardFileReader cardFileReader)
        {
   			_cardFileReader = cardFileReader;

        }

        [HttpGet("{deckId}/cards")]
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
			// Assuming the file is always 001.txt if you want to display more files in a static way then you can do modifications inf GetFileContent
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
