import { FormEvent, useState } from 'react'
import { apiClient } from '../api/client'
import { FileParameter, ProductDetailDto } from '../Client'

export function ProductCreatePage() {
  const [title, setTitle] = useState('')
  const [description, setDescription] = useState('')
  const [price, setPrice] = useState<number | undefined>(undefined)
  const [tradable, setTradable] = useState<boolean | undefined>(undefined)
  const [images, setImages] = useState<FileParameter[]>([])
  const [details, setDetails] = useState<ProductDetailDto[]>([])
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [success, setSuccess] = useState(false)

  function onFilesSelected(files: FileList | null) {
    if (!files) return
    const arr: FileParameter[] = Array.from(files).map(f => ({ data: f, fileName: f.name }))
    setImages(arr)
  }

  async function onSubmit(e: FormEvent) {
    e.preventDefault()
    setLoading(true)
    setError(null)
    setSuccess(false)
    try {
      await apiClient.productsPOST(title, description, undefined, undefined, tradable, price, details, images)
      setSuccess(true)
      setTitle('')
      setDescription('')
      setPrice(undefined)
      setTradable(undefined)
      setImages([])
      setDetails([])
    } catch (err: any) {
      setError(err?.message ?? 'Create failed')
    } finally {
      setLoading(false)
    }
  }

  return (
    <div style={{ maxWidth: 520 }}>
      <h2>New Product</h2>
      <form onSubmit={onSubmit}>
        <div style={{ display: 'flex', flexDirection: 'column', gap: 12 }}>
          <input placeholder="Title" value={title} onChange={e => setTitle(e.target.value)} />
          <textarea placeholder="Description" value={description} onChange={e => setDescription(e.target.value)} />
          <input placeholder="Price" type="number" value={price ?? ''} onChange={e => setPrice(e.target.value ? Number(e.target.value) : undefined)} />
          <label style={{ display: 'flex', alignItems: 'center', gap: 8 }}>
            <input type="checkbox" checked={!!tradable} onChange={e => setTradable(e.target.checked)} />
            Tradable
          </label>
          <input type="file" multiple onChange={e => onFilesSelected(e.target.files)} />
          <button disabled={loading} type="submit">{loading ? '...' : 'Create'}</button>
        </div>
      </form>
      {error && <p style={{ color: 'crimson' }}>{error}</p>}
      {success && <p style={{ color: 'green' }}>Created.</p>}
    </div>
  )
}


