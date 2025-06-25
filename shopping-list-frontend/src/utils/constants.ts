//export const API_BASE_URL = 'https://localhost:7066'; 
export const API_BASE_URL = 'https://shopping-list-api.azurewebsites.net';

export const API_ENDPOINTS = {
  CATEGORIES: '/api/categories',
  SHOPPING: {
    CATEGORIES: '/api/shopping/categories',
    ADD_ITEM: '/api/shopping/add-item',
    CURRENT_CART: '/api/shopping/current-cart',
    COMPLETE_ORDER: '/api/shopping/complete-order',
    UPDATE_ITEM: '/api/shopping/update-item',
    REMOVE_ITEM: '/api/shopping/remove-item',
    NEW_SESSION: '/api/shopping/new-session'
  }
} as const;

export const DEFAULT_VALUES = {
  UNIT: 'יחידה',
  PRIORITY: 'Normal',
  STATUS: 'Active'
} as const;