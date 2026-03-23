import React, { useState } from "react";
import { Link } from "react-router-dom";
import { useAuth } from "../../../context/authContext";
import { authService } from "../../../services/authService";
import InputData from "../../../components/InputData/InputData";
import { Button } from "../../../components/Button/Button";
import styles from "./RegisterForm.module.scss";

interface TaskCardProps {
  task: any; 
  onFinish: (id: string) => void;
}

export const RegisterForm: React.FC<TaskCardProps> = ({ task, onFinish}) => {
  const { login } = useAuth();
  
  const [formData, setFormData] = useState({
    username: "",
    email: "",
    password: "",
    confirmPassword: "",
  });
  const [error, setError] = useState("");
  const [isLoading, setIsLoading] = useState(false);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    setFormData((prev) => ({ ...prev, [e.target.id]: e.target.value }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError("");
    
    if (formData.password !== formData.confirmPassword) {
      setError("Passwords do not match.");
      return;
    }

    setIsLoading(true);
    try {
      const response = await authService.register(
        formData.username,
        formData.email,
        formData.password
      );
      
      // Extract the token and the full user object
      const token = response?.token;
      const userData = response?.user;

      if (token && userData) {
        // Pass the FULL user object to your Context
        login({
          id: userData.id,
          email: userData.email,
          userName: userData.userName,
          profilePictureUrl: userData.profilePictureUrl
        }, token);
        
        onFinish(task.id);
      } else {
        setError("Registration succeeded, but failed to automatically log in.");
      }
      
    } catch (err: any) {
      console.error("Eroare la înregistrare:", err);
      const errorMessage = err.response?.data?.message || "Registration failed.";
      setError(errorMessage);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <form className={styles.loginForm} onSubmit={handleSubmit}>
      <InputData
        type="text"
        id="username"
        label="Your User Name"
        required
        onChange={handleChange}
      />

      <InputData
        type="email"
        id="email"
        label="Email Address"
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

      <InputData
        type="password"
        id="confirmPassword"
        label="Confirm Password"
        required
        onChange={handleChange}
      />

      <Button size="lg" type="submit" disabled={isLoading}>
        {isLoading ? "Creating Account..." : "Sign Up"}
      </Button>
      <div className={styles.changeAuth}>
        Already have an account? <Link to="/login">Log in here</Link>
        {error && <div className={styles.errorMessage}>{error}</div>}
      </div>
    </form>
  );
};