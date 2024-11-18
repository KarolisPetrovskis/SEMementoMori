/* eslint-disable @typescript-eslint/no-explicit-any */
import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { useParams } from 'react-router-dom';
import {
  Typography,
  TextField,
  Box,
  Switch,
  FormControlLabel,
  Button,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
} from '@mui/material';
import TagSelector, { getTagId } from '../deckBrowser/TagSelector';
import { List, ListItem, ListItemText } from '@mui/material';
import Divider from '@mui/material/Divider';
interface Deck {
  id: string;
  isPublic: boolean;
  title: string;
  description?: string;
  cardCount: number;
  tags: number[];
  cards: Card[];
}
interface Card {
  id: string;
  question: string;
  description: string;
  answer: string;
}
interface NewCard {
  question: string;
  description: string;
  answer: string;
}

export default function EditDeck() {
  const { deckId } = useParams<{ deckId: string }>();
  const [deck, setDeck] = useState<Deck | null>(null);
  const [originalDeck, setOriginalDeck] = useState<Deck | null>(null);
  const [activeEditCardId, setActiveEditCardId] = useState<string | null>(null);
  const [editQuestion, setEditQuestion] = useState<string>('');
  const [editAnswer, setEditAnswer] = useState<string>('');
  const [editDescription, setEditDescription] = useState<string>('');
  const [showAddCardDialog, setShowAddCardDialog] = useState(false);
  const [newCardQuestion, setNewCardQuestion] = useState<string>('');
  const [newCardAnswer, setNewCardAnswer] = useState<string>('');
  const [newCardDescription, setNewCardDescription] = useState<string>('');
  const [questionError, setQuestionError] = useState<string | null>(null);
  const [answerError, setAnswerError] = useState<string | null>(null);
  const [titleError, setTitleError] = useState<string | null>(null);
  const [numberForId, setNumberForId] = useState<number>(0);
  const [selectedTags, setSelectedTags] = useState<string[]>([]);
  const [alteredCards, setAlteredCards] = useState<Card[] | null>(null);
  const [newCards, setNewCards] = useState<NewCard[] | null>(null);
  const [removeCards, setRemoveCards] = useState<string[] | null>(null);

  useEffect(() => {
    async function fetchDeck() {
      try {
        const response = await axios.get(`/Decks/${deckId}/EditorView`);
        const fetchedDeck = response.data;
        setDeck(fetchedDeck);
        setOriginalDeck(fetchedDeck);
        setSelectedTags(fetchedDeck.tags || []);
      } catch (error) {
        console.error('Error fetching deck:', error);
      }
    }
    fetchDeck();
  }, [deckId]);

  useEffect(() => {
    if (deck) {
      setDeck({ ...deck, tags: selectedTags.map((tag) => getTagId(tag)) });
    }
  }, [selectedTags]);

  const revertChanges = () => {
    setDeck(originalDeck);
    setNumberForId(0);
    setTitleError('');
  };

  const handleTitleChange = (event: { target: { value: any } }) => {
    const value = event.target.value.trim();
    if (!value) {
      setTitleError('Title cannot be empty.');
    } else {
      setTitleError(null);
    }
    if (deck) setDeck({ ...deck, title: event.target.value });
  };

  const handleDescriptionChange = (event: { target: { value: any } }) => {
    const value = event.target.value;
    if (deck) setDeck({ ...deck, description: value });
  };

  const handleIsPublicChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    if (deck) setDeck({ ...deck, isPublic: event.target.checked });
  };

  const deleteCard = (index: number) => {
    if (deck) {
      const cardToDelete = deck.cards[index];

      const updatedCards = deck.cards.filter((_, i) => i !== index);
      setDeck({ ...deck, cards: updatedCards });

      setAlteredCards((prevAlteredCards) => {
        if (!prevAlteredCards) return null;

        return prevAlteredCards.filter((card) => card.id !== cardToDelete.id);
      });

      setNewCards((prevNewCards) => {
        if (!prevNewCards) return null;

        const isCardInNewCards = prevNewCards.some(
          (newCard) =>
            newCard.question === cardToDelete.question &&
            newCard.description === cardToDelete.description &&
            newCard.answer === cardToDelete.answer
        );

        if (isCardInNewCards) {
          return prevNewCards.filter(
            (newCard) =>
              newCard.question !== cardToDelete.question ||
              newCard.description !== cardToDelete.description ||
              newCard.answer !== cardToDelete.answer
          );
        }

        return prevNewCards;
      });

      if (newCards?.length === 0) setNewCards(null);

      setRemoveCards((prevRemoveCards) => {
        const isCardInNewCards = newCards?.some(
          (newCard) =>
            newCard.question === cardToDelete.question &&
            newCard.description === cardToDelete.description &&
            newCard.answer === cardToDelete.answer
        );

        if (!isCardInNewCards) {
          if (!prevRemoveCards) return [cardToDelete.id];
          return [...prevRemoveCards, cardToDelete.id];
        }

        return prevRemoveCards;
      });

      setActiveEditCardId(null);
    }
  };

  const createCard = () => {
    const trimmedQuestion = newCardQuestion.trim();
    const trimmedAnswer = newCardAnswer.trim();

    if (!trimmedQuestion) {
      setQuestionError('Question cannot be empty.');
    } else {
      setQuestionError(null);
    }

    if (!trimmedAnswer) {
      setAnswerError('Answer cannot be empty.');
    } else {
      setAnswerError(null);
    }

    if (!trimmedQuestion || !trimmedAnswer) return;

    if (deck) {
      const newCard: Card = {
        id: numberForId.toString(),
        question: trimmedQuestion,
        answer: trimmedAnswer,
        description: newCardDescription.trim(),
      };
      const addCard: NewCard = {
        question: trimmedQuestion,
        answer: trimmedAnswer,
        description: newCardDescription.trim(),
      };
      setNumberForId(numberForId + 1);
      setNewCards((prevNewCards) => [...(prevNewCards || []), addCard]);
      setDeck({ ...deck, cards: [...deck.cards, newCard] });
    }

    setNewCardQuestion('');
    setNewCardAnswer('');
    setNewCardDescription('');
    setShowAddCardDialog(false);
  };

  const addCard = () => {
    setActiveEditCardId(null); // Close edit mode
    setShowAddCardDialog(true); // Open Add Card dialog
    setNewCardQuestion('');
    setNewCardAnswer('');
    setNewCardDescription('');
  };

  const modifyCard = (index: number) => {
    const trimmedQuestion = editQuestion.trim();
    const trimmedAnswer = editAnswer.trim();
    const trimmedDescription = editDescription.trim();
    if (!trimmedQuestion) {
      setQuestionError('Question cannot be empty.');
      return;
    } else {
      setQuestionError(null);
    }

    if (!trimmedAnswer) {
      setAnswerError('Answer cannot be empty.');
      return;
    } else {
      setAnswerError(null);
    }

    if (deck) {
      const updatedCards = [...deck.cards];
      const updatedCard = {
        ...updatedCards[index],
        question: trimmedQuestion,
        answer: trimmedAnswer,
        description: trimmedDescription,
      };

      updatedCards[index] = updatedCard;
      setDeck({ ...deck, cards: updatedCards });

      setAlteredCards((prevAlteredCards) => {
        if (!prevAlteredCards) return [updatedCard];

        const existingIndex = prevAlteredCards.findIndex(
          (card) => card.id === updatedCard.id
        );

        if (existingIndex !== -1) {
          // Update the existing card
          const updatedAlteredCards = [...prevAlteredCards];
          updatedAlteredCards[existingIndex] = updatedCard;
          return updatedAlteredCards;
        }
        // Add new card to alteredCards
        return [...prevAlteredCards, updatedCard];
      });
    }

    setActiveEditCardId(null);
    setEditQuestion('');
    setEditAnswer('');
    setEditDescription('');
  };

  const toggleEditField = (cardId: string) => {
    if (activeEditCardId === cardId) {
      setActiveEditCardId(null);
      setEditQuestion('');
      setEditAnswer('');
    } else {
      setActiveEditCardId(cardId);
      const card = deck?.cards.find((c) => c.id === cardId);
      setEditQuestion(card?.question || '');
      setEditAnswer(card?.answer || '');
      setEditDescription(card?.description || '');
    }
  };

  const hasChanges = () => {
    return JSON.stringify(originalDeck) !== JSON.stringify(deck);
  };

  if (!deck) return <Typography>Loading...</Typography>;
  const saveAllChanges = async () => {
    if (deck && originalDeck) {
      const postDeck = {
        id: deck.id,
        isPublic: deck.isPublic,
        title: deck.title,
        description: deck.description,
        cardCount: deck.cardCount,
        tags: deck.tags,
      };
      //setPostDeck(postDeck);
      // Modify the post endpoint according to your needs
      const response = await axios.post(`/Decks/${deckId}/editDeck`, {
        Deck: postDeck,
        NewCards: newCards,
        RemovedCards: removeCards,
        Cards: alteredCards,
      });
      if (response.status === 200) {
        setOriginalDeck(deck);
      }
      /*       console.log(postDeck);
      console.log(newCards);
      console.log(removeCards);
      console.log(alteredCards); */
      setNewCards(null);
      setRemoveCards(null);
      setAlteredCards(null);
      setOriginalDeck(deck);
    }
  };

  return (
    <Box sx={{ maxWidth: 600, mx: 'auto', mt: 4, p: 2, boxShadow: 3 }}>
      <Typography variant="h4" gutterBottom>
        Edit Deck {deck.title}
      </Typography>
      <TextField
        label="Title"
        fullWidth
        variant="outlined"
        margin="normal"
        value={deck.title}
        onChange={handleTitleChange}
        error={!!titleError}
        helperText={titleError || ' '}
      />
      <TextField
        label="Description"
        fullWidth
        multiline
        rows={4}
        variant="outlined"
        margin="normal"
        value={deck.description || ''}
        onChange={handleDescriptionChange}
      />
      <TagSelector
        selectedTags={selectedTags}
        setSelectedTags={setSelectedTags}
      />
      <Box sx={{ display: 'flex', justifyContent: 'left', mb: 2 }}>
        <FormControlLabel
          control={
            <Switch
              checked={deck.isPublic}
              onChange={handleIsPublicChange}
              color="primary"
            />
          }
          label={deck.isPublic ? 'Public' : 'Private'}
        />
      </Box>
      <Typography variant="h5" gutterBottom>
        <Divider
          variant="fullWidth"
          sx={{
            '&::before, &::after': {
              borderColor: '#4657b9',
              borderWidth: 2.5,
            },
          }}
        >
          Cards
        </Divider>
      </Typography>
      <List disablePadding>
        {deck.cards.map((card, index) => (
          <ListItem
            key={card.id}
            sx={{
              display: 'flex',
              alignItems: 'center',
              transition: 'background-color 0.3s, box-shadow 0.3s',
              borderRadius: '8px', // Rounded corners for the ListItem
              '&:hover': {
                backgroundColor: 'rgba(0, 0, 0, 0.075)', // Light gray background on hover
                boxShadow: '0px 4px 12px rgba(0, 0, 0, 0.2)', // Shadow effect on hover
              },
            }}
          >
            <ListItemText
              primary={
                <>
                  <Typography component="span" fontStyle="italic">
                    Question {index + 1}:<br></br>
                  </Typography>
                  {card.question}
                </>
              }
              secondary={`Answer: ${card.answer}`}
            />
            <Button
              variant="contained"
              color="primary"
              sx={{ ml: 2 }}
              onClick={() => toggleEditField(card.id)}
            >
              Edit Card
            </Button>
            <Dialog
              open={activeEditCardId === card.id}
              onClose={() => {
                setActiveEditCardId(null);
                //setAnswerError('');
                //setQuestionError('');
              }}
            >
              <DialogTitle>Edit Card Details</DialogTitle>
              <DialogContent>
                <TextField
                  label="Edit Question"
                  fullWidth
                  variant="outlined"
                  margin="normal"
                  value={editQuestion}
                  onChange={(e) => setEditQuestion(e.target.value)}
                  error={!!questionError}
                  helperText={questionError || ' '}
                />
                <TextField
                  label="Edit Description"
                  fullWidth
                  value={editDescription}
                  onChange={(e) => setEditDescription(e.target.value)}
                />
                <TextField
                  label="Edit Answer"
                  fullWidth
                  variant="outlined"
                  margin="normal"
                  value={editAnswer}
                  onChange={(e) => setEditAnswer(e.target.value)}
                  error={!!answerError}
                  helperText={answerError || ' '}
                />
              </DialogContent>
              <DialogActions>
                <Button color="primary" onClick={() => modifyCard(index)}>
                  Save
                </Button>
                <Button color="primary" onClick={() => deleteCard(index)}>
                  Delete
                </Button>
                <Button
                  onClick={() => {
                    setActiveEditCardId(null);
                    setAnswerError('');
                    setQuestionError('');
                  }}
                >
                  Close
                </Button>
              </DialogActions>
            </Dialog>
          </ListItem>
        ))}
      </List>
      <Button
        variant="contained"
        color="primary"
        fullWidth
        sx={{ mt: 2 }}
        onClick={addCard}
      >
        Add Card
      </Button>
      <Dialog
        open={showAddCardDialog}
        onClose={() => setShowAddCardDialog(false)}
      >
        <DialogTitle>Add New Card</DialogTitle>
        <DialogContent>
          <TextField
            label="Question"
            fullWidth
            variant="outlined"
            margin="normal"
            value={newCardQuestion}
            onChange={(e) => setNewCardQuestion(e.target.value)}
            error={!!questionError} // Show error if question is empty
            helperText={questionError || ' '} // Show error message if validation fails
          />
          <TextField
            label="Description"
            fullWidth
            value={newCardDescription}
            onChange={(e) => setNewCardDescription(e.target.value)}
          />
          <TextField
            label="Answer"
            fullWidth
            variant="outlined"
            margin="normal"
            value={newCardAnswer}
            onChange={(e) => setNewCardAnswer(e.target.value)}
            error={!!answerError} // Show error if answer is empty
            helperText={answerError || ' '} // Show error message if validation fails
          />
        </DialogContent>
        <DialogActions>
          <Button color="primary" onClick={createCard}>
            {/*Can implement in this onclick the adding of a new card for quests, advisably have a new card array that you can create a new post for once user clicks on save all changes*/}
            Create
          </Button>
          <Button
            onClick={() => {
              setShowAddCardDialog(false);
              setAnswerError('');
              setQuestionError('');
            }}
          >
            Cancel
          </Button>
        </DialogActions>
      </Dialog>
      {hasChanges() && (
        <Box
          sx={{
            display: 'flex',
            justifyContent: 'space-between',
            alignItems: 'center',
            mt: 2,
            gap: 2,
          }}
        >
          <Button
            variant="contained"
            color="primary"
            fullWidth
            sx={{ flex: 1 }}
            onClick={() => saveAllChanges()}
          >
            Save all changes
          </Button>
          <Button
            variant="contained"
            color="primary"
            fullWidth
            sx={{ flex: 1 }}
            onClick={() => {
              revertChanges();
              console.log(removeCards);
              console.log(newCards);
              console.log(alteredCards);
            }}
          >
            Revert Changes
          </Button>
        </Box>
      )}
    </Box>
  );
}
