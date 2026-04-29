// Espeja exactamente OrderStatus.cs y ShippingMethod.cs del backend
export interface CatalogoItemDto {
  id: number;
  name: string; // el backend devuelve "name", no "nombre"
}
