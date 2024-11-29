﻿namespace MementoMori.Server.Models
{
    public class User : DatabaseObject
    {
        public required string Username { get; set; }
        public required string Password { get; set; }

        public override bool CanEdit(Guid editorId)
        {
            return Id == editorId;
        }
    }
}
