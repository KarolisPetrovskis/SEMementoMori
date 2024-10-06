import * as React from 'react';
import Box from '@mui/material/Box';
import Card from '@mui/material/Card';
import CardActions from '@mui/material/CardActions';
import CardContent from '@mui/material/CardContent';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import TextField from '@mui/material/TextField';
import axios from 'axios';
import TagSelector from './deckBrowser/TagSelector';

export default function OutlinedCard() {
    const [dynamicInputValue, setDynamicInputValue] = React.useState('');
    const [cardIdField, setCardIdField] = React.useState('');
    const [fontSize, setFontSize] = React.useState(24); 
    const [deckIdError, setDeckIdError] = React.useState(''); 
    const [dynamicTextError, setDynamicTextError] = React.useState(''); 
    const [requestError, setRequestError] = React.useState(''); 
    const [postedTags, setPostedTags] = React.useState('');
    const [postedText, setPostedText] = React.useState('');
    const [selectedTags, setSelectedTags] = React.useState<string[]>([]);

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

    const handleCardIdField = (event: { target: { value: any } }) => {
        const value = event.target.value;
        setCardIdField(value);
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
        if (cardIdField.length === 0)
        {
            setDeckIdError('Deck Id cannot be empty');
        }
        else
            setDeckIdError('');

        try {
            // Create an object to send to the backend
            const cardData = {
                deckId: cardIdField,
                tags: selectedTags,
                text: dynamicInputValue,
            };

            // Send POST request to the server
            const response = await axios.post('/CardData/createCard', cardData);

            if (response.status === 200) {
                //console.log("Card data successfully saved");
                setPostedTags(response.data.tags);
                setPostedText(response.data.text);

                // Optionally, you can clear the form here or give feedback to the user
            } else {
                setRequestError('Error: Failed to save card data');
            }
        } catch (error) {
            if (error.response && error.response.status === 409) {
                // Handle the case where the file already exists
                setRequestError('Error: The file "CardInfo.txt" already exists.');
            } else {
                // Handle other errors
                setRequestError('Error: An unexpected error occurred. Error message: ' + error.message);
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
            <Card variant="outlined" sx={{ width: '100%', height: 'auto', minHeight: '30vh' }}>
                <CardContent>
                    {/* Existing input fields and logic */}
                    <Typography gutterBottom sx={{ color: 'text.secondary', fontSize: 14 }}>
                        Enter the Id of an exsisting Deck
                    </Typography>
                    <TextField
                        fullWidth
                        variant="outlined"
                        onChange={handleCardIdField}
                        label="Deck Id"
                        sx={{
                            marginBottom: '16px',
                        }}
                        value={cardIdField}  // Reset field on form submission
                        error={Boolean(deckIdError)}
                        helperText={deckIdError}
                    />
                    <Typography gutterBottom sx={{ color: 'text.secondary', fontSize: 14 }}>
                        Enter Tag Names (Each Subsequent Tag Must Be Divided By ';')
                    </Typography>

                    <TagSelector setSelectedTags={setSelectedTags} />

                    <Typography gutterBottom sx={{ color: 'text.secondary', fontSize: 14 }}>
                        Enter Text For Card
                    </Typography>
                    <TextField
                        fullWidth
                        multiline
                        variant="outlined"
                        onChange={handleDynamicChange}
                        label="Write something"
                        InputProps={{
                            style: { fontSize: fontSize, zIndex: 10000 },
                        }}
                        value={dynamicInputValue}  // Reset field on form submission
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

                    {/* Display the posted tags and text */}
                    {postedTags && (
                        <Box sx={{ marginTop: '16px' }}>
                            <Typography sx={{ fontWeight: 'bold', marginBottom: '8px' }}>Posted Tags:</Typography>
                            <Typography>{selectedTags}</Typography>
                        </Box>
                    )}

                    {postedText && (
                        <Box sx={{ marginTop: '16px' }}>
                            <Typography sx={{ fontWeight: 'bold', marginBottom: '8px' }}>Posted Text:</Typography>
                            <Typography sx={{ whiteSpace: 'pre-line' }}>
                                {postedText}
                            </Typography>
                        </Box>
                    )}
                </CardContent>
                <CardActions>
                    <Button size="medium" onClick={handleCreate} variant="contained" sx={{ backgroundColor: '#7FFFD4' }}>
                        Create
                    </Button>
                </CardActions>
            </Card>
        </Box>
    );
}

