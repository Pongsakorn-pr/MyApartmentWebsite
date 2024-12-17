import React from 'react';
import './Navbar.css';
import Button from 'react-bootstrap/Button'
import { HouseGearFill } from 'react-bootstrap-icons'
import { Link, useNavigate } from 'react-router-dom';


const Navbar = () => {
    const navigate = useNavigate();
    const homePage = () => {
        navigate('/data', { replace: true });
    };
    const logOut = () => {
        localStorage.removeItem('isAuthenticated');
        navigate('/', { replace: true });
        window.location.reload();
    };
    return (
        <nav className="navbar">
            {/* Left: Logo or Brand */}
            <div className="navbar-left">
                <Link to="/" className="brand">
                    <HouseGearFill /> Properly Manage
                </Link>
            </div>

            {/* Right: User Details */}
            <div className="navbar-right">
                <Button variant="primary" onClick={homePage}>Home</Button>
                <Button variant="danger" onClick={logOut}>Logout</Button>
            </div>
        </nav>
    );
};

export default Navbar;
