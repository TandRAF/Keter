import { NavBar } from "../NavBar/NavBar"
import styles from "./Header.module.scss"
import { ProjectNav } from "../../features/projects/ProjectsNav/ProjectNav"
import { useAuth } from "../../context/authContext";
import { useLocation} from "react-router-dom";
import { Logo } from "../Logo/Logo";

export const Header = () => {
  const { isAuthenticated} = useAuth();
  const location = useLocation();

  const hideNavbarPaths = ["/login", "/wellcome", "/register"];
   if (hideNavbarPaths.includes(location.pathname)) {
    return null;
  }

  return (
    <>
    {isAuthenticated && (
      <>
    <NavBar/>
    <div className={styles.additionalNav}>
      <Logo/>
      <ProjectNav/>
    </div>
      </>
    )}
  </>
  )
}
