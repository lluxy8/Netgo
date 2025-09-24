import { api } from './apiClient';

// kendi /types klasöründen import et
import type {
  GetProductsDTO,
  CreateProductDTO,
  UpdateProductDTO,
  ProductDTO,
  ListProductDTO,
  ProductWithOwnerDTO
} from '@/types/dtos';

export async function getProducts(filter: GetProductsDTO) {
  const res = await api.get<ListProductDTO[]>('/products', {
    params: filter
  });
  return res.data;
}
export async function getProductById(id: string) {
  const res = await api.get<ProductDTO>(`/products/${id}`);
  return res.data;
}

export async function createProduct(product: CreateProductDTO) {
  const res = await api.post('/products', product);
  return res.data;
}

export async function updateProduct(product: UpdateProductDTO) {
  const res = await api.put('/products', product);
  return res.data;
}

export async function getProductsByUserId(userId: string) {
  const res = await api.get<ProductDTO[]>(`/products/user/${userId}`);
  return res.data;
}

export async function GetProductWithOwner(productId: string) {
  const res = await api.get<ProductWithOwnerDTO>(`/products/${productId}/withowner`);
  return res.data;
}