import { Link } from "@tanstack/react-router";
import * as React from "react";

interface INavbarItemProps {
  routeName: string;
  to: string;
}

const NavbarItem: React.FC<INavbarItemProps> = ({ routeName, to }) => {
  return (
    <li>
      <Link
        to={to}
        className="hover:text-base-100 hover:border-accent text-primary-content text-xl hover:border-b-2"
      >
        {routeName}
      </Link>
    </li>
  );
};

export default NavbarItem;
