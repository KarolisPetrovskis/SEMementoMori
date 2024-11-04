import { Breadcrumbs, Link } from '@mui/material';
import { useNavigate, useLocation } from 'react-router-dom';

const DynamicBreadcrumb = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const pathname = location.pathname.split('/');
  pathname.shift(); // Remove the initial empty string

  const handleNavigate = (path: string[]) => {
    navigate(`/${path.join('/')}`);
  };

  const styles = {
    breadcrumb: {
      color: 'indigo',
      fontSize: '1.5rem', // Adjust font size as needed
    },
  };

  // Capitalize function for consistent capitalization
  const capitalize = (str: string) => {
    return str.charAt(0).toUpperCase() + str.slice(1);
  };

  return (
    <Breadcrumbs separator="›" aria-label="breadcrumb" sx={styles.breadcrumb}>
      {pathname.map((pathSegment, index) => (
        <Link
          key={index}
          underline="hover"
          color="inherit"
          href="#"
          onClick={() => handleNavigate(pathname.slice(0, index + 1))}
        >
          {capitalize(pathSegment)}
        </Link>
      ))}
    </Breadcrumbs>
  );
};

export default DynamicBreadcrumb;
