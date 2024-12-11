import React, { Component } from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import LoginPage from './Login'; // Your LoginPage component
import DataPage from './data'; // Your DataPage component

export default class App extends Component {
    static displayName = App.name;

    render() {
        return (
            <Router> {/* Wrap the app with Router */}
                <Routes>
                    <Route path="/" element={<LoginPage />} /> {/* Home route for LoginPage */}
                    <Route path="/data" element={<DataPage />} /> {/* Data page route */}
                </Routes>
            </Router>
        );
    }
}
