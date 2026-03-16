import React, { useState } from 'react';
import { DragDropContext, Droppable, Draggable, type DropResult } from '@hello-pangea/dnd';
import styles from "./Boars.module.scss"
import { Task } from '../../task/Task/Task';
import { type TaskT } from '../../../services/taskService';

interface Column {
  id: string;
  title: string;
  status: "TODO" | "IN_PROGRESS" | "DONE";
  tasks: TaskT[];
}

export const Board: React.FC = () => {
  const initialColumns: Column[] = [
    {
      id: 'column-1',
      title: 'To Do',
      status:"TODO",
      tasks: [
        { id: 't1', boardId: '1', title: 'Implementare JWT', order: 0,status:'TODO' },
        { id: 't2', boardId: '1', title: 'Configurare Docker', order: 1,status:'TODO' },
      ],
    },
    {
      id: 'column-2',
      title: 'In Progress',
      status:"IN_PROGRESS",
      tasks: [{ id: 't3',boardId: '1', title: 'Lucru la Keter UI', order: 0,status:"IN_PROGRESS" }],
    },
    {
      id: 'column-3',
      title: 'Done',
      status: 'DONE',
      tasks: [],
    },
  ];

  const [columns, setColumns] = useState<Column[]>(initialColumns);

  const onDragEnd = (result: DropResult): void => {
    const { destination, source, draggableId } = result;

    if (!destination) return;
    if (destination.droppableId === source.droppableId && destination.index === source.index) return;

    const newColumns = [...columns];
    const sourceCol = newColumns.find(c => c.id === source.droppableId);
    const destCol = newColumns.find(c => c.id === destination.droppableId);
    const status = destCol?.status;

    if (!sourceCol || !destCol) return;

    const [movedTask] = sourceCol.tasks.splice(source.index, 1);
    movedTask.status = destCol.status;
    destCol.tasks.splice(destination.index, 0, movedTask);


    setColumns(newColumns);

    const moveTaskData = {
      taskId: draggableId,
      newBoardId: destination.droppableId,
      newOrder: destination.index,
      newStatus: status
    };

    console.log("API Move Data:", moveTaskData);
  };

  return (
    <DragDropContext 
    onDragEnd={onDragEnd}>
      <div
      className={styles.board}
    >
        {columns.map((column) => (
          <div
            className={styles.column}
          >
            <h4>{column.title}</h4>
            <Droppable droppableId={column.id}>
              {(provided, snapshot) => (
                <div 
                  {...provided.droppableProps} 
                  ref={provided.innerRef}
                 style={{
                    border: snapshot.isDraggingOver ? "dashed solid 0.5px white" : "none",
                   }}
                >
                  {column.tasks.map((task, index) => (
                    <Draggable key={task.id} draggableId={task.id} index={index}>
                      {(provided,snapshot) => (
                        <Task 
                        provided = {provided}
                        snapshot = {snapshot}
                        taskData = {task}/>
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