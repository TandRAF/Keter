import React, { useState, useEffect, useRef } from 'react';
import { type TagT } from '../../services/taskService';
import { taskService } from '../../services/taskService';
import { tagService } from '../../services/tagService'; 

interface TagSelectorProps {
    taskId: string;
    currentTags: TagT[];
    projectId: string;
    onTagsUpdated: () => void;
}

export const TagSelector: React.FC<TagSelectorProps> = ({ taskId, currentTags = [], projectId, onTagsUpdated }) => {
    const [projectTags, setProjectTags] = useState<TagT[]>([]);
    const [isOpen, setIsOpen] = useState(false);
    const [isCreating, setIsCreating] = useState(false);

    const [newTagName, setNewTagName] = useState('');
    const [newTagColor, setNewTagColor] = useState('#734FCF'); 
    
    const dropdownRef = useRef<HTMLDivElement>(null);

    // 1. Încarcă tag-urile proiectului când deschizi meniul
    useEffect(() => {
        if (isOpen) {
            // Presupunând că tagService e implementat să returneze o listă
            tagService.getProjectTags(projectId)
                .then(setProjectTags)
                .catch(err => console.error("Eroare la încărcarea tag-urilor:", err));
        }
    }, [isOpen, projectId]);

    // Închide dropdown-ul dacă dai click în afara lui
    useEffect(() => {
        const handleClickOutside = (event: MouseEvent) => {
            if (dropdownRef.current && !dropdownRef.current.contains(event.target as Node)) {
                setIsOpen(false);
                setIsCreating(false);
            }
        };
        document.addEventListener('mousedown', handleClickOutside);
        return () => document.removeEventListener('mousedown', handleClickOutside);
    }, []);

    // 2. Funcție pentru bifat/debifat un tag pe task
    const toggleTag = async (tagId: string) => {
        const isAssigned = currentTags.some(t => t.id === tagId);
        const newTagIds = isAssigned 
            ? currentTags.filter(t => t.id !== tagId).map(t => t.id) 
            : [...currentTags.map(t => t.id), tagId];

        try {
            // Trimitem lista nouă de tag-uri către backend. 
            // NOTĂ: Am folosit "as any" deoarece TaskUpdatePayload cere și title/status etc.
            // Dacă backend-ul tău dă eroare de validare, va trebui să trimiți și titlul/statusul vechi aici.
            await taskService.updateTask(taskId, { tagIds: newTagIds } as any);
            onTagsUpdated(); // Face refresh la UI
        } catch (error) {
            console.error("Failed to update tags on task", error);
        }
    };

    // 3. Funcție pentru crearea unui tag nou în proiect
    const handleCreateNewTag = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!newTagName.trim()) return;

        try {
            const newTag = await tagService.createTag(projectId, newTagName.trim(), newTagColor);
            setProjectTags([...projectTags, newTag]);
            setNewTagName('');
            setIsCreating(false);
            
            // Asignează-l automat la task-ul curent
            const newTagIds = [...currentTags.map(t => t.id), newTag.id];
            await taskService.updateTask(taskId, { tagIds: newTagIds } as any);
            onTagsUpdated();
        } catch (error) {
            console.error("Failed to create new tag", error);
        }
    };

    return (
        <div style={{ position: 'relative', display: 'flex', alignItems: 'center', gap: '8px', flexWrap: 'wrap' }} ref={dropdownRef}>
            
            {/* Afișează tag-urile curente ale task-ului */}
            {currentTags.map(tag => (
                <span 
                    key={tag.id}
                    style={{
                        padding: '4px 10px', 
                        fontSize: '0.8rem', 
                        borderRadius: '12px', 
                        backgroundColor: tag.colorHex, 
                        color: 'white',
                        fontWeight: 'bold'
                    }}
                >
                    {tag.name}
                </span>
            ))}

            {/* Butonul de adăugare tag */}
            <button 
                type="button"
                onClick={() => setIsOpen(!isOpen)}
                style={{
                    padding: '4px 10px',
                    fontSize: '0.8rem',
                    borderRadius: '12px',
                    backgroundColor: 'transparent',
                    color: '#aaa',
                    border: '1px dashed #555',
                    cursor: 'pointer'
                }}
            >
                + Add Tag
            </button>

            {/* Meniul Dropdown */}
            {isOpen && (
                <div style={{
                    position: 'absolute',
                    top: '100%',
                    left: 0,
                    marginTop: '8px',
                    backgroundColor: '#222',
                    border: '1px solid #444',
                    borderRadius: '6px',
                    padding: '12px',
                    zIndex: 100,
                    width: '240px',
                    boxShadow: '0 4px 12px rgba(0,0,0,0.5)'
                }}>
                    <h6 style={{ margin: '0 0 10px 0', color: '#fff', fontSize: '0.9rem' }}>Project Tags</h6>
                    
                    {/* Lista cu tag-urile existente în proiect */}
                    <div style={{ display: 'flex', flexDirection: 'column', gap: '6px', maxHeight: '150px', overflowY: 'auto', marginBottom: '12px' }}>
                        {projectTags.length === 0 && !isCreating && (
                            <span style={{ fontSize: '0.8rem', color: '#888' }}>No tags in this project yet.</span>
                        )}
                        {projectTags.map(tag => {
                            const isAssigned = currentTags.some(t => t.id === tag.id);
                            return (
                                <div 
                                    key={tag.id} 
                                    onClick={() => toggleTag(tag.id)}
                                    style={{
                                        display: 'flex',
                                        alignItems: 'center',
                                        justifyContent: 'space-between',
                                        padding: '6px 8px',
                                        borderRadius: '4px',
                                        cursor: 'pointer',
                                        backgroundColor: isAssigned ? 'rgba(255,255,255,0.1)' : 'transparent'
                                    }}
                                >
                                    <div style={{ display: 'flex', alignItems: 'center', gap: '8px' }}>
                                        <div style={{ width: '12px', height: '12px', borderRadius: '50%', backgroundColor: tag.colorHex }}></div>
                                        <span style={{ color: '#ddd', fontSize: '0.85rem' }}>{tag.name}</span>
                                    </div>
                                    {isAssigned && <span style={{ color: '#734FCF', fontSize: '0.8rem' }}>✓</span>}
                                </div>
                            );
                        })}
                    </div>

                    {/* Secțiunea de Creare Tag Nou */}
                    {isCreating ? (
                        <form onSubmit={handleCreateNewTag} style={{ display: 'flex', flexDirection: 'column', gap: '8px', borderTop: '1px solid #444', paddingTop: '10px' }}>
                            <input 
                                autoFocus
                                placeholder="Tag name..." 
                                value={newTagName}
                                onChange={(e) => setNewTagName(e.target.value)}
                                style={{ padding: '6px', borderRadius: '4px', border: '1px solid #555', backgroundColor: '#111', color: '#fff', fontSize: '0.85rem' }}
                                maxLength={30}
                            />
                            <div style={{ display: 'flex', alignItems: 'center', gap: '8px' }}>
                                <input 
                                    type="color" 
                                    value={newTagColor}
                                    onChange={(e) => setNewTagColor(e.target.value)}
                                    style={{ width: '30px', height: '30px', padding: '0', border: 'none', cursor: 'pointer', backgroundColor: 'transparent' }}
                                />
                                <span style={{ color: '#888', fontSize: '0.8rem' }}>Pick a color</span>
                            </div>
                            <div style={{ display: 'flex', gap: '6px', marginTop: '4px' }}>
                                <button type="submit" disabled={!newTagName.trim()} style={{ flex: 1, padding: '6px', backgroundColor: '#734FCF', color: '#fff', border: 'none', borderRadius: '4px', cursor: 'pointer', fontSize: '0.8rem' }}>Save</button>
                                <button type="button" onClick={() => setIsCreating(false)} style={{ flex: 1, padding: '6px', backgroundColor: '#444', color: '#fff', border: 'none', borderRadius: '4px', cursor: 'pointer', fontSize: '0.8rem' }}>Cancel</button>
                            </div>
                        </form>
                    ) : (
                        <button 
                            type="button" 
                            onClick={() => setIsCreating(true)}
                            style={{ width: '100%', padding: '6px', backgroundColor: '#333', color: '#ddd', border: 'none', borderRadius: '4px', cursor: 'pointer', fontSize: '0.85rem', borderTop: '1px solid #444' }}
                        >
                            + Create New Tag
                        </button>
                    )}
                </div>
            )}
        </div>
    );
};