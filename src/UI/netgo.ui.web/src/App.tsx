import { BrowserRouter, Route, Routes } from 'react-router-dom'
import HomePage from './pages/Home'
import LoginPage from './pages/Login'
import RegisterPage from './pages/Register'
import NotFoundPage from './pages/NotFound'
import MePage from './pages/Me'
import ProductsPage from './pages/Products'
import CreatePage from './pages/Create'
import { DetailsPage } from './pages/Details'
import EditPage from './pages/Edit'

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path='/' element={<HomePage/>}></Route>        
        <Route path='*' element={<NotFoundPage/>}></Route>

        <Route path='login' element={<LoginPage/>}></Route>
        <Route path='register' element={<RegisterPage/>}></Route>
        <Route path='products' element={<ProductsPage />}></Route>
        <Route path='me' element={<MePage/>}></Route>
        <Route path='create' element={<CreatePage/>}></Route>
        <Route path='edit:/id' element={<EditPage/>}></Route>
        <Route path='details/:id' element={<DetailsPage/>}></Route>
      </Routes>
    </BrowserRouter>
  )
}

export default App
