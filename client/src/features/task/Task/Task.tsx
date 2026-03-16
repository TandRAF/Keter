import { type DraggableProvided,type DraggableStateSnapshot } from "@hello-pangea/dnd"
import { type TaskT } from '../../../services/taskService';
import style from "./Task.module.scss"
import { KeterProfile } from "../../../components/Icons/Icons";
 
interface props {
    provided:DraggableProvided
    snapshot:DraggableStateSnapshot
    taskData:TaskT
}

export const Task = ({provided,snapshot,taskData}:props) => {
  return (
    <div
        ref={provided.innerRef}
        {...provided.draggableProps}
        {...provided.dragHandleProps}
        className={style.task}
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
                  <span className={style.tag}></span>
                  <span className={style.tag}></span>
              </div>
              <h2>{taskData.title}</h2>
          </div>
          <div className={style.icon}>
              <KeterProfile/>
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
