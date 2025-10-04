import { Client } from "../Client";

const baseUrl = import.meta.env.API_BASE_URL ?? "";

export const apiClient = new Client(baseUrl);


