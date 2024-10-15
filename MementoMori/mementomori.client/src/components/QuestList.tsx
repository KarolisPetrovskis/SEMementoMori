import * as React from 'react';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import ListItemText from '@mui/material/ListItemText';
import ListItemAvatar from '@mui/material/ListItemAvatar';
import Avatar from '@mui/material/Avatar';
import Typography from '@mui/material/Typography';
import AssignmentIcon from '@mui/icons-material/Assignment';
import CheckCircleIcon from '@mui/icons-material/CheckCircle';
import { useEffect, useState } from 'react';
import axios from 'axios';

// Define the Quest interface
interface Quest {
  id: string;
  title: string;
  description: string;
  valueNeeded: number;
  reward: string;
  color?: string; // Optional color for Avatar
  status?: string; // Optional status property
}

export default function QuestList() {
  const [questData, setQuestData] = useState<Quest[]>([]);
  const [isComplete, setIsComplete] = useState(false);
  const [quests, setQuests] = useState<JSX.Element[]>([]);

  useEffect(() => {
    const fetchQuests = async () => {
      try {
        const response = await axios.get('/quests.json');
        setQuestData(response.data);

        // Fetch isComplete status (separate API call)
        const isCompleteResponse = await axios.get(
          '/QuestController/isComplete'
        );
        setIsComplete(isCompleteResponse.data);
      } catch (error) {
        console.error('Error fetching quests:', error);
      }
    };

    fetchQuests();
  }, []);

  useEffect(() => {
    if (questData) {
      setQuests(
        questData.map((quest) => {
          return (
            <ListItem key={quest.id} alignItems="center">
              <ListItemAvatar>
                <Avatar>
                  <AssignmentIcon />
                </Avatar>
              </ListItemAvatar>
              <ListItemText
                primary={quest.title}
                sx={{ color: '#6be0fe' }}
                secondary={
                  <React.Fragment>
                    <Typography
                      component="span"
                      variant="body2"
                      sx={{ color: '#ecffff', display: 'inline' }}
                    >
                      {quest.description}
                    </Typography>
                    {' — ' + quest.reward}
                  </React.Fragment>
                }
              />
              {isComplete && quest.id === 1 && (
                <CheckCircleIcon color="success" />
              )}
            </ListItem>
          );
        })
      );
    }
  }, [questData]);

  return (
    <List
      sx={{
        width: '100%',
        maxWidth: 360,
        bgcolor: '#6E6DB3',
        position: 'absolute',
        border: 1,
        borderRadius: '16px',
        borderColor: '#D4A017',
        borderWidth: 2,
        right: 0,
        top: 200,
        overflowY: 'auto', // Enable scrolling if there are many quests
      }}
    >
      {quests}
    </List>
  );
}
