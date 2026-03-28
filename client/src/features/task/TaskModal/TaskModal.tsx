import React, { useState } from 'react';
import { type TaskT } from '../../../services/taskService';
import styles from './TaskModal.module.scss';

interface TaskModalProps {
  task: TaskT;
  onClose: () => void;
  onSave: (updatedTask: TaskT) => void;
}

export const TaskModal: React.FC<TaskModalProps> = ({ task, onClose, onSave }) => {
  const [editedTask, setEditedTask] = useState<TaskT>(task);
  const [editingField, setEditingField] = useState<'title' | 'description' | null>(null);

  const handleBlur = () => {
    setEditingField(null);
    onSave(editedTask);
  };

  const handleKeyDown = (e: React.KeyboardEvent) => {
    if (e.key === 'Enter' && editingField === 'title') {
      handleBlur();
    }
  };

  return (
    <div className={styles.taskDetails} onClick={onClose}>
      <div className={styles.content} onClick={e => e.stopPropagation()}>
        <button className={styles.closeBtn} onClick={onClose}>&times;</button>
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
          <p onDoubleClick={() => setEditingField('title')}>
            {editedTask.title}
          </p>
        )}

        <div className={styles.body}>
          <span>Description</span>
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