import { Outlet } from 'react-router-dom';
import Sidebar from './Sidebar';
import Topbar from './TopBar';
import Player from '../features/home/Player';

export default function MainLayout () {
  return (
    <div className="flex flex-col h-screen bg-background text-foreground">
      <div className="flex flex-1 overflow-hidden">
        <div className="w-64 bg-muted border-r border-border">
          <Sidebar />
        </div>
        <div className="flex-1 flex flex-col">
          <Topbar />
          <main className="flex-1 overflow-y-auto bg-background pb-20">
            <Outlet />
          </main>
        </div>
      </div>
      <Player />
    </div>
  );
};