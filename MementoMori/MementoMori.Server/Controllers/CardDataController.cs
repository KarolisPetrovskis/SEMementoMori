using Microsoft.AspNetCore.Mvc;
using MementoMori.Server.Extensions;

namespace MementoMori.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CardDataController : ControllerBase
    {
        [HttpPost("createCard")]
        public IActionResult PostInputData([FromBody] CardData data)
        {
            // Usage of extension method
            if (!data.IsValid())
            {
                return BadRequest("Invalid input data");
            }

            try
            {
                // Usage of the extension method to initialize FileWriter class
                var fileWriter = this.InitializeFileWriter();
                fileWriter.CreateFile(data.Tags, data.Text, data.DeckId.ToString());

                return Ok(new { message = "Data received successfully", tags = data.Tags, text = data.Text, cardId = data.DeckId });
            }
            catch (InvalidOperationException ex)
            {
                // If the file already exists, return a 409 Conflict status code
                return Conflict(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred: " + ex.Message });
            }
        }
    }
}
