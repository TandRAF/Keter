import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { DragDropContext, Droppable, Draggable, type DropResult } from '@hello-pangea/dnd';
import styles from "./Boars.module.scss";
import { Task } from '../../task/Task/Task';
import { type ColumnT, type TaskT, taskService } from '../../../services/taskService';
import { boardService } from '../../../services/boardService';
import { TaskModal } from '../../task/TaskModal/TaskModal'; 
import { useProjectContext } from '../../../pages/ProjectsDetailsPage/ProjectDetailsPage'; 

export const Board: React.FC = () => {
  const { boardId } = useParams<{ boardId: string }>();
  const navigate = useNavigate();
  const { project } = useProjectContext(); // Removed isOwner here temporarily for testing

  const [columns, setColumns] = useState<ColumnT[]>([]);
  const [boardName, setBoardName] = useState(""); 
  const [isLoading, setIsLoading] = useState(true);
  const [selectedTask, setSelectedTask] = useState<TaskT | null>(null);

  // --- STRICT MODE FIX: Delay rendering of Droppables ---
  const [isBrowserReady, setIsBrowserReady] = useState(false);

  useEffect(() => {
    const animation = requestAnimationFrame(() => setIsBrowserReady(true));
    return () => cancelAnimationFrame(animation);
  }, []);
  // ------------------------------------------------------

  useEffect(() => {
    const fetchBoard = async () => {
      if (!boardId) return;
      try {
        const boardData = await boardService.getBoardData(boardId);
        setColumns(boardData.columns || []); 
        setBoardName(boardData.name);
      } catch (error) {
        console.error("Eroare la încărcarea board-ului Keter:", error);
        navigate(`/projects/${project.id}`); 
      } finally {
        setIsLoading(false);
      }
    };

    fetchBoard();
  }, [boardId, project.id, navigate]);

  const onDragEnd = async (result: DropResult): Promise<void> => {
    const { destination, source, draggableId } = result;
    if (!destination) return;
    if (destination.droppableId === source.droppableId && destination.index === source.index) return;
    
    const newColumns = JSON.parse(JSON.stringify(columns)) as ColumnT[];
    
    // Convert IDs back to strings for comparison just in case
    const sourceCol = newColumns.find(c => String(c.id) === source.droppableId);
    const destCol = newColumns.find(c => String(c.id) === destination.droppableId);

    if (!sourceCol || !destCol) return;
    const [movedTask] = sourceCol.tasks.splice(source.index, 1);
    
    // Update the task's new column and status
    movedTask.columnId = destCol.id;
    movedTask.status = destCol.title; 
    
    // Insert into new position
    destCol.tasks.splice(destination.index, 0, movedTask);

    // Optimistically update the UI
    setColumns(newColumns);

    try {
      await taskService.moveTask(draggableId, destCol.id, destination.index);
    } catch (error) {
      console.error("Eroare la salvarea pe server. Reverting UI...", error);
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
      <div style={{ marginBottom: '20px', display: 'flex', alignItems: 'center', gap: '15px' }}>
         <button 
           onClick={() => navigate(`/projects/${project.id}`)}
           style={{ background: 'transparent', color: 'white', border: '1px solid #555', padding: '5px 10px', borderRadius: '4px', cursor: 'pointer' }}
         >
           ← Back to Boards
         </button>
         <h2 style={{ margin: 0 }}>{boardName}</h2>
      </div>

      <DragDropContext onDragEnd={onDragEnd}>
        <div className={styles.board}>
          {columns.map((column) => (
            <div className={styles.column} key={column.id}>
              <h4>{column.title}</h4>
            
              {isBrowserReady && (
                <Droppable droppableId={String(column.id)}>
                  {(provided, snapshot) => (
                    <div 
                      {...provided.droppableProps} 
                      ref={provided.innerRef}
                      style={{
                        border: snapshot.isDraggingOver ? "dashed solid 0.5px white" : "none",
                        minHeight: "150px", 
                        padding: "10px",    
                        transition: "border 0.2s ease"
                      }}
                    >
                      {column.tasks && column.tasks.map((task, index) => (
                        <Draggable 
                          key={String(task.id)} 
                          draggableId={String(task.id)} 
                          index={index}
                        >
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
            </div>
          ))}

        {selectedTask && (
          <TaskModal 
            key={selectedTask.id}
            task={selectedTask} 
            onClose={() => setSelectedTask(null)} 
            onSave={handleSaveTaskDetails}
          />
        )}
        </div>
      </DragDropContext>
    </>
  );
};