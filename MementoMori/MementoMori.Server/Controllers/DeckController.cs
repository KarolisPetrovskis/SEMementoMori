using Microsoft.AspNetCore.Mvc;
using MementoMori.Server.DTOS;
using MementoMori.Server.Extensions;
using MementoMori.Server.Service;
using MementoMori.Server.Database;
using MementoMori.Server.Models;

namespace MementoMori.Server.Controllers
{
    [ApiController]
    //[Route("[controller]")]
    [Route("[controller]/{deckId}")]
    public class DecksController : ControllerBase
    {
        private readonly DeckHelper _deckHelper;
        private readonly AuthService _authService;
        private readonly AppDbContext _context;
        private readonly ISpacedRepetition _spacedRepetition;

        public DecksController(DeckHelper deckHelper, AppDbContext context, ISpacedRepetition spacedRepetition, AuthService authService)
        {
            _authService = authService;
            _context = context;
            _deckHelper = deckHelper;
            _spacedRepetition = spacedRepetition;
        }

        [HttpGet("deck")]
        public IActionResult View(Guid deckId) {

            if (deckId == Guid.Empty)
            {
                return BadRequest(new { errorCode = ErrorCode.InvalidInput, message = "Invalid deck ID." });
            }

            var Deck = _deckHelper.Filter(ids: [deckId]).FirstOrDefault();

            if (Deck == null)
                return NotFound("Deck not found.");

            var DeckDTO = new DeckDTO
            {
                Id = Deck.Id,
                creatorId = Deck.creatorId,
                CardCount = Deck.CardCount,
                Modified = Deck.Modified,
                Rating = Deck.Rating,
                Tags = Deck.TagsToString(),
                Title = Deck.Title,
                Description = Deck.Description
                
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
        //change to work with database and use IsDueForReview()
        [HttpGet("cards")]
        public async Task<IActionResult> GetDueCards(Guid deckId)
        {

            Guid userId = _authService.getUserId(HttpContext);

            if (deckId == Guid.Empty || userId == Guid.Empty)
            {
                return BadRequest(new { errorCode = "InvalidInput", message = "Invalid deck or user ID." });
            }

            // Fetch UserCardData entries for the specified deckId and userId
            var userCards = _context.UserCards.Where(card => card.DeckId == deckId && card.UserId == userId).ToList();

            // Filter cards due for review
            var dueForReviewCards = userCards
                .Where(card => _spacedRepetition.IsDueForReview(card))
                .ToList();

            if (!dueForReviewCards.Any())
            {
                return NotFound("No cards due for review.");
            }

            return Ok(dueForReviewCards);
        }
        // POST: /Deck/{deckId}/cards/update/{cardId}

        [HttpPost("deck/addToCollection/{deckId}")]
        public async Task<IActionResult> AddToCollection(Guid deckId)
        {
            Guid userId = _authService.getUserId(HttpContext);

            if(deckId == Guid.Empty || userId == Guid.Empty)
            {
                return BadRequest(new { errorCode = "InvalidInput", message = "Invalid deck or user ID." });
            }

            var deck = _deckHelper.Filter(ids: [deckId]).FirstOrDefault();

            if (deck == null)
                return NotFound("Deck not found.");
            
            foreach(var card in deck.Cards)
            {
                var existingUserCard = _context.UserCards
                .FirstOrDefault(uc => uc.DeckId == deckId && uc.CardId == card.Id && uc.UserId == userId);
                var userCardData = new UserCardData
                    {
                        CardId = card.Id,
                        DeckId = deckId,
                        UserId = userId,
                        Interval = 1,
                        Repetitions = 0,
                        EaseFactor = 2.5,
                        LastReviewed = DateTime.Now
                    };

                await _context.UserCards.AddAsync(userCardData);            
            }
             await _context.SaveChangesAsync();

            return Ok(new { message = "Deck successfully added to user's collection." });
        }
        [HttpPost("cards/update/{cardId}")]
        public async Task<IActionResult> UpdateCard(Guid deckId, Guid cardId, [FromBody] int quality)
        {
            Guid userId = _authService.getUserId(HttpContext);

            if (deckId == Guid.Empty || userId == Guid.Empty || cardId == Guid.Empty)
            {
                return BadRequest(new { errorCode = "InvalidInput", message = "Invalid deck, card or user ID." });
            }

            var userCardData = _context.UserCards
            .FirstOrDefault(uc => userId == uc.UserId && deckId == uc.DeckId && cardId == uc.CardId);

            if (userCardData == null)
            {
                return NotFound(new { errorCode = "CardNotFound", message = "Card data not found for the specified user, deck, and card ID." });
            }

            _spacedRepetition.UpdateCard(userCardData, quality);

            userCardData.LastReviewed = DateTime.Now;
            _context.UserCards.Update(userCardData);
            await _context.SaveChangesAsync();

            return Ok(new {message = "Card updated successfully"});
        }

    }
}
