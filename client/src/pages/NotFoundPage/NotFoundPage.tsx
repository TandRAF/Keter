import { Link } from 'react-router-dom';

export const NotFoundPage = () => {
  return (
    <div style={{ 
      width:'100%',
      display: 'flex', 
      flexDirection: 'column', 
      alignItems: 'center', 
      justifyContent: 'center', 
      height: '100vh',
      color: 'white',
      textAlign: 'center'
    }}>
      <h1 style={{ fontSize: '4rem', margin: '0' }}>404</h1>
      <h2 style={{ marginBottom: '20px' }}>Page Not Found</h2>
      <p style={{ color: '#aaa', marginBottom: '30px' }}>
        The page you are looking for doesn't exist or has been moved.
      </p>
      
      <Link 
        to="/" 
        style={{
          padding: '10px 20px',
          backgroundColor: '#734FCF',
          color: 'white',
          textDecoration: 'none',
          borderRadius: '5px',
          fontWeight: 'bold'
        }}
      >
        Go to Home
      </Link>
    </div>
  );
};