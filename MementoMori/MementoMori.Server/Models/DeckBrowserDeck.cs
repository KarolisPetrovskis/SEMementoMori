namespace MementoMori.Server
{
    public class DeckBrowserDeck
    {
        public required Guid Id { get; set; }

        public required string Title { get; set; }

        public double Rating { get; set; }

        public long RatingCount { get; set; }

        public required DateOnly Modified { get; set; }

        public required long Cards { get; set; }

        public string[]? Tags { get; set; }
    }
}