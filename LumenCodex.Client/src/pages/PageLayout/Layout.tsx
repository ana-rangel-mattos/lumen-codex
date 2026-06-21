import { Outlet } from "@tanstack/react-router";
import { Header } from "../../components";
import { Toaster } from "react-hot-toast";

const Layout = () => {
  return (
    <div className="h-full">
      <Toaster />
      <Header />
      <main className="h-full">
        <Outlet />
      </main>
    </div>
  );
};

export default Layout;
