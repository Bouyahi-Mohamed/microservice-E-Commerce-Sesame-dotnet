import axios from 'axios';
import { API_URLS } from "../../../apiConfig";
import './productList.css';
import { Link } from 'react-router-dom';
export default function ProductList({ products = [] }) {
// handle add to cart
const handleAddToCart = async (productId) => {
  try {
    await axios.post(API_URLS.CART, { product: productId, quantity: 1 });
    alert('Product added to cart');
  } catch (error) {
    console.error('Error adding product to cart:', error);
  }
};

  let ProductItems = products.map((product) => {
    return (
      <div className="product-container" key={product._id}>
        <Link to={`/product/${product._id}`}>
          <div className="product-image-container">
            <img className="product-image"
              src={product.image} alt={product.name} />
          </div>
        </Link>
          <div className="product-name limit-text-to-2-lines">
            {product.name}
          </div>

          <div className="product-rating-container">
            <img className="product-rating-stars"
              src={`../images/ratings/rating-${Math.round(product.rating.stars * 2) * 5}.png`} alt={`${product.rating.stars} stars`} />
            <div className="product-rating-count link-primary">
              {product.rating.count} 
            </div>
          </div>

          <div className="product-price">
            ${(product.priceCents / 100).toFixed(2)}
          </div>

          

          <div className="product-spacer"></div>

          <div className="added-to-cart">
            <img src={`images/icons/checkmark.png`} alt="Added to cart" />
            Added
          </div>
        
          <button className="add-to-cart-button button-primary"
          onClick={() => handleAddToCart(product._id)}>
            Add to Cart
          </button>
        </div>
    );
  });

  return (
    <div className="main">
      <div className="products-grid">
        {ProductItems}
      </div>
    </div>
  );
}