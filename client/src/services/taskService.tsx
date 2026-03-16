import { api } from "./api";

export interface TagT {
  id: string;
  name: string;
  colorHex: string;
}
export interface TaskT {
  id: string;
  title: string;
  description?: string;
  columnId: string;
  order: number;
  status: string; 
  assignedUserId?: string | null;
  tags: TagT[];
}

export interface ColumnT {
  id: string;
  title: string;
  order: number;
  tasks: TaskT[];
}

export const taskService = {
  moveTask: async (taskId: string, newColumnId: string, newOrder: number): Promise<void> => {
    await api.put(`/task/${taskId}/move`, { 
      newColumnId, 
      newOrder 
    });
  }
};