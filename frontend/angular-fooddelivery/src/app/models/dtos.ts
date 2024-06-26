export interface FoodItemDTO {
    name: string;
    price: number;
  }
  
  export interface OrderInsertDTO {
    address: string;
    price: number;
    items: {
      name: string;
      price: number;
      categoryId: number; // Ensure categoryId is provided or inferred
    }[];
  }
  
  export interface OrderReadOnlyDTO {
    id: number;
    address: string;
    price: number;
    items: FoodItemDTO[];
  }