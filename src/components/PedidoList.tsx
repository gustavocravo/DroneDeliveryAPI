import { useEffect, useState } from "react";
import api from "../services/api";

interface Pedido {
  id: number;
  nome: string;
  peso: number;
}

export function PedidoList() {
  const [pedidos, setPedidos] = useState<Pedido[]>([]);

  useEffect(() => {
    api.get("/pedidos")
       .then(res => setPedidos(res.data))
       .catch(err => console.error(err));
  }, []);

  return (
    <div>
      <h2>Pedidos</h2>
      <ul>
        {pedidos.map(p => (
          <li key={p.id}>
            {p.nome} - {p.peso}kg
          </li>
        ))}
      </ul>
    </div>
  );
}
