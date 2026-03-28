import React, { useEffect, useState, createContext, useContext } from 'react';
import { useParams, Link } from 'react-router-dom';
import { projectService, type Project } from '../../services/projectService';
import { boardService, type BoardReadDto } from '../../services/boardService'; // <-- Importă serviciul pentru board
import { AddMemberButton } from '../../features/projects/AddMemberButton/AddmemberButton';
import style from './ProjectDetailsPage.module.scss';
import { Boards } from '../../components/Icons/Icons';

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
  const { projectId, boardId } = useParams<{ projectId: string; boardId: string }>(); 
  const [project, setProject] = useState<Project | null>(null);
  const [boardName, setBoardName] = useState<string | null>(null); 
  const [isLoading, setIsLoading] = useState(true);

  const currentUser = JSON.parse(localStorage.getItem("user") || "{}");
  const currentUserId = currentUser.id;

  const fetchData = async () => {
    if (!projectId) return;
    try {
      const projectData = await projectService.getProjectById(projectId);
      setProject(projectData);
      if (boardId) {
        const boardData = await boardService.getBoardData(boardId);
        setBoardName(boardData.name);
      } else {
        setBoardName(null); 
      }
      
    } catch (error) {
      console.error("Error fetching details:", error);
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    fetchData();
  }, [projectId, boardId]);

  if (isLoading || !project) return <div>Loading project workspace...</div>;

  const isOwner = String(project.owner?.id || project.ownerId) === String(currentUserId);

  return (
    <ProjectContext.Provider value={{ project, isOwner }}>
      <div className={style.container}>
        <header>
          <div className={style.navBoards}>
            <Link to={`/projects/${project.id}`}>
            <Boards/> 
            <span>Boards</span>
          </Link>
          </div>
          <h1>{project.name}</h1>
          <div className={style.members}>
            <div>
              {project.members?.map(m => (
                  <img 
                    key={m.id} 
                    src={m.profilePictureUrl || "/default-avatar.png"} 
                    alt={m.userName} 
                    title={m.userName} 
                  />
              ))}
            </div>
            <AddMemberButton 
              projectId={project.id} 
              isOwner={isOwner} 
              onMemberAdded={fetchData} 
            />
          </div>
        </header>
        <div className={style.projectNav}>
            <h1 className={style.breadcrumbs}>
              <Link to={`/projects`} className={style.breadcrumbLink}>
                Projects
              </Link>
              <span className={style.separator}> / </span>
              <Link to={`/projects/${project.id}`} className={style.breadcrumbLink}>
                { project.name}
              </Link>
              {boardName && (
                <>
                  <span className={style.separator}> / </span>
                  <span className={style.breadcrumbActive}>{boardName}</span>
                </>
              )}
            </h1>
          </div>
        <main>
          {children}
        </main>
      </div>
    </ProjectContext.Provider>
  );
};