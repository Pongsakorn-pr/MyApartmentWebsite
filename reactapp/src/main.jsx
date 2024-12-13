import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App'
import Mynavbar from './Navbar'

createRoot(document.getElementById('root')).render(
    <StrictMode>
        <App /> 
    </StrictMode>
);

