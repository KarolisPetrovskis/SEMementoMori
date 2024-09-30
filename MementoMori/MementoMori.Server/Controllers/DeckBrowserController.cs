using Microsoft.AspNetCore.Mvc;

namespace MementoMori.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DeckBrowserController : ControllerBase
    {
        /*
         * This is just a random deck array for development and testing, 
         * remove it and pull data from db when it is available
         */
        public static readonly Deck[] Decks = new Deck[]
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
                Tags = new[] { DeckTag.Biology, DeckTag.Beginner }
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
                Tags = new[] { DeckTag.Physics, DeckTag.Advanced }
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
                Tags = new[] { DeckTag.Chemistry, DeckTag.Beginner }
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
                Tags = new[] { DeckTag.History, DeckTag.Intermediate }
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
                Tags = new[] { DeckTag.Mathematics, DeckTag.Expert }
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
                Tags = new[] { DeckTag.Geography, DeckTag.Beginner }
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
                Tags = new[] { DeckTag.Languages, DeckTag.Intermediate }
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
                Tags = new[] { DeckTag.Literature, DeckTag.Advanced }
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
                Tags = new[] { DeckTag.Art, DeckTag.History, DeckTag.Beginner }
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
                Tags = new[] { DeckTag.Mathematics, DeckTag.Intermediate }
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
                Tags = new[] { DeckTag.Mathematics, DeckTag.Beginner }
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
                Tags = new[] { DeckTag.Music }
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
                Tags = new[] { DeckTag.Philosophy, DeckTag.Expert }
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
                Tags = new[] { DeckTag.ComputerScience, DeckTag.Beginner }
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

    /*
     * 
     */


        [HttpGet("getDecks")]
        public ActionResult<DeckBrowserDeck> GetDecks([FromQuery] string[] selectedTags, string? searchString)
        {
            var filteredDecks = Decks.AsQueryable().Where(deck => deck.isPublic);

            if (selectedTags.Length != 0)
            {
                filteredDecks = filteredDecks.Where(deck => deck.Tags != null && selectedTags.All(tag => deck.Tags.Contains(tag)));
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                filteredDecks = filteredDecks.Where(deck => deck.Title.Contains(searchString, StringComparison.OrdinalIgnoreCase));
            }

            filteredDecks = filteredDecks.Order().Reverse();

            var result = filteredDecks.Select(deck => new DeckBrowserDeck
            {
                Id = deck.Id,
                Title = deck.Title,
                Rating = deck.Rating,
                Modified = deck.Modified,
                Cards = deck.Cards,
                Tags = deck.Tags
            }).ToArray();

            return Ok(result);
        }
    }
}
