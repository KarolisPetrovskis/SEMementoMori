using MementoMori.Server.DTOS;
using MementoMori.Server.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace MementoMori.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserDecksController: ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IDeckHelper _deckHelper;
        public UserDecksController(IAuthService authService, IDeckHelper deckHelper)
        {
            _authService = authService;
            _deckHelper = deckHelper;
        }
                [HttpGet("userInformation")]
        public async Task<ActionResult> userInformation()
        {
            var requesterId = _authService.GetRequesterId(HttpContext);
            if (requesterId != null)
            {
                var userDecks = await _deckHelper.GetUserDecks((Guid)requesterId);
                var userInfo = new UserDeckInformationDTO{
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