import styles from "./cart-main.module.css";
import { useState, useEffect } from "react";
// date-fns
import { format, addDays } from 'date-fns';
//import images
import arrowUp from "../../../assets/icons/arrow-up.png";
import arrowDown from "../../../assets/icons/arrow-down.png";
//end import images
import axios from "axios";
import { API_URLS } from "../../../apiConfig";
import DeliveryOptions from "./DeliveryOptions";
export default function CartList({ carts }) {
  // Get token dynamically to ensure it's current
  const token = localStorage.getItem("token");

  // fetch delivery options from backend
  const [deliveryOptions, setDeliveryOptions] = useState([]);

  useEffect(() => {
    const fetchDeliveryOptions = async () => {
      try {
        const options = await axios.get(API_URLS.DELIVERY_OPTIONS, {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });
        setDeliveryOptions(options.data);
      } catch (error) {
        console.error("Error fetching delivery options:", error);
      }
    };
    fetchDeliveryOptions();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  // calculate totals
  const totalCart = () => {
    let total = 0;
    carts.forEach((item) => {
      if (item.product && item.product.priceCents) {
        total += item.product.priceCents * item.quantity / 100;
      }
    });
    return total.toFixed(2);
  };

  // calculate total shipping
  let  totalShipping = () => {
    let shippingCost = 0;
    carts.forEach((item) => {
      if (item.deliveryOption && item.deliveryOption.priceCents) {
          shippingCost += item.deliveryOption.priceCents / 100;
      }
    });
    return shippingCost.toFixed(2);
  };

  // calculate tax and total after tax
  let totalBeforeTax = (parseFloat(totalCart()) + parseFloat(totalShipping())).toFixed(2);

  // calculate tax
  let tax = (totalBeforeTax * 0.1).toFixed(2);

  // calculate total after tax
  let totalAfterTax = (parseFloat(totalBeforeTax) + parseFloat(tax)).toFixed(2);

  // handle increase quantity
  const handleIncreaseQuantity = (cart) => {
    axios.patch(`http://localhost:5038/cart/`, {
      id: cart._id,
      quantity: cart.quantity + 1,
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
  };
  // handle decrease quantity
  const handleDecreaseQuantity = (cart) => {
    if (cart.quantity > 1) {
      axios.patch(`http://localhost:5038/cart/`, {
        id: cart._id,
        quantity: cart.quantity - 1,
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });
    } else {
      axios.delete(`http://localhost:5038/cart/`, {
        data: { id: cart._id },
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });
    }
  };

  // handle delete item
  const handleDeleteItem = (cart) => {
    axios.delete(`http://localhost:5038/cart/`, {
      data: { id: cart._id },
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
  };

  // create cart items
  let cartItems = carts.map((cart) => (
    <div className={styles["cart-item-container"]} key={cart._id}>
      <div className={styles["delivery-date"]}>
        Delivery date:{" "}
        {cart.deliveryOption ? format(addDays(new Date(), cart.deliveryOption.estimatedDays), 'MMMM dd, yyyy') : "TBD"}
      </div>
      <div className={styles["cart-item-details-grid"]}>
        <img
          className={styles["product-image"]}
          src={`/${cart.product.image}`}
          alt={cart.name}
        />

        <div className={styles["cart-item-details"]}>
          <div className={styles["product-name"]}>{cart.product.name}</div>
          <div className={styles["product-price"]}>{cart.price}</div>
          <div className={styles["product-quantity"]}>
            <span>
              Quantity:{cart.quantity}
              <span className={styles[`quantity-label`]}>
                {" "}
                {cart.product.quantity}{" "}
              </span>
            </span>
            <div className="quantity-link">
              <div className="update-quantity-link link-primary">
                <img
                  className={`${styles["chg-quantity"]} ${styles["update-cart"]} ${styles["img-arrow"]}`}
                  src={arrowUp}
                  alt="Increase quantity"
                  onClick={() => handleIncreaseQuantity(cart)}
                />
                <img
                  className={`${styles["chg-quantity"]} ${styles["update-cart"]} ${styles["img-arrow"]}`}
                  src={arrowDown}
                  alt="Decrease quantity"
                  onClick={() => handleDecreaseQuantity(cart)}
                />
              </div>
            </div>
            <span
              className={`${styles["delete-quantity-link"]} ${"link-primary"}`}
              onClick={() => handleDeleteItem(cart)}
            >
              Delete{" "}
            </span>
          </div>
        </div>

        <div className={styles["delivery-options"]}>
          <div className={styles["delivery-options-title"]}>
            Choose a delivery option:
          </div>
          {deliveryOptions.map((option) => (
            <DeliveryOptions
              key={option._id}
              deliveryOptions={[option]}
              cart={cart}
            />
          ))}
        </div>
      </div>
    </div>
  ));

  return (
    <>
      <div className={styles.main}>
        <div className={styles["page-title"]}>Review your order</div>

        <div className={styles["checkout-grid"]}>
          <div className={styles["order-summary"]}>{cartItems}</div>

          <div className={styles["payment-summary"]}>
            <div className={styles["payment-summary-title"]}>Order Summary</div>

            <div className={styles["payment-summary-row"]}>
              <div>Items ({carts.length}):</div>
              <div className={styles["payment-summary-money"]}>
                ${totalCart()}
              </div>
            </div>

            <div className={styles["payment-summary-row"]}>
              <div>Shipping &amp; handling:</div>
              <div className={styles["payment-summary-money"]}>
                $
                {totalShipping()}
              </div>
            </div>

            <div
              className={`${styles["payment-summary-row"]} ${styles["subtotal-row"]}`}
            >
              <div>Total before tax:</div>
              <div className={styles["payment-summary-money"]}>${totalBeforeTax}</div>
            </div>

            <div className={styles["payment-summary-row"]}>
              <div>Estimated tax (10%):</div>
              <div className={styles["payment-summary-money"]}>${tax}</div>
            </div>

            <div
              className={`${styles["payment-summary-row"]} ${styles["total-row"]}`}
            >
              <div>Order total:</div>
              <div className={styles["payment-summary-money"]}>${totalAfterTax}</div>
            </div>

            <button
              className={`${
                styles["place-order-button"]
              } ${"button-primary"}`}
              onClick={async () => {
                try {
                  // Explicitly send Authorization header
                  const currentToken = localStorage.getItem('token');
                  if (!currentToken) {
                      alert('Please log in to place an order.');
                      window.location.href = '/login';
                      return;
                  }
                  
                  await axios.post(API_URLS.ORDERS, {}, {
                      headers: {
                          'Authorization': `Bearer ${currentToken}`
                      }
                  });
                  
                  alert('Order placed successfully!');
                  window.location.href = '/orders';
                } catch (error) {
                  console.error('Error placing order:', error);
                  // Check if backend returned a specific error message (object or string)
                  const errorData = error.response?.data;
                  const errorMessage = errorData?.message || (typeof errorData === 'string' ? errorData : '');
                  const status = error.response?.status;
                  
                  if (errorMessage) {
                    alert(`Failed to place order: ${errorMessage}`);
                  } else if (status) {
                     alert(`Failed to place order. Status: ${status} (${error.response.statusText})`);
                  } else {
                    alert(`Failed to place order: ${error.message}`);
                  }
                }
              }}
            >
              Place your order
            </button>
          </div>
        </div>
      </div>
    </>
  );
}
