import './App.css';
import QuestList from './homePage/QuestList.tsx';
import MainHeader from './homePage/MainHeader.tsx';
import DeckMenu from './homePage/DeckMenu.tsx';

function Home() {
  return (
    <>
      <DeckMenu />
      <QuestList />
    </>
  );
}

export default Home;
