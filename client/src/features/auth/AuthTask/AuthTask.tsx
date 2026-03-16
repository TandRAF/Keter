import { Button } from '../../../components/Button/Button';
import { LoginForm } from '../LoginForm/LoginForm';
import { RegisterForm } from '../RegisterForm/RegisterForm';

interface TaskCardProps {
  task: any; 
  onFinish: (id: string) => void;
  type:string;
}

export const AuthTask: React.FC<TaskCardProps> = ({ task, onFinish, type }) => {
    if(type == "text"){
        return (
            <div style={{paddingTop : "2rem"}}>
                <Button 
                    size='lg' 
                        onClick={() => onFinish(task.id)}
                >
                Let’s start
            </Button>
        </div>
  );
    }else if(type == "register"){
         return (
            <RegisterForm 
            task={task} 
            onFinish={onFinish}
            />
         )
    }else if(type == "login"){
         return (
            <LoginForm
            task={task} 
            onFinish={onFinish}
            />
         )
    }
};