using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SistemaInventarioComposicion.Gestores;
using SistemaInventarioComposicion.Modelos;
using SistemaInventarioComposicion.Gui.Services;

namespace SistemaInventarioComposicion.Gui.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly GestorDeInventario _gestor = new();
    private readonly InventarioStore _store = new();

    [ObservableProperty]
    private decimal? idNumerico;

    [ObservableProperty]
    private string nombreTexto = string.Empty;

    [ObservableProperty]
    private decimal? precioNumerico;

    [ObservableProperty]
    private decimal? cantidadNumerico;

    [ObservableProperty]
    private Producto? productoSeleccionado;

    [ObservableProperty]
    private string mensaje = string.Empty;

    [ObservableProperty]
    private bool puedeModificarEliminar;

    [ObservableProperty]
    private bool modoEdicion;

    [ObservableProperty]
    private int productoEnEdicionId;

    [ObservableProperty]
    private string textoBotonAccion = "Agregar";

    [ObservableProperty]
    private string colorMensaje = "#1F3B73";

    [ObservableProperty]
    private bool esError;

    public ObservableCollection<ProductoView> Productos { get; } = new();
    public ObservableCollection<StockBajoView> StockBajoLista { get; } = new();

    [ObservableProperty]
    private int totalItems;

    [ObservableProperty]
    private decimal valorTotal;

    [ObservableProperty]
    private int totalProductos;

    [ObservableProperty]
    private decimal precioMasAlto;

    [ObservableProperty]
    private decimal precioMasBajo;

    [ObservableProperty]
    private string stockBajo = string.Empty;

    [ObservableProperty]
    private string productoMasCaroTexto = string.Empty;

    [ObservableProperty]
    private string productoMasBaratoTexto = string.Empty;

    public Func<string, Task<bool>>? SolicitarConfirmacion { get; set; }

    private void MarcarMensaje(string texto, bool error)
    {
        Mensaje = texto;
        EsError = error;
        ColorMensaje = error ? "#DC2626" : "#1F3B73";
    }

    private void GuardarAutomatico()
    {
        try
        {
            _store.Guardar(_gestor.ObtenerProductos());
        }
        catch (Exception ex)
        {
            MarcarMensaje($"Error al guardar automáticamente: {ex.Message}", true);
        }
    }

    [RelayCommand]
    private async Task Agregar()
    {
        MarcarMensaje(string.Empty, false);
        if (ModoEdicion)
        {
            await GuardarCambios();
            return;
        }

        if (IdNumerico == null || IdNumerico <= 0)
        {
            MarcarMensaje("El ID debe ser un número entero mayor a 0.", true);
            return;
        }
        if (string.IsNullOrWhiteSpace(NombreTexto))
        {
            MarcarMensaje("El nombre del producto es obligatorio.", true);
            return;
        }
        if (PrecioNumerico == null || PrecioNumerico < 0)
        {
            MarcarMensaje("El precio debe ser un valor numérico mayor o igual a 0.", true);
            return;
        }
        if (CantidadNumerico == null || CantidadNumerico < 0)
        {
            MarcarMensaje("La cantidad debe ser un valor numérico mayor o igual a 0.", true);
            return;
        }

        int id = (int)Math.Round(IdNumerico.Value);
        decimal precio = Math.Round(PrecioNumerico.Value, 2);
        int cantidad = (int)Math.Round(CantidadNumerico.Value);

        foreach (var p in _gestor.ObtenerProductos())
        {
            if (p.Id == id)
            {
                MarcarMensaje("Ya existe un producto con ese ID. No se permite repetir IDs.", true);
                return;
            }
        }

        bool confirmar = true;
        if (SolicitarConfirmacion != null)
        {
            confirmar = await SolicitarConfirmacion($"¿Desea agregar el producto '{NombreTexto.Trim()}' con ID {id}?");
        }
        if (!confirmar) return;

        var prod = new Producto(id, NombreTexto.Trim(), precio, cantidad);
        _gestor.AgregarProducto(prod);
        GuardarAutomatico();
        CancelarEdicion();
        ActualizarSiguienteId();
        Refrescar();
        MarcarMensaje("Producto agregado correctamente.", false);
    }

    private async Task GuardarCambios()
    {
        MarcarMensaje(string.Empty, false);
        if (string.IsNullOrWhiteSpace(NombreTexto))
        {
            MarcarMensaje("El nombre del producto es obligatorio.", true);
            return;
        }
        if (PrecioNumerico == null || PrecioNumerico < 0)
        {
            MarcarMensaje("El precio debe ser un valor numérico mayor o igual a 0.", true);
            return;
        }
        if (CantidadNumerico == null || CantidadNumerico < 0)
        {
            MarcarMensaje("La cantidad debe ser un valor numérico mayor o igual a 0.", true);
            return;
        }

        bool confirmar = true;
        if (SolicitarConfirmacion != null)
        {
            confirmar = await SolicitarConfirmacion($"¿Desea guardar los cambios del producto ID {ProductoEnEdicionId}?");
        }
        if (!confirmar) return;

        int cantidad = (int)Math.Round(CantidadNumerico.Value);
        decimal precio = Math.Round(PrecioNumerico.Value, 2);
        _gestor.ActualizarProducto(ProductoEnEdicionId, NombreTexto.Trim(), precio, cantidad);
        GuardarAutomatico();
        CancelarEdicion();
        ActualizarSiguienteId();
        Refrescar();
        MarcarMensaje("Producto actualizado correctamente.", false);
    }

    [RelayCommand]
    private void Modificar()
    {
        Mensaje = string.Empty;
        if (ProductoSeleccionado == null)
        {
            Mensaje = "Selecciona un producto del listado para modificar su stock.";
            return;
        }
        if (CantidadNumerico == null || CantidadNumerico < 0)
        {
            Mensaje = "La cantidad debe ser un valor numérico >= 0.";
            return;
        }
        int cantidad = (int)Math.Round(CantidadNumerico.Value);
        _gestor.ModificarCantidad(ProductoSeleccionado.Id, cantidad);
        LimpiarFormularioInterno();
        ProductoSeleccionado = null;
        Refrescar();
        Mensaje = "Stock actualizado correctamente.";
    }

    [RelayCommand]
    private async Task Eliminar()
    {
        Mensaje = string.Empty;
        if (ProductoSeleccionado == null)
        {
            Mensaje = "Selecciona un producto del listado para eliminar.";
            return;
        }
        await EliminarPorId(ProductoSeleccionado.Id, ProductoSeleccionado.Nombre);
    }

    [RelayCommand]
    private async Task EliminarDesdeFila(ProductoView? item)
    {
        if (item == null) return;
        await EliminarPorId(item.Id, item.Nombre);
    }

    private async Task EliminarPorId(int id, string nombre)
    {
        bool confirmar = true;
        if (SolicitarConfirmacion != null)
        {
            confirmar = await SolicitarConfirmacion($"¿Está seguro de eliminar '{nombre}' (ID: {id})?\nEsta acción no se puede deshacer.");
        }
        if (!confirmar) return;
        _gestor.EliminarProducto(id);
        GuardarAutomatico();
        CancelarEdicion();
        Refrescar();
        MarcarMensaje("Producto eliminado correctamente.", false);
    }

    [RelayCommand]
    private void EditarDesdeFila(ProductoView? item)
    {
        if (item == null) return;
        var producto = _gestor.ObtenerProductos().FirstOrDefault(p => p.Id == item.Id);
        if (producto == null) return;
        ModoEdicion = true;
        ProductoEnEdicionId = producto.Id;
        IdNumerico = producto.Id;
        NombreTexto = producto.Nombre;
        PrecioNumerico = producto.Precio;
        CantidadNumerico = producto.Cantidad;
        ProductoSeleccionado = producto;
        ActualizarTextoBoton();
        MarcarMensaje($"Editando producto ID: {producto.Id}. El ID no se puede modificar.", false);
        Refrescar();
    }

    [RelayCommand]
    private void CancelarEdicion()
    {
        ModoEdicion = false;
        ProductoEnEdicionId = 0;
        LimpiarFormularioInterno();
        ProductoSeleccionado = null;
        MarcarMensaje(string.Empty, false);
        ActualizarTextoBoton();
        ActualizarSiguienteId();
        Refrescar();
    }

    [RelayCommand]
    private void LimpiarFormulario()
    {
        CancelarEdicion();
        MarcarMensaje("Formulario limpiado.", false);
        ActualizarSiguienteId();
    }

    [RelayCommand]
    private void Guardar()
    {
        try
        {
            _store.Guardar(_gestor.ObtenerProductos());
            MarcarMensaje($"Guardado en: {_store.GetPath()}", false);
        }
        catch (Exception ex)
        {
            MarcarMensaje($"Error al guardar: {ex.Message}", true);
        }
        Refrescar();
    }

    [RelayCommand]
    private void Cargar()
    {
        try
        {
            var lista = _store.Cargar();
            foreach (var p in _gestor.ObtenerProductos().ToList())
            {
                _gestor.EliminarProducto(p.Id);
            }
            foreach (var p in lista)
            {
                bool existe = false;
                foreach (var ep in _gestor.ObtenerProductos())
                {
                    if (ep.Id == p.Id) { existe = true; break; }
                }
                if (!existe) _gestor.AgregarProducto(p);
            }
            CancelarEdicion();
            ActualizarSiguienteId();
            Refrescar();
            MarcarMensaje($"Cargado desde: {_store.GetPath()}", false);
        }
        catch (Exception ex)
        {
            MarcarMensaje($"Error al cargar: {ex.Message}", true);
        }
    }

    [RelayCommand]
    private void ActualizarReportes()
    {
        Refrescar();
    }

    partial void OnProductoSeleccionadoChanged(Producto? value)
    {
        PuedeModificarEliminar = value != null;
        if (value != null && !ModoEdicion)
        {
            IdNumerico = value.Id;
            NombreTexto = value.Nombre;
            PrecioNumerico = value.Precio;
            CantidadNumerico = value.Cantidad;
            Mensaje = string.Empty;
        }
    }

    partial void OnModoEdicionChanged(bool value)
    {
        ActualizarTextoBoton();
    }

    private void ActualizarTextoBoton()
    {
        TextoBotonAccion = ModoEdicion ? "Guardar cambios" : "Agregar";
    }

    private void ActualizarSiguienteId()
    {
        if (ModoEdicion) return;
        var lista = _gestor.ObtenerProductos();
        if (lista.Count == 0)
        {
            IdNumerico = 1;
            return;
        }
        IdNumerico = lista.Max(p => p.Id) + 1;
    }

    private void LimpiarFormularioInterno()
    {
        IdNumerico = null;
        NombreTexto = string.Empty;
        PrecioNumerico = null;
        CantidadNumerico = null;
    }

    private void Refrescar()
    {
        Productos.Clear();
        var lista = _gestor.ObtenerProductos();
        foreach (var p in lista.OrderBy(x => x.Id))
        {
            Productos.Add(new ProductoView
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Precio = p.Precio,
                Cantidad = p.Cantidad,
                Subtotal = p.Precio * p.Cantidad
            });
        }
        TotalItems = Productos.Sum(x => x.Cantidad);
        ValorTotal = Productos.Sum(x => x.Subtotal);
        ActualizarReportesDatos(lista);
        PuedeModificarEliminar = ProductoSeleccionado != null;
    }

    private void ActualizarReportesDatos(List<Producto> lista)
    {
        TotalProductos = lista.Count;
        StockBajoLista.Clear();
        if (lista.Count == 0)
        {
            PrecioMasAlto = 0;
            PrecioMasBajo = 0;
            StockBajo = "Sin productos registrados.";
            ProductoMasCaroTexto = "N/A";
            ProductoMasBaratoTexto = "N/A";
            return;
        }
        var masCaro = lista.OrderByDescending(x => x.Precio).First();
        var masBarato = lista.OrderBy(x => x.Precio).First();
        PrecioMasAlto = masCaro.Precio;
        PrecioMasBajo = masBarato.Precio;
        ProductoMasCaroTexto = $"{masCaro.Nombre} (ID {masCaro.Id}) - {masCaro.Precio:C}";
        ProductoMasBaratoTexto = $"{masBarato.Nombre} (ID {masBarato.Id}) - {masBarato.Precio:C}";

        var bajos = lista.Where(x => x.Cantidad <= 5).OrderBy(x => x.Cantidad).ToList();
        if (bajos.Count == 0) StockBajo = "Ninguno (todos con stock mayor a 5).";
        else
        {
            StockBajo = $"{bajos.Count} producto(s) con stock bajo.";
            foreach (var b in bajos)
            {
                StockBajoLista.Add(new StockBajoView
                {
                    Id = b.Id,
                    Nombre = b.Nombre,
                    Stock = b.Cantidad,
                    Precio = b.Precio,
                    ValorReponer = b.Precio * b.Cantidad
                });
            }
        }
    }

    public class ProductoView
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal { get; set; }
    }

    public class StockBajoView
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int Stock { get; set; }
        public decimal Precio { get; set; }
        public decimal ValorReponer { get; set; }
    }
}
