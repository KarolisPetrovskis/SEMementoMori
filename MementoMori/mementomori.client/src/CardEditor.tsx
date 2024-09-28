import * as React from 'react';
import Box from '@mui/material/Box';
import Card from '@mui/material/Card';
import CardActions from '@mui/material/CardActions';
import CardContent from '@mui/material/CardContent';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import TextField from '@mui/material/TextField';
import { TagValidator } from './Validator';

export default function OutlinedCard() {
    const [dynamicInputValue, setDynamicInputValue] = React.useState(''); // State for dynamic text field
    const [staticInputValue, setStaticInputValue] = React.useState(''); // State for static text field
    const [fontSize, setFontSize] = React.useState(24); // Initial font size for dynamic field
    const [tagError, setTagError] = React.useState(''); // State for tag errors
    const [dynamicTextError, setDynamicTextError] = React.useState(''); // State for dynamic text errors

    // Handle change for the dynamic text field with increasing height and dynamic font size
    const handleDynamicChange = (event: { target: { value: any } }) => {
        const value = event.target.value;
        setDynamicInputValue(value);

        // Remove newlines from length calculation for dynamic font size
        const textLength = value.replace(/\n/g, '').length;

        // Adjust font size dynamically based on length of input
        if (textLength < 100) {
            setFontSize(24); // Larger font for shorter text
        } else if (textLength < 250) {
            setFontSize(18); // Medium font for moderate text
        } else {
            setFontSize(14); // Smaller font for longer text
        }
    };

    // Handle change for the static text field
    const handleStaticChange = (event: { target: { value: any } }) => {
        const value = event.target.value;
        setStaticInputValue(value);
    };

    // Use the TagValidator class to validate the tags
    const validateTags = (tags: string) => {
        const validator = new TagValidator();
        validator.setTags(tags);
        const validationError = validator.returnError();
        return validationError;
    };

    // Validate the dynamic text field
    const validateDynamicText = (text: string) => {
        if (!text.trim()) {
            return 'Error: Dynamic text cannot be empty.';
        }
        return '';
    };

    // Handle saving data to a local file with validation
    const handleCreate = () => {
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

        const Prefix = '!Start!';
        const Suffix = '!End!';
        const fileContent = `${Prefix}\nTags: ${staticInputValue}\nText: ${dynamicInputValue}\n${Suffix}`;

        const blob = new Blob([fileContent], { type: 'text/plain' });
        const link = document.createElement('a');
        link.href = URL.createObjectURL(blob);
        link.download = 'inputData.txt';
        link.click();

        // Clear any previous errors after successful save
        setTagError('');
        setDynamicTextError('');
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
                        helperText={tagError} // Show validation error for tags
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
                            style: { fontSize: fontSize }, // Dynamic font size for the dynamic field
                        }}
                        sx={{
                            overflowY: 'auto', // Allow scrolling if input gets too large
                            resize: 'none', // Disable manual resizing
                            marginBottom: '16px', // Add some spacing between text fields
                        }}
                        error={Boolean(dynamicTextError)}
                        helperText={dynamicTextError} // Show validation error for dynamic text
                    />
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
