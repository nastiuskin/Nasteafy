import axios from "axios";
import { Client } from "./apiClient";

const axiosInstance = axios.create({
  baseURL: "https://localhost:7041",
  withCredentials: true,
});

axiosInstance.interceptors.request.use((config) => {
  const token = localStorage.getItem("accessToken");
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export const client = new Client(undefined, axiosInstance);

