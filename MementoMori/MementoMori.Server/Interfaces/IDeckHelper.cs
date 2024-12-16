﻿using MementoMori.Server.DTOS;
using MementoMori.Server.Models;

namespace MementoMori.Server.Interfaces
{
    public interface IDeckHelper
    {
        Task<List<Deck>> Filter(Guid[]? ids = null, string? titleSubstring = null, string[]? selectedTags = null, Guid? userId = null);
        Task UpdateDeckAsync(EditedDeckDTO editedDeckDTO, Guid editorId);
        Task<Guid> CreateDeckAsync(EditedDeckDTO createDeck, Guid requesterId);
        Task DeleteDeckAsync(Guid deckId, Guid requesterId);
        Task<UserDeckDTO[]> GetUserDecks(Guid userId);
        Task<Deck> GetDeckAsync(Guid deckId);
    }
}