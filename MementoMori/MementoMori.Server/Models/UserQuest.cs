namespace MementoMori.Server.Models
{
    public class UserQuest : DatabaseObject
    {
        public required int QuestNr { get; set; }
        public Guid UserId { get; set; }
        public required int Progress { get; set; }


    }
}