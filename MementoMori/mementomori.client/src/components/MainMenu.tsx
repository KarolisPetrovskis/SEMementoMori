import * as React from 'react';
import Box from '@mui/material/Box';
import Avatar from '@mui/material/Avatar';
import Menu from '@mui/material/Menu';
import MenuItem from '@mui/material/MenuItem';
import ListItemIcon from '@mui/material/ListItemIcon';
import Divider from '@mui/material/Divider';
import IconButton from '@mui/material/IconButton';
import Tooltip from '@mui/material/Tooltip';
import PersonAdd from '@mui/icons-material/PersonAdd';
import Settings from '@mui/icons-material/Settings';
import Logout from '@mui/icons-material/Logout';
import DeckMenu from './DeckMenu.tsx';
import Button from '@mui/material/Button';
import { AuthDialog } from '../AuthDialog/AuthDialog.tsx';

export default function MainMenu() {
  const [isAuthDialogVisible, setIsAuthDialogVisible] = React.useState(false);
  const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);
  const open = Boolean(anchorEl);
  const handleAuthDialogClose = () => {
    setIsAuthDialogVisible(false);
  };
  const handleClick = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorEl(event.currentTarget);
  };
  const handleClose = () => {
    setAnchorEl(null);
  };
  return (
    <React.Fragment>
      {isAuthDialogVisible ? (
        <AuthDialog closeCallback={handleAuthDialogClose} />
      ) : null}
      <Box
        sx={{
          display: 'flex',
          textAlign: 'center',
          bgcolor: '#6E6DB3',
          position: 'absolute',
          right: 460,
          top: 0,
          border: 1,
          borderRadius: '16px',
          borderColor: '#D4A017',
          borderWidth: 2,
        }}
      >
        <DeckMenu />
        <Button
          sx={{ minWidth: 150, color: '#ecffff' }}
          style={{ textTransform: 'capitalize' }}
          variant="text"
          onClick={() => {
            window.location.href = `/browser`;
          }}
        >
          Deck browser
        </Button>
        <Button
          sx={{ minWidth: 150, color: '#ecffff' }}
          style={{ textTransform: 'capitalize' }}
          variant="text"
          onClick={() => setIsAuthDialogVisible(true)}
        >
          Login
        </Button>
        <Button
          sx={{ minWidth: 150, color: '#ecffff' }}
          style={{ textTransform: 'capitalize' }}
          variant="text"
        >
          Profile
        </Button>
        <Tooltip title="Account settings">
          <IconButton
            onClick={handleClick}
            size="small"
            sx={{ ml: 2 }}
            aria-controls={open ? 'account-menu' : undefined}
            aria-haspopup="true"
            aria-expanded={open ? 'true' : undefined}
          >
            <Avatar sx={{ width: 32, height: 32 }}>D</Avatar>
          </IconButton>
        </Tooltip>
      </Box>
      <Menu
        anchorEl={anchorEl}
        id="account-menu"
        open={open}
        onClose={handleClose}
        onClick={handleClose}
        slotProps={{
          paper: {
            elevation: 0,
            sx: {
              border: 1,
              borderWidth: 2,
              borderColor: '#D4A017',
              borderRadius: '16px',
              bgcolor: '#6E6DB3',
              color: '#ecffff',
              overflow: 'visible',
              filter: 'drop-shadow(0px 2px 8px rgba(0,0,0,0.32))',
              mt: 1.5,
              '& .MuiAvatar-root': {
                width: 32,
                height: 32,
                ml: -0.5,
                mr: 1,
              },
              '&::before': {
                content: '""',
                display: 'block',
                position: 'absolute',
                top: 0,
                right: 14,
                width: 10,
                height: 10,
                transform: 'translateY(-50%) rotate(45deg)',
                zIndex: 0,
              },
            },
          },
        }}
        transformOrigin={{ horizontal: 'right', vertical: 'top' }}
        anchorOrigin={{ horizontal: 'right', vertical: 'bottom' }}
      >
        <MenuItem onClick={handleClose}>
          <Avatar /> Profile
        </MenuItem>
        <MenuItem onClick={handleClose}>
          <Avatar /> My account
        </MenuItem>
        <Divider />
        <MenuItem onClick={handleClose}>
          <ListItemIcon>
            <PersonAdd sx={{ color: '#ecffff' }} fontSize="small" />
          </ListItemIcon>
          Add another account
        </MenuItem>
        <MenuItem onClick={handleClose}>
          <ListItemIcon>
            <Settings sx={{ color: '#ecffff' }} fontSize="small" />
          </ListItemIcon>
          Settings
        </MenuItem>
        <MenuItem onClick={handleClose}>
          <ListItemIcon>
            <Logout sx={{ color: '#ecffff' }} fontSize="small" />
          </ListItemIcon>
          Logout
        </MenuItem>
      </Menu>
    </React.Fragment>
  );
}
