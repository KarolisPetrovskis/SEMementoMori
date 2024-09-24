import * as React from "react";
import List from "@mui/material/List";
import ListItem from "@mui/material/ListItem";
import Divider from "@mui/material/Divider";
import ListItemText from "@mui/material/ListItemText";
import ListItemAvatar from "@mui/material/ListItemAvatar";
import Avatar from "@mui/material/Avatar";
import Typography from "@mui/material/Typography";
import AssignmentIcon from "@mui/icons-material/Assignment";

export default function QuestList() {
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
      }}
    >
      <ListItem alignItems="center">
        <ListItemAvatar>
          <Avatar sx={{ bgcolor: "#f03efc" }}>
            <AssignmentIcon />
          </Avatar>
        </ListItemAvatar>
        <ListItemText
          primary="Daily quest"
          sx={{ color: "#6be0fe" }}
          secondary={
            <React.Fragment>
              <Typography
                component="span"
                variant="body2"
                sx={{ color: "#ecffff", display: "inline" }}
              >
                Go to the gym, loser
              </Typography>
              {" — I'll be watching."}
            </React.Fragment>
          }
        />
      </ListItem>
      <Divider variant="inset" component="li" />
      <ListItem alignItems="center">
        <ListItemAvatar>
          <Avatar sx={{ bgcolor: "#fc5f3f" }}>
            <AssignmentIcon />
          </Avatar>
        </ListItemAvatar>
        <ListItemText
          primary="Quest 2"
          sx={{ color: "#6be0fe" }}
          secondary={
            <React.Fragment>
              <Typography
                component="span"
                variant="body2"
                sx={{ color: "#ecffff", display: "inline" }}
              >
                Do
              </Typography>
              {" — Something"}
            </React.Fragment>
          }
        />
      </ListItem>
      <Divider variant="inset" component="li" />
      <ListItem alignItems="center">
        <ListItemAvatar>
          <Avatar sx={{ bgcolor: "#39a677" }}>
            <AssignmentIcon />
          </Avatar>
        </ListItemAvatar>
        <ListItemText
          primary="Quest 2"
          sx={{ color: "#6be0fe" }}
          secondary={
            <React.Fragment>
              <Typography
                component="span"
                variant="body2"
                sx={{ color: "#ecffff", display: "inline" }}
              >
                Go somewhere
              </Typography>
              {" — Do something"}
            </React.Fragment>
          }
        />
      </ListItem>
    </List>
  );
}
