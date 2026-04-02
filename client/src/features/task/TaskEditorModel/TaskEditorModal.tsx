import React, { useState, useEffect } from 'react';
import { type TaskT, type TagT, taskService } from '../../../services/taskService';
import InputData from '../../../components/InputData/InputData';
import styles from './TaskEditorModal.module.scss'; 
import { tagService } from '../../../services/tagService';
import { TagSelect } from '../../tag/TagSelect/TagSelect';

interface TaskEditorModalProps {
    projectId: string; 
    columnId: string;
    columnTitle: string;
    task?: TaskT | null;
    onClose: () => void;
    onSaveSuccess: () => void; 
    members: any[];
    availableTags?: TagT[];
}

export const TaskEditorModal: React.FC<TaskEditorModalProps> = ({
    projectId, 
    columnId,
    columnTitle,
    task,
    onClose,
    onSaveSuccess,
    members,
    availableTags = []
}) => {
    const isEditing = !!task;

    const [title, setTitle] = useState(task?.title || "");
    const [assignedUserId, setAssignedUserId] = useState<string>(task?.assignedUser?.id || "");
    const [deadline, setDeadline] = useState<string>(task?.deadline ? task.deadline.split('T')[0] : "");
    const [selectedTagIds, setSelectedTagIds] = useState<string[]>(task?.tags?.map(t => t.id) || []);
    const [isLoading, setIsLoading] = useState(false);

    const [localTags, setLocalTags] = useState<TagT[]>(availableTags);

    useEffect(() => {
        setLocalTags(availableTags);
    }, [availableTags]);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        const cleanTitle = title.trim();
        if (!cleanTitle) return;

        setIsLoading(true);

        try {
            if (isEditing && task) {
                await taskService.updateTask(task.id, {
                    title: cleanTitle,
                    status: task.status, 
                    assignedUserId: assignedUserId || null,
                    deadline: deadline || undefined,
                    tagIds: selectedTagIds
                });
            } else {
                await taskService.createTask({
                    title: cleanTitle,
                    columnId: columnId,
                    status: columnTitle,
                    assignedUserId: assignedUserId || null,
                    deadline: deadline || undefined,
                    tagIds: selectedTagIds.length > 0 ? selectedTagIds : undefined
                });
            }
            
            onSaveSuccess();
            onClose();
        } catch (error) {
            console.error(`Failed to ${isEditing ? 'update' : 'create'} task:`, error);
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
        <div className={styles.EditTaskContainer}>
            <div className={styles.modalContent}>
                <h2>{isEditing ? "Edit Task" : "Create New Task"}</h2>
                
                <form onSubmit={handleSubmit} className={styles.formContainer}>
                    <div className={styles.inputGroup}>
                        <input 
                            autoFocus 
                            placeholder="Enter task title..." 
                            value={title} 
                            onChange={(e) => setTitle(e.target.value)}
                            maxLength={100}
                            className={styles.titleInput}
                        />
                    </div>
                    
                    <InputData
                        type="user-select"
                        id={`editor-assignee`} 
                        label="Assign To:"
                        placeholder="-- Unassigned --"
                        userOptions={members}
                        value={assignedUserId}
                        onChange={(e) => setAssignedUserId(e.target.value)}
                    />

                    <InputData
                        type="calendar"
                        id={`editor-deadline`}
                        label="Deadline:"
                        value={deadline} 
                        onChange={(e) => setDeadline(e.target.value)}
                    />

                    <TagSelect
                        label="Tags"
                        placeholder="Select tags..."
                        tagOptions={localTags}
                        selectedTags={selectedTagIds}
                        onTagToggle={toggleTag}
                        onCreateTag={async (name, color) => {
                            const newTag = await tagService.createTag(projectId, name, color);
                            setLocalTags(prev => [...prev, newTag]); 
                            setSelectedTagIds(prev => [...prev, newTag.id]); 
                        }}
/>

                    <div className={styles.buttonGroup}>
                        <button type="submit" disabled={isLoading || !title.trim()} className={styles.saveButton}>
                            {isLoading ? "Saving..." : (isEditing ? "Save Changes" : "Create Task")}
                        </button>
                        <button type="button" onClick={onClose} className={styles.cancelButton}>
                            Cancel
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
};