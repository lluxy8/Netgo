import { api } from './apiClient';

import type { AuthRequest, AuthResponseDTO, RegistrationRequest } from '@/types/dtos'; 

export async function login(auth: AuthRequest) {
  const res = await api.post<AuthResponseDTO>('/auth/login', auth);
  return res.data;
}

export async function register(registerData: RegistrationRequest) {
  const res = await api.post('/auth/register', registerData);
  return res.data; 
}

export async function createPasswordResetToken(id: string, oldPassword: string) {
  const res = await api.post(`/auth/passwordresettoken/${id}`, oldPassword, {
    headers: { 'Content-Type': 'application/json' },
  });
  return res.data;
}

export async function confirmPasswordReset(id: string, token: string, newPassword: string) {
  const res = await api.post(`/auth/passwordreset/${id}/${encodeURIComponent(token)}`, newPassword, {
    headers: { 'Content-Type': 'application/json' },
  });
  return res.data;
}

export async function createEmailConfirmToken(id: string) {
  const res = await api.post(`/auth/emailconfirmtoken/${id}`);
  return res.data;
}

export async function confirmEmail(id: string, token: string) {
  const res = await api.post(`/auth/emailconfirm/${id}/${encodeURIComponent(token)}`);
  return res.data;
}
