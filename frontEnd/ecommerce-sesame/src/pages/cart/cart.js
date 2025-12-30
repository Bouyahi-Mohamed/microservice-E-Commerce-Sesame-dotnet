import Header2 from "../../components/header2/header2";
import CartList from "./CartList/CartList";

export default function Cart({ carts }) {



  return (
    <div>
      <Header2 carts={carts} />
      <CartList carts={carts} />
    </div>
  );
}