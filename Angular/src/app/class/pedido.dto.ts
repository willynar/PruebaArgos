// Espeja exactamente el modelo Order.cs del backend
export interface OrderDto {
  id: number;
  customerName: string;
  orderStatusId: number;
  orderStatus?: OrderStatusDto;
  shippingMethodId: number;
  shippingMethod?: ShippingMethodDto;
  totalAmount: number;
  rowVersion?: string | null; // byte[] serializado como base64 — control de concurrencia (Optimistic Locking)
}

// Embebido para cuando el backend devuelva el objeto anidado
export interface OrderStatusDto {
  id: number;
  name: string;
}

export interface ShippingMethodDto {
  id: number;
  name: string;
}
