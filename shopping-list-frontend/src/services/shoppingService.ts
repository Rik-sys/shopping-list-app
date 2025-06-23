import apiClient from './api';
import { ApiResponse } from '../types/api';
import { 
  Category, 
  ShoppingCartSummary, 
  AddItemRequest, 
  ShoppingCartItem,
  CompleteOrderRequest 
} from '../types/shopping';
import { API_ENDPOINTS } from '../utils/constants';

export const shoppingService = {
  // Get all categories
  getCategories: async (): Promise<Category[]> => {
    const response = await apiClient.get<ApiResponse<Category[]>>(API_ENDPOINTS.SHOPPING.CATEGORIES);
    return response.data.data || [];
  },

  // Create new session
  createNewSession: async (): Promise<string> => {
    const response = await apiClient.post<ApiResponse<string>>(API_ENDPOINTS.SHOPPING.NEW_SESSION);
    return response.data.data || '';
  },

  // Add item to cart
  addItem: async (request: AddItemRequest): Promise<ShoppingCartItem> => {
    const response = await apiClient.post<ApiResponse<ShoppingCartItem>>(
      API_ENDPOINTS.SHOPPING.ADD_ITEM, 
      request
    );
    return response.data.data!;
  },

  // Get current cart
  getCurrentCart: async (sessionId: string): Promise<ShoppingCartSummary> => {
    const response = await apiClient.get<ApiResponse<ShoppingCartSummary>>(
      `${API_ENDPOINTS.SHOPPING.CURRENT_CART}/${sessionId}`
    );
    return response.data.data!;
  },

  // Complete order
  completeOrder: async (request: CompleteOrderRequest): Promise<any> => {
    const response = await apiClient.post<ApiResponse<any>>(
      API_ENDPOINTS.SHOPPING.COMPLETE_ORDER,
      request
    );
    return response.data.data;
  },

  // Remove item
  removeItem: async (itemId: number): Promise<boolean> => {
    const response = await apiClient.delete<ApiResponse<boolean>>(
      `${API_ENDPOINTS.SHOPPING.REMOVE_ITEM}/${itemId}`
    );
    return response.data.data || false;
  }
};
