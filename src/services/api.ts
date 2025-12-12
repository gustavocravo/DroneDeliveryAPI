import axios from "axios";

const api = axios.create({
  baseURL: "http://localhost:7019/api"
});

export default api;

