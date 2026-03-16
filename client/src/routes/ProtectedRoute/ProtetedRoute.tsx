import { Navigate, Outlet } from "react-router-dom";
import { useAuth } from "../../context/authContext";

export const ProtectedRoute = () => {
  const { isAuthenticated } = useAuth();

  // If not logged in, boot them to the login page
  if (!isAuthenticated) {
    return <Navigate to="/login" replace />;
  }

  // If logged in, render the child routes
  return <Outlet />;
};