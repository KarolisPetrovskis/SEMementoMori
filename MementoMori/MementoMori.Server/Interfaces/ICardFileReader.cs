namespace MementoMori.Server
{
    public interface ICardFileReader
    {
        Card[] ExtractCards(Guid deckId);
    }
}
