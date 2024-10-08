import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Home from './Home';
import Browser from './deckBrowser/Browser';
import './App.css';
import DeckPage from './mainFunctionality/DeckPage';
import Cards from './CardCreator.tsx';
import SpecificDeck from './SpecificDeck.tsx';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';

const client = new QueryClient();

function App() {
  return (
    <QueryClientProvider client={client}>
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/decks/:deckId" element={<DeckPage />} />
          <Route path="/browser" element={<Browser />} />
          <Route path="/cards" element={<Cards />} />
          <Route path="/001" element={<SpecificDeck />} />
        </Routes>
      </BrowserRouter>
    </QueryClientProvider>
  );
}

export default App;
