import { FormEvent, useState } from 'react'
import { apiClient } from '../api/client'
import { RegistrationRequest } from '../Client'

export function RegisterPage() {
  const [form, setForm] = useState<RegistrationRequest>({
    firstName: '',
    lastName: '',
    contactInfo: '',
    location: '',
    email: '',
    password: '',
    profilePicture: undefined
  })
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [success, setSuccess] = useState(false)

  function update<K extends keyof RegistrationRequest>(key: K, value: RegistrationRequest[K]) {
    setForm(prev => ({ ...prev, [key]: value }))
  }

  async function onSubmit(e: FormEvent) {
    e.preventDefault()
    setLoading(true)
    setError(null)
    setSuccess(false)
    try {
      await apiClient.register(form)
      setSuccess(true)
    } catch (err: any) {
      setError(err?.message ?? 'Register failed')
    } finally {
      setLoading(false)
    }
  }

  return (
    <div style={{ maxWidth: 520 }}>
      <h2>Register</h2>
      <form onSubmit={onSubmit}>
        <div style={{ display: 'flex', flexDirection: 'column', gap: 12 }}>
          <input placeholder="First name" value={form.firstName ?? ''} onChange={e => update('firstName', e.target.value)} />
          <input placeholder="Last name" value={form.lastName ?? ''} onChange={e => update('lastName', e.target.value)} />
          <input placeholder="Contact info" value={form.contactInfo ?? ''} onChange={e => update('contactInfo', e.target.value)} />
          <input placeholder="Location" value={form.location ?? ''} onChange={e => update('location', e.target.value)} />
          <input placeholder="Email" value={form.email ?? ''} onChange={e => update('email', e.target.value)} />
          <input placeholder="Password" type="password" value={form.password ?? ''} onChange={e => update('password', e.target.value)} />
          <button disabled={loading} type="submit">{loading ? '...' : 'Create account'}</button>
        </div>
      </form>
      {error && <p style={{ color: 'crimson' }}>{error}</p>}
      {success && <p style={{ color: 'green' }}>Registered.</p>}
    </div>
  )
}


