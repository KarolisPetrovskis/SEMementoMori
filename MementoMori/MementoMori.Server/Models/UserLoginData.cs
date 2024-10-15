namespace MementoMori.Server.Models
{
    public record UserLoginData
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
