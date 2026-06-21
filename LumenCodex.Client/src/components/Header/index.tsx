import Navbar from "../Navbar";

const Header = () => {
  return (
    <header className="text-base-100 bg-base-content sticky flex items-center space-x-8 px-8 py-6">
      <h1 className="text-accent text-5xl">LumenCodex</h1>
      <Navbar />
    </header>
  );
};

export default Header;
