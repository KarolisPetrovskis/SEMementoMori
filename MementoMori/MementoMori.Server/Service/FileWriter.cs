using MementoMori.Server.Database;
using Microsoft.EntityFrameworkCore;
namespace MementoMori.Server.Service
{
    public class FileWriter
    {
        private readonly string _directoryPath;
        private readonly AppDbContext _context;
        public FileWriter(AppDbContext context)
        {
            string serverDirectory = Directory.GetCurrentDirectory();
            // Set the path to "CardFile" folder
            _directoryPath = Path.Combine(serverDirectory, "CardFile");
            _context = context;
        }

        public void CreateFile(string question, string text, Guid deckId)
        {
            //Guid cardId = Guid.NewGuid();
            Deck deck = _context.Decks.SingleOrDefault(c => c.Id == deckId);

            // Create a new card entity
            var newCard = new Card
            {
                Id = Guid.NewGuid(),
                Question = question,
                Description = "NULL",
                Answer = text,
                lastInterval = null,
                nextShow = null
            };
            if(deck != null)
            {
                deck.Cards.Add(newCard)  ;

                // Add the new card to the context
                _context.Decks.Add(deck);

                // Save changes to insert the new card record
                _context.SaveChanges();
            }
        }

        // If you do not want to update a value pass 'null'. CardId is mandatory
        public void UpdateCardData(Guid cardId, string? question, string? description, string? answer, int? lastInterval, DateOnly? nextShow)
        {
            var card = _context.Cards.SingleOrDefault(c => c.Id == cardId);
                if (card != null) // Check if the card exists
            {
                // Update properties
                if (question != null)
                {
                    card.Question = question;
                }
                if (description != null)
                {
                    card.Description = description;
                }
                if (answer != null)
                {
                    card.Answer = answer;
                }
                if (lastInterval != null)
                {
                    card.lastInterval = lastInterval;
                }
                if (nextShow != null)
                {
                    card.nextShow = nextShow;
                }
                // Save changes to the database
                _context.SaveChanges();
            }
        }

    }
}
