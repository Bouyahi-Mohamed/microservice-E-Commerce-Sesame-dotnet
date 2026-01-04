import "./ProductDetails.css";
import { Link, useParams } from "react-router-dom";
import { useState, useEffect } from "react";
import Header from "../../components/header1/header1";
import axios from "axios";
import { API_URLS } from "../../apiConfig";

export default function ProductDetails({ carts }) {
  // state for product details
  const [productDetails, setProductDetails] = useState(null);
  
  // state for select content
  const [selectedQuantity, setSelectedQuantity] = useState(1);

  const { id } = useParams();

  // function to handle add to cart
  const handleAddToCart = async (product, quantity) => {
    try {
      await axios.post(API_URLS.CART, { product: product._id, quantity });
      // update cart state
      // Assuming fetchCart is passed as a prop or available in context if needed
      // fetchCart(); 
      alert('Product added to cart');
    } catch (error) {
      console.log(error);
    }
  };
  //  fetch for product details

  useEffect(() => {
    //fetch data
    fetch(`${API_URLS.PRODUCTS}/${id}`)
      .then((response) => {
        if (!response.ok) {
          throw new Error("Network response was not ok");
        }
        return response.json();
      })
      .then((data) => {
        setProductDetails(data);
      })
      .catch((error) => {
        console.error("Error fetching products:", error);
        setProductDetails(null);
      });
  }, []);

  return (
    <div className="product-details-page">
      <Header carts={carts} />
      {/* start Close Button */}
      <Link to="/" className="back-button">
        <div className="close-button">
          <i className="fa-solid fa-circle-xmark fa-2xl"></i>
        </div>
      </Link>
      {/* end Close Button */}

      {productDetails ? (
        <div className="container" key={productDetails._id}>
          <div className="product-image">
            <img src={`/${productDetails.image}`} alt={productDetails.name} />
          </div>
          <div className="product-details">
            <img
              src={`/images/ratings/rating-${Math.round(productDetails.rating.stars * 2) * 5}.png`}
              alt={`${productDetails.rating.stars} stars`}
            />
            <h1>{productDetails.name}</h1>

            <h2>${(productDetails.priceCents / 100).toFixed(2)}</h2>
            <div className="product-description">
              <h3>Description:</h3>
              <p>{productDetails.description}</p>
            </div>
            <div className="product-rating-count">
              <div className="add-to-cart">
                <select
                  className="quantity-select"
                  value={selectedQuantity}
                  onChange={(e) => setSelectedQuantity(Number(e.target.value))}
                >
                  <option value="1">1</option>
                  <option value="2">2</option>
                  <option value="3">3</option>
                  <option value="4">4</option>
                  <option value="5">5</option>
                </select>
                <button
                  className="button-primary"
                  onClick={() => handleAddToCart(productDetails, selectedQuantity)}
                >
                  {" "}
                  Add to Cart{" "}
                </button>
              </div>
              <div className="review-count">{productDetails.rating.count} reviews</div>
            </div>
          </div>
        </div>
      ) : (
        <div className="loading">Loading product details...</div>
      )}
    </div>
  );
}