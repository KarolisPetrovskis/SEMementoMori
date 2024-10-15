﻿namespace MementoMori.Server.DTOS
{
    public class DeckDTO
    {
        public required Guid Id { get; set; }

        public required Guid creatorId { get; set; }

        public required string Title { get; set; }

        public string? Description { get; set; }

        public double Rating { get; set; }

        public required DateOnly Modified { get; set; }

        public required long CardCount { get; set; }

        public string[]? Tags { get; set; }

    }
}
