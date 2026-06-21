import NavbarItem from "../NavbarItem";

const Navbar = () => {
  return (
    <nav>
      <ul className="flex cursor-pointer gap-x-6">
        <NavbarItem routeName="Home" to="/" />
        <NavbarItem routeName="Courses Dashboard" to="/dashboard" />
      </ul>
    </nav>
  );
};

export default Navbar;
