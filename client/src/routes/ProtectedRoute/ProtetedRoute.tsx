import { Navigate, Outlet } from "react-router-dom";
import { useAuth } from "../../context/authContext";

export const ProtectedRoute = () => {
  const { isAuthenticated } = useAuth();

  if (!isAuthenticated) {
    return <Navigate to="/register" replace />;
  }

  return <Outlet />;
};