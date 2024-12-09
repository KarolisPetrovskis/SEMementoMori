namespace MementoMori.Server.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(Guid id) : base("This user does not exist") 
        {
        }
    }
}
