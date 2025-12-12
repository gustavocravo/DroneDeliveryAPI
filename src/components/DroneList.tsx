import { useEffect, useState } from "react";
import api from "../services/api";

interface Drone {
  id: number;
  nome: string;
  bateria: number;
}

export function DroneList() {
  const [drones, setDrones] = useState<Drone[]>([]);

  useEffect(() => {
    api.get("/drones")
       .then(res => setDrones(res.data))
       .catch(err => console.error(err));
  }, []);

  return (
    <div>
      <h2>Drones</h2>
      <ul>
        {drones.map(d => (
          <li key={d.id}>
            {d.nome} - {d.bateria}%
          </li>
        ))}
      </ul>
    </div>
  );
}
