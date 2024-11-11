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
import TagSelector from '../deckBrowser/TagSelector';

interface Deck {
  id: string;
  isPublic: boolean;
  title: string;
  description?: string;
  cardCount: number;
  tags: string[];
  cards: Card[];
}

interface Card {
  id: string;
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
  const [showAddCardDialog, setShowAddCardDialog] = useState(false); // State for Add Card dialog
  const [newCardQuestion, setNewCardQuestion] = useState<string>(''); // New card question
  const [newCardAnswer, setNewCardAnswer] = useState<string>(''); // New card answer
  const [questionError, setQuestionError] = useState<string | null>(null);
  const [answerError, setAnswerError] = useState<string | null>(null);
  const [titleError, setTitleError] = useState<string | null>(null);
  const [numberForId, setNumberForId] = useState<number>(0);
  const [selectedTags, setSelectedTags] = useState<string[]>([]);
  useEffect(() => {
    async function fetchDeck() {
      try {
        const response = await axios.get(`/Decks/${deckId}/EditorView`);
        const fetchedDeck = response.data;
        setDeck(fetchedDeck);
        setOriginalDeck(fetchedDeck);

        // Set selectedTags directly after fetching deck
        setSelectedTags(fetchedDeck.tags || []);
        console.log(selectedTags);
        console.log(response.data);
      } catch (error) {
        console.error('Error fetching deck:', error);
      }
    }
    fetchDeck();
  }, [deckId]);

  useEffect(() => {
    if (deck) {
      setDeck({ ...deck, tags: selectedTags });
    }
  }, [selectedTags]);

  const revertChanges = () => {
    setDeck(originalDeck);
    setNumberForId(0);
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
      const updatedCards = deck.cards.filter((_, i) => i !== index);
      setDeck({ ...deck, cards: updatedCards });
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
        description: '',
      };
      setNumberForId(numberForId + 1);
      console.log(newCard.id);
      setDeck({ ...deck, cards: [...deck.cards, newCard] });
    }

    setNewCardQuestion('');
    setNewCardAnswer('');
    setShowAddCardDialog(false);
  };

  const addCard = () => {
    setActiveEditCardId(null); // Close edit mode
    setShowAddCardDialog(true); // Open Add Card dialog
    setNewCardQuestion('');
    setNewCardAnswer('');
  };

  const modifyCard = (index: number) => {
    const trimmedQuestion = editQuestion.trim();
    const trimmedAnswer = editAnswer.trim();

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
      updatedCards[index] = {
        ...updatedCards[index],
        question: trimmedQuestion,
        answer: trimmedAnswer,
      };
      setDeck({ ...deck, cards: updatedCards });
    }
    setActiveEditCardId(null);
    setEditQuestion('');
    setEditAnswer('');
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
    }
  };

  const hasChanges = () => {
    return JSON.stringify(originalDeck) !== JSON.stringify(deck);
  };

  if (!deck) return <Typography>Loading...</Typography>;

  const saveAllChanges = async () => {
    if (deck && originalDeck) {
      const response = await axios.post('/CardData/editDeck', {
        editedDeck: deck,
        originalDeck: originalDeck,
      });
      console.log(response.data); // Log the data to see the backend response
      if (response.status === 200) {
        setOriginalDeck(deck);
        console.log('Success');
      } else {
        console.log('The post request has failed');
      }
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
        Cards
      </Typography>
      {deck.cards.map((card, index) => (
        <Box
          key={card.id}
          sx={{ mb: 2, display: 'flex', alignItems: 'center' }}
        >
          <TextField
            label={`Question ${index + 1}`}
            fullWidth
            variant="outlined"
            margin="normal"
            value={card.question}
            InputProps={{ readOnly: true }}
          />
          <Button
            variant="contained"
            color="primary"
            sx={{ ml: 2, height: '56px' }}
            onClick={() => toggleEditField(card.id)}
          >
            Edit Card
          </Button>
          <Dialog
            open={activeEditCardId === card.id}
            onClose={() => setActiveEditCardId(null)}
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
              <Button onClick={() => setActiveEditCardId(null)}>Close</Button>
            </DialogActions>
          </Dialog>
        </Box>
      ))}
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
            Create
          </Button>
          <Button onClick={() => setShowAddCardDialog(false)}>Cancel</Button>
        </DialogActions>
      </Dialog>
      {hasChanges() && (
        <Box
          sx={{
            display: 'flex',
            justifyContent: 'space-between',
            alignItems: 'center',
            mt: 2,
            gap: 2, // Adds spacing between the buttons
          }}
        >
          <Button
            variant="contained"
            color="primary"
            fullWidth
            sx={{ flex: 1 }}
            onClick={() => {
              saveAllChanges();
              console.log('Click Works');
              console.log(deck.cards);
            }}
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
              console.log(deck);
            }}
          >
            Revert Changes
          </Button>
        </Box>
      )}
    </Box>
  );
}
