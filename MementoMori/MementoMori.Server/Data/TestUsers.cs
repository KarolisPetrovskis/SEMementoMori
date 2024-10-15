using System;
using System.Collections.Generic;
using MementoMori.Server.Models;

namespace MementoMori.Server.Data
{
    public static class TestUsers
    {
        public static List<User> Users = new List<User>
        {
            new User
            {
                Id = new Guid("d1f3f87e-85a1-4bde-9999-bff327fb26bb"),
                UserLoginData = new UserLoginData
                {
                    UserName = "john_doe",
                    Password = "password123"
                }
            },
            new User
            {
                Id = new Guid("aec3f12e-f7a4-4e44-8dff-91e4c2c938cb"),
                UserLoginData = new UserLoginData
                {
                    UserName = "jane_smith",
                    Password = "welcome456"
                }
            },
            new User
            {
                Id = new Guid("29f5a672-e4f8-4a96-91c6-6e8cbf14dbf7"),
                UserLoginData = new UserLoginData
                {
                    UserName = "bob_marley",
                    Password = "reggae789"
                }
            },
            new User
            {
                Id = new Guid("3c58cf85-8467-4be9-8f5d-7b58c2fc59a7"),
                UserLoginData = new UserLoginData
                {
                    UserName = "alice_wonder",
                    Password = "rabbitHole101"
                }
            },
            new User
            {
                Id = new Guid("14a8c3e9-cf65-4d0e-b154-313c013d846f"),
                UserLoginData = new UserLoginData
                {
                    UserName = "charlie_brown",
                    Password = "peanuts202"
                }
            }
        };
    }
}
