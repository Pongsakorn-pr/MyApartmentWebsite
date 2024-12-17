import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App'10
import Navbar from './Navbar'

createRoot(document.getElementById('root')).render(
    <StrictMode>
        <App /> 
    </StrictMode>
);
