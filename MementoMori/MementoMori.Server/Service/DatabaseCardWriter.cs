using MementoMori.Server.Database;
using MementoMori.Server.DTOS;
using MementoMori.Server.DTOS.DeckEditorDTOS;
using MementoMori.Server.Models;
using Microsoft.EntityFrameworkCore;
namespace MementoMori.Server.Service
{
    public class DatabaseCardWriter
    {
        private readonly AppDbContext _context;
        private readonly DeckHelper _deckHelper;
        public DatabaseCardWriter(AppDbContext context, DeckHelper deckHelper)
        {
            _context = context;
            _deckHelper = deckHelper;
        }

        public void AddCard(string question, string text, string? description, Guid cardId, Guid deckId)
        {
            Deck deck = _deckHelper.Filter(ids: [deckId]).First();
            if (description == null)
                description = " ";
            
            var newCard = new Card
            {
                Id = cardId,
                Question = question,
                Description = description,
                Answer = text,
                lastInterval = null,
                nextShow = null,
            };
            if(deck != null)
            {
                deck.Cards.Add(newCard);
                _context.Cards.Add(newCard);
                _context.SaveChanges();
            }
        }

        public void UpdateCardData(Guid cardId, string? question = null, string? description = null, string? answer = null, int? lastInterval = null, DateOnly? nextShow = null)
        {
            var card = _context.Cards.SingleOrDefault(c => c.Id == cardId);
            if (card != null)
            {
                if (question != null)
                {
                    card.Question = question;
                }
                if (description != null)
                {
                    card.Description = description;
                }
                if (answer != null)
                {
                    card.Answer = answer;
                }
                if (lastInterval != null)
                {
                    card.lastInterval = lastInterval;
                }
                if (nextShow != null)
                {
                    card.nextShow = nextShow;
                }
                _context.Cards.Update(card);
                _context.SaveChanges();
            }
        }

        private DeckUpdateDTO FillEmptyGuids(DeckUpdateDTO deck)
        {
            int i = 0;
            foreach(var card in deck.Cards)
            {
                if (card.realId == Guid.Empty)
                {
                    string stringGuid = Guid.NewGuid().ToString();
                    DeckUpdateCards newCard = new DeckUpdateCards
                    {
                        Id = deck.Cards[i].Id,
                        Answer = card.Answer,
                        Question = card.Question,
                        Description = card.Description
                    };
                    newCard.ConvertRealId(newCard.Id);
                    deck.Cards[i] = newCard;
                }
                i++;
            }
            return deck;
        }

        private void RemoveCardFromDb(Guid id)
        {
            try
            {
                Card card = _context.Cards.SingleOrDefault(c => c.Id == id);

                if (card == null)
                {
                    throw new InvalidOperationException($"Card with ID {id} was not found.");
                }
                _context.Cards.Remove(card);
                _context.SaveChanges();
            }
                        catch (DbUpdateException ex)
            {
                Console.WriteLine("An error occurred while trying to remove the card from the database: " + ex.Message);
            }
        }

        public void UpdateDeck(DeckEditRequestDTO newDeck)
        {
            Guid deckId = newDeck.OriginalDeck.Id;
            Deck deck = _deckHelper.Filter(ids: [deckId]).First();            

            if(deck != null && deckId != Guid.Empty)
            {
                newDeck.OriginalDeck = FillEmptyGuids(newDeck.OriginalDeck);
                newDeck.EditedDeck = FillEmptyGuids(newDeck.EditedDeck);
                
                var originalCards = newDeck.OriginalDeck.Cards.ToDictionary(c => c.realId);
                var editedCards = newDeck.EditedDeck.Cards.ToDictionary(c => c.realId);

                _context.Entry(deck).Reload();
                if (deck.isPublic != newDeck.EditedDeck.isPublic)
                {
                    deck.isPublic = newDeck.EditedDeck.isPublic; 
                }
                if (deck.Description != newDeck.EditedDeck.Description)
                {
                    deck.Description = newDeck.EditedDeck.Description;
                }
                if (deck.Title != newDeck.EditedDeck.Title)
                {
                    deck.Title = newDeck.EditedDeck.Title;
                }
                if (newDeck.EditedDeck.Cards != null && deck.Cards != null)
                {

                   int i = 0;
                    foreach(DeckUpdateCards card in newDeck.OriginalDeck.Cards)
                    {
                        int y = 0, count = 0;
                        if (newDeck.OriginalDeck.Cards != null)
                        {
                            foreach(DeckUpdateCards originalCard in newDeck.EditedDeck.Cards)
                            {
                                if (newDeck.OriginalDeck.Cards[i].realId == newDeck.EditedDeck.Cards[y].realId)
                                {
                                    count++;
                                    if (newDeck.OriginalDeck.Cards[i].Question != newDeck.EditedDeck.Cards[y].Question)
                                    {
                                        deck.Cards[i].Question = newDeck.EditedDeck.Cards[y].Question;
                                        _context.Entry(deck.Cards[i]).State = EntityState.Modified;
                                    }
                                    if (newDeck.OriginalDeck.Cards[i].Answer != newDeck.EditedDeck.Cards[y].Answer)
                                    {
                                        deck.Cards[i].Answer = newDeck.EditedDeck.Cards[y].Answer;
                                        _context.Entry(deck.Cards[i]).State = EntityState.Modified;
                                    }
                                    if (newDeck.OriginalDeck.Cards[i].Description != newDeck.EditedDeck.Cards[y].Description)
                                    {
                                        deck.Cards[i].Description = newDeck.EditedDeck.Cards[y].Description;
                                        _context.Entry(deck.Cards[i]).State = EntityState.Modified;
                                    }
                                }
                                y++;
                            }
                            if (count == 0)
                            {
                                    int x = 0;
                                    foreach(Card deckCards in deck.Cards)
                                    {
                                        if(x==i)
                                        {
                                            deck.Cards.Remove(deckCards);
                                            RemoveCardFromDb(deckCards.Id);
                                            i--;
                                            _context.Decks.Update(deck);
                                            break;
                                        }
                                        ++x;
                                    }
                            }
                        }
                        i++;
                    }

                    int xx = 0;
                    foreach(var card in newDeck.EditedDeck.Cards)
                    {
                        int y = 0; 
                        foreach(var oldCards in deck.Cards)
                        {
                            if (card.realId == oldCards.Id)
                            y++;
                        }
                        if (y == 0)
                        {
                            AddCard(card.Question, card.Answer, card.Description, card.realId, deckId);
                        }
                        xx++;
                    }

                    TagTypes[] selectedTagEnums;
                    if (newDeck.OriginalDeck.Tags != newDeck.EditedDeck.Tags)
                    {
                        selectedTagEnums = newDeck.EditedDeck.Tags.Select(tag => Enum.Parse<TagTypes>(tag)).ToArray();
                        deck.Tags = selectedTagEnums.ToList();

                    }
                    try
                    {
                        _context.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        throw new InvalidOperationException("Concurrency conflict detected while updating the deck.", ex);
                    }
                }
            }
        }
    }
}