import { api } from './apiClient';

import type {
  CategoryCreateDTO,
  CategoryUpdateDTO,
  CategoryDTO
} from '../types/dtos';

export async function getCategoryById(id: string) {
  const res = await api.get<CategoryDTO>(`/categories/${id}`);
  return res.data;
}

export async function getCategories() {
  const res = await api.get<CategoryDTO[]>('/categories');
  return res.data;
}

export async function createCategory(dto: CategoryCreateDTO) {
  const res = await api.post('/categories', dto);
  return res.data;
}

export async function updateCategory(dto: CategoryUpdateDTO) {
  const res = await api.put('/categories', dto);
  return res.data;
}

export async function deleteCategory(id: string) {
  const res = await api.delete(`/categories/${id}`);
  return res.data;
}
