﻿namespace MementoMori.Server.Exceptions
{
    public class UnauthorizedEditingException : Exception
    {
        public UnauthorizedEditingException() : base("Editing was blocked due to lack of access") 
        {
        }
    }
}
