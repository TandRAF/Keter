import React, { useState, useEffect, useCallback } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { DragDropContext, Droppable, Draggable, type DropResult } from '@hello-pangea/dnd';
import styles from "./Boars.module.scss"; 
import { Task } from '../../task/Task/Task';
import { type ColumnT, type TaskT, taskService, type TagT } from '../../../services/taskService';
import { boardService } from '../../../services/boardService';
import { columnService } from '../../../services/columnService'; 
import { useProjectContext } from '../../../pages/ProjectsDetailsPage/ProjectDetailsPage'; 
import { tagService } from '../../../services/tagService';

import { TaskEditorModal } from '../../task/TaskEditorModel/TaskEditorModal'; 

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
      setIsLoading(true);

      const boardData = await boardService.getBoardData(boardId);
      setColumns(boardData.columns || []); 
      setBoardName(boardData.name);
      
      if (project?.id) {

          try {
             const fetchedTags = await tagService.getProjectTags(project.id);
             setBoardTags(fetchedTags); 
          } catch (tagError) {
             console.warn("Nu s-au putut aduce tag-urile (endpoint lipsă):", tagError);
             setBoardTags([]); 
          }
      }

    } catch (error) {
      console.error("Eroare la încărcarea board-ului:", error);
      navigate(`/projects/${project?.id}`); 
    } finally {
      setIsLoading(false);
    }
  }, [boardId, project?.id, navigate]);

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
      await taskService.moveTask(draggableId, destCol.id, destination.index, destCol.title);
    } catch (error) {
      console.error("Eroare la salvarea pe server. Reverting UI...", error);
      fetchBoard();
    }
  };

  const isModalOpen = Boolean(activeColumnForNewTask || selectedTask);

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
                                  onDoubleClick={() => setSelectedTask(task)} // Triggers EDIT mode
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
                    onClick={() => setActiveColumnForNewTask(column.id)} // Triggers CREATE mode
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
      {isModalOpen && project && (
        <TaskEditorModal 
          projectId={project.id}
          columnId={activeColumnForNewTask || selectedTask?.columnId || ""} 
          columnTitle={columns.find(c => c.id === (activeColumnForNewTask || selectedTask?.columnId))?.title || ""} 
          task={selectedTask} 
          onClose={() => {
             setActiveColumnForNewTask(null);
             setSelectedTask(null);
          }}
          onSaveSuccess={() => {
             fetchBoard(); 
             setActiveColumnForNewTask(null);
             setSelectedTask(null);
          }} 
          members={project.members || []} 
          availableTags={boardTags} 
        />
      )}

    </>
  );
};