import { useState, useRef } from 'react';
import { useQuery } from '@tanstack/react-query';
import { useParams } from 'react-router-dom';
import axios from 'axios';
import {
  Box,
  Chip,
  CircularProgress,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
} from '@mui/material';
import { Typography } from '@mui/joy';
import ButtonGroup from '@mui/material/ButtonGroup';
import Button from '@mui/material/Button';
import { ArrowDropDown } from '@mui/icons-material';
import ClickAwayListener from '@mui/material/ClickAwayListener';
import Grow from '@mui/material/Grow';
import Paper from '@mui/material/Paper';
import Popper from '@mui/material/Popper';
import MenuItem from '@mui/material/MenuItem';
import MenuList from '@mui/material/MenuList';

type DeckQueryData = {
  id: string;
  creatorName: string;
  cardCount: number;
  modified: Date;
  rating: number;
  tags?: string[];
  title: string;
  description: string;
  isOwner: boolean;
};

type TagsProps = {
  tags?: string[];
};

function Tags(props: TagsProps) {
  return props.tags ? (
    <Box
      sx={{
        flexDirection: 'row',
        display: 'flex',
        alignItems: 'flex-start',
        justifyContent: 'flex-start',
        gap: 1,
      }}
    >
      {props.tags.map((tag) => (
        <Chip label={tag} variant="outlined" />
      ))}
    </Box>
  ) : (
    <>No tags provided</>
  );
}

type ButtonProps = {
  isOwner: boolean;
  inCollection: boolean;
};

function Buttons(props: ButtonProps) {
  const anchorRef = useRef<HTMLDivElement>(null);
  const [open, setOpen] = useState(false);
  const [dialogOpen, setDialogOpen] = useState(false);
  const [inCollection, setInCollection] = useState(props.inCollection);
  const { deckId } = useParams<{ deckId: string }>();

  const handleToggle = () => {
    setOpen((prevOpen) => !prevOpen);
  };

  const handleClose = (event: Event) => {
    if (
      anchorRef.current &&
      anchorRef.current.contains(event.target as HTMLElement)
    ) {
      return;
    }

    setOpen(false);
  };

  const onPracticeClick = () => {
    window.location.href = `/decks/${deckId}/practice`;
  };
  const onAddToMyCollectionClick = () => {
    // send request to backend to verify that can add and then add
    // show spinner until response
    setInCollection(true);
  };

  const onRemoveClick = async () => {
    try {
      const response = await axios.post(`/Decks/${deckId}/deleteDeck`, {
        Id: deckId,
      });
      if (response.status === 200) {
        window.location.href = `https://localhost:5173/browser`;
      } else {
        console.error('Failed to delete the deck');
      }
    } catch (error) {
      console.error('Error while deleting the deck:', error);
    }
  };

  const openDialog = () => {
    setDialogOpen(true);
  };

  const closeDialog = () => {
    setDialogOpen(false);
  };

  const confirmRemove = () => {
    onRemoveClick();
    closeDialog();
  };

  const onEditClick = () => {
    window.location.href = `/decks/${deckId}/edit`;
  };

  const onUseAsTemplateClick = () => {
    console.error();
  };

  const onDeleteClick = () => {
    // send req to backend
    console.error();
  };

  return (
    <Box
      sx={{
        flexDirection: 'row',
        display: 'flex',
        alignItems: 'flex-start',
        justifyContent: 'flex-end',
        gap: 1,
      }}
    >
      {inCollection ? (
        <>
          <Button color="success" onClick={onPracticeClick} variant="contained">
            Practice
          </Button>
          <Button
            color="error"
            onClick={openDialog} // Open the dialog instead of directly executing the removal
            variant="contained"
          >
            Remove
          </Button>
          <Dialog
            open={dialogOpen}
            onClose={closeDialog}
            aria-labelledby="alert-dialog-title"
            aria-describedby="alert-dialog-description"
          >
            <DialogTitle id="alert-dialog-title">
              {'Confirm Removal'}
            </DialogTitle>
            <DialogContent>
              <DialogContentText id="alert-dialog-description">
                Are you sure you want to remove this Deck from your collection?
                This action cannot be undone.
              </DialogContentText>
            </DialogContent>
            <DialogActions>
              <Button onClick={closeDialog} color="primary">
                No
              </Button>
              <Button onClick={confirmRemove} color="error" autoFocus>
                Yes
              </Button>
            </DialogActions>
          </Dialog>
        </>
      ) : (
        <Button
          color="success"
          onClick={onAddToMyCollectionClick}
          variant="contained"
        >
          Actions
        </Button>
      )}
      {props.isOwner ? (
        <>
          <ButtonGroup
            color="info"
            variant="contained"
            ref={anchorRef}
            aria-label="Button group with a nested menu"
          >
            <Button onClick={onEditClick}>Edit</Button>
            <Button size="small" onClick={handleToggle}>
              <ArrowDropDown />
            </Button>
          </ButtonGroup>
          <Popper
            sx={{ zIndex: 1 }}
            open={open}
            anchorEl={anchorRef.current}
            role={undefined}
            transition
            disablePortal
          >
            {({ TransitionProps, placement }) => (
              <Grow
                {...TransitionProps}
                style={{
                  transformOrigin:
                    placement === 'bottom' ? 'center top' : 'center bottom',
                }}
              >
                <Paper>
                  <ClickAwayListener onClickAway={handleClose}>
                    <MenuList id="split-button-menu" autoFocusItem>
                      <MenuItem
                        key={'Use as a template'}
                        onClick={onUseAsTemplateClick}
                      >
                        Use as a template
                      </MenuItem>
                      <MenuItem
                        sx={{ color: 'red' }}
                        onClick={onDeleteClick}
                        key={'Delete'}
                      >
                        Delete
                      </MenuItem>
                    </MenuList>
                  </ClickAwayListener>
                </Paper>
              </Grow>
            )}
          </Popper>
        </>
      ) : null}
    </Box>
  );
}

export function Deck() {
  const { deckId } = useParams<{ deckId: string }>();
  const { data, isFetched, isError } = useQuery({
    queryKey: ['main', 'deck', 'deckId'] as const,
    queryFn: async () => {
      const response = await axios.get<DeckQueryData>(`/Decks/${deckId}/deck`);
      return response.data;
    },
  });

  return isFetched ? (
    !isError && data ? (
      <Box
        sx={{
          flexDirection: 'column',
          display: 'flex',
          alignItems: 'flex-start',
          justifyContent: 'space-between',
          minWidth: '100%',
        }}
      >
        <Box
          sx={{
            paddingLeft: 4,
            paddingRight: 4,
            bgcolor: 'lightgray',
            flexDirection: 'row',
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'space-between',
            minWidth: '94.3%',
            //marginTop: '20px',
            borderRadius: '6px',
          }}
        >
          <Typography level="h1">{data.title}</Typography>
          <Buttons isOwner={data.isOwner} inCollection={false} />{' '}
          {/*Provide actual values when users are implemented*/}
        </Box>
        <h2>Tags:</h2>
        <Tags tags={data.tags} />
        <h2>Description</h2>
        <p>{data.description}</p>
      </Box>
    ) : (
      <p>Failed to fetch deck data </p>
    )
  ) : (
    <CircularProgress />
  );
}
