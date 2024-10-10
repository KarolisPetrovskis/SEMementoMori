namespace MementoMori.Server
{
    public interface ICardFileReader
    {
        string[] ExtractCards(string filePath);
    }
}
