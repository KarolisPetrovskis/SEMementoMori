
using System.Text.Json;

namespace MementoMori.Server
{
    public class QuestComparer
    {
        public static async Task<bool> CompareQuestsAsync(string filePath)
        {
            try
            {
                // Read data from the JSON file
                string jsonString = await File.ReadAllTextAsync(filePath);
                var questData = JsonSerializer.Deserialize<Quest[]>(jsonString);

                // Create QuestRecord objects
                var questRecordFromData = new QuestRecord(questData[0].id, questData[0].valueNeeded);
                var questRecordFromVariables = new QuestRecord(1, 10); // Replace with your temporary variables

                // Compare objects
                var isComplete = questRecordFromData == questRecordFromVariables;

                return isComplete;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading quests file: {ex.Message}");
                return false;
            }
        }
    }
}