import { createBrowserRouter } from "react-router-dom";
import App from "../App";
import { HomePage } from "../pages/HomePage/HomePage";
import { ProtectedRoute } from "./ProtectedRoute/ProtetedRoute";
import { UIKit } from "../pages/UiKit/UiKit";
import { BoardContainer } from "../features/board/BoardContainer/BoardContainer";
import { AuthPage } from "../pages/AuthPage/AuthPage";
import {WellcomePage} from "../pages/WellcomePage/WellcomePage"

export const router = createBrowserRouter([
  {
    path: "/",
    element: <App />,
    children: [
      { 
        path: "", 
        element: <HomePage /> 
      },
      {
        path: "register",
        element: <AuthPage type="register"/>
      },
      {
        path: "login",
        element: <AuthPage type="login"/>
      },
      { 
        path: "project",
        element:<BoardContainer/>
      },
      {
        path: "uikit",
        element:<UIKit/>
      },
      {
        element: <ProtectedRoute />, 
        children: [
          { path: "wellcome", element:<WellcomePage/>},
          { path: "dashboard", element: <BoardContainer/> },
          { path: "profile", element: <div>Profile Settings</div> },
        ]
      }
    ]
  }
]);