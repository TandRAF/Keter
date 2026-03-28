import { api } from "./api";
import { type TagT } from "./taskService";

export const tagService = {
  getProjectTags: async (projectId: string): Promise<TagT[]> => {
    const response = await api.get(`/projects/${projectId}/tags`);
    return response.data;
  },
  createTag: async (projectId: string, name: string, colorHex: string): Promise<TagT> => {
    const response = await api.post(`/projects/${projectId}/tags`, { name, colorHex });
    return response.data;
  }
};