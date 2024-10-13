namespace MementoMori.Server
{
    public interface ICardFileReader
    {
        CardData[] ExtractCards(string filePath);
    }
}
