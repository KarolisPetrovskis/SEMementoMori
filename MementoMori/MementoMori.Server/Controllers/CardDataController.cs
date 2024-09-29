using Microsoft.AspNetCore.Mvc;

namespace MementoMori.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CardDataController : ControllerBase
    {
        [HttpPost("postCard")]
        //[Route("CardData")]
        public IActionResult PostInputData([FromBody] CardData data)
        {
            if (data == null || string.IsNullOrEmpty(data.Tags) || string.IsNullOrEmpty(data.Text))
            {
                return BadRequest("Invalid input data");
            }

            // Process the data (e.g., save to the database or handle the logic)
            Console.WriteLine($"Received Tags: {data.Tags}");
            Console.WriteLine($"Received Text: {data.Text}");

            // Return a success response
            return Ok(new { message = "Data received successfully. Tags:  " + data.Tags});


        }

   
    }
}
