import { api } from './apiClient';

import type {
  CreateMessageDTO,
  UpdateMessageDTO,
  ChatDTO
} from '@/types/dtos';

export async function createMessage(message: CreateMessageDTO) {
  const res = await api.post('/chat', message);
  return res.data; 
}

export async function updateMessage(message: UpdateMessageDTO) {
  const res = await api.put('/chat', message);
  return res.data;
}

export async function getChatById(id: string) {
  const res = await api.get<ChatDTO>(`/chat/${id}`);
  return res.data;
}

export async function getChatsByUserId(id: string) {
  const res = await api.get<ChatDTO[]>(`/chat/user/${id}`);
  return res.data;
}

export async function deleteMessage(id: string) {
  const res = await api.delete(`/chat/${id}`);
  return res.data;
}
