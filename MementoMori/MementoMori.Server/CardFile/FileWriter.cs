using System;
using System.IO;
namespace MementoMori.Server
{
    public class FileWriter
    {
        private readonly string _directoryPath;

        // Constructor to initialize the directory where files will be stored
        public FileWriter()
        {
            // Get the current directory of the MementoMori.Server project
            string serverDirectory = Directory.GetCurrentDirectory();

            // Set the path to "CardFile" folder
            _directoryPath = Path.Combine(serverDirectory, "CardFile");

        }

        // Method to create a file using tags and text
        public void CreateFile(string tags, string text, string deckId)
        {
            // Use the specified file name "001.txt"
            string fileName = "001.txt";
            string filePath = Path.Combine(_directoryPath, fileName);
            Guid cardId = Guid.NewGuid();

            // Validate based on the whole direcotry or smth
            if (fileName != deckId + ".txt")
                throw new InvalidOperationException("Error: The deck file does not exist.");

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
                // Add change to the nr of cards, add extra card id to the line seperated by ';'
                string[] fileLines = File.ReadAllLines(filePath);
                fileLines[2] += $"; {cardId}";
                File.WriteAllLines(filePath, fileLines);

                string[] cardIds = fileLines[2].Substring(fileLines[2].IndexOf(':') + 1).Split(';', StringSplitOptions.RemoveEmptyEntries);
                fileLines[1] = "Number of cards: " + cardIds.Length;
                File.WriteAllLines(filePath, fileLines);
            }

            // Create the content to write to the file
            string fileContent = "(Start)\nCardId " + cardId + $"\nTags: {tags}\n\nText:\n{text}\n" + "\n(End)\n";

            try
            {
                // Write the content to the file
                using (StreamWriter sw = File.AppendText(filePath))
                {
                    sw.WriteLine(fileContent);
                }
                //Console.WriteLine($"File created successfully at {filePath}");
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during file writing
                Console.WriteLine($"Error creating file: {ex.Message}");
                throw; // Rethrow the exception to be caught by the controller
            }
        }
    }
}
