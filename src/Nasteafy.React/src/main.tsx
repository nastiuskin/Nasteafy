import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App';
import './index.css';
import { BrowserRouter } from 'react-router-dom';
import { AudioPlayerProvider } from './contexts/AudioPlayerContext';
import { AuthProvider } from './contexts/AuthContext';
import AdPopup from './components/AdPopup';
import { TooltipProvider } from '@radix-ui/react-tooltip';

const theme = localStorage.getItem("theme");
if (theme === "dark") {
  document.documentElement.classList.add("dark");
} else {
  document.documentElement.classList.remove("dark");
}

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <BrowserRouter>
      <TooltipProvider>
        <AuthProvider>
          <AudioPlayerProvider>
            <AdPopup />
            <App />
          </AudioPlayerProvider>
        </AuthProvider>
      </TooltipProvider>
    </BrowserRouter>
  </React.StrictMode>
);


