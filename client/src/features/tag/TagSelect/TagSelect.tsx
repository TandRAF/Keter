import React, { useState, useRef, useEffect } from "react";
import styles from "./TagSelect.module.scss";

export type TagOption = {
  id: string;
  name: string;
  colorHex: string;
};

interface TagSelectProps {
  label: string;
  placeholder?: string;
  tagOptions: TagOption[];
  selectedTags: string[];
  onTagToggle: (tagId: string) => void;
  onCreateTag: (name: string, colorHex: string) => Promise<void>;
}

export const TagSelect: React.FC<TagSelectProps> = ({
  label,
  placeholder = "Select tags...",
  tagOptions,
  selectedTags,
  onTagToggle,
  onCreateTag,
}) => {
  const [open, setOpen] = useState(false);
  const [isCreating, setIsCreating] = useState(false);
  const [newTagName, setNewTagName] = useState("");
  const [newTagColor, setNewTagColor] = useState("#734FCF");
  const dropdownRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (dropdownRef.current && !dropdownRef.current.contains(event.target as Node)) {
        setOpen(false);
        setIsCreating(false);
      }
    };
    document.addEventListener("mousedown", handleClickOutside);
    return () => document.removeEventListener("mousedown", handleClickOutside);
  }, []);

  const handleCreate = async () => {
    if (!newTagName.trim()) return;
    await onCreateTag(newTagName.trim(), newTagColor);
    setNewTagName("");
    setIsCreating(false);
  };

  return (
    <div className={styles.selectTagContainer} style={{ position: "relative" }} ref={dropdownRef}>
      <label>{label}</label>
      <div
      className={styles.select}
      >
        {selectedTags.map((tagId) => {
          const tag = tagOptions.find((t) => t.id === tagId);
          if (!tag) return null;
          return (
            <span
              key={tagId}
            >
              {tag.name}
              <span onClick={() => onTagToggle(tagId)}>
                ×
              </span>
            </span>
          );
        })}

        <button
          type="button"
          onClick={() => setOpen(!open)}
        >
          {selectedTags.length === 0 ? placeholder : "+ Add"}
        </button>
      </div>
      {open && (
        <div
          style={{
            position: "absolute", top: "100%", left: 0, right: 0, marginTop: "5px",
            background: "#2a2a2a", border: "1px solid #444", borderRadius: "4px",
            zIndex: 10, padding: "10px", boxShadow: "0 4px 10px rgba(0,0,0,0.5)",
          }}
        >
          <div style={{ display: "flex", flexDirection: "column", gap: "5px", maxHeight: "150px", overflowY: "auto", marginBottom: "10px" }}>
            {tagOptions.filter((t) => !selectedTags.includes(t.id)).map((tag) => (
              <div
                key={tag.id}
                onClick={() => onTagToggle(tag.id)}
                style={{ padding: "6px 10px", display: "flex", alignItems: "center", gap: "8px", cursor: "pointer", borderRadius: "4px" }}
                onMouseEnter={(e) => (e.currentTarget.style.background = "#3a3a3a")}
                onMouseLeave={(e) => (e.currentTarget.style.background = "transparent")}
              >
                <div style={{ width: "12px", height: "12px", borderRadius: "50%", backgroundColor: tag.colorHex }}></div>
                <span style={{ color: "white", fontSize: "0.9rem" }}>{tag.name}</span>
              </div>
            ))}
            {tagOptions.filter((t) => !selectedTags.includes(t.id)).length === 0 && !isCreating && (
              <span style={{ color: "#888", fontSize: "0.8rem", padding: "5px" }}>No more tags available.</span>
            )}
          </div>

          {/* Formularul de Creare Tag Nou */}
          {isCreating ? (
            <div style={{ display: "flex", flexDirection: "column", gap: "8px", borderTop: "1px solid #444", paddingTop: "10px" }}>
              <input
                autoFocus
                placeholder="New tag name..."
                value={newTagName}
                onChange={(e) => setNewTagName(e.target.value)}
                style={{ padding: "6px", borderRadius: "4px", border: "1px solid #555", backgroundColor: "#111", color: "#fff", fontSize: "0.85rem" }}
                maxLength={30}
              />
              <div style={{ display: "flex", alignItems: "center", gap: "8px" }}>
                <input
                  type="color"
                  value={newTagColor}
                  onChange={(e) => setNewTagColor(e.target.value)}
                  style={{ width: "30px", height: "30px", padding: "0", border: "none", cursor: "pointer", backgroundColor: "transparent" }}
                />
                <span style={{ color: "#888", fontSize: "0.8rem" }}>Pick a color</span>
              </div>
              <div style={{ display: "flex", gap: "6px" }}>
                <button type="button" onClick={handleCreate} disabled={!newTagName.trim()} style={{ flex: 1, padding: "6px", backgroundColor: "#734FCF", color: "#fff", border: "none", borderRadius: "4px", cursor: "pointer", fontSize: "0.8rem" }}>
                  Save
                </button>
                <button type="button" onClick={() => setIsCreating(false)} style={{ flex: 1, padding: "6px", backgroundColor: "#444", color: "#fff", border: "none", borderRadius: "4px", cursor: "pointer", fontSize: "0.8rem" }}>
                  Cancel
                </button>
              </div>
            </div>
          ) : (
            <button
              type="button"
              onClick={() => setIsCreating(true)}
              style={{ width: "100%", padding: "8px", backgroundColor: "#333", color: "#ddd", border: "none", borderRadius: "4px", cursor: "pointer", fontSize: "0.85rem", borderTop: "1px solid #444" }}
            >
              + Create New Tag
            </button>
          )}
        </div>
      )}
    </div>
  );
};