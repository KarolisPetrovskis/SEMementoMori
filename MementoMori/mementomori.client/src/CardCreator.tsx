/* eslint-disable @typescript-eslint/no-explicit-any */
import * as React from 'react';
import Box from '@mui/material/Box';
import Card from '@mui/material/Card';
import CardActions from '@mui/material/CardActions';
import CardContent from '@mui/material/CardContent';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import TextField from '@mui/material/TextField';
import axios from 'axios';
import { useParams } from 'react-router-dom';

export default function OutlinedCard() {
  const [dynamicInputValue, setDynamicInputValue] = React.useState('');
  const [fontSize, setFontSize] = React.useState(24);
  const [dynamicTextError, setDynamicTextError] = React.useState('');
  const [requestError, setRequestError] = React.useState('');
  const [postedText, setPostedText] = React.useState('');
  const [questionText, setQuestionText] = React.useState('');
  const [questionTextError, setQuestionTextError] = React.useState('');
  const [postedQuestion, setPostedQuestion] = React.useState('');
  const { deckId } = useParams<{ deckId: string }>();
    console.log(deckId);
  // Handle change for the dynamic text field with increasing height and dynamic font size
  const handleDynamicChange = (event: { target: { value: any } }) => {
    const value = event.target.value;
    setDynamicInputValue(value);

    const textLength = value.replace(/\n/g, '').length;

    if (textLength < 100) {
      setFontSize(24);
    } else if (textLength < 250) {
      setFontSize(18);
    } else {
      setFontSize(14);
    }
  };

  const handleQuestionText = (event: { target: { value: any } }) => {
    const value = event.target.value;
    setQuestionText(value);
  };

  const validateDynamicText = (text: string) => {
    if (!text.trim()) {
      return 'Error: Dynamic text cannot be empty.';
    }
    return '';
  };

  // Send form data to backend server
  const handleCreate = async () => {
    const dynamicTextValidationError = validateDynamicText(dynamicInputValue);
    if (dynamicTextValidationError) {
      setDynamicTextError(dynamicTextValidationError);
      return;
    } else {
      setDynamicTextError('');
    }
    if (questionText.length === 0) {
      setQuestionTextError('Cannot be empty');
      return;
    } else {
      setQuestionTextError('');
    }
    try {
      // Create an object to send to the backend
      const cardData = {
        deckId: deckId,
        question: questionText,
        answer: dynamicInputValue,
      };

      // Send POST request to the server
      const response = await axios.post('/CardData/createCard', cardData);
      console.log(response.data); // Log the data to see the backend response
      if (response.status === 200) {
        setPostedText(response.data.text);
        setPostedQuestion(response.data.question);
      } else {
        setRequestError('Error: Failed to save card data');
      }
    } catch (error) {
      if (error.response && error.response.status === 409) {
        // Handle the case where the file already exists
        setRequestError('Error: The file "CardInfo.txt" already exists.');
      } else {
        // Handle other errors
        setRequestError(
          'Error: An unexpected error occurred. Error message: ' + error.message
        );
      }
    }
  };

  return (
    <Box
      sx={{
        minWidth: '50vw',
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
        margin: 'auto',
      }}
    >
      <Card
        variant="outlined"
        sx={{ width: '100%', height: 'auto', minHeight: '30vh' }}
      >
        <CardContent>
          {/* Existing input fields and logic */}
          <TextField
            fullWidth
            multiline
            variant="outlined"
            onChange={handleQuestionText}
            label="Write a question"
            value={questionText} // Reset field on form submission
            sx={{
              overflowY: 'auto',
              resize: 'none',
              marginBottom: '16px',
            }}
            error={Boolean(questionTextError)}
            helperText={questionTextError}
          />

          {/*Add description if necessary*/}

          <Typography
            gutterBottom
            sx={{ color: 'text.secondary', fontSize: 14 }}
          >
            Enter answer/explanation for the question
          </Typography>
          <TextField
            fullWidth
            multiline
            variant="outlined"
            onChange={handleDynamicChange}
            label="Write an answer to the question"
            InputProps={{
              style: { fontSize: fontSize, zIndex: 10000 },
            }}
            value={dynamicInputValue} // Reset field on form submission
            sx={{
              overflowY: 'auto',
              resize: 'none',
              marginBottom: '16px',
            }}
            error={Boolean(dynamicTextError)}
            helperText={dynamicTextError}
          />

          {requestError && (
            <Typography color="error" sx={{ marginTop: '16px' }}>
              {requestError}
            </Typography>
          )}
          {/* Display the posted tags and text  */}
          {postedQuestion && (
            <Box sx={{ marginTop: '16px' }}>
              <Typography sx={{ fontWeight: 'bold', marginBottom: '8px' }}>
                Posted Question:
              </Typography>
              <Typography sx={{ whiteSpace: 'pre-line' }}>
                {postedQuestion}
              </Typography>
            </Box>
          )}

          {postedText && (
            <Box sx={{ marginTop: '16px' }}>
              <Typography sx={{ fontWeight: 'bold', marginBottom: '8px' }}>
                Posted Text:
              </Typography>
              <Typography sx={{ whiteSpace: 'pre-line' }}>
                {postedText}
              </Typography>
            </Box>
          )}
        </CardContent>
        <CardActions>
          <Button
            size="medium"
            onClick={handleCreate}
            variant="contained"
            sx={{ backgroundColor: '#7FFFD4' }}
          >
            Create
          </Button>
        </CardActions>
      </Card>
    </Box>
  );
}
