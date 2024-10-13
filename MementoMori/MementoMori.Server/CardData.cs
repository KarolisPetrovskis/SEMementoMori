namespace MementoMori.Server
{
    public class CardData
    {
        public string DeckId { get; set; }
        public required string Question { get; set; }
        public required string Answer { get; set; }
    }

}
