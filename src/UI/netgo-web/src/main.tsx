import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App.tsx'
import { createBrowserRouter, RouterProvider } from 'react-router-dom'
import NotFound from './pages/NotFound.tsx'
import Detail from './pages/Detail.tsx'
import { GetProductWithOwner } from './Services/productService.ts'
import Home from './pages/Home.tsx'

const router = createBrowserRouter([
  {
    path: "/",
    element: <App />,
    errorElement: <NotFound />,
    children: [{
      index: true,
      element: <Home />,
    },
    {
      path: "details/:id",
      element: <Detail />,
      loader: async ({ params }) => {
        try {
          const res = await GetProductWithOwner(params.id!);
          return res;
        } catch {
          throw new Response("Not Found", { status: 404 });
        }
      }
    },
  ]}
]);

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <RouterProvider router={router} />
  </StrictMode>
)
