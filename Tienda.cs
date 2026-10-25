using System.Collections.Generic;
using SistemaInventarioComposicion.Gestores;
using SistemaInventarioComposicion.Modelos;
using SistemaInventarioComposicion.Servicios;

namespace SistemaInventarioComposicion
{
    // Tienda compone el gestor y el servicio de reportes para centralizar las operaciones del inventario.
    public class Tienda
    {
        private readonly GestorDeInventario _gestor;
        private readonly Reportes _reportes;

        public Tienda() : this(new GestorDeInventario(), new Reportes())
        {
        }

        public Tienda(GestorDeInventario gestor, Reportes reportes)
        {
            _gestor = gestor;
            _reportes = reportes;
        }

        // El contenedor delega las operaciones en sus componentes y evita que la GUI los instancie directamente.
        public IReadOnlyList<Producto> ObtenerProductos()
        {
            return _gestor.ObtenerProductos();
        }

        public void AgregarProducto(Producto producto)
        {
            _gestor.AgregarProducto(producto);
        }

        public void EliminarProducto(int id)
        {
            _gestor.EliminarProducto(id);
        }

        public void ModificarCantidad(int id, int nuevaCantidad)
        {
            _gestor.ModificarCantidad(id, nuevaCantidad);
        }

        public void ActualizarProducto(int id, string nombre, decimal precio, int cantidad)
        {
            _gestor.ActualizarProducto(id, nombre, precio, cantidad);
        }

        // Reportes calcula el resumen para que la GUI lo presente sin duplicar las reglas de negocio.
        public ReporteInventario GenerarReporte()
        {
            return _reportes.CalcularReporte(_gestor.ObtenerProductos());
        }

        // La consola conserva la impresión del reporte mediante el componente existente.
        public void ImprimirReporte()
        {
            _reportes.GenerarReporteInventario(_gestor.ObtenerProductos());
        }
    }
}
