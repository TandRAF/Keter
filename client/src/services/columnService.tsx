import { api } from "./api";
import { type ColumnT } from "./taskService"; 

export const columnService = {
  createColumn: async (boardId: string, title: string, order: number): Promise<ColumnT> => {
    const response = await api.post<ColumnT>("/column", { 
      boardId, 
      title, 
      order 
    });
    return response.data;
  },

  updateColumn: async (columnId: string, title: string, order: number): Promise<void> => {
    await api.put(`/column/${columnId}`, { 
      title, 
      order 
    });
  },

  deleteColumn: async (columnId: string): Promise<void> => {
    await api.delete(`/column/${columnId}`);
  }
};