import { Outlet } from 'react-router-dom';
import Sidebar from './Sidebar';
import Topbar from './TopBar';
import Player from '../features/home/Player';

const MainLayout = () => {
  return (
    <div className="flex flex-col h-screen bg-black text-white">
      <div className="flex flex-1 overflow-hidden">
        <div className="w-64 bg-neutral-950 border-r border-neutral-800">
          <Sidebar />
        </div>

        <div className="flex-1 flex flex-col">
          <Topbar />
          <main className="flex-1 overflow-y-auto bg-neutral-900">
            <Outlet />
          </main>
        </div>
      </div>
      <Player />
    </div>
  );
};

export default MainLayout;
