import { Header } from './layouts/Header/Header'
import { Footer } from './layouts/Footer/Footer'
import { Outlet } from 'react-router'
import { AuthProvider } from './context/authContext'

const App:React.FC = () =>{
  return (
     <>
     <AuthProvider>
      <Header/>
      <Outlet/>
      <Footer/>
     </AuthProvider>
    </>
  )
}
export default App