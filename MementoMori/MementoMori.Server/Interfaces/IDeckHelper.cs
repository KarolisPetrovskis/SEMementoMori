using MementoMori.Server.DTOS;
using MementoMori.Server.Models;

namespace MementoMori.Server.Interfaces
{
    public interface IDeckHelper
    {
        List<Deck> Filter(Guid[]? ids = null, string? titleSubstring = null, string[]? selectedTags = null);
        void UpdateDeck(EditedDeckDTO editedDeckDTO, Guid editorId);
        Guid CreateDeck (EditedDeckDTO createDeck, Guid requesterId);
        void DeleteDeck(Guid deckId);
    }
}