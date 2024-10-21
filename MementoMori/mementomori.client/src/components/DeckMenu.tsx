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
  return (
    <div style={{ position: 'relative' }}>
      <Typography
        variant="h6"
        sx={{
          color: 'black',
          position: 'absolute',
          top: 15,
          background: 'white',
          padding: 2,
          width: 268,
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
          top: 80,
          minWidth: 300,
          maxWidth: 300,
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
        {[1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12].map((value, index) => (
          <React.Fragment key={value}>
            <ListItem disableGutters>
              <Button
                variant="text"
                sx={{ cursor: 'pointer' }}
                onClick={() => {
                  // Handle button click here
                  console.log(`Line item ${value} clicked`);
                }}
              >
                <ListItemText primary={`Line item ${value}`} />
              </Button>
              <IconButton
                aria-label="settings"
                sx={{ cursor: 'pointer' }}
                style={{ marginLeft: 'auto' }}
              >
                <SettingsIcon />
              </IconButton>
            </ListItem>
            {index !== 11 && <Divider />}{' '}
            {/* Render Divider only if not the last item  TO BE CHANGED*/}
          </React.Fragment>
        ))}
      </List>
    </div>
  );
}
