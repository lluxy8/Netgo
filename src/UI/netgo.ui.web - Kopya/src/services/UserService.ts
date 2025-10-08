import { api } from './apiClient';

import type { UpdateUserDTO, User } from '../types/dtos';

export async function getUserById(id: string) {
  const res = await api.get<User>(`/users/${id}`);
  return res.data;
}

export async function getMe() {
  const res = await api.get<User>(`/users/me`);
  return res.data;
}


export async function updateUser(user: UpdateUserDTO) {
  const res = await api.put('/users', user);
  return res.data;
}


