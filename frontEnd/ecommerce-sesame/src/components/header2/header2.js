
// Import css file
import './cart-header.css'
// Import react router dom
import { Link } from "react-router-dom";
// Import images
import Logo from '../../assets/logo/sesame-logo.png'
import logoImage from '../../assets/logo/sesame-mobile-logo.png'
import checkoutLogo from '../../assets/icons/checkout-lock-icon.png'


export default function Header2({ carts }) {
  return (
   <>
       <div className="cart-header">
      <div className="header-content">
        <div className="cart-header-left-section">
          <Link to="/">
            <img className="sesame-logo" src={Logo} alt="Sesame Logo" />
            <img className="sesame-mobile-logo" src={logoImage} alt="Sesame Mobile Logo" />
          </Link>
        </div>

        <div className="cart-header-middle-section">
          Checkout (<Link className="return-to-home-link"
            to="/">{carts.length} items</Link>)
        </div>

        <div className="cart-header-right-section">
          <img src={checkoutLogo} alt="Checkout" />
        </div>
      </div>
    </div>
   </>
  );
}