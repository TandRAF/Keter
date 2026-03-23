import React, { useState } from "react"; // Nu uita să adaugi useState aici!
import { Link, NavLink, useNavigate, useLocation } from "react-router-dom";
import { useAuth } from "../../context/authContext";
import { Button } from "../../components/Button/Button";
import styles from "./NavBar.module.scss";
import { NavHome, NavNotification, NavProjects, NavTasks } from "../../components/Icons/Icons";
import { ProjectNav } from "../../features/projects/ProjectsNav/ProjectNav";

export const NavBar = () => {
  const { isAuthenticated, user, logout } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();
  const hideNavbarPaths = ["/login", "/wellcome", "/register"];
  
  const [isDropdownOpen, setIsDropdownOpen] = useState(false);
  console.log(user);
  const handleLogout = () => {
    logout();
    navigate("/login");
  };

  if (hideNavbarPaths.includes(location.pathname)) {
    return null;
  }

  return (
    <header className={styles.header}>
      {isAuthenticated && (
        <nav className={styles.navBar}>
          <div  className={styles.navLinks}>
               <NavLink 
                to="/" 
                end 
                className={({ isActive }) => (isActive ? styles.active : "")}
              >
                <NavHome/>
              </NavLink>
              <NavLink 
                to="/myTask" 
                className={({ isActive }) => (isActive ? styles.active : "")}
              >
                <NavTasks/>
              </NavLink>
              <NavLink 
                to="/projects" 
                className={({ isActive }) => (isActive ? styles.active : "")}
              >
                <NavProjects/>
              </NavLink>
              <NavLink 
                to="/notification" 
                className={({ isActive }) => (isActive ? styles.active : "")}
              >
                <NavNotification/>
              </NavLink>
          </div>
          <div className={styles.authActions}>
        {isAuthenticated ? (
          <div className={styles.userMenu}>
            <img 
              src={user?.profilePictureUrl} 
              alt="Profile" 
              className={styles.profileIcon} 
              onClick={() => setIsDropdownOpen(!isDropdownOpen)} // Schimbă starea la click
            />

            {isDropdownOpen && (
              <div className={styles.dropdownMenu}>
                <Button onClick={handleLogout} size="sm" className={styles.logoutBtn}>
                  Log Out
                </Button>
              </div>
            )}
          </div>
        ) : (
          <Link to="/login">
            <Button size="sm">Log In</Button>
          </Link>
        )}
      </div>
        </nav>
      )}
    </header>
  );
};