using Microsoft.AspNetCore.Mvc;
using MementoMori.Server.DTOS;
using MementoMori.Server.Extensions;
using MementoMori.Server.Interfaces;
using MementoMori.Server.Exceptions;

namespace MementoMori.Server.Controllers
{
    [ApiController]
    [Route("[controller]/{deckId}")]
    public class DecksController(IDeckHelper deckHelper, IAuthService authService) : ControllerBase
    {
        private readonly IDeckHelper _deckHelper = deckHelper;
        private readonly IAuthService _authService = authService;

        [HttpGet("deck")]
        public async Task<ActionResult> ViewAsync(Guid deckId)
        {

            if (deckId == Guid.Empty)
            {
                return BadRequest(new { errorCode = ErrorCode.InvalidInput, message = "Invalid deck ID." });
            }

            var requesterId = _authService.GetRequesterId(HttpContext);

            var deck = (await _deckHelper.Filter(ids: [deckId], userId: requesterId)).FirstOrDefault();

            if (deck == null)
                return NotFound("Deck not found.");

            var DeckDTO = new DeckDTO
            {
                Id = deck.Id,
                CreatorName = deck.Creator?.Username ?? "deleted",
                CardCount = deck.CardCount,
                Modified = deck.Modified,
                Rating = deck.Rating,
                Tags = deck.TagsToString(),
                Title = deck.Title,
                Description = deck.Description,
                IsOwner = requesterId != null && requesterId == deck.Creator?.Id,
            };
            return Ok(DeckDTO);
        }

        [HttpGet("EditorView")]
        public async Task<ActionResult> EditorViewAsync(Guid deckId)
        {

            if (deckId == Guid.Empty)
            {
                return BadRequest(new { errorCode = ErrorCode.InvalidInput, message = "Invalid deck ID." });
            }

            var requesterId = _authService.GetRequesterId(HttpContext);

            var deck = (await _deckHelper.Filter(ids: [deckId], userId: requesterId)).FirstOrDefault();

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
        public async Task<ActionResult> GetCards(Guid deckId)
        {

            if (deckId == Guid.Empty)
            {
                return BadRequest(new { errorCode = ErrorCode.InvalidInput, message = "Invalid deck ID." });
            }

            var deck = (await _deckHelper.Filter(ids: [deckId])).First();

            if (deck == null)
                return NotFound("Deck not found.");


            var Cards = deck.Cards.Select(Card => new CardDTO
            {
                Id = Card.Id,
                Question = Card.Question,
                Description = Card.Description,
                Answer = Card.Answer,

            }).ToList();

            return Ok(Cards);

        }

        [HttpPost("editDeck")]
        public async Task<ActionResult> EditDeck(EditedDeckDTO editedDeckDTO)
        {
            var requesterId = _authService.GetRequesterId(HttpContext);
            if (requesterId == null)
                return Unauthorized();
            try
            {
                await _deckHelper.UpdateDeckAsync(editedDeckDTO, (Guid)requesterId);
            }
            catch (UnauthorizedEditingException)
            {
                return Unauthorized();
            }
            return Ok();
        }
        
        [HttpPost("createDeck")]
        public async Task<ActionResult<Guid>> CreateDeck(EditedDeckDTO createDeckDTO)
        {
            var requesterId = _authService.GetRequesterId(HttpContext);
            if (requesterId == null)
                return Unauthorized();
            try
            {
                var newDeckId = await _deckHelper.CreateDeckAsync(createDeckDTO, (Guid)requesterId);
                return Ok(newDeckId);
            }
            catch
            {
                return StatusCode(500);
            }
        }
        [HttpPost("deleteDeck")]
        public async Task<ActionResult> DeleteDeck(Guid deckId)
        {
            var requesterId = _authService.GetRequesterId(HttpContext);
            try
            {
                if (requesterId != null)
                {
                    await _deckHelper.DeleteDeckAsync(deckId, (Guid)requesterId);
                    return Ok();
                }
                else
                    return Unauthorized();

            }
            catch
            {
                return StatusCode(500);
            }
        }
    }
}