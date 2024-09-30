namespace MementoMori.Server
{
    public struct DeckTag
    {
        public const string Biology = "Biology";
        public const string History = "History";
        public const string Languages = "Languages";
        public const string Physics = "Physics";
        public const string Mathematics = "Mathematics";
        public const string Chemistry = "Chemistry";
        public const string Geography = "Geography";
        public const string Literature = "Literature";
        public const string ComputerScience = "Computer Science";
        public const string Art = "Art";
        public const string Music = "Music";
        public const string Philosophy = "Philosophy";
        public const string Economics = "Economics";
        public const string Psychology = "Psychology";
        public const string Beginner = "Beginner";
        public const string Intermediate = "Intermediate";
        public const string Advanced = "Advanced";
        public const string Expert = "Expert";
    }

    public class Deck : IComparable<Deck>
    {
        public required Guid Id { get; set; }

        public required Guid creatorId { get; set; }

        public required bool isPublic { get; set; }

        public required string Title { get; set; }

        public double Rating { get; set; }

        public long RatingCount { get; set; }

        public required DateOnly Modified { get; set; }

        public required long CardCount { get; set; }

        public string[]? Tags { get; set; }

        public Card[]? Cards { get; set; }


        public int CompareTo(Deck? other)
        {
            if (other == null) return 1;
            return (Rating * RatingCount).CompareTo(other.Rating * other.RatingCount);
        }
    }
}