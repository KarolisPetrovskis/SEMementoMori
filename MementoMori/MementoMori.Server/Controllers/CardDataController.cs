using Microsoft.AspNetCore.Mvc;
using MementoMori.Server.Extensions;
using MementoMori.Server.Interfaces;
namespace MementoMori.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CardDataController : ControllerBase
    {
        private readonly IDatabaseCardWriter _databaseCardWriter;
        public CardDataController(IDatabaseCardWriter databaseCardWriter)
        {
            _databaseCardWriter = databaseCardWriter ?? throw new ArgumentNullException(nameof(databaseCardWriter));
        }

        [HttpPost("editDeck")]
        public IActionResult UpdateDeck([FromBody] DeckEditRequestDTO request)
        {
            try
            {
                if (!request.IsValid())
                {
                    return BadRequest("Invalid input data");
                }

                _databaseCardWriter.UpdateDeck(request);
                return Ok(request);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error =  "An error occurred while updating the deck. " + ex.Message});
            }
        }
    }
}
