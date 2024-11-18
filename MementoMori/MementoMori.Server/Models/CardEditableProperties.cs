namespace MementoMori.Server.Models
{
    public class CardEditableProperties : DatabaseObject
    {
        public required string Question { get; set; }

        public string? Description { get; set; }

        public required string Answer { get; set; }

        public int? lastInterval { get; set; }

        public DateOnly? nextShow { get; set; }
    }
}
