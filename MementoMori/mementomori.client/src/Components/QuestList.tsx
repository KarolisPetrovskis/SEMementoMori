import * as React from "react";
import List from "@mui/material/List";
import ListItem from "@mui/material/ListItem";

import ListItemText from "@mui/material/ListItemText";

import ListItemAvatar from "@mui/material/ListItemAvatar";
import Avatar from "@mui/material/Avatar";
import Typography from "@mui/material/Typography";
import AssignmentIcon from "@mui/icons-material/Assignment";

import { useEffect, useState } from "react";
import axios from "axios";

// Define the Quest interface
interface Quest {
  id: number; // Or string depending on your data format
  title: string;
  description: string;
  reward: string;
  color?: string; // Optional color for Avatar
  status?: string; // Optional status property
}

export default function QuestList() {
  const [questData, setQuestData] = useState<Quest[]>([]);

  useEffect(() => {
    const fetchQuests = async () => {
      try {
        const response = await axios.get("/Quests.json"); // Replace with actual endpoint
        const quests: Quest[] = response.data; // Cast to Quest[]
        setQuestData(quests);
      } catch (error) {
        console.error("Error fetching quests:", error);
      }
    };

    fetchQuests();
  }, []);

  const renderedQuests: React.ReactNode[] = []; // Explicitly annotate type

  questData.forEach((quest) => {
    renderedQuests.push(
      <ListItem key={quest.id} alignItems="center">
        <ListItemAvatar>
          <Avatar>
            <AssignmentIcon />
          </Avatar>
        </ListItemAvatar>
        <ListItemText
          primary={quest.title}
          sx={{ color: "#6be0fe" }}
          secondary={
            <React.Fragment>
              <Typography
                component="span"
                variant="body2"
                sx={{ color: "#ecffff", display: "inline" }}
              >
                {quest.description}
              </Typography>
              {" — " + quest.reward}
            </React.Fragment>
          }
        />
      </ListItem>
    );
  });

  return (
    <List
      sx={{
        width: "100%",
        maxWidth: 360,
        bgcolor: "#6E6DB3",
        position: "absolute",
        border: 1,
        borderRadius: "16px",
        borderColor: "#D4A017",
        borderWidth: 2,
        right: 0,
        top: 200,
        overflowY: "auto", // Enable scrolling if there are many quests
      }}
    >
      {renderedQuests}
    </List>
  );
}
