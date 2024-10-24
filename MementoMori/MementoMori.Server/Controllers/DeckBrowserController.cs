﻿using MementoMori.Server.DTOS;
using MementoMori.Server.Extensions;
using MementoMori.Server.Service;
using Microsoft.AspNetCore.Mvc;

namespace MementoMori.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DeckBrowserController : ControllerBase
    {
        private readonly DeckHelper _deckHelper;
        public DeckBrowserController() 
        {
            _deckHelper = new DeckHelper();
        }

        [HttpGet("getDecks")]
        public ActionResult<DeckBrowserDTO> getDecks([FromQuery] string[] selectedTags, string? searchString)
        {
            var filteredDecksList = _deckHelper.Filter(titleSubstring: searchString, selectedTags: selectedTags);
            filteredDecksList.Sort();

            var result = filteredDecksList.Select(deck => new DeckBrowserDTO
            {
                Id = deck.Id,
                Title = deck.Title,
                Rating = deck.Rating,
                Modified = deck.Modified,
                Cards = deck.CardCount,
                Tags = deck.TagsToString(),
            }).ToArray();

            return Ok(result);
        }
    }
}
