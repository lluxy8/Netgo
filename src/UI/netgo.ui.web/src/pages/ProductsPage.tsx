import { FormEvent, useEffect, useState } from 'react'
import { apiClient } from '../api/client'

type Product = {
  id: string
  title: string
  price?: number
}

export function ProductsPage() {
  const [items, setItems] = useState<Product[]>([])
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [title, setTitle] = useState('')

  async function load() {
    setLoading(true)
    setError(null)
    try {
      await apiClient.productsGET(title || undefined, undefined, undefined, undefined, undefined, undefined, undefined, 1, 20)
      // The generated client returns void; in a real API we'd parse JSON.
      // For now, just simulate empty list.
      setItems([])
    } catch (err: any) {
      setError(err?.message ?? 'Failed to fetch')
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => {
    load()
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [])

  function onSearch(e: FormEvent) {
    e.preventDefault()
    load()
  }

  return (
    <div>
      <h2>Products</h2>
      <form onSubmit={onSearch} style={{ marginBottom: 12 }}>
        <input placeholder="Search title" value={title} onChange={e => setTitle(e.target.value)} />
        <button type="submit" disabled={loading} style={{ marginLeft: 8 }}>Search</button>
      </form>
      {loading && <p>Loading...</p>}
      {error && <p style={{ color: 'crimson' }}>{error}</p>}
      {!loading && items.length === 0 && <p>No products yet.</p>}
      <ul>
        {items.map(p => (
          <li key={p.id}>{p.title} {p.price ? `- ${p.price}` : ''}</li>
        ))}
      </ul>
    </div>
  )
}


