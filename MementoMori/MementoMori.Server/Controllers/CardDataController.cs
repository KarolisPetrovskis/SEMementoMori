using Microsoft.AspNetCore.Mvc;
using MementoMori.Server.DTOS;
using MementoMori.Server.Service;
using MementoMori.Server.Extensions;

using System.Diagnostics;

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
