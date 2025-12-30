import sesameLogo from '../../assets/logo/sesame-logo.png';
import './sesame-footer.css';
function Footer() {
  return (
    <div className="footer">
      <div className="footer-left-section">
        <a href="/" className="footer-link">
          <img className="sesame-logo" src={sesameLogo} alt="Sesame Logo" />
        </a>
        <div className="footer-text">
          &copy; 2025 SesameShop. All rights reserved.
        </div>
      </div>
    </div>
  );
}

export default Footer;
