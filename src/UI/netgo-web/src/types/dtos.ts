export interface AuthResponseDTO {
    id: string;
    token: string;
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

export interface CategoryUpdateDTO {
    id: string;
    name: string;
}

export interface ChatDTO {
    firstUserId: string;
    secondUserId: string;
    messages: MessageDTO[];
}

export interface ListChatDTO {
    id: string;
    firstUserId: string;
    secondUserId: string;
}

export interface ListChatDTO {
    id: string;
    firstUserId: string;
    secondUserId: string;
}

export interface DeleteMessageDTO {
    id: string;
}

export interface MessageDTO {
    id: string;
    userId: string;
    chatId: string;
    content: string;
    displayContent: string;
    oldContents: string[];
    dateRead: string | null;
    dateDeleted: string | null;
    dateModified: string | null;
}

export interface MessageSentDTO {
    messageId: string;
    chatId: string;
}

export interface UpdateMessageDTO {
    id: string;
}

export interface CreateProductDTO {
    userId: string;
    categoryId: string;
    title: string;
    description: string;
    tradable: boolean;
    price: number;
    details: ProductDetailDto[];
}

export interface GetProductsDTO extends PagedRequest {
    title?: string | null;
    categoryId?: string | null;
    priceMin?: number | null;
    priceMax?: number | null;
    priceFixed?: number | null;
    tradable?: boolean | null;
    sold?: boolean | null;
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
    dateArchived: string | null;
}

export interface UpdateProductDTO {
    id: string;
    categoryId: string;
    title: string;
    description: string;
    tradable: boolean;
    details: ProductDetailDto[];
    images: string[];
    archieved: boolean;
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

export interface AuthRequest {
    email: string;
    password: string;
}

export interface AuthResponse {
    id: string;
    token: string;
}

export interface ConfirmEmailRequest {
    userId: string;
    token: string;
}

export interface RegistrationRequest {
    firstName: string;
    lastName: string;
    contactInfo: string;
    location: string;
    email: string;
    password: string;
    profilePicture: string;
}

export interface RegistrationResponse {
    userId: string;
}

export interface UserDTO {
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

export interface PagedResult<T> {
    items: T[];
    totalCount: number;
    page: number;
    pageSize: number;
}

export interface PagedRequest {
    page: number;
    pageSize: number;
}

export interface CreateMessageDTO {
    from: string;
    to: string;
    content: string;
}

export interface ProductWithOwnerDTO {
    productId: string;
    productTitle: string;
    productDescription: string;
    productImages: string[];
    ownerId: string;
    ownerName: string;
    ownerLocation: string;
    ownerAvatar: string;
    ownerVerifiedSeller: boolean;
    productDetails: ProductDetailDto[];
}

export interface PreRegister {
    email: string;
    firstname: string;
    lastname: string;
}