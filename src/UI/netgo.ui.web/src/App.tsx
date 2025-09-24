import './App.css'
import { Link, Outlet, Route, Routes } from 'react-router-dom'
import { LoginPage } from './pages/LoginPage'
import { RegisterPage } from './pages/RegisterPage'
import { ProductsPage } from './pages/ProductsPage'
import { ProductCreatePage } from './pages/ProductCreatePage'
import { ProfilePage } from './pages/ProfilePage'

function Layout() {
  return (
    <div>
      <nav style={{ display: 'flex', gap: 12, padding: 12, borderBottom: '1px solid #eee' }}>
        <Link to="/">Products</Link>
        <Link to="/products/new">New Product</Link>
        <Link to="/profile">Profile</Link>
        <span style={{ marginLeft: 'auto', display: 'flex', gap: 12 }}>
          <Link to="/login">Login</Link>
          <Link to="/register">Register</Link>
        </span>
      </nav>
      <main style={{ padding: 16 }}>
        <Outlet />
      </main>
    </div>
  )
}

function App() {
  return (
    <Routes>
      <Route element={<Layout />}> 
        <Route index element={<ProductsPage />} />
        <Route path="products/new" element={<ProductCreatePage />} />
        <Route path="profile" element={<ProfilePage />} />
        <Route path="login" element={<LoginPage />} />
        <Route path="register" element={<RegisterPage />} />
      </Route>
    </Routes>
  )
}

export default App
