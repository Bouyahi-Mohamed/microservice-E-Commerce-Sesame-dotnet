// start import component 
import  Footer  from '../../components/footer/footer.js';
import Header1 from '../../components/header1/header1.js';
import ProductList from './productList/productList.js';
// end import component 

function Home({ products, carts }) {

  return (
   <>
    <Header1 carts={carts} />
    <ProductList products={products} />
    <Footer />
  </>
  );
}
export default Home;
