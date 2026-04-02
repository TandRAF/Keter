import { type ButtonHTMLAttributes, type ReactNode } from "react";
import styles from "./Button.module.scss"; // optional if using SCSS modules

type ButtonVariant = "primary" | "secondary" | "ghost" |"tertiary";
type ButtonSize = "sm" | "md" | "lg";

type ButtonProps = ButtonHTMLAttributes<HTMLButtonElement> & {
  variant?: ButtonVariant;
  size?: ButtonSize;
  icon?: ReactNode;
  children?: ReactNode;
  iconPosition?: "left" | "right";
};

export const Button = ({
  variant = "primary",
  size = "md",
  icon,
  children,
  iconPosition = "left",
  className = "",
  ...props
}: ButtonProps) => {
  return (
    <button
      className={`${styles.button} ${styles[variant]} ${styles[size]} ${className}`}
      {...props}
    >
      {icon && iconPosition === "left" && <span className={styles.icon}>{icon}</span>}
      {children && <span className={styles.text}>{children}</span>}
      {icon && iconPosition === "right" && <span className={styles.icon}>{icon}</span>}
    </button>
  );
};
