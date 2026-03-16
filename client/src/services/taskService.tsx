import { api } from "./api";

export interface TaskT {
  id: string;
  boardId: string;
  title: string;
  order: number;
  status: "TODO" | "IN_PROGRESS" | "DONE";
}

export const taskService = {
  getTasksByBoard: async (boardId: string): Promise<TaskT[]> => {
    const response = await api.get<TaskT[]>(`/boards/${boardId}/tasks`);
    return response.data;
  },
  updateTaskStatus: async (taskId: string, status: TaskT["status"]): Promise<TaskT> => {
    const response = await api.patch<TaskT>(`/tasks/${taskId}`, { status });
    return response.data;
  }
};