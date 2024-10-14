using System.Collections.Generic;
using System.IO;
using MementoMori.Server;
namespace MementoMori.Server
{
    public class CardFileReader : ICardFileReader
    {
        public Card[] ExtractCards(string filePath)
        {
            // Read all lines into a list first
            List<string> linesList = new List<string>();
            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    while (!reader.EndOfStream)
                    {
                        linesList.Add(reader.ReadLine());
                    }
                }
            }
            catch (Exception ex)
            {
                return [];
            }
            // Convert list to array
            string[] fileContent = linesList.ToArray();
            Guid deckId = new Guid (fileContent[0].Split(':')[1].Trim()); // Extract DeckId from the first line
            string[] parts = fileContent[1].Split(':');
            int numberOfCards = int.Parse(parts[1].Trim());

            Card[] cards = new Card[numberOfCards];
            int cardCount = 0;
            bool isCard = false;
            bool isQuestion = false;
            bool isAnswer = false;
            string question = "";
            string text = "";
            bool skipNextLine = false;

            foreach (var line in fileContent)
            {      
                if (line.Contains("(Start)"))
                {
                    isCard = true;
                    question = ""; // Reset question content
                    text = ""; // Reset answer content
                    isQuestion = false;
                    isAnswer = false;
                    skipNextLine = true; // Skip the CardId line
                    continue;
                }

                if (skipNextLine)
                {
                    // Skip the cardId line
                    skipNextLine = false;
                    continue;
                }

                if (line.Contains("(End)"))
                {
                    isCard = false;
                    isQuestion = false;
                    isAnswer = false;

                    cards[cardCount] = new Card
                    {
                        Id = deckId,
                        Question = question,
                        Answer = text
                    };

                    cardCount++;

                    if (cardCount >= numberOfCards)
                    {
                        break;
                    }
                }

                if (isCard)
                {
                    if (line.Contains("(Question)"))
                    {
                        isQuestion = true;
                        isAnswer = false;
                        continue;
                    }
                    if (line.Contains("(Answer)"))
                    {
                        isQuestion = false;
                        isAnswer = true;
                        continue;
                    }
                    if (isQuestion)
                    {
                        question += line + "\n";
                    }
                    if (isAnswer)
                    {
                        text += line + "\n";
                    }
                }
            }

            return cards;
        }
    }
}