namespace MementoMori.Server
{

    public class Card
    {
        public required Guid Id { get; set; }

        public required string Header { get; set; }

        public string Description { get; set; }

        public required string Anwser { get; set; }

        public int priority { get; set; }

    }
}

