using MementoMori.Server.DTOS;

namespace MementoMori.Server.Interfaces
{
    public interface IDeckHelper
    {
        List<Deck> Filter(Guid[]? ids = null, string? titleSubstring = null, string[]? selectedTags = null);
        void UpdateDeck(EditedDeckDTO editedDeckDTO, Guid editorId);
    }
}