import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { shareReplay, catchError, tap } from 'rxjs/operators';
import { CatalogoItemDto } from '../class/catalogo.dto';
import { environment } from './api.config';

@Injectable({ providedIn: 'root' })
export class CatalogoService {
  constructor(private http: HttpClient) {}

  getEstados(): Observable<CatalogoItemDto[]> {
    return this.http.get<CatalogoItemDto[]>(`${environment.apiUrl}/Catalogs/order/status`)
      .pipe(
        catchError(() => of([]))
      );
  }

  getMetodosEnvio(): Observable<CatalogoItemDto[]> {
    return this.http.get<CatalogoItemDto[]>(`${environment.apiUrl}/Catalogs/shipping/methods`)
      .pipe(
        catchError(() => of([]))
      );
  }

}
