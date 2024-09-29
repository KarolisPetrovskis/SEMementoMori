import * as React from 'react';
import Box from '@mui/material/Box';
import Card from '@mui/material/Card';
import CardActions from '@mui/material/CardActions';
import CardContent from '@mui/material/CardContent';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import TextField from '@mui/material/TextField';
import { TagValidator } from './Validator';

import axios from 'axios';

export default function OutlinedCard() {
    const [dynamicInputValue, setDynamicInputValue] = React.useState('');
    const [staticInputValue, setStaticInputValue] = React.useState('');
    const [fontSize, setFontSize] = React.useState(24); 
    const [tagError, setTagError] = React.useState(''); 
    const [dynamicTextError, setDynamicTextError] = React.useState(''); 
    const [requestError, setRequestError] = React.useState(''); 

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

    // Handle change for the static text field
    const handleStaticChange = (event: { target: { value: any } }) => {
        const value = event.target.value;
        setStaticInputValue(value);
    };

    const validateTags = (tags: string) => {
        const validator = new TagValidator();
        validator.setTags(tags);
        const validationError = validator.returnError();
        return validationError;
    };

    const validateDynamicText = (text: string) => {
        if (!text.trim()) {
            return 'Error: Dynamic text cannot be empty.';
        }
        return '';
    };

    // Send form data to backend server
    const handleCreate = async () => {
        const tagValidationError = validateTags(staticInputValue);
        if (tagValidationError) {
            setTagError(tagValidationError);
            return;
        } else {
            setTagError('');
        }

        const dynamicTextValidationError = validateDynamicText(dynamicInputValue);
        if (dynamicTextValidationError) {
            setDynamicTextError(dynamicTextValidationError);
            return;
        } else {
            setDynamicTextError('');
        }

        try {
            // Create an object to send to the backend
            const cardData = {
                tags: staticInputValue,
                text: dynamicInputValue,
            };

            // Send POST request to the server
            const response = await axios.post('http://localhost:5001/CardData/postCards', cardData);

            if (response.status === 200) {
                console.log("Card data successfully saved");
                // Optionally, you can clear the form here or give feedback to the user
            } else {
                setRequestError('Error: Failed to save card data');
            }
        } catch (error) {
            console.error(error);
            setRequestError('Error: An unexpected error occurred. Error message: ' + error.message);
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
                    <Typography gutterBottom sx={{ color: 'text.secondary', fontSize: 14 }}>
                        Enter Tag Names (Each Subsequent Tag Must Be Divided By ';')
                    </Typography>
                    <TextField
                        fullWidth
                        variant="outlined"
                        onChange={handleStaticChange}
                        label="Tags"
                        sx={{
                            marginBottom: '16px',
                        }}
                        error={Boolean(tagError)}
                        helperText={tagError}
                    />
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
                            style: { fontSize: fontSize },
                        }}
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

                </CardContent>
                <CardActions>
                    <Button size="medium" onClick={handleCreate} sx={{ backgroundColor: '#7FFFD4' }}>
                        Create
                    </Button>
                </CardActions>
            </Card>
        </Box>
    );
}

