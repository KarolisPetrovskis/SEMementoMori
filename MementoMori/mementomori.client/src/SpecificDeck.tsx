import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { Box, Typography, Paper, Button, Divider } from '@mui/material';

const FileDisplay: React.FC = () => {
    const [cards, setCards] = useState<string[]>([]);
    const [currentCardIndex, setCurrentCardIndex] = useState(0); // To track the current card
    const [error, setError] = useState<string | null>(null);

    // Fetch file content from the backend
    useEffect(() => {
        const fetchFileContent = async () => {
            try {
                const response = await axios.get('/CardFile/getFileContent');
                setCards(response.data);
            } catch (err) {
                setError('Error fetching file content.');
                console.error(err);
            }
        };

        fetchFileContent();
    }, []);


    {/*

    // Function to extract card content from the file
    const extractCards = () => {
        const cards: string[] = [];
        let isCard = false;
        let card = '';
        let skipNextLine = false;

        fileContent.forEach((line) => {
            if (line.includes('(Start)')) {
                isCard = true;
                card = '';
                skipNextLine = true;
                return;
            }

            if (skipNextLine) {
                // Now skip this line (which contains the cardId)
                skipNextLine = false;
                return;
            }

            if (isCard) {
                if (!line.includes('(End)')) {
                    // Add all lines except '(Start)' and '(End)' and the cardId line
                    card += line + '\n';
                }
            }

            if (line.includes('(End)')) {
                isCard = false;
                // Push the card into array after the "(End)" line
                cards.push(card.trim());
            }
        });

        return cards;
    };

    const cards = extractCards(); // Get all card contents


    */}

    // Function to move to the next card
    const handleNext = () => {
        if (currentCardIndex < cards.length - 1) {
            setCurrentCardIndex(currentCardIndex + 1);
        }
    };

    // Function to move to the previous card
    const handlePrevious = () => {
        if (currentCardIndex > 0) {
            setCurrentCardIndex(currentCardIndex - 1);
        }
    };

    return (
        <Box sx={{ padding: '16px', display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
            {error ? (
                <Typography color="error">{error}</Typography>
            ) : (
                <Paper sx={{ padding: '16px', maxWidth: '80vw', marginBottom: '16px', bgcolor: '#dce9fa' }} elevation={8}>
                    <Typography variant="h6" gutterBottom>
                        <b>Card {currentCardIndex + 1} of {cards.length}</b>
                    </Typography>

                    {/* Add Divider between the header and card content */}
                    <Divider sx={{ marginY: '12px', borderBottomWidth: 3, bgcolor: '#245dab' }} />
                    {cards.length > 0 ? (
                        <Typography component="pre" sx={{ whiteSpace: 'pre-wrap', fontFamily: 'monospace' }}>
                            {cards[currentCardIndex]}
                        </Typography>
                    ) : (
                        <Typography>No content found in the file.</Typography>
                    )}
                </Paper>
            )}
            {/* Navigation Buttons */}
            <Box sx={{ display: 'flex', justifyContent: 'space-between', width: '50%' }}>
                <Button variant="contained" onClick={handlePrevious} disabled={currentCardIndex === 0}>
                    Previous
                </Button>
                <Button variant="contained" onClick={handleNext} disabled={currentCardIndex === cards.length - 1}>
                    Next
                </Button>
            </Box>
        </Box>
    );
};

export default FileDisplay