import { type DraggableProvided, type DraggableStateSnapshot } from "@hello-pangea/dnd"
import { type TaskT } from '../../../services/taskService';
import style from "./Task.module.scss"
import { AddUser } from "../../../components/Icons/Icons";
 
interface props {
    provided: DraggableProvided;
    snapshot: DraggableStateSnapshot;
    taskData: TaskT;
    onDoubleClick: () => void;
}

export const Task = ({ provided, snapshot, taskData, onDoubleClick }: props) => {
console.log(`Task-ul "${taskData.title}" are următoarele date:`, taskData);
  console.log(`Tag-urile pentru "${taskData.title}":`, taskData.tags);
  return (
    <div
        ref={provided.innerRef}
        {...provided.draggableProps}
        {...provided.dragHandleProps}
        className={style.task}
        onDoubleClick={onDoubleClick}
        style={{
          backgroundColor: snapshot.isDragging ? 'var(--color-card)' : 'var(--color-card)',
          boxShadow: snapshot.isDragging 
            ? '0 5px 10px rgba(0,0,0,0.2)' 
            : '0 1px 2px rgba(0,0,0,0.1)',
          ...provided.draggableProps.style,
        }}
    >
       <header>
          <div className={style.titleTags}>
              <div className={style.tags}>
                  {taskData.tags && taskData.tags.map(tag => (
                      <span 
                          key={tag.id} 
                          className={style.tag}
                          title={tag.name}
                          style={{
                              backgroundColor: tag.colorHex,
                          }}
                      >
                      </span>
                  ))}
              </div>
              
              <h2>{taskData.title}</h2>
          </div>
          <div className={style.icon}>
              {taskData.assignedUser ? (
                <img 
                  src={taskData.assignedUser.profilePictureUrl} 
                  alt={taskData.assignedUser?.userName || "User Profile"} 
                  title={taskData.assignedUser?.userName}
                />
              ) : (
                <AddUser/>
              )}
          </div>
       </header>
       <div className={style.taskData}>
          <div className={style.data}></div>
          <div className={style.status}>
            <span>
              {taskData.status}
            </span>
          </div>
       </div>
    </div>
  )
}