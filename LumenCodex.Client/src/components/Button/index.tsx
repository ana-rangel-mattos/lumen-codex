import * as React from "react";

interface IButtonProps extends React.DetailedHTMLProps<
  React.ButtonHTMLAttributes<HTMLButtonElement>,
  HTMLButtonElement
> {
  size?: "xs" | "sm" | "md" | "lg";
  buttonStyle?: "primary" | "secondary" | "warning";
  children?: React.ReactNode;
}

const Button: React.FC<IButtonProps> = ({
  size = "medium",
  buttonStyle = "primary",
  children,
  ...rest
}) => {
  const primaryClass =
    "bg-accent hover:bg-base-content text-base-100 border-accent";
  const secondaryClass =
    "bg-base-content hover:border-base-100 hover:text-accent text-base-300 border-base-300";
  const warningClass =
    "bg-error text-base-100 rounded-sm px-2 py-1 border-accent-content hover:border-error hover:bg-accent-content";

  return (
    <button
      {...rest}
      className={`btn-${size} hover:cursor-pointer ${buttonStyle === "primary" ? primaryClass : buttonStyle == "secondary" ? secondaryClass : warningClass} flex ${size == "xs" || size == "sm" ? "" : "w-45"} items-center justify-center gap-x-2 rounded-md border-2 ${size == "sm" || size == "xs" ? "px-2 py-1" : "px-3 py-1.5"} text-lg transition ease-in`}
    >
      {children}
    </button>
  );
};

export default Button;
