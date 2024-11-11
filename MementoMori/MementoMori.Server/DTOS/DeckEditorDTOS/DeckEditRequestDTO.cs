using MementoMori.Server.DTOS;
public class DeckEditRequestDTO
{
    public required DeckUpdateDTO EditedDeck { get; set; }
    public required DeckUpdateDTO OriginalDeck { get; set; }
}

