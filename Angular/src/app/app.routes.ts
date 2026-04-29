import { Routes } from '@angular/router';

export const routes: Routes = [
  // Ruta Angular clásica (componente Angular)
  {
    path: 'pedido',
    loadComponent: () =>
      import('./components/pedido-form/pedido-form.component')
        .then(m => m.PedidoFormComponent)
  },
  { path: '', redirectTo: 'pedido', pathMatch: 'full' },
  { path: '**', redirectTo: 'pedido' }
];
