export interface FoodItemDTO {
    name: string;
    price: number;
  }
  
  export interface OrderInsertDTO {
    userId: string,
    address: string;
    price: number;
    items: {
      name: string;
      price: number;
      categoryId: number;
    }[];
  }
  
  export interface OrderReadOnlyDTO {
    id: number;
    address: string;
    price: number;
    items: FoodItemDTO[];
  }