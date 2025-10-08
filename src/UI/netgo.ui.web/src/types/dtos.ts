export interface CreateProductDTO {
    userId: string;
    categoryId: string;
    title: string;
    description: string;
    tradable: boolean;
    price: number;
    details: ProductDetailDto[];
    images: File[]
}

export interface GetProductsDTO extends PagedRequest {
    title: string | null;
    categoryId: string | null;
    priceMin: number | null;
    priceMax: number | null;
    priceFixed: number | null;
    tradable: boolean | null;
    sold: boolean | null;
}

export interface ListProductDTO {
    id: string;
    userId: string;
    title: string;
    description: string;
    dateSold: string | null;
    tradable: boolean;
    price: number;
    image: string;
}

export interface ProductDetailDto {
    title: string;
    value: string;
}

export interface ProductDTO {
    id: string;
    categoryId: string;
    title: string;
    normalizedTitle: string;
    description: string;
    price: number;
    tradable: boolean;
    dateSold: string | null;
    details: ProductDetailDto[];
    images: string[];
}

export interface UpdateProductDTO {
    id: string;
    categoryId: string;
    tradable: boolean;
    details: ProductDetailDto[];
    images: string[];
    newImages: File[];
}

export interface PagedRequest {
    page: number;
    pageSize: number;
}

export interface PagedResult<T> {
    items: T[];
    totalCount: number;
    remainingCount: number;
    page: number;
    pageSize: number;
}

export interface ResetUserPasswordDTO {
    oldPassword: string;
    newPassword: string;
}

export interface UpdateUserDTO {
    id: string;
    firstName: string;
    lastName: string;
    contactInfo: string;
}

export interface CategoryCreateDTO {
    name: string;
}

export interface CategoryDTO {
    id: string;
    name: string;
    products: ProductDTO[];
}

export interface CategoryUpdateDTO {
    id: string;
    name: string;
}

export interface ListCategoryDTO {
    id: string;
    name: string;
}

export interface AuthRequest {
    email: string;
    password: string;
}

export interface AuthResponse {
    id: string;
    token: string;
}

export interface RegistrationRequest {
    firstName: string;
    lastName: string;
    contactInfo: string;
    location: string;
    email: string;
    password: string;
    profilePicture: File | undefined;
}

export interface RegistrationResponse {
    userId: string;
}

export interface User {
    id: string;
    firstName: string;
    lastName: string;
    normalizedUserName: string;
    emailConfirmed: boolean;
    email: string;
    contactInfo: string;
    location: string;
    verifiedSeller: boolean;
    profilePictureURL: string;
}