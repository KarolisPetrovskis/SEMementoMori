namespace MementoMori.Server.Models
{
    public class User : DatabaseObject
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public override bool CanEdit(Guid editorId)
        {
            return Id == editorId;
        }
    }
}
