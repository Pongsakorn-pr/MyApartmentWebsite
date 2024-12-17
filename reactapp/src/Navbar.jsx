import React from 'react';
import './Navbar.css';

const Navbar = () => {
    return (
        <nav className="navbar">
            {/* Left: Logo or Brand */}
            <div className="navbar-left">
                <a href="/" className="brand">MyWebsite</a>
            </div>

            {/* Right: User Details */}
            <div className="navbar-right">
                <span className="details">Welcome, John Doe</span>
                <a href="/profile" className="btn">Profile</a>
                <a href="/logout" className="btn btn-logout">Logout</a>
            </div>
        </nav>
    );
};

export default Navbar;
