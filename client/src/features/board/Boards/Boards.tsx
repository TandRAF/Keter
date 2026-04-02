import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { boardService, type BoardReadDto } from '../../../services/boardService'; 
import { useProjectContext } from '../../../pages/ProjectsDetailsPage/ProjectDetailsPage'; 
import style from "./Boards.module.scss"; 
import { AddBlock } from '../../../components/Icons/Icons';

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
        <AddBlock/>
        <h3>New Board</h3>
      </div>
    );
  }

  return (
    <div className={style.CreateBoardCard}>
      <form onSubmit={handleCreate} className={style.createForm} style={{ display: 'flex', flexDirection: 'column', height: '100%' }}>
        <input 
          type="text" 
          placeholder="Board Name" 
          value={name}
          onChange={(e) => setName(e.target.value)}
          required
          autoFocus 
          style={{ padding: '8px', background: '#222', color: '#fff', border: '1px solid #555', borderRadius: '4px' }}
        />
        
        <div style={{ display: 'flex', gap: '10px', marginTop: 'auto' }}>
          <button type="submit" disabled={isLoading || !name} style={{ flex: 1, padding: '8px', backgroundColor: '#734FCF', color: 'white', border: 'none', borderRadius: '4px', cursor: 'pointer' }}>
            {isLoading ? "Saving..." : "Save"}
          </button>
          <button type="button" onClick={() => setIsCreating(false)} style={{ flex: 1, padding: '8px', backgroundColor: '#444', color: '#fff', border: 'none', borderRadius: '4px', cursor: 'pointer' }}>
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

  const columns = board.columns || [];

  const totalTasks = columns.reduce((acc, col) => acc + (col.tasks?.length || 0), 0);
  const maxTasksInColumn = Math.max(...columns.map(col => col.tasks?.length || 0), 1); 

  return (
    <div className={style.boardCard}>
      {isEditingName && isOwner ? (
        <input 
          value={editName}
          onChange={(e) => setEditName(e.target.value)}
          onBlur={handleSaveEdit} 
          onKeyDown={(e) => e.key === 'Enter' && handleSaveEdit()} 
          autoFocus
        />
      ) : (
            <h3 
            onDoubleClick={() => isOwner && setIsEditingName(true)}
            title={isOwner ? "Double click to edit" : ""}
            >
            {board.name}
            </h3>
      )}

      <div className={style.columnContainer}>
          {columns.length > 0 ? (
            <>
              <div className={style.columns}>
                  {columns.map((col) => {
                      const taskCount = col.tasks?.length || 0;
                      const barHeight = (taskCount / maxTasksInColumn) * 50; 
                      
                      return (
                          <div key={col.id} style={{ flex: 1, display: 'flex', flexDirection: 'column', alignItems: 'center', gap: '4px' }}>
                              <span>{taskCount}</span>
                              <div style={{ 
                                  width: '100%', 
                                  height: `${barHeight || 2}px`,
                                  backgroundColor: taskCount > 0 ? '#734FCF' : '#333', 
                                  borderRadius: '2px 2px 0 0',
                                  transition: 'height 0.3s ease'
                              }}></div>
                          </div>
                      )
                  })}
              </div>
              <div className={style.columnTitles}>
                  {columns.map(col => (
                      <span className={style.title} key={col.id} style={{ flex: 1, textAlign: 'center', fontSize: '9px', color: '#666', overflow: 'hidden', textOverflow: 'ellipsis', whiteSpace: 'nowrap' }} title={col.title}>
                          {col.title}
                      </span>
                  ))}
              </div>
            </>
          ) : (
              <div className={style.noColumnYet} >
                  No columns yet
              </div>
          )}
      </div>

      <div style={{ marginTop: 'auto', display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
        <span style={{ fontSize: '0.8rem', color: '#888' }}>Total tasks: {totalTasks}</span>
        <Link 
            className={style.viewLink} 
            to={`/projects/${projectId}/boards/${board.id}`}
        >
            Open Board
        </Link>
      </div>
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

  if (isLoading) return <div style={{ color: '#fff', padding: '20px' }}>Loading Boards...</div>;

  return (
    <div className={style.container} style={{width:"100%"}}>
      <h2>Boards ({boards.length})</h2>
      
      <div className={style.BoardsContainer}>
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