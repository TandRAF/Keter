import { createBrowserRouter } from "react-router-dom";
import App from "../App";
import { HomePage } from "../pages/HomePage/HomePage";
import { ProtectedRoute } from "./ProtectedRoute/ProtetedRoute";
import { UIKit } from "../pages/UiKit/UiKit";
import { BoardContainer } from "../features/board/BoardContainer/BoardContainer";
import { AuthPage } from "../pages/AuthPage/AuthPage";
import { WellcomePage } from "../pages/WellcomePage/WellcomePage"
import { TestPage } from "../pages/TestPage/TestPage";
import { ProjectsPage } from "../pages/ProjectsPage/ProjectsPage";
import { ProjectDetails } from "../pages/ProjectsDetailsPage/ProjectDetailsPage";
import { Boards } from "../features/board/Boards/boards";
import { Board } from "../features/board/Board/Board"; // IMPORT THE NEW BOARD COMPONENT
import { NotFoundPage } from "../pages/NotFoundPage/NotFoundPage";

export const router = createBrowserRouter([
  {
    path: "/",
    element: <App />,
    children: [
      { path: "", element: <HomePage /> },
      { path: "register", element: <AuthPage key="register" type="register"/> },
      { path: "login", element: <AuthPage key="login" type="login"/> },
      { path: "uikit", element:<UIKit/> },
      { path: "test", element:<TestPage/> },
      {
        element: <ProtectedRoute />, 
        children: [
          { path: "wellcome", element:<WellcomePage/>},
          { path: "profile", element: <div>Profile Settings</div> },
          { path: "projects", element: <ProjectsPage /> },
          { 
            path: "projects/:projectId", 
            element: <ProjectDetails><Boards /></ProjectDetails> 
          },
          { 
            path: "projects/:projectId/boards/:boardId", 
            element: <ProjectDetails><Board /></ProjectDetails> 
          },
        ],
      },
      {
        path: "*",
        element: <NotFoundPage />
      }
    ]
  }
]);