using Microsoft.AspNetCore.Mvc;
using MementoMori.Server.DTOS;
using MementoMori.Server.Service;
using MementoMori.Server.Extensions;

namespace MementoMori.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CardDataController : ControllerBase
    {
        private readonly DatabaseCardWriter _databaseCardWriter;

        public CardDataController(DatabaseCardWriter databaseCardWriter)
        {
            _databaseCardWriter = databaseCardWriter;
        }

        [HttpPost("createCard")]
        public IActionResult PostInputData([FromBody] CardData data)
        {
            // Validate input data
            if (!data.IsValid())
            {
                return BadRequest("Invalid input data");
            }

            try
            {
                // Use FileWriter to create the file
                _databaseCardWriter.AddCard(data.Question, data.Answer, data.DeckId);
                return Ok(new { message = "Data received successfully", question = data.Question, text = data.Answer, cardId = data.DeckId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred: " + ex.Message });
            }
        }
    }
}