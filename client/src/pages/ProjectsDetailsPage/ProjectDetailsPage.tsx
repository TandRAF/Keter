import React, { useEffect, useState, createContext, useContext } from 'react';
import { useParams } from 'react-router-dom';
import { projectService, type Project } from '../../services/projectService';
import { AddMemberButton } from '../../features/projects/AddMemberButton/AddmemberButton';
interface ProjectContextType {
  project: Project;
  isOwner: boolean;
}
const ProjectContext = createContext<ProjectContextType | null>(null);

export const useProjectContext = () => {
  const context = useContext(ProjectContext);
  if (!context) throw new Error("Trebuie folosit în interiorul <ProjectDetails>");
  return context;
};

export const ProjectDetails = ({ children }: { children: React.ReactNode }) => {
  const { projectId } = useParams<{ projectId: string }>();
  const [project, setProject] = useState<Project | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  const currentUser = JSON.parse(localStorage.getItem("user") || "{}");
  const currentUserId = currentUser.id;

  const fetchProjectData = async () => {
    if (!projectId) return;
    try {
      const projectData = await projectService.getProjectById(projectId);
      setProject(projectData);
    } catch (error) {
      console.error("Error fetching project details:", error);
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    fetchProjectData();
  }, [projectId]);

  if (isLoading || !project) return <div>Loading project workspace...</div>;

  const isOwner = String(project.owner?.id || project.ownerId) === String(currentUserId);

  return (
    <ProjectContext.Provider value={{ project, isOwner }}>
      <div style={{ padding: '2rem', display: 'flex', flexDirection: 'column', height: '100vh' }}>
        <header style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', borderBottom: '1px solid #333', paddingBottom: '1rem', marginBottom: '2rem' }}>
          <div>
            <h1 style={{ margin: 0 }}>{project.name}</h1>
            <p style={{ margin: 0, color: '#888' }}>{project.description}</p>
          </div>
          <div style={{ display: 'flex', alignItems: 'center', gap: '15px' }}>
            <div style={{ display: 'flex', gap: '5px' }}>
              {project.owner && (
                 <span title={`${project.owner.userName} (Owner)`}>👑</span>
              )}
              {project.members?.map(m => (
                  <img 
                    key={m.id} 
                    src={m.profilePictureUrl || "/default-avatar.png"} 
                    alt={m.userName} 
                    title={m.userName} 
                    style={{ width: '30px', height: '30px', borderRadius: '50%' }}
                  />
              ))}
            </div>
            <AddMemberButton 
              projectId={project.id} 
              isOwner={isOwner} 
              onMemberAdded={fetchProjectData} 
            />
          </div>
        </header>
        <main style={{ flex: 1, overflowY: 'auto' }}>
          {children}
        </main>
      </div>
    </ProjectContext.Provider>
  );
};