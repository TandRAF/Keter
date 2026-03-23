import React, { useState } from "react";
import { useAuth } from "../../../context/authContext";
import { authService } from "../../../services/authService";
import InputData from "../../../components/InputData/InputData";
import { Button } from "../../../components/Button/Button";
import styles from "./LoginForm.module.scss";

interface LoginFormProps {
  task: any; 
  onFinish: (id: string) => void;
}

export const LoginForm: React.FC<LoginFormProps> = ({ task, onFinish }) => {
  const { login } = useAuth();
  const [formData, setFormData] = useState({ username: "", password: "" });
  const [error, setError] = useState("");
  const [isLoading, setIsLoading] = useState(false);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    setFormData((prev) => ({ ...prev, [e.target.id]: e.target.value }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError("");
    setIsLoading(true);

    try {
      const response = await authService.login(formData.username, formData.password);
      const token = response?.token;
      const userData = response?.user;

      if (token && userData) {
        login({
          id: userData.id,
          email: userData.email,
          userName: userData.userName,
          profilePictureUrl: userData.profilePictureUrl
        }, token); 
        
        onFinish(task.id);
      } else {
        setError("Token or user data not found in server response.");
      }
    } catch (err: any) {
      console.error("DEBUG LOGIN:", err);
      const errorMessage = err.response?.data?.message || "Eroare de conexiune sau format date invalid.";
      setError(errorMessage);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <form className={styles.loginForm} onSubmit={handleSubmit}>
      {error && <div className={styles.errorMessage}>{error}</div>}

      <InputData
        type="text"
        id="username"
        label="Username"
        required
        onChange={handleChange}
      />
      
      <InputData
        type="password"
        id="password"
        label="Password"
        required
        onChange={handleChange}
      />
      
      <Button size="lg" type="submit" disabled={isLoading}>
        {isLoading ? "Logging in..." : "Log In"}
      </Button>
    </form>
  );
};