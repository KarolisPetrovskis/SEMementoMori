using MementoMori.Server.DTOS;
namespace MementoMori.Server.Extensions
{
    public static class DeckEditRequestDTODataValidation
    {
        public static bool IsValid(this DeckEditRequestDTO data)
        {
            return !(data == null || string.IsNullOrEmpty(data.EditedDeck.Title) || string.IsNullOrEmpty(data.OriginalDeck.Title));
        }
    }
}