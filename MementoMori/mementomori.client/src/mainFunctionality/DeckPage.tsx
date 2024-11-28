import React, { useState, useEffect } from "react";
import { useParams } from "react-router-dom";

interface Card {
  id: string;
  question: string;
  description?: string | null;
  answer: string;
  lastReviewed?: string; // Optional: For tracking spaced repetition dates
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
        console.log("Raw Response:", text);

        try {
          const data = JSON.parse(text);
          setCards(data);
        } catch (jsonError) {
          console.error("Failed to parse JSON:", jsonError);
        }
      } catch (error) {
        console.error("Failed to fetch cards", error);
      }
    };

    fetchCards();
  }, [deckId]);

  useEffect(() => {
    if (cards.length > 0) {
      setCurrentCard(cards[currentIndex]);
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
        //https://localhost:5173/cards/dbba9b7e-6571-4238-ab4c-7bfdae98eee2/cards/update/91cd03ee-8d08-4a15-a228-f4cd43294ae6
        //https://localhost:7122/Decks/dbba9b7e-6571-4238-ab4c-7bfdae98eee2/cards/update/dbba9b7e-6571-4238-ab4c-7bfdae98eee2
        `https://localhost:5173/Decks/${deckId}/cards/update/${currentCard.id}`,
        {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify({ quality }),
        }
      );

      if (response.ok) {
        console.log(`Successfully updated card with quality: ${quality}`);
        handleNext(); // Move to the next card after submitting quality
      } else {
        console.error("Failed to update card");
      }
    } catch (error) {
      console.error("Error updating card", error);
    }
  };

  return (
    <div>
      {cards.length && currentCard ? (
        <div className="card-container">
          <div className="card">
            <h2>{currentCard.question}</h2>
            <p>{currentCard.description}</p>

            {showAnswer && (
              <p>
                <strong>Answer: </strong>
                {currentCard.answer ?? "No answer provided"}
              </p>
            )}

            <div className="navigation">
              <button onClick={handlePrev}>Previous</button>
              <button onClick={handleNext}>Next</button>
            </div>

            <div className="spaced-repetition-buttons">
              <button
                onClick={() => {
                  setShowAnswer(true);
                  handleQualitySubmit(1);
                }}
              >
                Hard
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
        <div>Loading cards...</div>
      )}
    </div>
  );
}
