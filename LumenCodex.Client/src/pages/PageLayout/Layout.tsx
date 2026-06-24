import { Outlet } from "@tanstack/react-router";
import { Header } from "../../components";
import { Bounce, ToastContainer } from "react-toastify";

const Layout = () => {
  return (
    <div className="h-full">
      <Header />
      <main className="h-full">
        <Outlet />
      </main>
      <ToastContainer
        position="top-right"
        autoClose={5000}
        hideProgressBar={false}
        newestOnTop
        closeOnClick={false}
        rtl={false}
        pauseOnFocusLoss
        draggable
        pauseOnHover
        theme="dark"
        transition={Bounce}
      />
    </div>
  );
};

export default Layout;
