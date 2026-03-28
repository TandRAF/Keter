import React, { useState, useEffect } from 'react';
import { NavLink } from 'react-router-dom';
import { projectService, type Project } from '../../../services/projectService';
import { boardService, type BoardReadDto } from '../../../services/boardService';
import styles from './ProjectNav.module.scss';
import { span } from 'framer-motion/client';

export const ProjectNav = () => {
  const [projects, setProjects] = useState<Project[]>([]);
  const [boardsByProject, setBoardsByProject] = useState<Record<string, BoardReadDto[]>>({});
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const fetchAllData = async () => {
      try {
        const projectsData = await projectService.getAllProjects();
        setProjects(projectsData);

        const boardsMap: Record<string, BoardReadDto[]> = {};
        
        await Promise.all(
          projectsData.map(async (project) => {
            const projectBoards = await boardService.getBoardsByProject(project.id);
            boardsMap[project.id] = projectBoards;
          })
        );
        
        setBoardsByProject(boardsMap);
      } catch (error) {
        console.error("Eroare la aducerea datelor:", error);
      } finally {
        setIsLoading(false);
      }
    };

    fetchAllData();
  }, []);

  if (isLoading) return <div className={styles.loading}>Se încarcă...</div>;

  return (
    <nav className={styles.navContainer}>
      <h3 className={styles.title}># Projects</h3>
      <ul className={styles.projectList}>

        {projects.length === 0 ? (
          <div className={styles.noProjectItem}>
            No Projects <br /> 
          </div>
        ) : (
          projects.map((project) => {
            const projectBoards = boardsByProject[project.id] || [];

            return (
              <li key={project.id} className={styles.projectItem}>
                
                <div className={styles.projectHeader}>
                  <NavLink 
                    to={`/projects/${project.id}`} 
                    className={({ isActive }) => isActive ? styles.activeProject : styles.projectLink}
                  >
                    {project.name}
                  </NavLink>
                </div>
                
                <ul className={styles.boardList}>
                  {projectBoards.length === 0 ? (
                      <span></span>
                  ) : (
                    projectBoards.map((board) => (
                      <li key={board.id}>
                        <svg width="16" height="10" viewBox="0 0 16 10" fill="none" xmlns="http://www.w3.org/2000/svg">
                          <path d="M15.25 9H9.25C4.27944 9 0.25 4.97056 0.25 0" stroke="#4D4E52" strokeWidth="0.5"/>
                        </svg>
                        
                        <NavLink 
                          to={`/projects/${project.id}/boards/${board.id}`} 
                          className={({ isActive }) => isActive ? styles.activeBoard : styles.boardLink}
                        >
                          {board.name}
                        </NavLink>

                        <span className={styles.taskCount}>
                          ({0})
                        </span>
                        
                      </li>
                    ))
                  )}
                </ul>
              </li>
            );
          })
        )}

      </ul>
    </nav>
  );
};