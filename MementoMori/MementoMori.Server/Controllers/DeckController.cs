using Microsoft.AspNetCore.Mvc;
using MementoMori.Server.DTOS;
using MementoMori.Server.Extensions;
using MementoMori.Server.Service;
using Microsoft.VisualBasic;
using System.Collections.Concurrent;
using MementoMori.Server.Interfaces;

namespace MementoMori.Server.Controllers
{
    [ApiController]
    [Route("[controller]/{deckId}")]
    public class DecksController(IDeckHelper deckHelper, IAuthService authService, ICardService cardService, 
    ConcurrentQueue<(Guid UserId, Guid DeckId, Guid CardId, int Quality)> updateQueue) : ControllerBase
    {
        private readonly IDeckHelper _deckHelper = deckHelper;
        private readonly IAuthService _authService = authService;
        private readonly ICardService _cardService = cardService;
        private readonly ConcurrentQueue<(Guid UserId, Guid DeckId, Guid CardId, int Quality)> _updateQueue = updateQueue;

        [HttpGet("deck")]
        public IActionResult View(Guid deckId) {

            if (deckId == Guid.Empty)
            {
                return BadRequest(new { errorCode = ErrorCode.InvalidInput, message = "Invalid deck ID." });
            }

            var deck = _deckHelper.Filter(ids: [deckId]).FirstOrDefault();

            if (deck == null)
                return NotFound("Deck not found.");

            var requesterId = _authService.GetRequesterId(HttpContext); 

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
        public IActionResult EditorView(Guid deckId) 
        {

            if (deckId == Guid.Empty)
            {
                return BadRequest(new { errorCode = ErrorCode.InvalidInput, message = "Invalid deck ID." });
            }

            var deck = _deckHelper.Filter(ids: [deckId]).FirstOrDefault();

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
        public IActionResult GetDueCards(Guid deckId)
        {
            try
            {
                Guid? userId = _authService.GetRequesterId(HttpContext);

                if (deckId == Guid.Empty || userId == Guid.Empty)
                {
                    return BadRequest(new { errorCode = "InvalidInput", message = "Invalid deck or user ID." });
                }

                List<Card> dueForReviewCards = _cardService.GetCardsForReview(deckId, userId.Value);

                if (!dueForReviewCards.Any())
                {
                    return NotFound("No cards due for review.");
                }

                var dueCardDtos = dueForReviewCards.Select(c => new CardDTO
                {
                    Id = c.Id,
                    Question = c.Question,
                    Description = c.Description,
                    Answer = c.Answer
                }).ToList();
                
                return Ok(dueCardDtos);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"Error in GetDueCards: {ex.Message} - {ex.StackTrace}");
                return StatusCode(500, new { errorCode = "ServerError", message = "An unexpected error occurred." });
            }
        }

        [HttpPost("addToCollection")]
        public IActionResult AddCardsToCollection(Guid deckId)
        {
            Guid? userId = _authService.GetRequesterId(HttpContext);

            if(deckId == Guid.Empty || userId == Guid.Empty || userId == null )
            {
                return BadRequest(new { errorCode = "InvalidInput", message = "Invalid deck or user ID." });
            }
            if(userId != null)
                _cardService.AddCardsToCollection(userId.Value, deckId); 

            return Ok(new { message = "Deck successfully added to user's collection." });
        }
        
        [HttpPost("cards/update/{cardId}")]
        public IActionResult UpdateCard(Guid deckId, Guid cardId, [FromBody] int quality)
        {
            Guid? userId = _authService.GetRequesterId(HttpContext);

            if (deckId == Guid.Empty || userId == Guid.Empty || cardId == Guid.Empty)
            {
                return BadRequest(new { errorCode = "InvalidInput", message = "Invalid deck, card or user ID." });
            }
            
            if(userId != null)
                _updateQueue.Enqueue((userId.Value, deckId, cardId, quality));
            
            return Ok(new {message = "Card updated successfully"});
        }
    }
}
