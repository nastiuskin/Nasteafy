import { Routes, Route } from 'react-router-dom';
import { useEffect, useState } from 'react';
import MainLayout from './layout/MainLayout';
import { Toaster } from 'react-hot-toast';
import Login from './features/auth/LoginPage';
import Register from './features/auth/RegisterPage';
import ProfilePage from './features/auth/UserProfile';
import HomePage from './features/home/HomePage';
import PlaylistsPage from './features/playlists/PlaylistsPage';
import PlaylistInfoCard from './features/playlists/components/PlaylistInfoCard';
import ArtistsPage from './features/artists/ArtistsPage';
import ArtistProfilePage from './features/artists/ArtistProfilePage';
import SplashScreen from './components/SplashScreen';
import AlbumPage from './features/albums/AlbumPage';
import SubscriptionsPage from './features/subscriptions/SubscriptionsPage';
import { PayPalScriptProvider } from "@paypal/react-paypal-js";
import LikedSongsPage from './features/tracks/LikedSongsPage';

function App() {
  const [loading, setLoading] = useState(true);

  const initialOptions = {
    clientId: "Adek0KYf3MuCn6V5keD-lXt32ezhs3bJx_kDbbZ5aLhge-vHO6GlU4M_osONC7MgR16d0rWkineXH-u7",
    currency: "USD",
    intent: "capture",
  };

  useEffect(() => {
    const timeout = setTimeout(() => setLoading(false), 1500);  
    return () => clearTimeout(timeout);
  }, []);

  if (loading) return <SplashScreen />;

  return (
    <>
      <Routes>
        <Route path='/login' element={<Login />} />
        <Route path='/register' element={<Register />} />
        <Route element={<MainLayout />}>
          <Route path="/" element={<HomePage />} />
          <Route path="/playlists" element={<PlaylistsPage />} />
          <Route path="/profile" element={<ProfilePage />} />
          <Route path="/playlists/:id" element={<PlaylistInfoCard />} />
          <Route path="/artists" element={<ArtistsPage />} />
          <Route path="/artists/:id" element={<ArtistProfilePage />} />
          <Route path="/albums/:id" element={<AlbumPage />} />
          <Route path="/liked-songs" element={<LikedSongsPage />} />
          <Route
            path='/subscriptions'
            element={
              <PayPalScriptProvider options={initialOptions}>
                <SubscriptionsPage />
              </PayPalScriptProvider>
            }
          />
        </Route>
      </Routes>
      <Toaster position="top-right" toastOptions={{ duration: 1000 }} />
    </>
  );
}

export default App;
