import React, { useState } from "react";
import { Link } from "react-router-dom"; // Am șters useNavigate
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
      const userEmail = response?.user?.email || formData.email; 
      const token = response?.token;

      if (token) {
        login(userEmail, token);
      }
      onFinish(task.id);
      
    } catch (err: any) {
      console.error("Eroare la înregistrare:", err);
      setError("Registration failed.");
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
      <div style={{ marginTop: "1rem", textAlign: "center", fontSize: "0.9rem" }}>
        Already have an account? <Link to="/login">Log in here</Link>
        {error && <div className={styles.errorMessage}>{error}</div>}
      </div>
    </form>
  );
};