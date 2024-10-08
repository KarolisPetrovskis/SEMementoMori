namespace MementoMori.Server
{
	public class CardFileReader
    {
		private string _filePath;
		public CardFileReader(string _filePath)
		{
			this._filePath = _filePath;
		}

		public string[] ExtractCards()
		{
			// Read all lines into a list first
			List<string> linesList = new List<string>();

			using (StreamReader reader = new StreamReader(_filePath))
			{
				while (!reader.EndOfStream)
				{
					linesList.Add(reader.ReadLine());
				}
			}

			// Convert list to array
			string[] fileContent = linesList.ToArray();
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
						// Stop if we have extracted the expected number of cards
						break; 
					}
				}
			}

			return cards;
		}
	}
}
