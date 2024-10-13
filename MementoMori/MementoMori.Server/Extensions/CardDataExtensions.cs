using MementoMori.Server;
namespace MementoMori.Server.Extensions
{
    public static class CardDataExtensions
    {
        public static bool IsValid(this CardData data)
        {
            return !(data == null || string.IsNullOrEmpty(data.Answer) || string.IsNullOrEmpty(data.Question));
        }
    }
}