namespace MementoMori.Server
{
    public interface ICardFileReader
    {
        Card[] ExtractCards(string filePath);
    }
}
