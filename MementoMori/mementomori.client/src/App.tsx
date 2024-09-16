import * as React from 'react';
import Box from '@mui/material/Box';
import Card from '@mui/material/Card';
import CardActions from '@mui/material/CardActions';
import CardContent from '@mui/material/CardContent';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import TextField from '@mui/material/TextField'; // Import TextField

export default function OutlinedCard() {
    const [inputValue, setInputValue] = React.useState(''); // State for input value
    const [fontSize, setFontSize] = React.useState(24); // Initial font size

    const handleChange = (event) => {
        const value = event.target.value;
        setInputValue(value);

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

    const handleCreate = () => {
        console.log("Created:", inputValue); // Action for Create button
    };

    return (
        <Box
            sx={{
                minWidth: '50vw',
                display: 'flex',
                justifyContent: 'center',
                alignItems: 'rigth',
                margin: 'auto',
            }}
        >
            <Card variant="outlined" sx={{ width: '100%', height: 'auto', minHeight: '20vh' }}>
                <CardContent>
                    <Typography gutterBottom sx={{ color: 'text.secondary', fontSize: 14 }}>
                        Enter Text
                    </Typography>
                    <TextField
                        fullWidth
                        multiline
                        variant="outlined"
                        value={inputValue}
                        onChange={handleChange}
                        label="Write something"
                        InputProps={{
                            style: { fontSize: fontSize }, // Dynamic font size
                        }}
                        sx={{
                            overflowY: 'auto', // Allow scrolling if input gets too large
                            resize: 'none', // Disable manual resizing
                        }}
                    />
                </CardContent>
                <CardActions>
                    <Button size="small" onClick={handleCreate}>
                        Create
                    </Button>
                </CardActions>
            </Card>
        </Box>
    );
}
