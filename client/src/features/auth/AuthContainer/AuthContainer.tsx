import React, { useState, useEffect } from 'react';
import { motion, AnimatePresence } from 'framer-motion';
import { useNavigate } from 'react-router-dom';
import style from "./AuthBoard.module.scss";
import { KeterProfile } from '../../../components/Icons/Icons';
import { AuthTask } from '../AuthTask/AuthTask';

interface Task {
  id: string;
  title: string;
  content: React.ReactNode;
  status: string;
  type: string;
}

interface Column {
  id: string;
  title: string;
  tasks: Task[];
}
interface AuthBoardProps {
  columns: Column[]; 
}

export const AuthBoard: React.FC<AuthBoardProps> = ({ columns: initialColumns }) => {
  const navigate = useNavigate();
  const [columns, setColumns] = useState<Column[]>(initialColumns);

  useEffect(() => {
    if (columns[1].tasks.length === 0 && columns[0].tasks.length > 0) {
      const timer = setTimeout(() => {
        setColumns((prev) => {
          const newColumns = prev.map(col => ({ ...col, tasks: [...col.tasks] }));
          if (newColumns[0].tasks.length > 0 && newColumns[1].tasks.length === 0) {
            const [nextTask] = newColumns[0].tasks.splice(0, 1);
            nextTask.status = 'col-2'; 
            newColumns[1].tasks.push(nextTask);
          }
          return newColumns;
        });
      }, 1500);
      return () => clearTimeout(timer);
    }
  }, [columns]);

useEffect(() => {
  const totalTasks = columns[0].tasks.length + columns[1].tasks.length + columns[2].tasks.length;
  const allTasksInDone = columns[2].tasks.length === totalTasks && totalTasks > 0;
  if (allTasksInDone) {
    const timer = setTimeout(() => {
      navigate('/wellcome');
    }, 1500); 
    return () => clearTimeout(timer);
  }
}, [columns, navigate]);
  const finishTask = (taskId: string) => {
    setColumns((prev) => {
      const newColumns = prev.map(col => ({ ...col, tasks: [...col.tasks] }));

      const taskIndex = newColumns[1].tasks.findIndex(t => t.id === taskId);
      
      if (taskIndex !== -1) {
        const [completedTask] = newColumns[1].tasks.splice(taskIndex, 1);
        const updatedTask = { ...completedTask, status: 'col-3' };
        newColumns[2].tasks.push(updatedTask);
      }
      return newColumns;
    });
  };

  return (
    <div className={style.pageWrapper}>
      <div className={style.authContainer}>
        {columns.map((column) => (
          <div className={style.column} key={column.id}>
            <h3>{column.title}</h3>
            <div className={style.taskList}>
              <AnimatePresence mode="popLayout">
                {column.tasks.map((task) => (
                  <motion.div
                    className={style.task}
                    key={task.id}
                    layoutId={task.id} // Esențial pentru animația între coloane
                    layout="position"
                    transition={{ type: "spring", stiffness: 300, damping: 30 }}
                  >
                    <header>
                      <div className={style.titleTags}>
                          <div className={style.tags}>
                              <span className={style.tag}></span>
                              <span className={style.tag}></span>
                          </div>
                          <h1>{task.title}</h1>
                      </div>
                      <div className={style.iconStatus}>
                        <KeterProfile/>
                        <span>
                          {columns.find(col => col.id === task.status)?.title || "To Do"}
                        </span>
                      </div>
                    </header>              

                    <AnimatePresence>
                      {/* Conținutul se expandează DOAR când task-ul este In Progress */}
                      {task.status === 'col-2' && (
                        <motion.div
                          initial={{ height: 0, opacity: 0 }}
                          animate={{ height: 'auto', opacity: 1 }}
                          exit={{ height: 0, opacity: 0 }}
                          transition={{ 
                            duration: 0.4, 
                            delay: 0.6 
                          }}
                          style={{ overflow: 'hidden' }}
                        >
                          <div className={style.contentWrapper}>
                            {task.content}
                            <AuthTask
                              key={task.id} 
                              task={task} 
                              onFinish={finishTask}
                              type={task.type}
                            />
                          </div>
                        </motion.div>
                      )}
                    </AnimatePresence>
                  </motion.div>
                ))}
              </AnimatePresence>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};