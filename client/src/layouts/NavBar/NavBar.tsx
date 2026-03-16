import { Link, NavLink, useNavigate, useLocation } from "react-router-dom"; // Adăugat useLocation
import { useAuth } from "../../context/authContext";
import { Button } from "../../components/Button/Button";
import styles from "./NavBar.module.scss";

export const NavBar = () => {
  const { isAuthenticated, user, logout } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();
  const hideNavbarPaths = ["/login", "/wellcome","/register"];

  const handleLogout = () => {
    logout();
    navigate("/login");
  };

  if (hideNavbarPaths.includes(location.pathname)) {
    return null;
  }

  return (
    <header className={styles.navbar}>
      <div className={styles.logo}>
        <Link to="/">Keter</Link>
      </div>

      <nav className={styles.navLinks}>
        <NavLink 
          to="/" 
          className={({ isActive }) => (isActive ? styles.active : "")}
        >
          Home
        </NavLink>

        {isAuthenticated && (
          <>
            <NavLink 
              to="/dashboard" 
              className={({ isActive }) => (isActive ? styles.active : "")}
            >
              Dashboard
            </NavLink>
            <NavLink 
              to="/profile" 
              className={({ isActive }) => (isActive ? styles.active : "")}
            >
              Profile
            </NavLink>
          </>
        )}
      </nav>

      <div className={styles.authActions}>
        {isAuthenticated ? (
          <div className={styles.userMenu}>
            <span className={styles.greeting}>Hi, {user?.email}</span>
            <Button onClick={handleLogout} size="sm">
              Log Out
            </Button>
          </div>
        ) : (
          <Link to="/login">
            <Button size="sm">Log In</Button>
          </Link>
        )}
      </div>
    </header>
  );
};