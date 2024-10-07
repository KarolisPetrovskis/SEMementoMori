using Microsoft.AspNetCore.Mvc;
using MementoMori.Server;

[ApiController]
[Route("[controller]")]
public class QuestController : ControllerBase
{
    [HttpGet("isComplete")]
    public async Task<IActionResult> GetIsComplete()
    {
        try
        {
            var isComplete = await QuestComparer.CompareQuestsAsync("MementoMori/MementoMori.Server/quests.json");
            return Ok(isComplete); // Return the isComplete value
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching quests: {ex.Message}");
            return StatusCode(500); // Internal Server Error
        }
    }
}