import { Routes, Route } from 'react-router-dom';
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

function App() {
  return (
    <>  
    <Routes>
      <Route path='/login' element={<Login/>}/>
       <Route path='/register' element={<Register/>}/>
       <Route element={<MainLayout />}>
       <Route path="/" element={<HomePage />} />
       <Route path="/playlists" element={<PlaylistsPage />} />
       <Route path='/profile' element={<ProfilePage/>}/>
      <Route path="/playlists/:id" element={<PlaylistInfoCard />} />
      <Route path="/artists" element={<ArtistsPage />} />
      <Route path="/artists/:id" element={<ArtistProfilePage />} />
      </Route>
    </Routes>
     <Toaster position="top-right" toastOptions={{ duration: 1000 }} />
    </>
  );
}

export default App;
