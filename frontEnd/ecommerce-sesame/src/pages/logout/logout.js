import { useEffect } from 'react';
import { Link } from 'react-router-dom';
import Header1 from '../../components/header1/header1';
import './logout.css';

function Logout({ carts }) {
    
    useEffect(() => {
        // Clear sensitive session data
        localStorage.removeItem('token');
        localStorage.removeItem('user');
        // We do NOT clear guestCartId so they can continue browsing as the same guest
    }, []);

    return (
        <>
            <Header1 carts={carts} />
            <div className="logout-container">
                <div className="logout-card">
                    <div className="logout-icon-wrapper">
                        <svg className="logout-icon" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M5 13l4 4L19 7" />
                        </svg>
                    </div>
                    <h1 className="logout-title">Logged Out</h1>
                    <p className="logout-message">
                        You have been successfully logged out of your account.
                        Your session has been securely ended.
                    </p>
                    
                    <Link to="/login" className="logout-button">
                        Sign In Again
                    </Link>
                    
                    <Link to="/" className="home-link">
                        Return to Home Page
                    </Link>
                </div>
            </div>
        </>
    );
}

export default Logout;