import { FormEvent, useState } from 'react'
import { apiClient } from '../api/client'
import { FileParameter } from '../Client'

export function ProfilePage() {
  const [firstName, setFirstName] = useState('')
  const [lastName, setLastName] = useState('')
  const [contactInfo, setContactInfo] = useState('')
  const [id, setId] = useState('')
  const [profilePicture, setProfilePicture] = useState<FileParameter | undefined>(undefined)
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [success, setSuccess] = useState(false)

  function onFileSelected(file: File | null) {
    if (!file) {
      setProfilePicture(undefined)
      return
    }
    setProfilePicture({ data: file, fileName: file.name })
  }

  async function onSubmit(e: FormEvent) {
    e.preventDefault()
    setLoading(true)
    setError(null)
    setSuccess(false)
    try {
      await apiClient.usersPUT(id || undefined, firstName || undefined, lastName || undefined, contactInfo || undefined, profilePicture)
      setSuccess(true)
    } catch (err: any) {
      setError(err?.message ?? 'Update failed')
    } finally {
      setLoading(false)
    }
  }

  return (
    <div style={{ maxWidth: 520 }}>
      <h2>Profile</h2>
      <form onSubmit={onSubmit}>
        <div style={{ display: 'flex', flexDirection: 'column', gap: 12 }}>
          <input placeholder="User ID" value={id} onChange={e => setId(e.target.value)} />
          <input placeholder="First name" value={firstName} onChange={e => setFirstName(e.target.value)} />
          <input placeholder="Last name" value={lastName} onChange={e => setLastName(e.target.value)} />
          <input placeholder="Contact info" value={contactInfo} onChange={e => setContactInfo(e.target.value)} />
          <input type="file" onChange={e => onFileSelected(e.target.files?.[0] ?? null)} />
          <button disabled={loading} type="submit">{loading ? '...' : 'Save profile'}</button>
        </div>
      </form>
      {error && <p style={{ color: 'crimson' }}>{error}</p>}
      {success && <p style={{ color: 'green' }}>Saved.</p>}
    </div>
  )
}


