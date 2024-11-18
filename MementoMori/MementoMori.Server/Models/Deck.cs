using MementoMori.Server.Models;
using System.ComponentModel.DataAnnotations;

namespace MementoMori.Server
{
    public class Deck : IComparable<Deck>
    {
        [Key]
        public required Guid Id { get; set; }

        public User? Creator { get; set; }

        public required bool isPublic { get; set; }

        public required string Title { get; set; }

        public string? Description { get; set; }

        public double Rating { get; set; }

        public long RatingCount { get; set; }

        public required DateOnly Modified { get; set; }

        public required long CardCount { get; set; }

        public List<TagTypes>? Tags { get; set; }

        public List<Card> Cards { get; set; }


        public int CompareTo(Deck? other)
        {
            if (other == null) return 1;
            return (Rating * RatingCount).CompareTo(other.Rating * other.RatingCount);
        }
    }
}