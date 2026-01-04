
import styles from "./cart-main.module.css";
import axios from "axios";
import { API_URLS } from "../../../apiConfig";

export default function DeliveryOptions({ deliveryOptions, cart }) {
  return (
    <>
      {deliveryOptions.map((option) => (
        <div className={styles["delivery-option"]} key={option._id}
        onClick={async () => {
          try {
            await axios.patch(`${API_URLS.CART}/delivery`, {
              id: cart._id,
              deliveryOption: option._id,
            });
          } catch (error) {
            console.error("Failed to update cart delivery option:", error);
          }
        }}>
          <input
            type="radio"
            className={styles["delivery-option-input"]}
            name={`delivery-option-${cart._id}`}
            value={option.id}
            checked={
              option._id === cart.deliveryOption._id ? true : false
            }
            onChange={() => {}}
          />
          <div>
            <div className={styles["delivery-option-date"]}>
              {option.estimatedDeliveryDate}
            </div>
            <div className={styles["delivery-option-price"]}>
              {option.priceCents / 100 === 0
                ? "FREE SHIPPING"
                : `$${(option.priceCents / 100).toFixed(2)} SHIPPING`}
            </div>
          </div>
        </div>
      ))}
    </>
  );
}
