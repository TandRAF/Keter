import { api } from "./api";

export interface Project {
  id: string;
  name: string;
  description: string;
  createdAt: string;
}

export const projectService = {
  getAllProjects: async (): Promise<Project[]> => {
    const response = await api.get<Project[]>("/projects");
    return response.data;
  },

  createProject: async (name: string, description: string): Promise<Project> => {
    const response = await api.post<Project>("/projects", { name, description });
    return response.data;
  },

  deleteProject: async (projectId: string): Promise<void> => {
    await api.delete(`/projects/${projectId}`);
  }
};