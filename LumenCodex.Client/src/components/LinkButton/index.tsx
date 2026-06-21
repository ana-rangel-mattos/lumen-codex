import * as React from "react";
import { createLink, type LinkComponent } from "@tanstack/react-router";

interface ILinkButtonProps extends React.AnchorHTMLAttributes<HTMLAnchorElement> {
  children?: React.ReactNode;
  icon?: string;
}

const BasicLinkComponent = React.forwardRef<
  HTMLAnchorElement,
  ILinkButtonProps
>(({ icon, children, ...rest }, ref) => {
  return (
    <a
      ref={ref}
      {...rest}
      className="bg-accent hover:bg-base-content text-base-100 border-accent flex items-center gap-x-2 rounded-md border-2 px-3 py-1.5 text-lg transition ease-in"
    >
      {icon && <i className={icon}></i>}
      {children}
    </a>
  );
});

const CreateLinkComponent = createLink(BasicLinkComponent);

const LinkButton: LinkComponent<typeof BasicLinkComponent> = (props) => {
  return <CreateLinkComponent preload="intent" {...props} />;
};

export default LinkButton;
