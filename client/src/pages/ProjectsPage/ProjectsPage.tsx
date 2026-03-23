import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { projectService, type Project } from '../../services/projectService';
import { userService } from '../../services/userService'; // Needed for the Create Card search
import { AddMemberButton } from '../../features/projects/AddMemberButton/AddmemberButton';
import style from "./ProjectPage.module.scss";

const CreateProjectCard = ({ onProjectCreated }: { onProjectCreated: () => void }) => {
  const [isCreating, setIsCreating] = useState(false);
  const [name, setName] = useState("");
  const [description, setDescription] = useState("");
  const [isLoading, setIsLoading] = useState(false);

  const [searchQuery, setSearchQuery] = useState("");
  const [searchResults, setSearchResults] = useState<{id: string, userName: string}[]>([]);
  const [selectedMembers, setSelectedMembers] = useState<{id: string, userName: string}[]>([]);
  useEffect(() => {
    const search = async () => {
      if (searchQuery.length < 2) {
        setSearchResults([]);
        return;
      }
      try {
        const results = await userService.searchUsers(searchQuery);
        const filtered = results.filter(r => !selectedMembers.find(sm => sm.id === r.id));
        setSearchResults(filtered);
      } catch (error) {
        console.error("Search failed", error);
      }
    };
    const timeoutId = setTimeout(search, 300); 
    return () => clearTimeout(timeoutId);
  }, [searchQuery, selectedMembers]);

  const handleAddPendingMember = (user: {id: string, userName: string}) => {
    setSelectedMembers([...selectedMembers, user]);
    setSearchQuery(""); 
    setSearchResults([]);
  };

  const handleRemovePendingMember = (id: string) => {
    setSelectedMembers(selectedMembers.filter(m => m.id !== id));
  };

  const handleCreate = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!name) return;

    setIsLoading(true);
    try {
      const newProject = await projectService.createProject(name, description);
      for (const member of selectedMembers) {
        try {
          await projectService.addMember(newProject.id, member.userName);
        } catch (err) {
          console.error(`Failed to add ${member.userName}`);
        }
      }
      setName("");
      setDescription("");
      setSelectedMembers([]);
      setIsCreating(false); 
      onProjectCreated();   
    } catch (error) {
      console.error("Failed to create project", error);
    } finally {
      setIsLoading(false);
    }
  };

  if (!isCreating) {
    return (
      <div 
        className={`${style.CreatepProjectCard} ${style.notSelected}`} 
        onClick={() => setIsCreating(true)}
      >
        <h3>+ <br/>New Project</h3>
      </div>
    );
  }

  return (
    <div className={style.CreatepProjectCard}>
      <form onSubmit={handleCreate} className={style.createForm}>
        <input 
          type="text" 
          placeholder="Project Name" 
          value={name}
          onChange={(e) => setName(e.target.value)}
          required
          autoFocus 
        />
        <textarea 
          placeholder="Short Description" 
          value={description}
          onChange={(e) => setDescription(e.target.value)}
          rows={3}
        />
        <div style={{ position: 'relative', marginTop: '5px' }}>
          <input 
            type="text" 
            placeholder="Search users to add..." 
            value={searchQuery}
            onChange={(e) => setSearchQuery(e.target.value)}
          />
          {searchResults.length > 0 && (
            <ul>
              {searchResults.map(user => (
                <li 
                  key={user.id} 
                  onClick={() => handleAddPendingMember(user)}
                  style={{ padding: '8px', cursor: 'pointer', borderBottom: '1px solid #444', color: 'white' }}
                >
                  {user.userName}
                </li>
              ))}
            </ul>
          )}
        </div>
        {selectedMembers.length > 0 && (
          <div>
            {selectedMembers.map(m => (
              <span key={m.id} style={{ background: '#734FCF', color: 'white', padding: '2px 8px', borderRadius: '12px', fontSize: '12px', display: 'flex', alignItems: 'center', gap: '5px' }}>
                {m.userName}
                <button type="button" onClick={() => handleRemovePendingMember(m.id)} style={{ background: 'transparent', border: 'none', color: 'white', cursor: 'pointer', fontSize: '14px', padding: 0 }}>&times;</button>
              </span>
            ))}
          </div>
        )}
        <div style={{ display: 'flex', gap: '10px', marginTop: '10px' }}>
          <button type="submit" disabled={isLoading || !name} style={{ flex: 1, padding: '8px', backgroundColor: '#734FCF', color: 'white', border: 'none', borderRadius: '4px', cursor: 'pointer' }}>
            {isLoading ? "Saving..." : "Save"}
          </button>
          <button type="button" onClick={() => { setIsCreating(false); setSelectedMembers([]); }} style={{ flex: 1, padding: '8px', backgroundColor: '#f1f1f1', color: '#333', border: 'none', borderRadius: '4px', cursor: 'pointer' }}>
            Cancel
          </button>
        </div>
      </form>
    </div>
  );
};

const ProjectCard = ({ project, onProjectUpdated }: { project: Project, onProjectUpdated: () => void }) => {
  const currentUser = JSON.parse(localStorage.getItem("user") || "{}");
  const currentUserId = currentUser.id;
  const isOwner = String(project.owner?.id || project.ownerId) === String(currentUserId);
  const [isEditingName, setIsEditingName] = useState(false);
  const [isEditingDesc, setIsEditingDesc] = useState(false);
  const [editName, setEditName] = useState(project.name);
  const [editDesc, setEditDesc] = useState(project.description);

  const handleSaveEdit = async () => {
    setIsEditingName(false);
    setIsEditingDesc(false);

    if (editName !== project.name || editDesc !== project.description) {
      try {
        await projectService.updateProject(project.id, editName, editDesc);
        onProjectUpdated(); 
      } catch (error) {
        console.error("Failed to update project", error);
        setEditName(project.name);
        setEditDesc(project.description);
      }
    }
  };

  return (
    <div className={style.projectCard}>
      <p className={style.own}>{project.owner?.userName || "Loading..."}</p>

      {isEditingName && isOwner ? (
        <input 
          value={editName}
          onChange={(e) => setEditName(e.target.value)}
          onBlur={handleSaveEdit} 
          onKeyDown={(e) => e.key === 'Enter' && handleSaveEdit()} 
          autoFocus
          style={{ fontSize: '1.17em', fontWeight: 'bold', width: '100%', marginBottom: '10px', padding: '5px' }}
        />
      ) : (
        <h3 
          onDoubleClick={() => isOwner && setIsEditingName(true)}
          style={{ 
            cursor: isOwner ? 'pointer' : 'default',
            userSelect: 'none',
            borderBottom: isOwner ? '1px dashed #ccc' : 'none' 
          }}
          title={isOwner ? "Double click to edit" : ""}
        >
          {project.name} {isOwner && <span style={{ fontSize: '12px', color: '#888' }}>✏️</span>}
        </h3>
      )}
      {isEditingDesc && isOwner ? (
        <textarea 
          value={editDesc}
          onChange={(e) => setEditDesc(e.target.value)}
          onBlur={handleSaveEdit} 
          autoFocus
          rows={3}
          style={{ width: '100%', resize: 'vertical', marginBottom: '10px', padding: '5px' }}
        />
      ) : (
        <p 
          onDoubleClick={() => isOwner && setIsEditingDesc(true)}
          style={{ 
            cursor: isOwner ? 'pointer' : 'default', 
            minHeight: '20px',
            userSelect: 'none',
            borderBottom: isOwner ? '1px dashed #ccc' : 'none'
          }}
          title={isOwner ? "Double click to edit" : ""}
        >
          {project.description || (isOwner ? "Double click to add a description..." : "No description.")}
        </p>
      )}

      <div>
        <div className={style.membersIcons}>
          {project.members?.map(m => (
              <img key={m.id} src={m.profilePictureUrl} alt={m.userName} title={m.userName} />
          ))}
          <AddMemberButton 
            projectId={project.id} 
            isOwner={isOwner} 
            onMemberAdded={onProjectUpdated} 
          />
        </div>
      </div>

      <Link className={style.viewLink} to={`/projects/${project.id}`}>View Project</Link>
    </div>
  );
};

export const ProjectsPage = () => {
  const [projects, setProjects] = useState<Project[]>([]);
  const [isLoading, setIsLoading] = useState(true);

  const fetchProjects = async () => {
    try {
      const data = await projectService.getAllProjects();
      setProjects(data);
    } catch (error) {
      console.error(error);
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => { fetchProjects(); }, []);

  if (isLoading) return <div>Loading Projects...</div>;

  return (
    <div className={style.container}>
      <h1>My Projects</h1>
      
      <div style={{ display: 'flex', flexWrap: 'wrap', gap: '20px' }}>

        <CreateProjectCard onProjectCreated={fetchProjects} />
        {projects.map((p) => (
          <ProjectCard key={p.id} project={p} onProjectUpdated={fetchProjects} />
        ))}
        
      </div>
    </div>
  );
};