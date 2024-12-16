namespace MementoMori.Server.Models
{
    public class UserQuests : DatabaseObject
    {
        public required int QuestNr { get; set; }
        public Guid UserId { get; set; }
        public required int Progress { get; set; }


    }
}