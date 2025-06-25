import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';
import { 
  Category, 
  ShoppingCartSummary, 
  AddItemRequest,
  CompleteOrderRequest 
} from '../types/shopping';
import { shoppingService } from '../services/shoppingService';

interface ShoppingState {
  categories: Category[];
  currentCart: ShoppingCartSummary | null;
  sessionId: string | null;
  loading: boolean;
  error: string | null;
}

const initialState: ShoppingState = {
  categories: [],
  currentCart: null,
  sessionId: null,
  loading: false,
  error: null,
};


export const fetchCategories = createAsyncThunk(
  'shopping/fetchCategories',
  async () => {
    return await shoppingService.getCategories();
  }
);

export const createSession = createAsyncThunk(
  'shopping/createSession',
  async () => {
    return await shoppingService.createNewSession();
  }
);

export const addItemToCart = createAsyncThunk(
  'shopping/addItem',
  async (request: AddItemRequest) => {
    return await shoppingService.addItem(request);
  }
);

export const fetchCurrentCart = createAsyncThunk(
  'shopping/fetchCurrentCart',
  async (sessionId: string) => {
    return await shoppingService.getCurrentCart(sessionId);
  }
);

export const completeOrder = createAsyncThunk(
  'shopping/completeOrder',
  async (request: CompleteOrderRequest) => {
    return await shoppingService.completeOrder(request);
  }
);

export const removeItem = createAsyncThunk(
  'shopping/removeItem',
  async (itemId: number) => {
    await shoppingService.removeItem(itemId);
    return itemId;
  }
);

const shoppingSlice = createSlice({
  name: 'shopping',
  initialState,
  reducers: {
    clearError: (state) => {
      state.error = null;
    },
    setSessionId: (state, action: PayloadAction<string>) => {
      state.sessionId = action.payload;
    },
  },
  extraReducers: (builder) => {
    builder
      
      .addCase(fetchCategories.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchCategories.fulfilled, (state, action) => {
        state.loading = false;
        state.categories = action.payload;
      })
      .addCase(fetchCategories.rejected, (state, action) => {
        state.loading = false;
        state.error = action.error.message || 'Failed to fetch categories';
      })
      
      
      .addCase(createSession.fulfilled, (state, action) => {
        state.sessionId = action.payload;
      })
      
      
      .addCase(addItemToCart.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(addItemToCart.fulfilled, (state) => {
        state.loading = false;
      })
      .addCase(addItemToCart.rejected, (state, action) => {
        state.loading = false;
        state.error = action.error.message || 'Failed to add item';
      })
      
      
      .addCase(fetchCurrentCart.fulfilled, (state, action) => {
        state.currentCart = action.payload;
      })
      
      
      .addCase(completeOrder.fulfilled, (state) => {
        state.currentCart = null;
      })
      
      
      .addCase(removeItem.fulfilled, (state) => {
      
      });
  },
});

export const { clearError, setSessionId } = shoppingSlice.actions;
export default shoppingSlice.reducer;