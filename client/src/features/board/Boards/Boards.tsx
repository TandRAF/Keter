import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { boardService, type BoardReadDto } from '../../../services/boardService'; 
import { useProjectContext } from '../../../pages/ProjectsDetailsPage/ProjectDetailsPage'; 
import style from "./Boards.module.scss"; 

const CreateBoardCard = ({ projectId, onBoardCreated }: { projectId: string, onBoardCreated: () => void }) => {
  const [isCreating, setIsCreating] = useState(false);
  const [name, setName] = useState("");
  const [isLoading, setIsLoading] = useState(false);

  const handleCreate = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!name) return;

    setIsLoading(true);
    try {
      await boardService.createBoard(projectId, name);
      setName("");
      setIsCreating(false); 
      onBoardCreated();   
    } catch (error) {
      console.error("Failed to create board", error);
    } finally {
      setIsLoading(false);
    }
  };

  if (!isCreating) {
    return (
      <div 
        className={`${style.CreateBoardCard} ${style.notSelected}`} 
        onClick={() => setIsCreating(true)}
      >
        <h3>+ <br/>New Board</h3>
      </div>
    );
  }

  return (
    <div className={style.CreateBoardCard}>
      <form onSubmit={handleCreate} className={style.createForm}>
        <input 
          type="text" 
          placeholder="Board Name" 
          value={name}
          onChange={(e) => setName(e.target.value)}
          required
          autoFocus 
        />
        
        <div style={{ display: 'flex', gap: '10px', marginTop: '10px' }}>
          <button type="submit" disabled={isLoading || !name} style={{ flex: 1, padding: '8px', backgroundColor: '#734FCF', color: 'white', border: 'none', borderRadius: '4px', cursor: 'pointer' }}>
            {isLoading ? "Saving..." : "Save"}
          </button>
          <button type="button" onClick={() => setIsCreating(false)} style={{ flex: 1, padding: '8px', backgroundColor: '#f1f1f1', color: '#333', border: 'none', borderRadius: '4px', cursor: 'pointer' }}>
            Cancel
          </button>
        </div>
      </form>
    </div>
  );
};

const BoardCard = ({ board, isOwner, projectId, onBoardUpdated }: { board: BoardReadDto, isOwner: boolean, projectId: string, onBoardUpdated: () => void }) => {
  const [isEditingName, setIsEditingName] = useState(false);
  const [editName, setEditName] = useState(board.name);

  const handleSaveEdit = async () => {
    setIsEditingName(false);

    if (editName !== board.name) {
      try {
        await boardService.updateBoard(board.id, editName);
        onBoardUpdated(); 
      } catch (error) {
        console.error("Failed to update board", error);
        setEditName(board.name);
      }
    }
  };

  return (
    <div className={style.boardCard}>
      {isEditingName && isOwner ? (
        <input 
          value={editName}
          onChange={(e) => setEditName(e.target.value)}
          onBlur={handleSaveEdit} 
          onKeyDown={(e) => e.key === 'Enter' && handleSaveEdit()} 
          autoFocus
          style={{ fontSize: '1.17em', fontWeight: 'bold', width: '100%', marginBottom: '15px', padding: '5px' }}
        />
      ) : (
        <h3 
          onDoubleClick={() => isOwner && setIsEditingName(true)}
          style={{ 
            cursor: isOwner ? 'pointer' : 'default',
            userSelect: 'none',
            borderBottom: isOwner ? '1px dashed #ccc' : 'none',
            marginBottom: '15px'
          }}
          title={isOwner ? "Double click to edit" : ""}
        >
          {board.name} {isOwner && <span style={{ fontSize: '12px', color: '#888' }}>✏️</span>}
        </h3>
      )}
      <Link className={style.viewLink} to={`/projects/${projectId}/boards/${board.id}`}>Open Board</Link>
    </div>
  );
};
export const Boards = () => {
  const { project, isOwner } = useProjectContext(); 
  const projectId = project.id; 

  const [boards, setBoards] = useState<BoardReadDto[]>([]);
  const [isLoading, setIsLoading] = useState(true);

  const fetchBoards = async () => {
    try {
      const data = await boardService.getBoardsByProject(projectId);
      setBoards(data);
    } catch (error) {
      console.error(error);
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => { 
    if (projectId) {
      fetchBoards(); 
    }
  }, [projectId]);

  if (isLoading) return <div>Loading Boards...</div>;

  return (
    <div className={style.container}>
      <h2>Project Boards</h2>
      
      <div style={{ display: 'flex', flexWrap: 'wrap', gap: '20px' }}>
        
        {isOwner && <CreateBoardCard projectId={projectId} onBoardCreated={fetchBoards} />}
        
        {boards.map((b) => (
          <BoardCard 
            key={b.id} 
            board={b} 
            isOwner={isOwner} 
            projectId={projectId}
            onBoardUpdated={fetchBoards} 
          />
        ))}
        
      </div>
    </div>
  );
};