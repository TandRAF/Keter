import React, { useState } from 'react';
import styles from './AvatarSelector.module.scss';
import { api } from '../../../services/api'; 
import { Button } from '../../../components/Button/Button';
import { useAuth } from '../../../context/authContext'; 

interface AvatarSelectorProps {
  task?: any; 
  onFinish?: (id: string) => void;
}

const link_avatars = [
    '/../avatars/BananaRaccoon.png',
    '/../avatars/BathRaccoon.png',
    '/../avatars/CardRaccoon.png',
    '/../avatars/coolRaccoon.png',
    '/../avatars/WorkingRaccoon.png',
    '/../avatars/AlcohoRaccoon.png',
    '/../avatars/BedRaccoon.png',
    '/../avatars/BikeRaccoon.png',
    '/../avatars/CatRaccoon.png',
    '/../avatars/dogRaccoon.png',
    '/../avatars/PizzaRaccoon.png',
    '/../avatars/StrongRaccoon.png',
    '/../avatars/TrashRaccoon.png',
]
export const AvatarSelector: React.FC<AvatarSelectorProps> = ({ task, onFinish }) => {
  const { user, updateUser } = useAuth(); 
  
  const [selectedAvatar, setSelectedAvatar] = useState<string | null>(user?.profilePictureUrl || null);
  const [isLoading, setIsLoading] = useState(false);

  const handleSave = async () => {
    if (!selectedAvatar) return;
    
    setIsLoading(true);
    try {
      await api.patch('/auth/update-avatar', {
        AvatarUrl: selectedAvatar
      });
      
      updateUser({ profilePictureUrl: selectedAvatar });
      if (onFinish) {
        onFinish(task?.id);
      }

    } catch (error) {
      console.error("Eroare la salvarea avatarului:", error);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className={styles.container}>
      <div className={styles.avatarGrid}>
        {link_avatars.map((imageVar, index) => (
          <img 
            key={index}
            src={imageVar}
            alt={`Avatar option ${index + 1}`}
            className={`${styles.avatarOption} ${selectedAvatar === imageVar ? styles.selected : ''}`}
            onClick={() => setSelectedAvatar(imageVar)}
          />
        ))}
      </div>

      <Button 
        size='lg'
        onClick={handleSave} 
        disabled={!selectedAvatar || isLoading || selectedAvatar === user?.profilePictureUrl}
        className={styles.saveBtn}
      >
        {isLoading ? "Is Saving..." : "Save Avatar"}
      </Button>
    </div>
  );
};