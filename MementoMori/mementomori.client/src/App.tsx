import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Home from './Home';
import Browser from './deckBrowser/Browser';
import './App.css';
import DeckPage from './mainFunctionality/DeckPage';
import SpecificDeck from './SpecificDeck.tsx';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { Deck } from './decks/Deck.tsx';
import EditDeck from './Editor/DeckEditor.tsx';

const client = new QueryClient();

function App() {
  return (
    <QueryClientProvider client={client}>
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/decks/:deckId/edit" element={<EditDeck />} />
          <Route path="/decks/:deckId" element={<Deck />} />
          <Route path="/decks/:deckId/practice" element={<DeckPage />} />
          <Route path="/browser" element={<Browser />} />
          <Route path="/chosenDeck" element={<SpecificDeck />} />
        </Routes>
      </BrowserRouter>
    </QueryClientProvider>
  );
}

export default App;
