import { api } from "./api";

export interface UserPayload {
  id: string;
  userName: string;
  email?: string;
  profilePictureUrl?: string;
}

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
  deadline?: string;
  assignedUser?: UserPayload | null;
  tags?: TagT[];
}

export interface ColumnT {
  id: string;
  title: string;
  order: number;
  tasks: TaskT[];
}

export interface TaskCreatePayload {
  title: string;
  description?: string;
  columnId: string;
  status: string; // <--- UITE AICI AM ADĂUGAT STATUSUL
  assignedUserId?: string | null;
  deadline?: string;
  tagIds?: string[];
}

export interface TaskUpdatePayload {
  title: string;
  description?: string;
  status: string;
  assignedUserId?: string | null;
  deadline?: string;
  tagIds?: string[];
}

export const taskService = {
  moveTask: async (taskId: string, newColumnId: string, newOrder: number,newStatus:string): Promise<void> => {
    await api.put(`/task/${taskId}/move`, { 
      newColumnId, 
      newOrder,
      newStatus,
    });
  },

  createTask: async (payload: TaskCreatePayload): Promise<TaskT> => {
    const response = await api.post<TaskT>("/task", payload);
    return response.data;
  },

  updateTask: async (taskId: string, payload: TaskUpdatePayload): Promise<void> => {
    await api.put(`/task/${taskId}`, payload);
  }
};