import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
const LoginPage = () => {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [errorMessage, setErrorMessage] = useState('');
    const [loading, setLoading] = useState(false);
    const [showPassword, setShowPassword] = useState(false);
    const navigate = useNavigate();

    const handleLogin = async (e) => {
        e.preventDefault();

        if (!username || !password) {
            setErrorMessage('Username and password are required.');
            return;
        }

        setLoading(true);
        const dataObj = {
            username: username,
            password: password
        };

        try {
            const response = await axios.post("https://localhost:7054/api/Login", dataObj, {
                headers: {
                    'Content-Type': 'application/json',
                    'Accept': 'application/json'
                },
                withCredentials: true  // Enable this if you need to include cookies in the request
            });
            navigate('/data');

        } catch (error) {
            if (error.response) {
                // Handle server-side errors
                console.error('Server error:', error.response.data);
            } else if (error.request) {
                // Handle network errors
                console.error('Network error:', error.request);
            } else {
                console.error('Request error:', error.message);
            }
        }
    };
    return (
        <div style={styles.container}>
            <form onSubmit={handleLogin} style={styles.form}>
                <h2>Login</h2>
                <div style={styles.inputGroup}>
                    <label htmlFor="username">Username</label>
                    <input
                        type="text"
                        id="username"
                        name="username"
                        value={username}
                        onChange={(e) => setUsername(e.target.value)}
                        style={styles.input}
                    />
                </div>
                <div style={styles.inputGroup}>
                    <label htmlFor="password">Password</label>
                    <div style={styles.passwordContainer}>
                        <input
                            type={showPassword ? 'text' : 'password'}
                            id="password"
                            name="password"
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                            style={styles.input}
                        />
                    </div>
                </div>
                {errorMessage && <p style={styles.error}>{errorMessage}</p>}
                <button type="submit" style={styles.button} disabled={loading}>
                    {loading ? 'Logging in...' : 'Login'}
                </button>
            </form>
        </div>
    );
};

const styles = {
    container: {
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
        height: '100vh',
        padding: '20px',
        textAlign: 'center',
    },
    error: {
        color: 'red'
    },
    form: {
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        width: '200px'
    },
    inputGroup: {
        marginBottom: '15px',
    },
    input: {
        padding: '10px',
        fontSize: '16px',
        width: '100%',
        marginTop: '5px',
        borderRadius: '5px',
        border: '1px solid #ccc',
        width: '200px'
    },
    button: {
        marginTop: '5px',
        padding: '10px 20px',
        fontSize: '16px',
        backgroundColor: '#4CAF50',
        color: '#fff',
        border: 'none',
        borderRadius: '5px',
        cursor: 'pointer',
        width: '100%',
        marginLeft: '2.5px'
    },
};
export default LoginPage;