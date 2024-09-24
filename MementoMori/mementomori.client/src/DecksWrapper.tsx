import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import BasicCard from './MainFunctionality.tsx'
import './index.css'
import Deck from './Deck.tsx'

createRoot(document.getElementById('root')!).render(
    <StrictMode>
        <BasicCard />
    </StrictMode>,
)