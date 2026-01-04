// start import css is like a general css 
import './App.css';
// end css import 
import axios from 'axios';// axios for fetching
import { API_URLS } from "./apiConfig"; 
import {useState ,useEffect} from 'react'; // usehooks 
import {Routes, Route} from 'react-router-dom'; //hooks for do routing in react 
//start import pages
import Home from './pages/home/home';
import Cart from './pages/cart/cart';
import Order from './pages/order/order';
import ProductDetails from './pages/ProductDetails/ProductDetails';
import Login from './pages/login/login';
import Logout from './pages/logout/logout';
import Signup from './pages/signup/signup';
import OrderItemDetail from './pages/OrderItemDetail/OrderItemDetail';
// end impot pages  

// function to generate UUID
function generateUUID() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
        var r = Math.random() * 16 | 0, v = c === 'x' ? r : ((r & 0x3) | 0x8);
        return v.toString(16);
    });
}

function App() {
    // Setup Axios interceptor to add Cart-ID header
    useEffect(() => {
        // Generate guest Cart-ID if not exists
        if (!localStorage.getItem('guestCartId')) {
            localStorage.setItem('guestCartId', `guest-${generateUUID()}`);
        }

        const interceptor = axios.interceptors.request.use(config => {
            const token = localStorage.getItem('token');
            const guestCartId = localStorage.getItem('guestCartId');
            
            // If token exists, send Authorization header. 
            // This ensures backend sees User.Identity.IsAuthenticated = true
            if (token) {
                config.headers['Authorization'] = `Bearer ${token}`;
            } else {
                // Only send guest ID if strictly anonymous (no token)
                config.headers['Cart-ID'] = guestCartId;
            }
            return config;
        }, error => {
            return Promise.reject(error);
        });

        return () => {
            axios.interceptors.request.eject(interceptor);
        };
    }, []);

    // start fetch products from backend
  // start fetch products from backend

 const [products, setProducts] = useState([]);
 const [filteredProducts, setFilteredProducts] = useState([]);
  const fetchProducts = async () => {
    try {
      const products = await axios.get(API_URLS.PRODUCTS);
      setProducts(products.data);
      setFilteredProducts(products.data);
    } catch (error) {
           console.log(error);
    }
  }
    useEffect(() => {
       fetchProducts();
    }, [products]);
  
  // end fetch product 

  //fetching cart
    const [carts, setCarts] = useState([]);
  const fetchCart = async () => {
    try {
      // If no token and no guest headers handled by interceptor, might fail, but let's assume interceptor works
        const carts = await axios.get(API_URLS.CART);
        setCarts(carts.data);
    } catch (error) {
      console.log(error);
    }
  }
    useEffect(() => {
        fetchCart(); // Changed to fetchCart
    }, [carts]);

    // ---end fetch products from backend---

  return (
    // start making routes
    <div className="App">
  
      <Routes>
        <Route path='/' element={<Home products={filteredProducts} carts={carts} />}/>
        <Route path='/home' element={<Home products={filteredProducts} carts={carts} />}/>

        <Route path='/product/:id' element={<ProductDetails carts={carts} />}/>
        <Route path='/orders' element={<Order carts={carts} />}/>
        <Route path='/cart' element={<Cart carts={carts} />}/>
        <Route path='/cart/:id' element={<OrderItemDetail carts={carts} />}/>
        <Route path='/login' element={<Login carts={carts} />}/>
        <Route path='/logout' element={<Logout carts={carts} />}/>
        <Route path='/signup' element={<Signup carts={carts} />}/>
        <Route path='*' element={<div>404 Not Found</div>}/>
      </Routes>
    </div>
    //end routes
  );
}

export default App;
