import { Breadcrumbs, Link } from '@mui/material';
import { useNavigate, useLocation } from 'react-router-dom';

const DynamicBreadcrumb = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const pathname = location.pathname.split('/');
  pathname.shift();

  const handleNavigate = (path: string[]) => {
    navigate(`/${path.join('/')}`);
  };

  const styles = {
    breadcrumb: {
      color: 'indigo',
      fontSize: '1.5rem',
    },
  };

  // Capitalize function for consistent capitalization
  const capitalize = (str: string) => {
    return str.charAt(0).toUpperCase() + str.slice(1);
  };

  return (
    <Breadcrumbs separator="›" aria-label="breadcrumb" sx={styles.breadcrumb}>
      {pathname.map((pathSegment, index) => {
        if (pathSegment === 'decks' && index === 0) {
          // Check for "/decks" at index 0
          return (
            <Link
              key={index}
              underline="hover"
              color="inherit"
              onClick={() => navigate('/browser')} // Navigate to "/browser"
            >
              Deck browser
            </Link>
          );
        } else {
          return (
            <Link
              key={index}
              underline="hover"
              color="inherit"
              href="#" // Keep "#" for default behavior
              onClick={() => handleNavigate(pathname.slice(0, index + 1))} // Navigate to other paths
            >
              {capitalize(pathSegment)}
            </Link>
          );
        }
      })}
    </Breadcrumbs>
  );
};

export default DynamicBreadcrumb;
