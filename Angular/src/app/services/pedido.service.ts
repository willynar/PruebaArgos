import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { OrderDto } from '../class/pedido.dto';
import { environment } from './api.config';

@Injectable({ providedIn: 'root' })
export class PedidoService {
  constructor(private http: HttpClient) {}

  obtenerPedidos(): Observable<OrderDto[]> {
    return this.http.get<OrderDto[]>(`${environment.apiUrl}/Orders`);
  }

  obtenerPedido(id: number): Observable<OrderDto> {
    return this.http.get<OrderDto>(`${environment.apiUrl}/Orders/${id}`);
  }

  crearPedido(order: OrderDto): Observable<OrderDto> {
    return this.http.post<OrderDto>(`${environment.apiUrl}/Orders`, order);
  }

  guardarPedido(order: OrderDto): Observable<OrderDto> {
    return this.http.put<OrderDto>(`${environment.apiUrl}/Orders/${order.id}`, order)
      .pipe(
        catchError((error: HttpErrorResponse) => {
          if (error.status === 409) return throwError(() => new Error('CONFLICTO_CONCURRENCIA'));
          return throwError(() => new Error('ERROR_DESCONOCIDO'));
        })
      );
  }
}
