using System;
using System.Collections.Generic;
using System.Linq;
using SistemaInventarioComposicion.Modelos;

namespace SistemaInventarioComposicion.Gestores
{
    public class GestorDeInventario
    {
        private List<Producto> productos = new List<Producto>();

        public void AgregarProducto(Producto producto)
        {
            productos.Add(producto);
            Console.WriteLine($"[Gestor] Producto '{producto.Nombre}' agregado al inventario.");
        }

        public void EliminarProducto(int id)
        {
            var producto = productos.FirstOrDefault(p => p.Id == id);
            if (producto != null)
            {
                productos.Remove(producto);
                Console.WriteLine($"[Gestor] Producto '{producto.Nombre}' eliminado del inventario.");
            }
        }

        public void ModificarCantidad(int id, int nuevaCantidad)
        {
            var producto = productos.FirstOrDefault(p => p.Id == id);
            if (producto != null)
            {
                producto.Cantidad = nuevaCantidad;
                Console.WriteLine($"[Gestor] La cantidad de '{producto.Nombre}' ha sido actualizada a {nuevaCantidad}.");
            }
        }

        public List<Producto> ObtenerProductos()
        {
            return productos;
        }
    }
}