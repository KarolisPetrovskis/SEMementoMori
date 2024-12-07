import React, { useEffect, useState } from 'react';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import ListItemText from '@mui/material/ListItemText';
import SettingsIcon from '@mui/icons-material/Settings';
import IconButton from '@mui/material/IconButton';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import Divider from '@mui/material/Divider';
import DeckCreateButton from '../DeckCreateButton.tsx';
import axios from 'axios';
import { Box } from '@mui/material';

interface Deck {
  id: string;
  title: string;
}
interface UserInformationResponse {
  isLoggedIn: boolean;
  decks: Deck[] | null;
}
export default function DeckMenu() {
  const [isLoggedOn, setIsLoggedOn] = useState<boolean>(false);
  const [decks, setDecks] = useState<Deck[] | null>(null);
  const data = [
    { id: 1, name: 'Deck 1' },
    { id: 2, name: 'Deck 2' },
    { id: 3, name: 'Deck 3' },
    { id: 4, name: 'Deck 4' },
    { id: 5, name: 'Deck 5' },
    { id: 6, name: 'Deck 6' },
    { id: 7, name: 'Deck 7' },
  ];

  const secondData = [
    { id: 1, name: 'List 1' },
    { id: 2, name: 'List 2' },
    { id: 3, name: 'List 3' },
    { id: 4, name: 'List 4' },
    { id: 5, name: 'List 5' },
  ];

  useEffect(() => {
    async function fetchDeck() {
      try {
        const response = await axios.get<UserInformationResponse>(
          `/auth/userInformation`
        );
        setIsLoggedOn(response.data.isLoggedIn);
        setDecks(response.data.decks);
      } catch (error) {
        console.error('Error fetching deck:', error);
      }
      console.log(decks);
      console.log(isLoggedOn);
    }
    fetchDeck();
  }, []);

  return (
    <Box sx={{ boxSizing: 'border-box', width: '30%' }}>
      <Typography
        variant="h6"
        sx={{
          color: 'black',
          position: 'absolute',
          top: 60,
          left: 106,
          background: 'white',
          padding: '15px 0px 15px 0px',
          width: '30%', // Total width of the Decks header
          border: 1,
          borderTopLeftRadius: '6px',
          borderTopRightRadius: '6px',
          borderBottomRightRadius: '0px',
          borderBottomLeftRadius: '0px',
          borderColor: '#D4A017',
          borderWidth: 2,
          boxSizing: 'border-box',
        }}
      >
        Decks
      </Typography>
      <Box
        sx={{
          position: 'absolute',
          left: 106,
          top: 121,
          width: '30%',
          display: 'flex',
        }}
      >
        {/* First Column */}
        <Box
          sx={{
            flex: 1,
            border: '2px solid #D4A017',
            backgroundColor: 'white',
            display: 'flex',
            flexDirection: 'column',
            height: '100%',
            boxSizing: 'border-box',
            borderRight: 'none',
          }}
        >
          <List
            sx={{
              flex: 1,
              overflowY: 'auto',
              maxHeight: '400px',
            }}
          >
            {data.length > 0 ? (
              data.map((deck, index) => (
                <React.Fragment key={deck.id}>
                  <ListItem disableGutters>
                    <Button
                      variant="text"
                      sx={{ cursor: 'pointer' }}
                      onClick={() => {
                        console.log(`Line item ${deck.name} clicked`);
                      }}
                    >
                      <ListItemText
                        primary={deck.name}
                        primaryTypographyProps={{
                          sx: {
                            color: 'black',
                          },
                        }}
                      />
                    </Button>
                    <IconButton
                      aria-label="settings"
                      sx={{ cursor: 'pointer' }}
                      onClick={() => {
                        console.log(`Settings of ${deck.name} clicked`);
                      }}
                      style={{ marginLeft: 'auto' }}
                    >
                      <SettingsIcon />
                    </IconButton>
                  </ListItem>
                  {index !== data.length - 1 && <Divider />}
                </React.Fragment>
              ))
            ) : (
              <ListItem>
                <ListItemText
                  primary="No decks in collection"
                  sx={{ color: 'black', fontSize: 20 }}
                />
              </ListItem>
            )}
          </List>
        </Box>

        {/* Border between the columns */}
        <Box
          sx={{
            width: '1px',
            backgroundColor: '#D4A017',
            height: '100%',
          }}
        />

        {/* Second Column */}
        {isLoggedOn ? (
          <Box
            sx={{
              flex: 1,
              border: '2px solid #D4A017',
              backgroundColor: 'white',
              display: 'flex',
              flexDirection: 'column',
              height: '100%',
              boxSizing: 'border-box',
            }}
          >
            <List
              sx={{
                flex: 1,
                overflowY: 'auto',
                maxHeight: '400px',
              }}
            >
              {decks && decks.length > 0 ? (
                decks.map((deck, index) => (
                  <React.Fragment key={deck.id}>
                    <ListItem
                      disableGutters
                      sx={{
                        paddingY: 1,
                        cursor: 'pointer',
                        display: 'flex',
                        alignItems: 'center',
                        transition: 'background-color 0.3s, box-shadow 0.3s',
                        borderRadius: '8px',
                        '&:hover': {
                          backgroundColor: 'rgba(0, 0, 0, 0.075)',
                          boxShadow: '0px 4px 12px rgba(0, 0, 0, 0.2)',
                        },
                      }}
                      onClick={() => {
                        window.location.href = `https://localhost:5173/decks/${deck.id}`;
                      }}
                    >
                      <ListItemText
                        primary={deck.title}
                        primaryTypographyProps={{
                          style: {
                            overflow: 'hidden',
                            textOverflow: 'ellipsis',
                            whiteSpace: 'nowrap',
                          },
                        }}
                        sx={{ padding: '3px 5px 0px 15px' }}
                      />
                    </ListItem>
                    {index !== decks.length - 1 && <Divider />}
                  </React.Fragment>
                ))
              ) : (
                <Typography
                  sx={{
                    padding: '20px',
                    textAlign: 'center',
                    color: '#D4A017',
                    fontSize: 16,
                  }}
                >
                  No decks available in your collection
                </Typography>
              )}
            </List>
            <div
              style={{
                padding: '15px 6px 15px 6px',
                borderTop: '1px solid #D4A017',
                display: 'flex',
                justifyContent: 'center',
              }}
            >
              <DeckCreateButton />
            </div>
          </Box>
        ) : null}
      </Box>
    </Box>
  );
}
