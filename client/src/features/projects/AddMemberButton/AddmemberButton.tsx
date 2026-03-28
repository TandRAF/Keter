import React, { useEffect, useState, useRef } from 'react';
import { projectService } from '../../../services/projectService';
import { userService } from '../../../services/userService';
import style from "./AddmemberButton.module.scss";

interface AddMemberButtonProps {
  projectId: string;
  isOwner: boolean;
  onMemberAdded: () => void;
}

export const AddMemberButton = ({ projectId, isOwner, onMemberAdded }: AddMemberButtonProps) => {
  const [isAdding, setIsAdding] = useState(false);
  const [searchQuery, setSearchQuery] = useState("");
  const [searchResults, setSearchResults] = useState<{id: string, userName: string}[]>([]);

  const containerRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (containerRef.current && !containerRef.current.contains(event.target as Node)) {
        setIsAdding(false);
      }
    };

    document.addEventListener("mousedown", handleClickOutside);
    return () => {
      document.removeEventListener("mousedown", handleClickOutside);
    };
  }, []);

  useEffect(() => {
    const search = async () => {
      if (searchQuery.length < 2) {
        setSearchResults([]);
        return;
      }
      try {
        const results = await userService.searchUsers(searchQuery);
        setSearchResults(results);
      } catch (error) {
        console.error("Search failed", error);
      }
    };
    const timeoutId = setTimeout(search, 300); 
    return () => clearTimeout(timeoutId);
  }, [searchQuery]);

  const handleAddUser = async (username: string) => {
    try {
      await projectService.addMember(projectId, username);
      setIsAdding(false);
      setSearchQuery("");
      onMemberAdded();
    } catch (error: any) {
      alert(error.response?.data?.message || "Error adding member"); 
    }
  };

  if (!isOwner) return null;

  return (
    <div ref={containerRef} className={style.container} style={{ position: 'relative', display: 'inline-block' }}>
      <button 
        onClick={() => setIsAdding(!isAdding)}
      >
        <svg width="22" height="21" viewBox="0 0 22 21" fill="none" xmlns="http://www.w3.org/2000/svg">
        <path d="M11.3711 10.0735C12.9112 10.0735 14.1592 8.81043 14.1592 7.25111C14.1592 5.6918 12.9112 4.42871 11.3711 4.42871C9.83101 4.42871 8.58301 5.6918 8.58301 7.25111C8.58301 8.81043 9.83101 10.0735 11.3711 10.0735Z" fill="#734FCF"/>
        <path d="M16.3423 14.1937C16.9169 15.1949 16.0186 16.2509 14.8625 16.2509H7.87781C6.72307 16.2509 5.82341 15.1949 6.39804 14.1937C7.3937 12.4616 9.2465 11.2959 11.3695 11.2959C13.4924 11.2959 15.3466 12.4602 16.3409 14.1937H16.3423Z" fill="#734FCF"/>
        <path d="M4.4832 10.0182H2.48229V8.02004C2.48229 7.75535 2.26697 7.54004 2.00229 7.54004C1.7376 7.54004 1.52229 7.75535 1.52229 8.02004V10.0182H0.48C0.215314 10.0182 0 10.2335 0 10.4982C0 10.7629 0.215314 10.9782 0.48 10.9782H1.52092V12.9764C1.52092 13.2411 1.73623 13.4564 2.00092 13.4564C2.2656 13.4564 2.48092 13.2411 2.48092 12.9764V10.9782H4.48183C4.74652 10.9782 4.96183 10.7629 4.96183 10.4982C4.96183 10.2335 4.74652 10.0182 4.48183 10.0182H4.4832Z" fill="#734FCF"/>
        <path d="M11.3719 20.4562C7.55658 20.4562 4.08412 18.3552 2.30949 14.9746C2.18606 14.7401 2.27658 14.4494 2.51109 14.3259C2.74561 14.2025 3.03635 14.293 3.15978 14.5276C4.76709 17.5913 7.91452 19.4949 11.3719 19.4949C16.4818 19.4949 20.64 15.3367 20.64 10.2267C20.64 5.1168 16.4818 0.96 11.3719 0.96C7.72526 0.96 4.40366 3.1104 2.91155 6.43886C2.80321 6.68023 2.51932 6.78857 2.27658 6.68023C2.0352 6.57189 1.92686 6.288 2.0352 6.04526C3.68229 2.37257 7.34812 0 11.3719 0C17.0112 0 21.6 4.5888 21.6 10.2281C21.6 15.8674 17.0112 20.4562 11.3719 20.4562Z" fill="#734FCF"/>
        </svg>
        Add Member
      </button>

      {isAdding && (
        <div
        className={style.searchContainer}
         style={{ position: 'absolute', top: '100%', left: 0}}>
          <input 
            className={style.memberInput}
            type="text" 
            placeholder="Search username..." 
            value={searchQuery}
            onChange={(e) => setSearchQuery(e.target.value)}
            style={{ width: '100%', boxSizing: 'border-box' }}
            autoFocus
          />
          <ul style={{ listStyle: 'none', padding: 0, margin: '5px 0 0 0', maxHeight: '150px', overflowY: 'auto' }}>
            {searchResults.map(user => (
              <li 
                key={user.id} 
                onClick={() => handleAddUser(user.userName)} 
                style={{ cursor: 'pointer'}}
              >
                {user.userName}
              </li>
            ))}
            {searchQuery.length >= 2 && searchResults.length === 0 && (
               <li style={{ padding: '4px', color: '#888', fontSize: '0.9em' }}>No users found</li>
            )}
          </ul>
        </div>
      )}
    </div>
  );
};