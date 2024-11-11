using MementoMori.Server.Database;
using MementoMori.Server.DTOS;
using MementoMori.Server.DTOS.DeckEditorDTOS;
using MementoMori.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System.Diagnostics;
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

        public void AddCard(string question, string text, Guid deckId)
        {
            //Guid cardId = Guid.NewGuid();
            Deck deck = _context.Decks.SingleOrDefault(c => c.Id == deckId);


            // Create a new card entity
            var newCard = new Card
            {
                Id = Guid.NewGuid(),
                Question = question,
                Description = "NULL",
                Answer = text,
                lastInterval = null,
                nextShow = null
            };
            if(deck != null)
            {
                // Add the new card to the context
                deck.Cards.Add(newCard);

                // Add the new card to the context
                _context.Decks.Add(deck);
                // Save changes to insert the new card record
                _context.SaveChanges();
            }
        }

        // If you do not want to update a value pass 'null'. CardId is mandatory
        public void UpdateCardData(Guid cardId, string? question = null, string? description = null, string? answer = null, int? lastInterval = null, DateOnly? nextShow = null)
        {
            var card = _context.Cards.SingleOrDefault(c => c.Id == cardId);
            if (card != null) // Check if the card exists
            {
                // Update properties
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
                // Save changes to the database
                _context.SaveChanges();
            }
        }

        private DeckUpdateDTO fillEmptyGuids(DeckUpdateDTO deck)
        {
            int i = 0;
            foreach(var card in deck.Cards)
            {
                if (card.realId == Guid.Empty)
                {
                    //Guid tempGuid = Guid.NewGuid();
                    //card.realId = tempGuid.ToString();
                    // DOES NOT PROPERLY GIVE THE NEW ID DUE TO TYPE ISSUES
                    //deck.Cards[i].realId = Guid.NewGuid();
                    string stringGuid = Guid.NewGuid().ToString();
                    DeckUpdateCards newCard = new DeckUpdateCards // Initialize newCard
                    {
                        Id = deck.Cards[i].Id,
                        Answer = card.Answer,
                        Question = card.Question,
                        Description = card.Description
                    };
                    newCard.convertRealId(newCard.Id);
                    deck.Cards[i] = newCard;
                }
                i++;
            }
            return deck;
        }



        private void removeCardFromDb(Guid id)
        {
            Card card = _context.Cards.SingleOrDefault(c => c.Id == id);
            _context.Cards.Remove(card);
            _context.SaveChanges();
        }

        public void UpdateDeck(DeckEditRequestDTO newDeck)
        {
            Guid deckId = newDeck.OriginalDeck.Id;
            //Deck deck = _context.Decks.SingleOrDefault(c => c.Id == deckId);
            Deck deck = _deckHelper.Filter(ids: [deckId]).First();            

            if(deck != null)
            {
                Console.WriteLine(newDeck);
                Debug.WriteLine(newDeck);
                //newDeck.OriginalDeck = fillEmptyGuids(newDeck.OriginalDeck);
                newDeck.EditedDeck = fillEmptyGuids(newDeck.EditedDeck);
                
                //originalCards.Cards = fillEmptyGuids(originalCards)

                var originalCards = newDeck.OriginalDeck.Cards.ToDictionary(c => c.realId);
                var editedCards = newDeck.EditedDeck.Cards.ToDictionary(c => c.realId);

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
                if(newDeck.EditedDeck.Cards != null && deck.Cards != null)
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
                                    }
                                    if (newDeck.OriginalDeck.Cards[i].Answer != newDeck.EditedDeck.Cards[y].Answer)
                                    {
                                        deck.Cards[i].Answer = newDeck.EditedDeck.Cards[y].Answer;
                                    }
                                    if (newDeck.OriginalDeck.Cards[i].Description != newDeck.EditedDeck.Cards[y].Description)
                                    {
                                        deck.Cards[i].Description = newDeck.EditedDeck.Cards[y].Description;
                                    }
                                }
                                y++;
                            }
                            if (count == 0)
                            {
                                //if(newDeck.OriginalDeck.Cards[i].realId == Guid.Empty)
                                //{
                                    int x = 0;
                                    //for(int x=0; deck.Cards.Count; ++x)
  /*                                   foreach(var deckCard in deck.Cards)
                                    {
                                        x++;
                                    }
                                    for (int xx = 0; xx<x; ++x)
                                    {
                                        if(xx==i)
                                    }
                                    */
                                    foreach(Card deckCards in deck.Cards)
                                    {
                                        if(x==i)
                                        {
                                            //removeCard(deck.Cards[i]);
                                            deck.Cards.Remove(deckCards);
                                            removeCardFromDb(deckCards.Id);
                                            i--;
                                            break;
                                        }
                                        ++x;
                                    }
                                    //deck.Cards.Remove(i);
                                    //deck.Cards.Add()
                                //}
                                //deck.Cards.Remove(deck.Cards[i]);
                            }
                        }
                        i++;
                    }
                    //Card addCards[newDeck.EditedDeck.Cards.Length()];
                    //foreach()
                    
                    foreach (var editedCard in editedCards.Values)
                    {
                        if (!originalCards.ContainsKey(editedCard.realId))
                        {
                            deck.Cards.Add(new Card
                            {
                                Id = editedCard.realId,
                                Question = editedCard.Question,
                                Answer = editedCard.Answer,
                                Description = editedCard.Description,
                                lastInterval = null,
                                nextShow = null
                            });
                        }
                    }

                    TagTypes[] selectedTagEnums;
                    if (newDeck.OriginalDeck.Tags != newDeck.EditedDeck.Tags)
                    {
                        try
                        {
                            selectedTagEnums = newDeck.EditedDeck.Tags.Select(tag => Enum.Parse<TagTypes>(tag)).ToArray();
                            deck.Tags = selectedTagEnums.ToList();
                        }
                        catch (Exception)
                        {
                            // handle error ()
                        }
                    }
                    //_context.Entry(deck).Reload();
                                      
                }
                    _context.Decks.Update(deck);
                    _context.SaveChanges();  
            }

        }



        private void removeCard(Card card)
        {
            
        }
    }
}




/* public void UpdateDeck(DeckEditRequestDTO newDeck)
        {
            var deckId = newDeck.OriginalDeck.Id;
            Deck deck = _deckHelper.Filter(ids: [deckId]).First();            
            if(deck != null)
            {
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
                if(newDeck.EditedDeck.Cards != null && deck.Cards != null)
                {
                    int i = 0;
                    foreach(DeckUpdateCards card in newDeck.OriginalDeck.Cards)
                    {
                        int y = 0, count = 0;
                        if (newDeck.OriginalDeck.Cards != null)
                        {
                            foreach(DeckUpdateCards originalCard in newDeck.OriginalDeck.Cards)
                            {
                                if (!(newDeck.OriginalDeck.Cards[i].realId != newDeck.EditedDeck.Cards[y].realId))
                                {
                                    count++;
                                    if (newDeck.OriginalDeck.Cards[i].Question != newDeck.EditedDeck.Cards[y].Question)
                                    {
                                        deck.Cards[i].Question = newDeck.EditedDeck.Cards[y].Question;
                                    }
                                    if (newDeck.OriginalDeck.Cards[i].Answer != newDeck.EditedDeck.Cards[y].Answer)
                                    {
                                        deck.Cards[i].Answer = newDeck.EditedDeck.Cards[y].Answer;
                                    }
                                    if (newDeck.OriginalDeck.Cards[i].Description != newDeck.EditedDeck.Cards[y].Description)
                                    {
                                        deck.Cards[i].Description = newDeck.EditedDeck.Cards[y].Description;
                                    }
                                }
                                y++;
                            }
                            if (count == 0)
                            {
                                if(newDeck.EditedDeck.Cards[i].realId == Guid.Empty)
                                {
                                    int x = 0;
                                    //for(int x=0; deck.Cards.Count; ++x)
                                    foreach(Card deckCards in deck.Cards)
                                    {
                                        if(x==i)
                                        {
                                            deck.Cards.Remove(deckCards);
                                        }
                                        ++x;
                                    }
                                    //deck.Cards.Remove(i);
                                    //deck.Cards.Add()
                                }
                                //deck.Cards.Remove(deck.Cards[i]);
                            }
                        }
                        i++;
                    }
                    Card addCards[newDeck.EditedDeck.Cards.Length()];
                    //foreach()

                }
                _context.SaveChanges();
            }

        }

    }
} */