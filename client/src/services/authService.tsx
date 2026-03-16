import { api } from "./api";

interface LoginResponse {
  token: string;
  user: {
    email: string;
    id: string;
  };
}

export const authService = {
  login: async (username: string, password: string): Promise<LoginResponse> => {
    const response = await api.post<LoginResponse>("/auth/sign-in", {
      username,
      password,
    });
    return response.data;
  },

  register: async (username: string, email: string, password: string) => {
    const response = await api.post("/auth/sign-up", {
      username,
      email,
      password,
    });
    return response.data;
  },
};