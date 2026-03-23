import { api } from "./api";
import { type ColumnT } from "./taskService"; 
export interface BoardReadDto {
  id: string;
  name: string;
  projectId: string;
  columns: ColumnT[];
}

export const boardService = {
  getBoardData: async (boardId: string): Promise<BoardReadDto> => {
    const response = await api.get<BoardReadDto>(`/board/${boardId}`);
    return response.data;
  },

  getBoardsByProject: async (projectId: string): Promise<BoardReadDto[]> => {
    const response = await api.get<BoardReadDto[]>(`/board/project/${projectId}`);
    return response.data;
  },

  createBoard: async (projectId: string, name: string): Promise<BoardReadDto> => {
    const response = await api.post<BoardReadDto>("/board", { projectId, name });
    return response.data;
  },

  updateBoard: async (boardId: string, name: string): Promise<void> => {
    await api.put(`/board/${boardId}`, { name });
  },

  deleteBoard: async (boardId: string): Promise<void> => {
    await api.delete(`/board/${boardId}`);
  }
};