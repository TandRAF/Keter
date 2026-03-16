import React from 'react'
import style from './AuthPage.module.scss'
import { AuthBoard } from '../../features/auth/AuthContainer/AuthContainer'
import { Spark } from '../../components/Icons/Icons'

interface AuthPageProps {
  type: 'login' | 'register'; // Folosim un union type pentru siguranta
}

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
const registerColumns:Column[] = [
            { 
            id: 'col-1', 
      title: 'To Do', 
      tasks: [
        { 
          id: 't1', 
          title: 'Welcome in Keter',
          status: 'col-1',
          content: (
            <p>
              The Task Developing App is a productivity tool designed to help individuals and teams efficiently manage and track their tasks from creation to completion. 
              <br/><br/>It allows users to create, assign, prioritize, and monitor tasks in a structured and intuitive interface.
            </p>
          ),
          type: "text"
        },
        { id: 't2', title: 'Create The Account', content: <></>, status: 'col-1', type: "register" },
        { id: 't3', title: 'Make your Workspace', content: <p>Install Vite and Framer Motion.</p>, status: 'col-1', type: "text" },
      ] 
    },
    { id: 'col-2', title: 'In Progress', tasks: [] },
    { id: 'col-3', title: 'Done', tasks: [] },
  ]
const logInColumns:Column[] = [
            { 
            id: 'col-1', 
            title: 'To Do', 
      tasks: [
        { id: 't2', title: 'Get in your Account', content: <></>, status: 'col-1', type: "login" },
      ] 
    },
    { id: 'col-2', title: 'In Progress', tasks: [] },
    { id: 'col-3', title: 'Done', tasks: [] },
  ]


export const AuthPage: React.FC<AuthPageProps> = ({ type }) => {
  return (
    <div className={style.authPage} >
        <h1>Complete Your First <span>Tasks</span><Spark/></h1>
        <AuthBoard columns={
            type === 'login' ? logInColumns:registerColumns
        }/>
    </div>
  )
}
