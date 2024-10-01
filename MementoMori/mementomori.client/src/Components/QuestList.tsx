import * as React from "react";
import List from "@mui/material/List";
import ListItem from "@mui/material/ListItem";

import ListItemText from "@mui/material/ListItemText";
import ListItemAvatar from "@mui/material/ListItemAvatar";
import Avatar from "@mui/material/Avatar";
import Typography from "@mui/material/Typography";
import AssignmentIcon from "@mui/icons-material/Assignment";

import { useEffect, useState } from "react";
import fs from "fs";

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
        const readableStream = fs.createReadStream("quests.json");

        readableStream.on("data", (chunk) => {
          const quests: Quest[] = JSON.parse(chunk.toString()); // Cast to Quest[]
          console.log(quests); // Log the fetched quests
          setQuestData(quests);
        });

        readableStream.on("end", () => {
          console.log("Quest data loaded.");
        });
      } catch (error) {
        console.error("Error fetching quests:", error);
      }
    };

    fetchQuests();
  }, []);

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
      {/* Map over quest data and render each quest */}
      {questData.map((quest) => (
        <ListItem key={quest.id} alignItems="center">
          console.log("1")
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
      ))}
    </List>
  );
}
