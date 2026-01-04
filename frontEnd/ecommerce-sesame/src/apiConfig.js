export const GATEWAY_URL = "http://localhost:8080";

export const SERVICES = {
    IDENTITY: "/identity",
    CATALOG: "/catalog",
    CART: "/cart",
    ORDER: "/order"
};

export const API_URLS = {
    // Identity
    LOGIN: `${GATEWAY_URL}${SERVICES.IDENTITY}/users/login`,
    SIGNUP: `${GATEWAY_URL}${SERVICES.IDENTITY}/users/signup`,

    // Catalog
    PRODUCTS: `${GATEWAY_URL}${SERVICES.CATALOG}/products`,

    // Cart
    CART: `${GATEWAY_URL}${SERVICES.CART}/cart`,
    DELIVERY_OPTIONS: `${GATEWAY_URL}${SERVICES.CART}/delivery-options`,

    // Order
    ORDERS: `${GATEWAY_URL}${SERVICES.ORDER}/orders`
};
