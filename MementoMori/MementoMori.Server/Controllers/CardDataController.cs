using Microsoft.AspNetCore.Mvc;
namespace MementoMori.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CardDataController : ControllerBase
    {
        [HttpPost("createCard")]
        public IActionResult PostInputData([FromBody] CardData data)
        {
            if (data == null || string.IsNullOrEmpty(data.Tags) || string.IsNullOrEmpty(data.Text))
            {
                return BadRequest("Invalid input data");
            }

            // Process the data (e.g., save to the database or handle the logic)

            try
            {
                // Create an instance of the FileWriter (no need to pass path now)
                FileWriter fileWriter = new FileWriter();

                // Attempt to create the file with the posted tags and text
                fileWriter.CreateFile(data.Tags, data.Text, data.DeckId); ;

                // Return a success response if the file was created successfully
                return Ok(new { message = "Data received successfully", tags = data.Tags, text = data.Text, cardId = data.DeckId });
            }
            catch (InvalidOperationException ex)
            {
                // If the file already exists, return a 409 Conflict status code
                return Conflict(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                // Handle any other exceptions that might occur
                return StatusCode(500, new { error = "An unexpected error occurred: " + ex.Message });
            }

        }


    }
}
