using MementoMori.Server.DTOS;
using MementoMori.Server.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace MementoMori.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserDecksController(IDeckHelper deckHelper, IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        private readonly IDeckHelper _deckHelper = deckHelper;

        [HttpGet("userInformation")]
        public async Task<ActionResult> UserInformation()
        {
            var requesterId = _authService.GetRequesterId(HttpContext);
            if (requesterId != null)
            {
                var userDecks = await _deckHelper.GetUserDecks((Guid)requesterId);
                var userInfo = new UserDeckInformationDTO
                {
                    Decks = userDecks ?? [],
                    IsLoggedIn = true,
                };

                return Ok(userInfo);
            }
            else
            {
                var userDecks = new UserDeckInformationDTO{Decks = null, IsLoggedIn = false};
                return Ok(userDecks);
            }
        }
    }
}