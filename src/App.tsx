import { DroneList } from "./components/DroneList";
import { PedidoList } from "./components/PedidoList";
import { ViagemList } from "./components/ViagemList";

function App() {
  return (
    <div style={{ padding: "20px", fontFamily: "Arial, sans-serif" }}>
      <h1>Drone Delivery</h1>
      <DroneList />
      <PedidoList />
      <ViagemList />
    </div>
  );
}

export default App;
