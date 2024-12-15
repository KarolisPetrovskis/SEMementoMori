import React, { useEffect, useState } from 'react';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import ListItemText from '@mui/material/ListItemText';
import IconButton from '@mui/material/IconButton';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import Divider from '@mui/material/Divider';
import DeckCreateButton from '../DeckCreateButton.tsx';
import axios from 'axios';
import { Box, Dialog } from '@mui/material';
import DeleteIcon from '@mui/icons-material/Delete';
import DialogTitle from '@mui/material/DialogTitle';
import DialogContent from '@mui/material/DialogContent';
import DialogActions from '@mui/material/DialogActions';

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
  const [collectionDecks, setCollectionDecks] = useState<Deck[] | null>(null);
  const [openDialog, setOpenDialog] = useState<boolean>(false);
  const [deckToDelete, setDeckToDelete] = useState<string | null>(null);

  async function fetchCollectionDecks() {
    try {
      const response = await axios.get<UserInformationResponse>(
        `/UserDecks/userCollectionDecksController`
      );
      setCollectionDecks(response.data.decks);
    } catch (error) {
      console.error('Error fetching decks in collection:', error);
    }
  }

  useEffect(() => {
    async function fetchDeck() {
      try {
        const response = await axios.get<UserInformationResponse>(
          `/UserDecks/userInformation`
        );
        setIsLoggedOn(response.data.isLoggedIn);
        setDecks(response.data.decks);
      } catch (error) {
        console.error('Error fetching deck:', error);
      }
    }
    fetchDeck();
    fetchCollectionDecks();
  }, []);

  const handleDelete = async () => {
    console.log('handleDelete: ', deckToDelete);
    if (deckToDelete) {
      try {
        await axios.post(`/UserDecks/userCollectionRemoveDeckController`, {
          Id: deckToDelete,
        });
        await fetchCollectionDecks(); // Refresh the collection decks
      } catch (error) {
        console.error('Error removing deck:', error);
      } finally {
        setDeckToDelete(null);
        setOpenDialog(false);
      }
    }
  };

  const handleOpenDialog = (deckId: string) => {
    console.log('handleOpenDialog: ', deckId);
    setDeckToDelete(deckId);
    setOpenDialog(true);
  };

  const handleCloseDialog = () => {
    setDeckToDelete(null);
    setOpenDialog(false);
  };

  return (
    <Box sx={{ boxSizing: 'border-box', width: '30%' }}>
      {/* Confirmation Dialog */}
      <Dialog open={openDialog} onClose={handleCloseDialog}>
        <DialogTitle>Confirm Deletion</DialogTitle>
        <DialogContent>
          Are you sure you want to remove this deck from your collection?
        </DialogContent>
        <DialogActions>
          <Button onClick={handleCloseDialog} color="primary">
            No
          </Button>
          <Button onClick={handleDelete} color="error">
            Yes
          </Button>
        </DialogActions>
      </Dialog>

      <Typography
        variant="h6"
        sx={{
          color: 'black',
          position: 'absolute',
          top: 60,
          left: 106,
          background: 'white',
          padding: '15px 0px 15px 0px',
          width: '30%',
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
          }}
        >
          <List
            sx={{
              flex: 1,
              overflowY: 'auto',
              maxHeight: '400px',
            }}
          >
            {isLoggedOn ? (
              collectionDecks && collectionDecks.length > 0 ? (
                collectionDecks.map((deck, index) => (
                  <React.Fragment key={deck.id}>
                    <>{console.log('DeckId: ', deck.id)}</>
                    <ListItem disableGutters>
                      <Button
                        variant="text"
                        sx={{ cursor: 'pointer' }}
                        onClick={() => {
                          window.location.href = `https://localhost:5173/decks/${deck.id}/practice`;
                        }}
                      >
                        <ListItemText
                          primary={deck.title}
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
                        onClick={() => handleOpenDialog(deck.id)}
                        style={{ marginLeft: 'auto' }}
                      >
                        <DeleteIcon />
                      </IconButton>
                    </ListItem>
                    {index !== collectionDecks.length - 1 && <Divider />}
                  </React.Fragment>
                ))
              ) : (
                <ListItem>
                  <ListItemText
                    primary="No decks in collection"
                    sx={{ color: 'black', fontSize: 20 }}
                  />
                </ListItem>
              )
            ) : (
              <ListItem>
                <ListItemText
                  primary="Log in to see the decks that you have in collection"
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

      {/* Single button lower down */}
      <Box
        sx={{
          position: 'absolute',
          top: 620,
          width: '30%',
          display: 'flex',
          justifyContent: 'left',
          padding: '10px 0',
        }}
      >
        {isLoggedOn && (
          <Button
            variant="contained"
            color="primary"
            fullWidth
            sx={{
              flex: 1,
              margin: '0 8px', // Adds a margin on the left and right
              borderRadius: '8px', // Adjusts the corner rounding
              maxWidth: 'calc(40% - 16px)', // Ensures the button width respects the margins
            }}
            onClick={() =>
              (window.location.href = window.location.href =
                `https://localhost:5173//shop`)
            }
          >
            Go to Shop
          </Button>
        )}
      </Box>
    </Box>
  );
}
