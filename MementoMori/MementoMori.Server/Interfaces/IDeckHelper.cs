namespace MementoMori.Server.Interfaces
{
    public interface IDeckHelper
    {
        List<Deck> Filter(Guid[]? ids = null, string? titleSubstring = null, string[]? selectedTags = null);
    }
}