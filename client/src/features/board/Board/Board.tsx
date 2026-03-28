import React, { useState, useEffect, useCallback } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { DragDropContext, Droppable, Draggable, type DropResult } from '@hello-pangea/dnd';
import styles from "./Boars.module.scss"; 
import { Task } from '../../task/Task/Task';
import { type ColumnT, type TaskT, taskService, type TagT } from '../../../services/taskService';
import { boardService } from '../../../services/boardService';
import { columnService } from '../../../services/columnService'; 
import { TaskModal } from '../../task/TaskModal/TaskModal'; 
import { useProjectContext } from '../../../pages/ProjectsDetailsPage/ProjectDetailsPage'; 
import InputData from '../../../components/InputData/InputData';

const CreateTaskBlock = ({ 
  columnId, 
  columnTitle, 
  onClose,
  onTaskCreated, 
  members,
  availableTags 
}: { 
  columnId: string; 
  columnTitle: string;
  onClose: () => void;
  onTaskCreated: () => void;
  members: any[]; 
  availableTags?: TagT[]; 
}) => {
  const [title, setTitle] = useState("");
  const [assignedUserId, setAssignedUserId] = useState<string>("");
  const [deadline, setDeadline] = useState<string>("");
  const [selectedTagIds, setSelectedTagIds] = useState<string[]>([]);
  const [isLoading, setIsLoading] = useState(false);

const handleCreate = async (e: React.FormEvent) => {
    e.preventDefault();
    const cleanTitle = title.trim();
    if (!cleanTitle) return;

    setIsLoading(true);

    const payload = {
        title: cleanTitle,
        columnId: columnId,
        status: columnTitle,
        assignedUserId: assignedUserId || null, 
        deadline: deadline || undefined, 
        tagIds: selectedTagIds.length > 0 ? selectedTagIds : undefined
    };

    console.log("SENDING TASK PAYLOAD TO SERVER:", payload);

    try {
      await taskService.createTask(payload);
      
      onTaskCreated(); 
      onClose();
    } catch (error) {
      console.error("Failed to create task:", error);
    } finally {
      setIsLoading(false);
    }
  };
  const toggleTag = (tagId: string) => {
    setSelectedTagIds(prev => 
      prev.includes(tagId) ? prev.filter(id => id !== tagId) : [...prev, tagId]
    );
  };

  return (
      <div className={styles.container}>
        <h5>Create New Task</h5>
        
        <form onSubmit={handleCreate}>
          
          <input className={styles.createTitle}
            autoFocus 
            placeholder="Task title..." 
            value={title} 
            onChange={(e) => setTitle(e.target.value)}
            maxLength={100}
          />
          
          <InputData
            type="user-select"
            id={`create-assignee`} 
            label="Assign To"
            placeholder="-- Unassigned --"
            userOptions={members}
            onChange={(e) => setAssignedUserId(e.target.value)}
          />

          <InputData
            type="calendar"
            id={`create-deadline`}
            label="Deadline"
            placeholder="Select deadline..."
            onChange={(e) => setDeadline(e.target.value)}
          />

          {availableTags && availableTags.length > 0 && (
            <div style={{ display: 'flex', flexWrap: 'wrap', gap: '4px', marginTop: '4px' }}>
              {availableTags.map(tag => (
                <span 
                  key={tag.id}
                  onClick={() => toggleTag(tag.id)}
                  style={{
                    padding: '4px 10px', fontSize: '0.8rem', borderRadius: '12px', cursor: 'pointer',
                    backgroundColor: selectedTagIds.includes(tag.id) ? tag.colorHex : '#444', color: 'white',
                    border: selectedTagIds.includes(tag.id) ? '1px solid white' : '1px solid transparent'
                  }}
                >
                  {tag.name}
                </span>
              ))}
            </div>
          )}

          <div style={{ display: 'flex', gap: '10px', marginTop: '16px' }}>
            <button 
              type="submit" 
              disabled={isLoading || !title.trim()} 
              style={{ flex: 1, padding: '10px', backgroundColor: '#734FCF', color: 'white', border: 'none', borderRadius: '4px', cursor: 'pointer', fontWeight: 'bold' }}
            >
              {isLoading ? "Creating..." : "Create Task"}
            </button>
            <button 
              type="button" 
              onClick={onClose} 
              style={{ flex: 1, padding: '10px', backgroundColor: '#444', color: 'white', border: 'none', borderRadius: '4px', cursor: 'pointer', fontWeight: 'bold' }}
            >
              Cancel
            </button>
          </div>
        </form>
      </div>
  );
};


export const Board: React.FC = () => {
  const { boardId } = useParams<{ boardId: string }>();
  const navigate = useNavigate();
  const { project } = useProjectContext(); 

  const [columns, setColumns] = useState<ColumnT[]>([]);
  const [boardName, setBoardName] = useState(""); 
  const [boardTags, setBoardTags] = useState<TagT[]>([]); 
  const [isLoading, setIsLoading] = useState(true);
  
  const [selectedTask, setSelectedTask] = useState<TaskT | null>(null);
  const [activeColumnForNewTask, setActiveColumnForNewTask] = useState<string | null>(null); 

  const [isCreatingCol, setIsCreatingCol] = useState(false);
  const [newColTitle, setNewColTitle] = useState("");
  const [editingColId, setEditingColId] = useState<string | null>(null);
  const [editColTitle, setEditColTitle] = useState("");

  const [isBrowserReady, setIsBrowserReady] = useState(false);

  useEffect(() => {
    const animation = requestAnimationFrame(() => setIsBrowserReady(true));
    return () => cancelAnimationFrame(animation);
  }, []);

  const fetchBoard = useCallback(async () => {
    if (!boardId) return;
    try {
      const boardData = await boardService.getBoardData(boardId);
      setColumns(boardData.columns || []); 
      setBoardName(boardData.name);
    } catch (error) {
      console.error("Eroare la încărcarea board-ului:", error);
      navigate(`/projects/${project.id}`); 
    } finally {
      setIsLoading(false);
    }
  }, [boardId, project.id, navigate]);

  useEffect(() => {
    fetchBoard();
  }, [fetchBoard]);

  const handleCreateColumn = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!newColTitle.trim() || !boardId) return;

    try {
      await columnService.createColumn(boardId, newColTitle, columns.length);
      setNewColTitle("");
      setIsCreatingCol(false);
      await fetchBoard();
    } catch (error) {
      console.error("Failed to create column", error);
    }
  };

  const handleUpdateColumnTitle = async (colId: string, currentOrder: number) => {
    setEditingColId(null);
    const col = columns.find(c => c.id === colId);

    if (col && col.title !== editColTitle && editColTitle.trim() !== "") {
      try {
        await columnService.updateColumn(colId, editColTitle, currentOrder);
        await fetchBoard();
      } catch (error) {
        console.error("Failed to update column", error);
      }
    }
  };

  const handleDeleteColumn = async (colId: string) => {
    if (window.confirm("Are you sure you want to delete this column and all its tasks?")) {
      try {
        await columnService.deleteColumn(colId);
        await fetchBoard();
      } catch (error) {
        console.error("Failed to delete column", error);
      }
    }
  };

  const onDragEnd = async (result: DropResult): Promise<void> => {
    const { destination, source, draggableId } = result;
    if (!destination) return;
    if (destination.droppableId === source.droppableId && destination.index === source.index) return;
    
    const newColumns = JSON.parse(JSON.stringify(columns)) as ColumnT[];
    
    const sourceCol = newColumns.find(c => String(c.id) === source.droppableId);
    const destCol = newColumns.find(c => String(c.id) === destination.droppableId);

    if (!sourceCol || !destCol) return;
    const [movedTask] = sourceCol.tasks.splice(source.index, 1);
    
    movedTask.columnId = destCol.id;
    movedTask.status = destCol.title; 
    
    destCol.tasks.splice(destination.index, 0, movedTask);

    setColumns(newColumns);
    try {
      await taskService.moveTask(draggableId, destCol.id, destination.index,destCol.title);
    } catch (error) {
      console.error("Eroare la salvarea pe server. Reverting UI...", error);
      fetchBoard();
    }
  };

  const handleSaveTaskDetails = async (updatedTask: TaskT) => {
    setColumns(prevColumns => prevColumns.map(col => ({
      ...col,
      tasks: col.tasks.map(t => t.id === updatedTask.id ? updatedTask : t)
    })));
  };

  if (isLoading) return <div style={{ color: "white", padding: "2rem" }}>Loading Board...</div>;

  return (
    <>
    <div className={styles.container}>
      <h2 style={{ margin: 0 }}>{boardName}</h2>
        <div className={styles.boardContainer}>
           <DragDropContext onDragEnd={onDragEnd}>
        <div className={styles.board}>
          {columns.map((column) => (
            <div className={styles.column} key={column.id}>
              <div>
                {editingColId === column.id ? (
                  <div>
                    <input 
                      autoFocus 
                      value={editColTitle} 
                      onChange={(e) => setEditColTitle(e.target.value)} 
                      onBlur={() => handleUpdateColumnTitle(column.id, column.order)}
                      onKeyDown={(e) => e.key === 'Enter' && handleUpdateColumnTitle(column.id, column.order)}
                      style={{ padding: '5px', fontSize: '1rem', width: '100%', borderRadius: '4px', border: '1px solid #734FCF', background: '#222', color: 'white' }}
                    />
                    <button 
                      onMouseDown={() => handleDeleteColumn(column.id)}
                      style={{ padding: '4px', fontSize: '0.8rem', backgroundColor: '#d93838', color: 'white', border: 'none', borderRadius: '4px', cursor: 'pointer', marginTop: '4px' }}
                    >
                      Delete Column
                    </button>
                  </div>
                ) : (
                  <h4 
                    onDoubleClick={() => { 
                      setEditingColId(column.id); 
                      setEditColTitle(column.title); 
                    }}
                    style={{ cursor: 'pointer', userSelect: 'none', margin: 0 }}
                    title="Double-click to edit or delete"
                  >
                    {column.title}
                  </h4>
                )}
              </div>
            
              {isBrowserReady && (
                <Droppable droppableId={String(column.id)}>
                  {(provided, snapshot) => (
                    <div 
                      {...provided.droppableProps} 
                      ref={provided.innerRef}
                      style={{ minHeight: '10px' }} 
                    >
                      {column.tasks && column.tasks.map((task, index) => (
                        <Draggable key={String(task.id)} draggableId={String(task.id)} index={index}>
                          {(provided, snapshot) => (
                            <Task 
                              provided={provided} 
                              snapshot={snapshot} 
                              taskData={task} 
                              onDoubleClick={() => setSelectedTask(task)}
                            />
                          )}
                        </Draggable>
                      ))}
                      {provided.placeholder}
                    </div>
                  )}
                </Droppable>
              )}
  
              <button 
                onClick={() => setActiveColumnForNewTask(column.id)} 
                style={{ width: '100%', padding: '8px', background: 'transparent', border: '1px dashed #555', color: '#aaa', borderRadius: '4px', cursor: 'pointer', marginTop: '10px' }}
              >
                + Add Task
              </button>

            </div>
          ))}
          <div className={styles.column} style={{ minWidth: '280px', backgroundColor: 'rgba(255,255,255,0.03)', border: '1px dashed #555', display: 'flex', flexDirection: 'column' }}>
            {isCreatingCol ? (
               <form onSubmit={handleCreateColumn} style={{ padding: '10px' }}>
                  <input 
                    autoFocus 
                    placeholder="Enter column name..." 
                    value={newColTitle} 
                    onChange={e => setNewColTitle(e.target.value)} 
                    style={{ width: '100%', padding: '8px', marginBottom: '10px', borderRadius: '4px', border: 'none' }} 
                  />
                  <div style={{ display: 'flex', gap: '5px' }}>
                    <button type="submit" disabled={!newColTitle} style={{ flex: 1, padding: '6px', backgroundColor: '#734FCF', color: 'white', border: 'none', borderRadius: '4px', cursor: 'pointer' }}>
                      Add
                    </button>
                    <button type="button" onClick={() => setIsCreatingCol(false)} style={{ flex: 1, padding: '6px', backgroundColor: '#444', color: 'white', border: 'none', borderRadius: '4px', cursor: 'pointer' }}>
                      Cancel
                    </button>
                  </div>
               </form>
            ) : (
               <button 
                 onClick={() => setIsCreatingCol(true)} 
                 style={{ width: '100%', height: '100%', minHeight: '100px', background: 'transparent', border: 'none', color: '#aaa', cursor: 'pointer', fontSize: '1.1rem', fontWeight: 'bold' }}
               >
                  + Add New Column
               </button>
            )}
          </div>

        </div>
      </DragDropContext>
        </div>
    </div>

  {activeColumnForNewTask && (
      <CreateTaskBlock 
        columnId={activeColumnForNewTask} 
        
        columnTitle={columns.find(c => c.id === activeColumnForNewTask)?.title || "To Do"} 
        
        onClose={() => setActiveColumnForNewTask(null)}
        onTaskCreated={() => {
           fetchBoard();
           setActiveColumnForNewTask(null);
        }} 
        members={project.members || []} 
        availableTags={boardTags} 
      />
    )}

    {selectedTask && (
      <TaskModal 
        key={selectedTask.id}
        task={selectedTask} 
        onClose={() => setSelectedTask(null)} 
        onSave={handleSaveTaskDetails}
      />
    )}

    </>
  );
};