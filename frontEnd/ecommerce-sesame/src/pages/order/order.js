import "./orders.css";
import { Link } from "react-router-dom";
import Header1 from "../../components/header1/header1.js";
import { useState, useEffect } from "react";
import axios from "axios";
import { API_URLS } from "../../apiConfig";
import { format } from 'date-fns';

function Order({ carts }) {
  const [orders, setOrders] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchOrders = async () => {
      try {
        const token = localStorage.getItem('token');
        if (!token) {
            setLoading(false);
            return;
        }

        const response = await axios.get(API_URLS.ORDERS); 
        // Interceptor in App.js adds Authorization header if token exists using axios
        // But to be safe/explicit or if this component runs in isolation:
        // verify App.js interceptor is global. Yes it is.
        setOrders(response.data);
      } catch (error) {
        console.error("Error fetching orders:", error);
      } finally {
        setLoading(false);
      }
    };

    fetchOrders();
  }, []);

  if (loading) return <div>Loading orders...</div>;

  return (
    <>
      <Header1 carts={carts} />
      <div className="orders-main-container">
        <div className="orders-page-header">
           <h2>Your Orders</h2>
        </div>
        
        {orders.length === 0 ? (
           <div className="no-orders">
             <p>No orders found.</p>
             <Link to="/" className="button-primary">Start Shopping</Link>
           </div>
        ) : (
          <div className="orders-grid">
            {orders.map((order) => (
              <div className="order-container" key={order._id}>
                <div className="order-header">
                  <div className="order-header-left">
                    <div className="order-date">
                      <span className="label">Order Placed:</span>
                      <span className="value">{format(new Date(order.dateOrdered), 'MMMM dd, yyyy')}</span>
                    </div>
                    <div className="order-total">
                      <span className="label">Total:</span>
                      <span className="value">${order.totalAmount.toFixed(2)}</span>
                    </div>
                  </div>
                  <div className="order-header-right">
                    <span className="order-id">Order ID: #{order._id.substring(0, 8)}</span>
                  </div>
                </div>
                
                <div className="order-items-list">
                  {order.orderItems.map((item) => (
                    <div className="order-item" key={item._id}>
                      <div className="item-image">
                         <img src={`/${item.product.image}`} alt={item.product.name} />
                      </div>
                      <div className="item-details">
                        <Link to={`/product/${item.product._id}`} className="item-name">
                          {item.product.name}
                        </Link>
                        <div className="item-meta">
                           <span>Quantity: {item.quantity}</span>
                           <span>Price: ${(item.product.priceCents / 100).toFixed(2)}</span>
                        </div>
                        {item.deliveryOption && (
                           <div className="delivery-info">
                             Delivery: {item.deliveryOption.name}
                           </div>
                        )}
                      </div>
                    </div>
                  ))}
                </div>
              </div>
            ))}
          </div>
        )}
      </div>
    </>
  );
}
export default Order;
