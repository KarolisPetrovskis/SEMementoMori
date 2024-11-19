using Microsoft.Extensions.Hosting;
using System.Collections.Concurrent;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MementoMori.Server.Services
{
    public class CardUpdateProcessor
{
    private readonly IServiceProvider _serviceProvider;

    public CardUpdateProcessor(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void ProcessQueue(ConcurrentQueue<(Guid UserId, Guid DeckId, Guid CardId, int Quality)> queue)
    {
        while (queue.TryDequeue(out var item))
        {
            using var scope = _serviceProvider.CreateScope();
            var cardService = scope.ServiceProvider.GetRequiredService<ICardService>();

            try
            {
                cardService.UpdateSpacedRepetition(item.UserId, item.DeckId, item.CardId, item.Quality);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to process update: {ex.Message}");
            }
        }
    }
}

}
