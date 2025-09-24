import { api } from './apiClient';

import type { UpdateUserDTO, UserDTO } from '@/types/dtos';

export async function getUserById(id: string) {
  const res = await api.get<UserDTO>(`/users/${id}`);
  return res.data;
}

export async function updateUser(user: UpdateUserDTO) {
  const res = await api.put('/users', user);
  return res.data;
}
