import React, { useState, useEffect } from 'react';
import { DragDropContext, Droppable, Draggable, type DropResult } from '@hello-pangea/dnd';
import styles from "./Boars.module.scss";
import { Task } from '../../task/Task/Task';
import { type ColumnT, taskService } from '../../../services/taskService';
import { boardService } from '../../../services/boardService';

export const Board: React.FC = () => {
  const [columns, setColumns] = useState<ColumnT[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const BOARD_ID = "22222222-2222-2222-2222-222222222221";
  useEffect(() => {
    const fetchBoard = async () => {
      try {
        const boardData = await boardService.getBoardData(BOARD_ID);
        setColumns(boardData.columns);
      } catch (error) {
        console.error("Eroare la încărcarea board-ului Keter:", error);
      } finally {
        setIsLoading(false);
      }
    };

    fetchBoard();
  }, []);
  const onDragEnd = async (result: DropResult): Promise<void> => {
    const { destination, source, draggableId } = result;
    if (!destination) return;
    if (destination.droppableId === source.droppableId && destination.index === source.index) return;
    const newColumns = JSON.parse(JSON.stringify(columns)) as ColumnT[];
    const sourceCol = newColumns.find(c => c.id === source.droppableId);
    const destCol = newColumns.find(c => c.id === destination.droppableId);

    if (!sourceCol || !destCol) return;
    const [movedTask] = sourceCol.tasks.splice(source.index, 1);
    movedTask.columnId = destCol.id;
    movedTask.status = destCol.title; 
    destCol.tasks.splice(destination.index, 0, movedTask);

    setColumns(newColumns);

    try {
      await taskService.moveTask(draggableId, destCol.id, destination.index);
      console.log("Poziție salvată cu succes în DB!");
    } catch (error) {
      console.error("Eroare la salvarea pe server. Poziția s-ar putea să nu se fi salvat.", error);
    }
  };

  if (isLoading) {
    return <div style={{ color: "white", padding: "2rem" }}>Loading Keter Workspace...</div>;
  }

  return (
    <DragDropContext onDragEnd={onDragEnd}>
      <div className={styles.board}>
        {columns.map((column) => (
          <div className={styles.column} key={column.id}>
            <h4>{column.title}</h4>
          
            <Droppable droppableId={column.id}>
              {(provided, snapshot) => (
                <div 
                  {...provided.droppableProps} 
                  ref={provided.innerRef}
                  style={{
                    border: snapshot.isDraggingOver ? "dashed solid 0.5px white" : "none",
                    minHeight: "150px"
                  }}
                >
                  {column.tasks.map((task, index) => (
                    <Draggable key={task.id} draggableId={task.id} index={index}>
                      {(provided, snapshot) => (
                        <Task 
                          provided={provided}
                          snapshot={snapshot}
                          taskData={task}
                        />
                      )}
                    </Draggable>
                  ))}
                  {provided.placeholder}
                </div>
              )}
            </Droppable>
          </div>
        ))}
      </div>
    </DragDropContext>
  );
};