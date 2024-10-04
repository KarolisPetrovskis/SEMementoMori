using System.IO;
namespace MementoMori.Server
{
	public class CardFileDataReturner
	{
		private string _filePath;
		public CardFileDataReturner(string _filePath)
		{
			this._filePath = _filePath;
		}

		public string[] ExtractCards()
		{
			string[] fileContent = System.IO.File.ReadAllLines(_filePath);


			// May want to validate it (probably no need unlesss someone goes into files to delete things
			string[] parts = fileContent[1].Split(':');
			int numberOfCards = int.Parse(parts[1].Trim());

			string[] cards = new string[numberOfCards];
			int cardCount = 0;
			bool isCard = false;
			string card = "";
			bool skipNextLine = false;

			foreach (var line in fileContent)
			{
				if (line.Contains("(Start)"))
				{
					isCard = true;
					card = "";
					skipNextLine = true;
					continue;
				}

				if (skipNextLine)
				{
					// Skip the cardId
					skipNextLine = false;
					continue;
				}

				if (isCard)
				{
					if (!line.Contains("(End)"))
					{
						// Add all lines except '(Start)', '(End)', and the cardId line
						card += line + "\n";
					}
				}

				if (line.Contains("(End)"))
				{
					isCard = false;
					// Push the card into the array after the "(End)" line
					cards[cardCount] = card.Trim();
					cardCount++;

					if (cardCount >= numberOfCards)
					{
						break; // Stop if we have extracted the expected number of cards
					}
				}
			}

			return cards;
		}

	}
}