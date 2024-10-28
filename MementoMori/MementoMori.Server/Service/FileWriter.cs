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

        // Method to create a file using tags and text
        public void oldCreateFile(string question, string text, string deckId)
        {
            string fileName = deckId + ".txt";
            string filePath = Path.Combine(_directoryPath, fileName);
            Guid cardId = Guid.NewGuid();

            if (!File.Exists(filePath))
            {
                using (FileStream fs = File.Create(filePath))
                {
                }
            }


            var fileInfo = new FileInfo(filePath);
            if (fileInfo.Length == 0)
            {
                using (StreamWriter sw = File.AppendText(filePath))
                {
                    sw.WriteLine("DeckId: " + deckId);
                    sw.WriteLine("Number of cards: " + 1);
                    sw.WriteLine("CardIds: " + cardId + "\n");
                }
            }
            else
            {
                // Add change to the nr of cards, add extra card id to the line separated by ';'
                string[] fileLines = File.ReadAllLines(filePath);
                fileLines[2] += $"; {cardId}";
                string[] cardIds = fileLines[2].Substring(fileLines[2].IndexOf(':') + 1).Split(';', StringSplitOptions.RemoveEmptyEntries);
                fileLines[1] = "Number of cards: " + cardIds.Length;
                File.WriteAllLines(filePath, fileLines);
            }

            // Create the content to write to the file
            string fileContent = "(Start)\nCardId " + cardId + $"\n(Question)\n{question}\n(Answer)\n{text}\n" + "\n(End)\n";

            try
            {
                // Write the content to the file
                using (StreamWriter sw = File.AppendText(filePath))
                {
                    sw.WriteLine(fileContent);
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during file writing
                Console.WriteLine($"Error creating file: {ex.Message}");
                // Rethrow the exception to be caught by the controller
                throw;
            }
        }
    }
}
