import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CatalogoService } from '../../services/catalogo.service';
import { PedidoService } from '../../services/pedido.service';
import { CatalogoItemDto } from '../../class/catalogo.dto';
import { OrderDto } from '../../class/pedido.dto';

@Component({
  selector: 'app-pedido-form',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './pedido-form.component.html',
  styleUrls: ['./pedido-form.component.css']
})
export class PedidoFormComponent implements OnInit {
  // Catálogos
  estados: CatalogoItemDto[] = [];
  metodosEnvio: CatalogoItemDto[] = [];

  // Metadatos para latencia
  latenciaEstados: number | null = null;
  latenciaMetodos: number | null = null;

  pedidos: OrderDto[] = [];
  formulario: OrderDto = this.formularioVacio();

  mensajeError: string | null = null;
  mensajeExito: string | null = null;
  guardando = false;
  cargando = false;
  cargandoCatalogos = false;

  get modoEditar(): boolean { return this.formulario.id > 0; }

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
    this.cargandoCatalogos = true;
    const inicio = performance.now();
    this.catalogoService.getEstados().subscribe({
      next: (data) => { 
        this.estados = data; 
        this.latenciaEstados = Math.round(performance.now() - inicio);
        this.cargandoCatalogos = false;
      },
      error: () => { this.cargandoCatalogos = false; }
    });
  }

  cargarMetodosEnvio() {
    this.cargandoCatalogos = true;
    const inicio = performance.now();
    this.catalogoService.getMetodosEnvio().subscribe({
      next: (data) => { 
        this.metodosEnvio = data; 
        this.latenciaMetodos = Math.round(performance.now() - inicio);
        this.cargandoCatalogos = false;
      },
      error: () => { this.cargandoCatalogos = false; }
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
