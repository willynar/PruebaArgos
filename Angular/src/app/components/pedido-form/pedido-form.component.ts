import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CatalogoService } from '../../services/catalogo.service';
import { PedidoService } from '../../services/pedido.service';
import { CatalogoItemDto } from '../../class/catalogo.dto';
import { OrderDto } from '../../class/pedido.dto';

interface CatalogoMeta {
  items: CatalogoItemDto[];
  cargando: boolean;
  tiempoMs: number | null;
}

@Component({
  selector: 'app-pedido-form',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './pedido-form.component.html',
  styleUrls: ['./pedido-form.component.css']
})
export class PedidoFormComponent implements OnInit {
  // Catálogos
  estadosMeta: CatalogoMeta = { items: [], cargando: false, tiempoMs: null };
  metodosMeta: CatalogoMeta  = { items: [], cargando: false, tiempoMs: null };

  pedidos: OrderDto[] = [];
  formulario: OrderDto = this.formularioVacio();

  mensajeError: string | null = null;
  mensajeExito: string | null = null;
  guardando = false;
  cargando = false;

  get modoEditar(): boolean { return this.formulario.id > 0; }
  get cargandoCatalogos(): boolean { return this.estadosMeta.cargando || this.metodosMeta.cargando; }

  constructor(
    private catalogoService: CatalogoService,
    private pedidoService: PedidoService
  ) {}

  ngOnInit() {
    this.cargarEstados();
    this.cargarMetodosEnvio();
    this.cargarPedidos();
  }

  cargarEstados() {
    this.estadosMeta = { ...this.estadosMeta, cargando: true, tiempoMs: null };
    const inicio = performance.now();

    this.catalogoService.getEstados().subscribe({
      next: (data) => {
        const ms = Math.round(performance.now() - inicio);
        this.estadosMeta = { items: data, cargando: false, tiempoMs: ms };
      },
      error: () => {
        this.estadosMeta = { ...this.estadosMeta, cargando: false };
      }
    });
  }

  cargarMetodosEnvio() {
    this.metodosMeta = { ...this.metodosMeta, cargando: true, tiempoMs: null };
    const inicio = performance.now();

    this.catalogoService.getMetodosEnvio().subscribe({
      next: (data) => {
        const ms = Math.round(performance.now() - inicio);
        this.metodosMeta = { items: data, cargando: false, tiempoMs: ms };
      },
      error: () => {
        this.metodosMeta = { ...this.metodosMeta, cargando: false };
      }
    });
  }

  private formularioVacio(): OrderDto {
    return { id: 0, customerName: '', orderStatusId: 0, shippingMethodId: 0, totalAmount: 0, rowVersion: null };
  }

  cargarPedidos() {
    this.cargando = true;
    this.pedidoService.obtenerPedidos().subscribe({
      next: (data) => { this.pedidos = data; this.cargando = false; },
      error: () => { this.mensajeError = 'No se pudieron cargar los pedidos.'; this.cargando = false; }
    });
  }

  seleccionarPedido(pedido: OrderDto) {
    this.formulario = { ...pedido };
    this.mensajeError = null;
    this.mensajeExito = null;
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  cancelarEdicion() {
    this.formulario = this.formularioVacio();
    this.mensajeError = null;
    this.mensajeExito = null;
  }

  submit() {
    this.mensajeError = null;
    this.mensajeExito = null;
    this.guardando = true;

    const esEdicion = this.modoEditar;
    const idEditado = this.formulario.id;

    const operacion = esEdicion
      ? this.pedidoService.guardarPedido(this.formulario)
      : this.pedidoService.crearPedido(this.formulario);

    operacion.subscribe({
      next: (resultado) => {
        this.guardando = false;
        this.mensajeExito = esEdicion
          ? `✅ Pedido #${idEditado} actualizado exitosamente.`
          : `✅ Pedido #${resultado.id} creado exitosamente.`;
        this.formulario = this.formularioVacio();
        this.cargarPedidos();
      },
      error: (err) => {
        this.guardando = false;
        if (err.message === 'CONFLICTO_CONCURRENCIA') {
          this.mensajeError = '⚠️ Conflicto: Otro agente modificó este pedido antes que tú. Por favor recarga los datos.';
        } else {
          this.mensajeError = '❌ Error al guardar el pedido. Intenta de nuevo.';
        }
      }
    });
  }
}
