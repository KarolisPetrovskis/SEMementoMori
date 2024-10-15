namespace MementoMori.Server.Models
{
    public struct Quest
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }

        public int Progress { get; set; }

        public int Required { get; set; }
    }
}
