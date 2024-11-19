import * as React from 'react';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import ListItemText from '@mui/material/ListItemText';
import SettingsIcon from '@mui/icons-material/Settings';
import IconButton from '@mui/material/IconButton';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import Divider from '@mui/material/Divider';

export default function DeckMenu() {
  const data = [
    { id: 1, name: 'Deck 1' },
    { id: 2, name: 'Deck 2' },
    { id: 3, name: 'Deck 4' },
    { id: 4, name: 'Deck 5' },
    { id: 5, name: 'Deck 6' },
    { id: 6, name: 'Deck 7' },
    { id: 7, name: 'Deck 8' },
  ];

  return (
    <div>
      <Typography
        variant="h6"
        sx={{
          color: 'black',
          position: 'absolute',
          top: 55,
          left: 106,
          background: 'white',
          padding: 2,
          width: 468,
          border: 1,
          borderTopLeftRadius: '6px',
          borderTopRightRadius: '6px',
          borderBottomRightRadius: '0px',
          borderBottomLeftRadius: '0px',
          borderColor: '#D4A017',
          borderWidth: 2,
        }}
      >
        Decks
      </Typography>
      <List
        sx={{
          position: 'absolute',
          left: 106,
          top: 121,
          minWidth: 500,
          maxWidth: 500,
          maxHeight: 400,
          overflowY: 'auto',
          border: 1,
          borderTopLeftRadius: '0px',
          borderTopRightRadius: '0px',
          borderBottomRightRadius: '6px',
          borderBottomLeftRadius: '6px',
          borderColor: '#D4A017',
          borderWidth: 2,
          bgcolor: 'white',
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
                    // Handle button click here
                    console.log(`Line item ${deck.name} clicked`);
                  }}
                >
                  <ListItemText primary={deck.name} />
                </Button>
                <IconButton
                  aria-label="settings"
                  sx={{ cursor: 'pointer' }}
                  onClick={() => {
                    console.log(`Settings of ${deck.name} clicked`); //Setting button onClick
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
    </div>
  );
}
