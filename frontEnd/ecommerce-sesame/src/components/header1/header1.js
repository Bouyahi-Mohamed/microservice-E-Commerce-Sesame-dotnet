// import styles for this component
import './sesame-header.css';
// import images and icons
import sesameLogo from '../../assets/logo/sesame-logo.png';
import sesameMobileLogo from '../../assets/logo/sesame-mobile-logo-white.png';
import searchIcon from '../../assets/icons/search-icon.png';
import Logout from '../../assets/icons/logout.png';
import Login from '../../assets/icons/login-avatar.png';
import Orders from '../../assets/icons/shopping-bag.png';
import Cart from '../../assets/icons/cart-icon.png';
// import react components and hooks
// import react-router-dom components
import { Link } from 'react-router-dom';
function Header1({ carts }) {
// calculate cart count
  const cartCount = carts.reduce((total, cart) => total + cart.quantity, 0);
  // cartCount = carts.length;

  return (
    <>
      <div className="sesame-header">
        <div className="sesame-header-left-section">
          <Link to="/Home" className="header-link">
            <img
              className="sesame-logo"
              src={sesameLogo}
              alt="Sesame Logo"
            />
            <img
              className="sesame-mobile-logo"
              src={sesameMobileLogo}
              alt="Sesame Mobile Logo"
            />
          </Link>
        </div>

        <div className="sesame-header-middle-section">
          <input
            className="search-bar"
            type="text"
            placeholder="Search"
          />
          <button className="search-button button-primary">
            <img
              className="search-icon"
              src={searchIcon}
              alt="Search Icon"
            />
          </button>
        </div>

        <div className="sesame-header-right-section">
          {localStorage.getItem('token') ? (
            <>
              
              <Link className="account-link header-link" to="/logout">
                <img
                  className="cart-icon"
                  src={Logout}
                  alt="Logout"
                  onClick={()=>{
                    {localStorage.removeItem('token')}
                  }}
                />
              </Link>
            </>
          )  : (
            <Link className="account-link header-link" to="/login">
              <img
                className="cart-icon"
                src={Login}
                alt="Login"
              />
            </Link>
          )}

          <Link className="orders-link header-link" to="/orders">
            <img
              className="cart-icon"
              src={Orders}
              alt="Orders"
            />
          </Link>

          <Link className="cart-link header-link" to="/cart">
            <img
              className="cart-icon"
              src={Cart}
              alt="Cart"
            />
            <div className="cart-text">Cart</div>
            <div className="cart-quantity">
              {cartCount}
            </div>
          </Link>
        </div>
      </div>
    </>
  );
}

export default Header1;
