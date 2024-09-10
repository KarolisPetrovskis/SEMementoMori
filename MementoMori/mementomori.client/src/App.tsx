import * as React from 'react';
import Box from '@mui/material/Box';
import Card from '@mui/material/Card';
import CardActions from '@mui/material/CardActions';
import CardContent from '@mui/material/CardContent';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import TextField from '@mui/material/TextField'; // Import TextField

export default function OutlinedCard() {
    const [dynamicInputValue, setDynamicInputValue] = React.useState(''); // State for dynamic text field
    const [staticInputValue, setStaticInputValue] = React.useState(''); // State for static text field
    const [fontSize, setFontSize] = React.useState(24); // Initial font size for dynamic field

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
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    const handleStaticChange = (event: { target: { value: any } }) => {
        const value = event.target.value;
        setStaticInputValue(value);
    };

    const handleCreate = () => {
        console.log("Created Text Input:", dynamicInputValue);
        console.log("Created Tag Input:", staticInputValue);
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
                        value={staticInputValue}
                        onChange={handleStaticChange}
                        label="Tags"
                        sx={{
                            marginBottom: '16px', // Add some spacing
                        }}
                    />
                    <Typography gutterBottom sx={{ color: 'text.secondary', fontSize: 14 }}>
                        Enter Text For Card
                    </Typography>
                    <TextField
                        fullWidth
                        multiline
                        variant="outlined"
                        value={dynamicInputValue}
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
                    />
                </CardContent>
                <CardActions>
                    <Button size="small" onClick={handleCreate} sx={{ backgroundColor: '#7FFFD4' }}>
                        Create
                    </Button>
                </CardActions>
            </Card>
        </Box>
    );
}
