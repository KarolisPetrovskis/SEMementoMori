using MementoMori.Server.Models;

namespace MementoMori.Server 
{
    public interface ISpacedRepetition
    {
    void UpdateCard(UserCardData card, int quality);
    bool IsDueForReview(UserCardData card);
    }
}