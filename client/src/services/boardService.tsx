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
  }
};