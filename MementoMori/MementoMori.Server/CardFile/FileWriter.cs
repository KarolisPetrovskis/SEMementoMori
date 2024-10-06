using System;
using System.IO;
namespace MementoMori.Server
{
    public class FileWriter
    {
        private readonly string _directoryPath;

        public FileWriter()
        {
            string serverDirectory = Directory.GetCurrentDirectory();
            // Set the path to "CardFile" folder
            _directoryPath = Path.Combine(serverDirectory, "CardFile");

        }

        // Method to create a file using tags and text
        public void CreateFile(string[]? tags, string text, string deckId)
        {
            string fileName= deckId + ".txt";
            string filePath = Path.Combine(_directoryPath, fileName);
            Guid cardId = Guid.NewGuid();

            if(!File.Exists(filePath))
            {
                using (FileStream fs = File.Create(filePath))
                {
                }
            }


            var fileInfo = new FileInfo(filePath);
            string tagsLine = "";
            if (fileInfo.Length == 0)
            {
                using (StreamWriter sw = File.AppendText(filePath))
                {
                    sw.WriteLine("DeckId: " + deckId);
                    sw.WriteLine("Number of cards: " + 1);
                    sw.WriteLine("CardIds: " + cardId + "\n");
                }
            }

            // Create the content to write to the file
            string fileContent = "(Start)\nCardId " + cardId + $"\nTags:{tagsLine}\n\nText:\n{text}\n" + "\n(End)\n";

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
