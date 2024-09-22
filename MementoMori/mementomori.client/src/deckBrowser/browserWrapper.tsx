import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import Browser from './Browser';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';

const client = new QueryClient();

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <QueryClientProvider client={client}>
      <Browser />
    </QueryClientProvider>
  </StrictMode>
);
