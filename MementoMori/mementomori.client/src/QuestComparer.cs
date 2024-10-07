using System.Text.Json;
using axios;

record QuestRecord
{
    public int Id { get; set; }
    public int ValueNeeded { get; set; }
}

public class QuestComparer
{
    public static async Task<bool> CompareQuestsAsync(string jsonUrl)
    {
        try
        {
            // Fetch data using axios
            var response = await axios.get(jsonUrl);
            var questData = JsonSerializer.Deserialize<Quest[]>(response.data);

            // Create QuestRecord objects
            var questRecordFromData = new QuestRecord(questData[0].Id, questData[0].ValueNeeded);
            var questRecordFromVariables = new QuestRecord(2, 10); // Replace with your temporary variables

            // Compare objects
            var isComplete = questRecordFromData == questRecordFromVariables;

            return isComplete;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching quests: {ex.Message}");
            return false;
        }
    }
}