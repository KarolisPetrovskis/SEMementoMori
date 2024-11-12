using MementoMori.Server.DTOS.DeckEditorDTOS;
namespace MementoMori.Server.DTOS
{
    public class DeckUpdateDTO
    {
        public required Guid Id { get; set; }

        public required bool isPublic { get; set; }

        public required string Title { get; set; }

        public string? Description { get; set; }

        public required long CardCount { get; set; }

        public List<string>? Tags { get; set; }

        public DeckUpdateCards[]? Cards { get; set; }
    }
}
