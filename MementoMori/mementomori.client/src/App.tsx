import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Home from './Home';
import Broswer from './deckBrowser/Browser';
import './App.css';
import DeckPage from './mainFunctionality/DeckPage';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';

const client = new QueryClient();

function App() {
  return (
    <QueryClientProvider client={client}>
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/browser" element={<Broswer />} />
          <Route path="/decks/:deckId" element ={<DeckPage />} />
        </Routes>
      </BrowserRouter>
    </QueryClientProvider>
  );
}

export default App;
