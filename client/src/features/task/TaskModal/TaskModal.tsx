import React, { useState, useRef, useEffect } from 'react';
import { type TaskT } from '../../../services/taskService';
import styles from './TaskModal.module.scss';
import { KeterProfile } from '../../../components/Icons/Icons'; 

interface TaskModalProps {
  task: TaskT;
  onClose: () => void;
  onSave: (updatedTask: TaskT) => void;
}

export const TaskModal: React.FC<TaskModalProps> = ({ task, onClose, onSave }) => {
  const [editedTask, setEditedTask] = useState<TaskT>(task);
  const [editingField, setEditingField] = useState<'title' | 'description' | null>(null);

  const handleOverlayClick = (e: React.MouseEvent<HTMLDivElement>) => {
    if (e.target === e.currentTarget) {
      onClose();
    }
  };

  const handleBlur = () => {
    setEditingField(null);
    if (editedTask.title !== task.title || editedTask.description !== task.description) {
      onSave(editedTask);
    }
  };

  const handleKeyDown = (e: React.KeyboardEvent) => {
    if (e.key === 'Enter' && editingField === 'title') {
      handleBlur();
    }
  };

  return (
    <div className={styles.overlay} onClick={handleOverlayClick}>
      <div className={styles.modalContent}>
        
        <button className={styles.closeBtn} onClick={onClose}>&times;</button>
        
        <header className={styles.header}>
          <div className={styles.icon}>
            <KeterProfile />
          </div>
          
          <div className={styles.titleSection}>
            <p className={styles.statusLabel}>in column: <span>{task.status}</span></p>
  
            {editingField === 'title' ? (
              <input 
                type="text"
                autoFocus
                className={styles.editInput}
                value={editedTask.title}
                onChange={(e) => setEditedTask({ ...editedTask, title: e.target.value })}
                onBlur={handleBlur}
                onKeyDown={handleKeyDown}
              />
            ) : (
              <h2 onDoubleClick={() => setEditingField('title')}>
                {editedTask.title}
              </h2>
            )}
          </div>
        </header>

        <div className={styles.body}>
          <h3>Description</h3>
          
          {editingField === 'description' ? (
            <textarea
              autoFocus
              className={styles.editTextarea}
              value={editedTask.description || ''}
              onChange={(e) => setEditedTask({ ...editedTask, description: e.target.value })}
              onBlur={handleBlur}
              placeholder="Add a more detailed description..."
            />
          ) : (
            <div 
              className={styles.descriptionText} 
              onDoubleClick={() => setEditingField('description')}
            >
              {editedTask.description ? (
                <p>{editedTask.description}</p>
              ) : (
                <p className={styles.placeholder}>Double click to add a description...</p>
              )}
            </div>
          )}
        </div>

      </div>
    </div>
  );
};