import Typography from "@mui/material/Typography";

export default function Title() {
  return (
    <Typography
      sx={{
        position: "absolute",
        bottom: 0,
        left: 30,
        color: "#FFFFFF",
      }}
      variant="h2"
      gutterBottom
    >
      Memento Mori...
    </Typography>
  );
}
