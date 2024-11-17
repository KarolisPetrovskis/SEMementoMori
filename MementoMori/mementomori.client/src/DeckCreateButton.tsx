/// A placeholder
import React from 'react';
import { Button } from '@mui/material';
const RedirectButton = () => {
  const handleRedirect = () => {
    window.location.href =
      'https://localhost:5173/decks/00000000-0000-0000-0000-000000000000/edit'; // Replace with your desired URL
  };

  return (
    <Button
      variant="contained"
      color="primary"
      fullWidth
      sx={{
        flex: 1,
        margin: '0 8px', // Adds a margin on the left and right
        borderRadius: '8px', // Adjusts the corner rounding
        maxWidth: 'calc(100% - 16px)', // Ensures the button width respects the margins
      }}
      onClick={handleRedirect}
    >
      Create a new Deck
    </Button>
  );
};
export default RedirectButton;
