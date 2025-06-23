export interface Category {
  id: number;
  name: string;
  description?: string;
  iconName?: string;
  sortOrder: number;
  isActive: boolean;
}

export interface ShoppingCartItem {
  id: number;
  sessionId: string;
  productName: string;
  categoryName: string;
  categoryId: number;
  quantity: number;
  unit: string;
  priority: string;
  isChecked: boolean;
  addedAt: string;
  notes?: string;
}

export interface CartCategory {
  categoryId: number;
  categoryName: string;
  categoryIcon?: string;
  totalItems: number;
  items: ShoppingCartItem[];
}

export interface ShoppingCartSummary {
  sessionId: string;
  totalItems: number;
  totalCategories: number;
  categories: CartCategory[];
  lastUpdated: string;
  status: string;
}

export interface AddItemRequest {
  productName: string;
  categoryId: number;
  quantity: number;
  unit: string;
  priority: string;
  notes?: string;
}

export interface CompleteOrderRequest {
  sessionId: string;
  notes?: string;
}