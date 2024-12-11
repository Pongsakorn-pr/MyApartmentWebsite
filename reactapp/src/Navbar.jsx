import React from 'react';
import { NavLink } from 'react-router-dom'; // Import for routing

const Navbar = () => {
    return (
        <nav className="navbar">
            <ul>
                <li>
                    <NavLink to="/">Login</NavLink>
                </li>
                <li>
                    <NavLink to="/data">Data</NavLink>
                </li>
            </ul>
        </nav>
    );
};

export default Navbar;