using MementoMori.Server.Models;
using System.Collections.Generic;

namespace MementoMori.Server.Data
{
    public class QuestTemplates
    {
        public static List<Quest> Quests = new List<Quest>
        {
            new Quest
            {
                Title = "Flashcard Mastery",
                Description = "Complete 50 flashcards to strengthen your memory retention.",
                Progress = 0,
                Required = 50
            },
            new Quest
            {
                Title = "Deck Creator",
                Description = "Create your own custom deck and add at least 10 cards to it.",
                Progress = 0,
                Required = 1
            },
            new Quest
            {
                Title = "Daily Reviewer",
                Description = "Review any deck daily for 7 consecutive days.",
                Progress = 0,
                Required = 7
            },
            new Quest
            {
                Title = "Accuracy Master",
                Description = "Achieve an 80% accuracy rate in a single study session.",
                Progress = 0,
                Required = 1
            },
        };
    }
}
