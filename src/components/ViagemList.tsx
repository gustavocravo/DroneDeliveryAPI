import { useEffect, useState } from "react";
import api from "../services/api";

interface Viagem {
  id: number;
  droneNome: string;
  pedidos: string[];
  distanciaTotalKm: number;
  consumoTotalPercentual: number;
}

export function ViagemList() {
  const [viagens, setViagens] = useState<Viagem[]>([]);

  useEffect(() => {
    api.get("/viagens")
       .then(res => setViagens(res.data))
       .catch(err => console.error(err));
  }, []);

  return (
    <div>
      <h2>Viagens</h2>
      {viagens.map(v => (
        <div key={v.id} style={{ border: "1px solid gray", marginBottom: "10px", padding: "5px" }}>
          <p>Drone: {v.droneNome}</p>
          <p>Pedidos: {v.pedidos.join(", ")}</p>
          <p>Distância: {v.distanciaTotalKm} km</p>
          <p>Consumo: {v.consumoTotalPercentual}%</p>
        </div>
      ))}
    </div>
  );
}
