import { api } from './apiClient';
import type { CreateProductDTO, GetProductsDTO, ListProductDTO, PagedResult, ProductDTO, UpdateProductDTO } from '../types/dtos';

export async function getProducts(filter?: GetProductsDTO) {
  if(!filter) {
    const res = await api.get<PagedResult<ListProductDTO>>("/products");
    return res.data;
  }
    
  const res = await api.get<PagedResult<ListProductDTO>>(
    "/products?" + 
    `PageSize=${filter.pageSize ?? 10}&` +
    `Page=${filter.page ?? 1}&` +
    (filter.title ? `Title=${filter.title}&` : "") +
    (filter.categoryId ? `CategoryId=${filter.categoryId}&` : "") +
    (filter.priceMin ? `PriceMin=${filter.priceMin}&` : "") +
    (filter.priceMax ? `PriceMax=${filter.priceMax}&` : "") +
    (filter.priceFixed ? `PriceFixed=${filter.priceFixed}&` : "") +
    (filter.tradable ? `Tradable=${filter.tradable}&` : "") +
    (filter.sold ? `Sold=${filter.sold}&` : "")
  );

  return res.data;
}


export async function getProductById(id: string) {
  const res = await api.get<ProductDTO>(`/products/${id}`);
  return res.data;
}

export async function updateProduct(dto: UpdateProductDTO) {
  const formData = new FormData();

  formData.append("id", dto.id);
  formData.append("categoryId", dto.categoryId);
  formData.append("tradable", dto.tradable.toString());
  formData.append("details", JSON.stringify(dto.details));

  dto.images.forEach((img) => {
    formData.append("images", img);
  });

  dto.newImages.forEach((file) => {
    formData.append("newImages", file, file.name);
  });

  const res = await api.put("/products", formData, {
    headers: {
      "Content-Type": "multipart/form-data",
    },
    withCredentials: true,
  });

  return res.data;
}


export async function createProduct(dto: CreateProductDTO) {
    const formData = new FormData();

    formData.append("userId", dto.userId);
    formData.append("categoryId", dto.categoryId);
    formData.append("title", dto.title);
    formData.append("description", dto.description);
    formData.append("tradable", dto.tradable.toString());
    formData.append("price", dto.price.toString());

    formData.append("details", JSON.stringify(dto.details));

    dto.images.forEach((file) => {
        formData.append("images", file, file.name);
    });

    const res = await api.post("/products", formData, {
        headers: {
            "Content-Type": "multipart/form-data",
        },
        withCredentials: true
    });

    return res.data;
}

