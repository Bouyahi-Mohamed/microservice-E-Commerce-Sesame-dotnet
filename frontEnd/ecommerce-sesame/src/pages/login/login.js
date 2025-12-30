
import './login.css';
import { Link } from 'react-router-dom';
import Header1 from '../../components/header1/header1';
import {useState } from 'react';
import { useNavigate } from 'react-router-dom';


function Login({ carts }) {
  const [login, setLogin] = useState({ username: '', password: '' });
  const navigate = useNavigate();
  // function to handle form submission
  const fetchLogin = async (e) => {
    handleSubmit(e);
    const guestCartId = localStorage.getItem('guestCartId');
    fetch('http://localhost:5038/users/login', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Cart-ID': guestCartId || '' // Send guest cart ID for merging
      },
      body: JSON.stringify(login),
    })
      .then((response) => response.json())
      .then((data) => {
        console.log('Login successful:', data);
        // You can redirect the user or store the token here
        localStorage.setItem('token', data.token);
        localStorage.setItem('user', JSON.stringify({
          userId: data.userId,
          customerId: data.customerId,
          username: data.username,
          email: data.email
        }));
        navigate('/'); // Redirect home page after login
      })
      .catch((error) => {
        console.error(`Login failed: ${error.message}`);
      });
  }



  const handleSubmit = (e) => {
    e.preventDefault();
    setLogin({
      username: e.target.username.value,
      password: e.target.password.value,
    });
  }

  return (
      <>
      <Header1 carts={carts} />
  <div className='big-container-login'>
    <div className="login-container">
        <h2>Login</h2>
        <form onSubmit={fetchLogin} className="login-form">
          <input type="text" id="username" name="username" placeholder="Username" value={login.username} required onChange={(e)=>{
            setLogin({ ...login, username: e.target.value });
          }} />
          <input type="password" id="password" name="password" placeholder="Password" value={login.password} required onChange={(e)=>{
            setLogin({ ...login, password: e.target.value });
          }} />
          {}
          <div className="remember-me">
            <input type="checkbox" id="remember-me" name="remember-me" />
            <label htmlFor="remember-me">Remember Me</label>
          </div>
          <button type="submit" className="button-primary" >Log In</button>
        </form>
        <div className="signup-link">
          Don't have an account? <Link to="/signup">Sign Up</Link>
        </div>
      </div>    
    </div>
        </>
    );
}
export default Login;