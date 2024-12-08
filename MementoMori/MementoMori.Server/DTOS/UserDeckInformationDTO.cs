namespace MementoMori.Server.DTOS
{
    public class UserDeckInformationDTO
    {
        public UserDecksDTO[]? Decks { get; set; }

        public bool IsLoggedIn { get; set; }

    }
}
