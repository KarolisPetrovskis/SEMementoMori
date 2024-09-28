import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Buttonn from './button.tsx';
import './App.css';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';

const client = new QueryClient();

function App() {
    return (
        <QueryClientProvider client={client}>
            <BrowserRouter>
                <Routes>
                    <Route path="/" element={<Buttonn />} />
                </Routes>
            </BrowserRouter>
        </QueryClientProvider>
    );
}

export default App;