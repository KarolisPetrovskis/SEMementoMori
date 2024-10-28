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
            // Generate a unique identifier for the card
            Guid cardId = Guid.NewGuid();

            // Change this if deckId is added as a shadow property

            // Set the SQL command to insert a new card record
            var sql = $@"
                INSERT INTO public.""Cards"" 
                (""Id"", ""Question"", ""Description"", ""Answer"", ""lastInterval"", ""nextShow"", ""DeckId"") 
                VALUES 
                (@cardId, @question, 'NULL', @text, NULL, NULL, @deckId)";
            // Execute the SQL command with parameters
            _context.Database.ExecuteSqlRaw(sql,
                new Npgsql.NpgsqlParameter("cardId", cardId),
                new Npgsql.NpgsqlParameter("question", question),
                new Npgsql.NpgsqlParameter("text", text),
                new Npgsql.NpgsqlParameter("deckId", deckId)
            );
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
