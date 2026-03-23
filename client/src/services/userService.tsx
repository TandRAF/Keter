import { api } from "./api";

export const userService = {
  searchUsers: async (query: string): Promise<{ id: string; userName: string }[]> => {
    const response = await api.get(`/auth/search?q=${query}`); 
    return response.data;
  }
};