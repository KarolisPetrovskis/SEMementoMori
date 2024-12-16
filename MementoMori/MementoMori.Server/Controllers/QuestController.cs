using MementoMori.Server.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MementoMori.Server.Database;
using Microsoft.EntityFrameworkCore;


namespace MementoMori.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuestController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IAuthService _authService;

        public QuestController(AppDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        [HttpGet("getQuests")]
        public async Task<IActionResult> GetQuests()
        {
            try
            {
                var userId = _authService.GetRequesterId(HttpContext);

                if (userId == null)
                {
                    return Unauthorized(new { Message = "User is not authenticated." });
                }

                var userQuests = await _context.UserQuests
                    .Where(uq => uq.UserId == userId)
                    .Join(
                        _context.Quests,
                        uq => uq.QuestNr,
                        q => q.Nr,
                        (uq, q) => new
                        {
                            Number = q.Nr,
                            Title = q.Title,
                            Description = q.Description,
                            Progress = uq.Progress,
                            Required = q.Required,
                            Reward = q.Reward
                        }
                    )
                    .ToListAsync();

                return Ok(userQuests);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}\n{ex.StackTrace}");

                return StatusCode(500, new { Message = "An error occurred while fetching quests." });
            }
        }

    }
}
