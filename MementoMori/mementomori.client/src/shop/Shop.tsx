import React from 'react';
import axios from 'axios';

const Shop: React.FC = () => {
  const updateHeaderColor = async (newColor: string) => {
    try {
      const response = await axios.post('/auth/newColor', {
        NewColor: newColor,
      });
      if (response.status === 200) {
        console.log('Header color updated successfully.');
        window.location.reload();
      } else {
        console.error('Failed to update header color:', response.data);
      }
    } catch (error) {
      console.error('Error updating header color:', error);
    }
  };

  return (
    <div style={{ padding: '20px' }}>
      <h1>Shop Page</h1>
      <p>Select a header background color:</p>
      <div>
        <button
          onClick={() => updateHeaderColor('lightblue')}
          style={{
            backgroundColor: 'lightblue',
            margin: '5px',
            padding: '10px',
            border: 'none',
            cursor: 'pointer',
          }}
        >
          Light Blue
        </button>
        <button
          onClick={() => updateHeaderColor('lightgreen')}
          style={{
            backgroundColor: 'lightgreen',
            margin: '5px',
            padding: '10px',
            border: 'none',
            cursor: 'pointer',
          }}
        >
          Light Green
        </button>
        <button
          onClick={() => updateHeaderColor('lightpink')}
          style={{
            backgroundColor: 'lightpink',
            margin: '5px',
            padding: '10px',
            border: 'none',
            cursor: 'pointer',
          }}
        >
          Light Pink
        </button>
        <button
          onClick={() => updateHeaderColor('lavender')}
          style={{
            backgroundColor: 'lavender',
            margin: '5px',
            padding: '10px',
            border: 'none',
            cursor: 'pointer',
          }}
        >
          Lavender
        </button>
        <button
          onClick={() => updateHeaderColor('white')}
          style={{
            backgroundColor: 'white',
            margin: '5px',
            padding: '10px',
            border: '1px solid black',
            cursor: 'pointer',
          }}
        >
          Reset to White
        </button>
      </div>
    </div>
  );
};

export default Shop;
