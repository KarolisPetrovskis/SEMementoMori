

namespace MementoMori.Server 
{
    public interface ICardService
    {
        //Task<Card?> GetUserCardDataAsync(Guid userId, Guid deckId, Guid cardId);
        void UpdateSpacedRepetition(Guid userId, Guid deckId, Guid cardId, int quality);
        void AddCardsToCollection(Guid userId, Guid deckId);

        List<Card> GetCardsForReview(Guid deckId);
    }
}