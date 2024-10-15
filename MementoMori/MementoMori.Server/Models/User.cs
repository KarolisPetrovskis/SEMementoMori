namespace MementoMori.Server.Models
{
    public class User
    {
        public Guid Id { get; set; }

        public UserLoginData UserLoginData {get; set;}
    }
}
