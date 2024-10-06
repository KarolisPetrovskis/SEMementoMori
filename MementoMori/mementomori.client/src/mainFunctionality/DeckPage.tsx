import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';

// Define the Card interface based on the C# model
interface Card {
    id: string;
    question: string;
    description?: string;
    anwser: string;
    lastInterval: number;
    nextShow: string; // ISO-formatted date string
}

const DeckPage: React.FC = () => {
    const { deckId } = useParams<{ deckId: string }>(); // Extract deckId from URL
    const [cards, setCards] = useState<Card[]>([]); // State to hold the deck's cards
    const [currentIndex, setCurrentIndex] = useState(0); // Track which card is being displayed

    // Fetch the cards from the backend API
    // useEffect(() => {
    //     const fetchCards = async () => {
    //         try {
    //             const response = await fetch(`https://localhost:5173/api/decks/${deckId}/cards`);
    //             const data: Card[] = await response.json();
    //             setCards(data);
    //         } catch (error) {
    //             console.error('Failed to fetch cards', error);
    //         }
    //     };
    useEffect(() => {
        const fetchCards = async () => {
            try {
                const response = await fetch(`https://localhost:5001/api/decks/${deckId}/cards`);
                const text = await response.text(); // Get raw response as text
                console.log('Raw Response:', text); // Log the raw response
    
                // Try parsing the text as JSON, if it is a valid JSON string
                try {
                    const data = JSON.parse(text);
                    setCards(data);
                } catch (jsonError) {
                    console.error('Failed to parse JSON:', jsonError);
                }
    
            } catch (error) {
                console.error('Failed to fetch cards', error);
            }
        };
    
        fetchCards();
    }, [deckId]);

    // Handler to go to the next card
    const handleNext = () => {
        setCurrentIndex((prevIndex) => (prevIndex + 1) % cards.length); // Wrap around
    };

    // Handler to go to the previous card
    const handlePrev = () => {
        setCurrentIndex((prevIndex) => (prevIndex - 1 + cards.length) % cards.length); // Wrap around
    };

    // Show loading or no cards available message
    if (cards.length === 0) {
        return <div>Loading cards...</div>;
    }

    // Get the current card being displayed
    const currentCard = cards[currentIndex];

    return (
        <div>
            <h1>Deck ID: {deckId}</h1>

            <div className="card-container">
                <div className="card">
                    <h2>{currentCard.question}</h2>
                    <p>{currentCard.description}</p>
                    <p><strong>Answer:</strong> {currentCard.anwser}</p>
                </div>
                <div className="navigation">
                    <button onClick={handlePrev}>Previous</button>
                    <button onClick={handleNext}>Next</button>
                </div>
            </div>
        </div>
    );
};

export default DeckPage;