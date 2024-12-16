import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';

interface Card {
  id: string;
  question: string;
  description?: string | null;
  answer: string;
  lastReviewed?: string;
}

export default function DeckPage() {
  const { deckId } = useParams<{ deckId: string }>();
  const [cards, setCards] = useState<Card[]>([]);
  const [currentIndex, setCurrentIndex] = useState(0);
  const [currentCard, setCurrentCard] = useState<Card | null>(null);
  const [showAnswer, setShowAnswer] = useState(false);

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

  useEffect(() => {
    if (cards.length > 0) {
      setCurrentCard(cards[currentIndex]);
    } else {
      setCurrentCard(null);
    }
  }, [cards, currentIndex]);

  const handleNext = () => {
    setShowAnswer(false); // Hide answer for next card
    setCurrentIndex((prevIndex) => (prevIndex + 1) % cards.length);
  };

  const handlePrev = () => {
    setShowAnswer(false); // Hide answer for previous card
    setCurrentIndex(
      (prevIndex) => (prevIndex - 1 + cards.length) % cards.length
    );
  };

  const handleQualitySubmit = async (quality: number) => {
    if (!currentCard) return;

    try {
      const response = await fetch(
        `https://localhost:5173/Decks/${deckId}/cards/update/${currentCard.id}`,
        {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify(quality),
        }
      );

      if (response.ok) {
        console.log(`Successfully updated card with quality: ${quality}`);

        // Remove the current card from the list
        setCards((prevCards) =>
          prevCards.filter((card) => card.id !== currentCard.id)
        );
      } else {
        console.error('Failed to update card');
      }
    } catch (error) {
      console.error('Error updating card', error);
    }
  };

  return (
    <div>
      {cards.length > 0 && currentCard ? (
        <div className='card-container'>
          <div className='card'>
            <h2>{currentCard.question}</h2>
            <p>{currentCard.description}</p>

            {showAnswer ? (
              <p>
                <strong>Answer: </strong>
                {currentCard.answer ?? 'No answer provided'}
              </p>
            ) : (
              <button onClick={() => setShowAnswer(true)}>Reveal Answer</button>
            )}

            <div className='navigation'>
              <button onClick={handlePrev}>Previous</button>
              <button onClick={handleNext}>Next</button>
            </div>

            <div className='spaced-repetition-buttons'>
              <button
                onClick={() => {
                  setShowAnswer(true);
                  handleQualitySubmit(1);
                }}
              >
                Forgot
              </button>
              <button
                onClick={() => {
                  setShowAnswer(true);
                  handleQualitySubmit(3);
                }}
              >
                Good
              </button>
              <button
                onClick={() => {
                  setShowAnswer(true);
                  handleQualitySubmit(5);
                }}
              >
                Easy
              </button>
            </div>
          </div>
        </div>
      ) : (
        <div>No cards due for review are left</div>
      )}
    </div>
  );
}
