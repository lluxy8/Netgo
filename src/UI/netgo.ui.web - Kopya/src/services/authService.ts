import { api } from './apiClient';

import type { AuthRequest, AuthResponse, RegistrationRequest } from '../types/dtos'; 

export async function login(auth: AuthRequest) {
  const res = await api.post<AuthResponse>('/auth/login', auth);
  return res.data;
}

export async function logOut() {
  const res = await api.post('/users/logout');
  return res.data;
}

export async function register(registerData: RegistrationRequest) {
  const formData = new FormData();
  formData.append("email", registerData.email);
  formData.append("password", registerData.password);
  formData.append("contactInfo", registerData.contactInfo);
  formData.append("location", registerData.location);
  formData.append("firstName", registerData.firstName);
  formData.append("lastName", registerData.lastName);

  if (registerData.profilePicture) {
    formData.append("profilePicture", registerData.profilePicture);
  }

  const res = await api.post("/auth/register", formData, {
    headers: { "Content-Type": "multipart/form-data" },
  });

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
