using System.ComponentModel.DataAnnotations;

namespace MementoMori.Server.Models
{
    public class User : DatabaseObject
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
