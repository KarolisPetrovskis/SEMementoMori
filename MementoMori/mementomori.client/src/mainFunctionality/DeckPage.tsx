import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';


interface Card {
  id: string;
  question: string;
  description?: string | null;
  answer: string;
}

export default function DeckPage() {
  const { deckId } = useParams<{ deckId: string }>(); 
  const [cards, setCards] = useState<Card[]>([]); 
  const [currentIndex, setCurrentIndex] = useState(0); 
  const [currentCard, setCurrentCard] = React.useState<Card>()

  useEffect(() => {
    const fetchCards = async () => {
      try {
        const response = await fetch(
          `https://localhost:5173/decks/${deckId}/cards`
        );
        const text = await response.text();
        console.log('Raw Response:', text); 

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

  const handleNext = () => {
    setCurrentIndex((prevIndex) => (prevIndex + 1) % cards.length); 
  };

  const handlePrev = () => {
    setCurrentIndex(
      (prevIndex) => (prevIndex - 1 + cards.length) % cards.length
    ); 
  };

  React.useEffect(()=> {setCurrentCard(cards[currentIndex])}, [cards, currentIndex])

  React.useEffect(()=> {console.log('c', currentCard)}, [currentCard])

  return (
    cards.length && currentCard ? (
    <div>
      <div className="card-container">
        <div className="card">
          <h2>{currentCard.question}</h2>
          <p>{currentCard.description}</p>
          <p>
            <strong>Answer: </strong> 
            {currentCard.answer ?? "No"}
          </p>
        </div>
        <div className="navigation">
          <button onClick={handlePrev}>Previous</button>
          <button onClick={handleNext}>Next</button>
        </div>
      </div>
    </div>) : <div>Loading cards...</div>
  );
};

