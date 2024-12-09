import React, { useState } from 'react';
import axios from 'axios';

const LoginPage = () => {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [errorMessage, setErrorMessage] = useState('');
    const [loading, setLoading] = useState(false);
    const [showPassword, setShowPassword] = useState(false);

    const handleLogin = async (e) => {
        e.preventDefault();

        if (!username || !password) {
            setErrorMessage('Username and password are required.');
            return;
        }

        setLoading(true);

        try {
            const response = await axios.post("https://localhost:7054/api/Login", {
                username: "admin",
                password: "admin"
            }, {
                headers: {
                    'Content-Type': 'application/json',
                    'Accept': 'application/json'
                }
            });

            if (!response.ok) {
                const errorData = await response.json(); // Try to parse error response
                setErrorMessage(errorData.message || 'Login failed.');
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            const data = await response.json();

            if (data.success) {
                // Handle successful login (e.g., store token, redirect)
                console.log('Login successful!');
            } else {
                setErrorMessage('Login failed.');
            }

        } catch (error) {
            setErrorMessage('An error occurred. Please try again later.');
            console.error('Login error:', error);
        } finally {
            setLoading(false);
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
    form: {
        display: 'flex',
        flexDirection: 'column',
        width: '200px',
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
    },
    error: {
        color: 'red',
        fontSize: '14px',
        marginBottom: '10px',
    },
    button: {
        padding: '10px 20px',
        fontSize: '16px',
        backgroundColor: '#4CAF50',
        color: 'green',
        border: 'none',
        borderRadius: '5px',
        cursor: 'pointer',
        width: '200px',
        marginLeft: '10px', // Corrected property name
    }
}
export default LoginPage;