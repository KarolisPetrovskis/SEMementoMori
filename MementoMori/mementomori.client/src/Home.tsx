import './App.css';
import QuestList from './components/QuestList.tsx';
import MainMenu from './components/MainMenu.tsx';
import DeckMenu from './components/DeckMenu.tsx';

function Home() {
  return (
    <>
      <DeckMenu />
      <QuestList />
      <MainMenu />
    </>
  );
}

export default Home;
