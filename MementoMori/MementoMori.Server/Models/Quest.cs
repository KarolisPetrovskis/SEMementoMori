namespace MementoMori.Server.Models
{
    public class Quest : DatabaseObject
    {
        public required int Nr { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required int Required { get; set; }
        public required string Reward { get; set; }

    }
}