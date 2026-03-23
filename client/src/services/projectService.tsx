import { api } from "./api";

export interface ProjectUser {
  id: string;
  userName: string;
  email?: string;
  profilePictureUrl?: string; 
}

export interface Project {
  id: string;
  name: string;
  description: string;
  createdAt: string;
  ownerId: string;         
  owner?: ProjectUser;   
  members?: ProjectUser[]; 
}

export const projectService = {
  getAllProjects: async (): Promise<Project[]> => {
    const response = await api.get<Project[]>("/project");
    return response.data;
  },

  getProjectById: async (projectId: string): Promise<Project> => {
    const response = await api.get<Project>(`/project/${projectId}`);
    return response.data;
  },

createProject: async (name: string, description: string): Promise<Project> => {
    const response = await api.post<Project>("/project", { name, description});
    return response.data;
  },

  updateProject: async (projectId: string, name: string, description: string): Promise<void> => {
    await api.put(`/project/${projectId}`, { name, description });
  },

  deleteProject: async (projectId: string): Promise<void> => {
    await api.delete(`/project/${projectId}`);
  },

  addMember: async (projectId: string, username: string): Promise<void> => {
    await api.post(`/project/${projectId}/members`, { Username: username }); 
  },

  removeMember: async (projectId: string, userId: string): Promise<void> => {
    await api.delete(`/project/${projectId}/members/${userId}`);
  }
};