import React, { Component } from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import LoginPage from './Login'; // Your LoginPage component
import DataPage from './Data'; // Your DataPage component
import EditDataForm from './Edit'; // Your EditDataForm component
import AddDataPage from './AddData'; // Your AddDataPage component
import DashboardPage from './dashboard'; 
import Navbar from './Navbar';

export default class App extends Component {
    constructor(props) {
        super(props);
        // Check if localStorage is available
        if (typeof localStorage === 'undefined') {
            console.log("localStorage is not available");
        }

        // Initialize state with value from localStorage
        const isAuthenticated = localStorage.getItem('isAuthenticated') === 'true';

        this.state = {
            isAuthenticated, // Set authentication state based on localStorage
        };
    }

    // Function to handle login and set authentication
    handleLogin = () => {
        this.setState({ isAuthenticated: true });
        localStorage.setItem('isAuthenticated', 'true'); // Store in localStorage
        console.log("Logged in, authentication saved in localStorage.");
    };

    // Function to handle logout
    handleLogout = () => {
        this.setState({ isAuthenticated: false });
        localStorage.removeItem('isAuthenticated'); // Remove from localStorage
        console.log("Logged out, authentication removed from localStorage.");
    };

    render() {
        const { isAuthenticated } = this.state;

        return (
            <Router>
                {isAuthenticated && <Navbar onLogout={this.handleLogout} />}

                <Routes>
                    {/* Public Route: Login Page */}
                    <Route
                        path="/"
                        element={
                            isAuthenticated ? (
                                <Navigate to="/data" replace /> // Redirect to /data if logged in
                            ) : (
                                <LoginPage onLogin={this.handleLogin} />
                            )
                        }
                    />

                    {/* Protected Routes */}
                    <Route
                        path="/data"
                        element={
                            isAuthenticated ? (
                                <DataPage onLogout={this.handleLogout} />
                            ) : (
                                <Navigate to="/" replace /> // Redirect to login if not authenticated
                            )
                        }
                    />
                    <Route
                        path="/edit/:id"
                        element={
                            isAuthenticated ? (
                                <EditDataForm />
                            ) : (
                                <Navigate to="/" replace /> // Redirect to login if not authenticated
                            )
                        }
                    />
                    <Route
                        path="/adddata"
                        element={
                            isAuthenticated ? (
                                <AddDataPage />
                            ) : (
                                <Navigate to="/" replace /> // Redirect to login if not authenticated
                            )
                        }
                    />
                    <Route
                        path="/dashboard"
                        element={<DashboardPage /> } />
                </Routes>
            </Router>
        );
    }
}
