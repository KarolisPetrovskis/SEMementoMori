import { Breadcrumbs, Link } from '@mui/material';
import { useNavigate, useLocation, useParams } from 'react-router-dom';
import { useQuery } from '@tanstack/react-query';
import axios from 'axios';

const DynamicBreadcrumb: React.FC = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const pathname = location.pathname.split('/');
  pathname.shift();

  const { deckId } = useParams<{ deckId: string }>();

  const {
    data: deckTitle,
    isFetched,
    isError,
  } = useQuery<string, Error>({
    queryKey: ['deckTitle', deckId],
    queryFn: async () => {
      try {
        const response = await axios.get(`/deck/getTitle/${deckId}`);
        console.log(
          'fgggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggg'
        );
        return response.data;
      } catch (err) {
        console.error('Error fetching deck title:', err);
        throw err;
      }
    },
    enabled: !!deckId,
  });

  const handleNavigate = (path: string[]) => {
    navigate(`/${path.join('/')}`);
  };

  const styles = {
    breadcrumb: {
      color: 'indigo',
      fontSize: '1.5rem',
    },
  };

  const capitalize = (str: string) =>
    str.charAt(0).toUpperCase() + str.slice(1);

  return (
    <Breadcrumbs separator="›" aria-label="breadcrumb" sx={styles.breadcrumb}>
      {pathname.map((pathSegment, index) => {
        if (pathSegment === 'decks' && index === 0) {
          return (
            <Link
              key={index}
              underline="hover"
              color="inherit"
              onClick={() => navigate('/browser')}
            >
              Deck browser
            </Link>
          );
        }

        if (pathSegment === deckId) {
          if (isError) {
            return <div key={index}>Error loading deck title.</div>;
          }
          if (isFetched) {
            return (
              <Link
                key={index}
                underline="hover"
                color="inherit"
                onClick={() => handleNavigate(pathname.slice(0, index + 1))}
              >
                {deckTitle}
              </Link>
            );
          }
          return <div key={index}>Loading...</div>;
        }

        return (
          <Link
            key={index}
            underline="hover"
            color="inherit"
            onClick={() => handleNavigate(pathname.slice(0, index + 1))}
          >
            {capitalize(pathSegment)}
          </Link>
        );
      })}
    </Breadcrumbs>
  );
};

export default DynamicBreadcrumb;
