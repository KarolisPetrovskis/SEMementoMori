﻿namespace MementoMori.Server.Interfaces
{
    public interface IDatabaseCardWriter
    {
        void AddCard(string question, string text, Guid deckId);
        void UpdateCardData(Guid cardId, string? question = null, string? description = null, string? answer = null, int? lastInterval = null, DateOnly? nextShow = null);
    }
}