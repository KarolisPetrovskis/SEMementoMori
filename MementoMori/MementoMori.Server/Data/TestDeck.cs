using MementoMori.Server.Models;
using System;
using System.Collections.Generic;

namespace MementoMori.Server
{

    public class TestDeck
    {
        public static List<Deck> Decks = new List<Deck>
        {
            new Deck
            {
                Id = Guid.NewGuid(),
                Title = "Biology for Beginners",
                Rating = 4.5,
                RatingCount = 30,
                creatorId = Guid.NewGuid(),
                isPublic = true,
                Modified = DateOnly.FromDateTime(DateTime.Now.AddDays(-10)),
                CardCount = 50,
                Tags = new List<TagTypes> { TagTypes.Biology, TagTypes.Beginner },
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam vitae dui sit amet tortor cursus aliquam. Praesent non lorem eget nunc tempus condimentum. Mauris a justo egestas, aliquet massa et, elementum sem. Nulla vehicula massa magna, in dapibus est hendrerit nec. Aenean eu aliquet dui. Aenean a nunc et augue porta faucibus. Fusce dolor massa, tincidunt id est sed, tincidunt elementum mi. Quisque in massa et orci iaculis placerat eu et nulla. Nam arcu nulla, accumsan non rutrum at, mollis bibendum ligula.\r\n\r\nPhasellus ornare euismod quam at tempor. Vivamus ut dapibus lorem. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Etiam laoreet fermentum sodales. Mauris bibendum enim eu nisl pellentesque, rhoncus dictum purus ullamcorper. Donec volutpat, magna id mollis viverra, ligula lectus euismod sem, at placerat ex elit et nisi. Cras id interdum nunc. Praesent a condimentum mi, in mollis erat. Suspendisse mattis, massa in ornare.",

                Cards = new List<Card> //public Card[] Cards = new Card[] //new list<Cards>
                {
                    new Card { Id = Guid.NewGuid(), Question = "Which organ is the aorta a part of? ", Answer = "Heart"},
                    new Card { Id = Guid.NewGuid(), Question = "How many bones are in the human body?", Answer = "206"}
                }
            },
            new Deck
            {
                Id = Guid.NewGuid(),
                Title = "Advanced Physics",
                Rating = 4.8,
                RatingCount = 100,
                creatorId = Guid.NewGuid(),
                isPublic = true,
                Modified = DateOnly.FromDateTime(DateTime.Now.AddDays(-5)),
                CardCount = 75,
                Tags = new List<TagTypes> { TagTypes.Physics, TagTypes.Advanced }
            },
            new Deck
            {
                Id = Guid.NewGuid(),
                Title = "Introduction to Chemistry",
                Rating = 4.1,
                RatingCount = 40,
                creatorId = Guid.NewGuid(),
                isPublic = true,
                Modified = DateOnly.FromDateTime(DateTime.Now.AddDays(-15)),
                CardCount = 45,
                Tags = new List<TagTypes> { TagTypes.Chemistry, TagTypes.Beginner }
            },
            new Deck
            {
                Id = Guid.NewGuid(),
                Title = "World History Overview",
                Rating = 4.3,
                RatingCount = 20,
                creatorId = Guid.NewGuid(),
                isPublic = true,
                Modified = DateOnly.FromDateTime(DateTime.Now.AddDays(-7)),
                CardCount = 60,
                Tags = new List<TagTypes> { TagTypes.History, TagTypes.Intermediate }
            },
            new Deck
            {
                Id = Guid.NewGuid(),
                Title = "Expert-Level Mathematics",
                Rating = 4.9,
                RatingCount = 10,
                creatorId = Guid.NewGuid(),
                isPublic = true,
                Modified = DateOnly.FromDateTime(DateTime.Now.AddDays(-3)),
                CardCount = 90,
                Tags = new List<TagTypes> { TagTypes.Mathematics, TagTypes.Expert }
            },
            new Deck
            {
                Id = Guid.NewGuid(),
                Title = "Basic Geography Concepts",
                Rating = 4.0,
                RatingCount = 4,
                creatorId = Guid.NewGuid(),
                isPublic = true,
                Modified = DateOnly.FromDateTime(DateTime.Now.AddDays(-20)),
                CardCount = 30,
                Tags = new List<TagTypes> { TagTypes.Geography, TagTypes.Beginner }
            },
            new Deck
            {
                Id = Guid.NewGuid(),
                Title = "Languages of the World",
                Rating = 4.6,
                RatingCount = 5,
                creatorId = Guid.NewGuid(),
                isPublic = true,
                Modified = DateOnly.FromDateTime(DateTime.Now.AddDays(-12)),
                CardCount = 55,
                Tags = new List<TagTypes> { TagTypes.Languages, TagTypes.Intermediate }
            },
            new Deck
            {
                Id = Guid.NewGuid(),
                Title = "Literature Analysis",
                Rating = 4.2,
                RatingCount = 5,
                creatorId = Guid.NewGuid(),
                isPublic = true,
                Modified = DateOnly.FromDateTime(DateTime.Now.AddDays(-8)),
                CardCount = 40,
                Tags = new List<TagTypes> { TagTypes.Literature, TagTypes.Advanced }
            },
            new Deck
            {
                Id = Guid.NewGuid(),
                Title = "Art History",
                Rating = 3.9,
                RatingCount = 20,
                creatorId = Guid.NewGuid(),
                isPublic = true,
                Modified = DateOnly.FromDateTime(DateTime.Now.AddDays(-25)),
                CardCount = 35,
                Tags = new List<TagTypes> { TagTypes.Art, TagTypes.History, TagTypes.Beginner }
            },
            new Deck
            {
                Id = Guid.NewGuid(),
                Title = "Introduction to Math",
                Rating = 4.4,
                RatingCount = 15,
                creatorId = Guid.NewGuid(),
                isPublic = true,
                Modified = DateOnly.FromDateTime(DateTime.Now.AddDays(-4)),
                CardCount = 65,
                Tags = new List<TagTypes> { TagTypes.Mathematics, TagTypes.Intermediate }
            },
            new Deck
            {
                Id = Guid.NewGuid(),
                Title = "Mathematics 101",
                Rating = 4.7,
                RatingCount = 10,
                creatorId = Guid.NewGuid(),
                isPublic = true,
                Modified = DateOnly.FromDateTime(DateTime.Now.AddDays(-6)),
                CardCount = 70,
                Tags = new List<TagTypes> { TagTypes.Mathematics, TagTypes.Beginner }
            },
            new Deck
            {
                Id = Guid.NewGuid(),
                Title = "Music Theory",
                Rating = 5.0,
                RatingCount = 1,
                creatorId = Guid.NewGuid(),
                isPublic = true,
                Modified = DateOnly.FromDateTime(DateTime.Now.AddDays(-30)),
                CardCount = 30,
                Tags = new List<TagTypes> { TagTypes.Music }
            },
            new Deck
            {
                Id = Guid.NewGuid(),
                Title = "Philosophy of Science",
                Rating = 3.8,
                RatingCount = 15,
                creatorId = Guid.NewGuid(),
                isPublic = true,
                Modified = DateOnly.FromDateTime(DateTime.Now.AddDays(-22)),
                CardCount = 35,
                Tags = new List<TagTypes> { TagTypes.Philosophy, TagTypes.Expert }
            },
            new Deck
            {
                Id = Guid.NewGuid(),
                Title = "Computer Science Basics",
                Rating = 4.5,
                RatingCount = 30,
                creatorId = Guid.NewGuid(),
                isPublic = true,
                Modified = DateOnly.FromDateTime(DateTime.Now.AddDays(-14)),
                CardCount = 55,
                Tags = new List<TagTypes> { TagTypes.ComputerScience, TagTypes.Beginner }
            },
            new Deck
            {
                Id = Guid.NewGuid(),
                Title = "Modern Political Theory",
                Rating = 4.2,
                RatingCount = 5,
                creatorId = Guid.NewGuid(),
                isPublic = true,
                Modified = DateOnly.FromDateTime(DateTime.Now.AddDays(-9)),
                CardCount = 50,
            }

        };
    }
}
