using MementoMori.Server.Data;
using Microsoft.AspNetCore.Mvc;

namespace MementoMori.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuestController : ControllerBase
    {
        private readonly Random _random;
        public QuestController()
        {
            _random = new Random();
        }

        [HttpGet("quests")]
        public IActionResult Quests()
        {
            while (TestQuests.Quests.Count < 3)
            {
                int index = _random.Next(QuestTemplates.Quests.Count);
                var newQuest = QuestTemplates.Quests[index];
                newQuest.Id = Guid.NewGuid();
                TestQuests.Quests.Add(newQuest);
            }

            return Ok(TestQuests.Quests);
        }

        [HttpPost("update")]
        public IActionResult UpdateQuest(Guid id, int newProgress)
        {
            var questIndex = TestQuests.Quests.FindIndex(q => q.Id == id);

            if (questIndex != -1)
            {
                var quest = TestQuests.Quests[questIndex];
                quest.Progress = newProgress;

                if (quest.Progress >= quest.Required)
                {
                    TestQuests.Quests.RemoveAt(questIndex);
                    return Ok(new { message = "Quest completed and removed." });
                }
                else
                {
                    TestQuests.Quests[questIndex] = quest;
                    return Ok(new { message = "Quest progress updated successfully", quest });
                }
            }
            else
            {
                return NotFound(new { message = "Quest not found" });
            }
        }

    }
}
