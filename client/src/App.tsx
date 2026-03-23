import { Header } from './layouts/Header/Header'
import { Outlet } from 'react-router'
import { AuthProvider } from './context/authContext'
import styles from "./App.module.scss";

const App:React.FC = () =>{
  return (
     <>
     <AuthProvider>
      <main className={styles.main}>
      <Header/>
      <div className={styles.outlet}>
        <Outlet/>
      </div>
      </main>
     </AuthProvider>
    </>
  )
}
export default App