﻿namespace MementoMori.Server
{

    public class Card
    {
        public required Guid Id { get; set; }

        public required string Question { get; set; }

        public string Description { get; set; }

        public required string Answer { get; set; }

        public int lastInterval { get; set; }

        public DateOnly nextShow { get; set; }

    }
}

