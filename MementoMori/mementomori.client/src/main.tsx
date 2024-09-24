import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import "./index.css";
import QuestList from "./Components/QuestList.tsx";
import MainMenu from "./Components/MainMenu.tsx";
import Title from "./Components/Title.tsx";

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <Title />
    <QuestList />
    <MainMenu />
  </StrictMode>
);
