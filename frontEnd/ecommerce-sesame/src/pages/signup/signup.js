
import './singup.css';
import { Link, useNavigate } from 'react-router-dom';
import Header1 from '../../components/header1/header1';
import { useState } from 'react';

function Signup({ carts }) {
  const [signup, setSignup] = useState({ username: '', email: '', password: '', name: '' });
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    
    try {
      const response = await fetch('http://localhost:5038/users/signup', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(signup),
      });

      if (!response.ok) {
        const error = await response.json();
        throw new Error(error.message || 'Signup failed');
      }

      const data = await response.json();
      console.log('Signup successful:', data);
      
      // Store token and user info
      localStorage.setItem('token', data.token);
      localStorage.setItem('user', JSON.stringify({
        userId: data.userId,
        customerId: data.customerId,
        username: data.username,
        email: data.email
      }));
      
      navigate('/'); // Redirect home after signup
    } catch (error) {
      console.error('Signup failed:', error.message);
      alert('Signup failed: ' + error.message);
    }
  };

  return (
    <>
      <Header1 carts={carts} />
      <div className="big-container-signup">
        <div className="signup-container">
          <h2>Create Account</h2>
          <form onSubmit={handleSubmit}>
            <input 
              type="text" 
              placeholder="Username" 
              required 
              value={signup.username}
              onChange={(e) => setSignup({ ...signup, username: e.target.value })}
            />
            <input 
              type="text" 
              placeholder="Name" 
              required 
              value={signup.name}
              onChange={(e) => setSignup({ ...signup, name: e.target.value })}
            />
            <input 
              type="email" 
              placeholder="Email" 
              required 
              value={signup.email}
              onChange={(e) => setSignup({ ...signup, email: e.target.value })}
            />
            <input 
              type="password" 
              placeholder="Password" 
              required 
              value={signup.password}
              onChange={(e) => setSignup({ ...signup, password: e.target.value })}
            />
            <button type="submit" className="button-primary">Sign Up</button>
          </form>
          <div className="login-link">
            Already have an account? <Link to="/login">Log in</Link>
          </div>
        </div>
      </div>
    </>
  );
}

export default Signup;