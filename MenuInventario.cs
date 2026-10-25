using System;
using System.Globalization;
using System.IO;
using System.Linq;
using SistemaInventarioComposicion.Modelos;

namespace SistemaInventarioComposicion
{
    // MenuInventario adapta las operaciones de Tienda a una interfaz de consola modular.
    public class MenuInventario
    {
        private readonly Tienda _tienda;

        public MenuInventario(Tienda tienda)
        {
            _tienda = tienda;
        }

        // Iniciar contiene el ciclo principal y delega cada operación en la tienda.
        public void Iniciar()
        {
            bool salir = false;
            while (!salir)
            {
                MostrarMenu();
                string opcion = LeerTexto("Selecciona una opción: ");
                switch (opcion)
                {
                    case "1":
                        Agregar();
                        break;
                    case "2":
                        Eliminar();
                        break;
                    case "3":
                        ModificarCantidad();
                        break;
                    case "4":
                        Actualizar();
                        break;
                    case "5":
                        _tienda.ImprimirReporte();
                        break;
                    case "6":
                        salir = true;
                        break;
                    default:
                        Console.WriteLine("Opción no válida.");
                        break;
                }

                if (!salir)
                {
                    Console.WriteLine("\nPresiona Enter para continuar...");
                    Console.ReadLine();
                }
            }

            Console.WriteLine("Saliendo del sistema...");
        }

        private static void MostrarMenu()
        {
            LimpiarPantalla();
            Console.WriteLine("=== SISTEMA DE INVENTARIO ===");
            Console.WriteLine("1. Agregar producto");
            Console.WriteLine("2. Eliminar producto");
            Console.WriteLine("3. Modificar cantidad");
            Console.WriteLine("4. Actualizar producto");
            Console.WriteLine("5. Generar reporte");
            Console.WriteLine("6. Salir");
            Console.WriteLine();
        }

        private void Agregar()
        {
            int id = LeerEntero("ID: ", 1);
            if (_tienda.ObtenerProductos().Any(p => p.Id == id))
            {
                Console.WriteLine("Ya existe un producto con ese ID.");
                return;
            }

            string nombre = LeerTexto("Nombre: ");
            decimal precio = LeerDecimal("Precio: ");
            int cantidad = LeerEntero("Cantidad: ");

            _tienda.AgregarProducto(new Producto(id, nombre, precio, cantidad));
            Console.WriteLine("Producto agregado correctamente.");
        }

        private void Eliminar()
        {
            MostrarProductos();
            int id = LeerEntero("ID a eliminar: ", 1);
            if (!_tienda.ObtenerProductos().Any(p => p.Id == id))
            {
                Console.WriteLine("No existe un producto con ese ID.");
                return;
            }

            _tienda.EliminarProducto(id);
            Console.WriteLine("Producto eliminado correctamente.");
        }

        private void ModificarCantidad()
        {
            MostrarProductos();
            int id = LeerEntero("ID: ", 1);
            if (!_tienda.ObtenerProductos().Any(p => p.Id == id))
            {
                Console.WriteLine("No existe un producto con ese ID.");
                return;
            }

            int cantidad = LeerEntero("Nueva cantidad: ");
            _tienda.ModificarCantidad(id, cantidad);
            Console.WriteLine("Cantidad actualizada correctamente.");
        }

        private void Actualizar()
        {
            MostrarProductos();
            int id = LeerEntero("ID a actualizar: ", 1);
            Producto? producto = _tienda.ObtenerProductos().FirstOrDefault(p => p.Id == id);
            if (producto == null)
            {
                Console.WriteLine("No existe un producto con ese ID.");
                return;
            }

            string nombre = LeerTexto($"Nombre [{producto.Nombre}]: ", producto.Nombre);
            decimal precio = LeerDecimal($"Precio [{producto.Precio}]: ", producto.Precio);
            int cantidad = LeerEntero($"Cantidad [{producto.Cantidad}]: ", 0, producto.Cantidad);

            _tienda.ActualizarProducto(id, nombre, precio, cantidad);
            Console.WriteLine("Producto actualizado correctamente.");
        }

        private void MostrarProductos()
        {
            var productos = _tienda.ObtenerProductos();
            if (productos.Count == 0)
            {
                Console.WriteLine("El inventario está vacío.");
                return;
            }

            Console.WriteLine("\nProductos actuales:");
            foreach (var producto in productos.OrderBy(p => p.Id))
            {
                Console.WriteLine($"ID: {producto.Id} | {producto.Nombre} | Precio: {producto.Precio:C} | Stock: {producto.Cantidad}");
            }
            Console.WriteLine();
        }

        private static void LimpiarPantalla()
        {
            try
            {
                Console.Clear();
            }
            catch (IOException)
            {
            }
        }

        private static string LeerTexto(string mensaje, string? valorPredeterminado = null)
        {
            while (true)
            {
                Console.Write(mensaje);
                string? texto = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(texto))
                {
                    if (!string.IsNullOrWhiteSpace(valorPredeterminado))
                    {
                        return valorPredeterminado!;
                    }

                    Console.WriteLine("El valor es obligatorio.");
                    continue;
                }

                return texto.Trim();
            }
        }

        private static int LeerEntero(string mensaje, int minimo = 0, int? valorPredeterminado = null)
        {
            while (true)
            {
                Console.Write(mensaje);
                string? texto = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(texto) && valorPredeterminado.HasValue)
                {
                    return valorPredeterminado.Value;
                }

                if (int.TryParse(texto, out int valor) && valor >= minimo)
                {
                    return valor;
                }

                Console.WriteLine($"Ingresa un número entero mayor o igual a {minimo}.");
            }
        }

        private static decimal LeerDecimal(string mensaje, decimal? valorPredeterminado = null)
        {
            while (true)
            {
                Console.Write(mensaje);
                string? texto = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(texto) && valorPredeterminado.HasValue)
                {
                    return valorPredeterminado.Value;
                }

                if (decimal.TryParse(texto, NumberStyles.Number, CultureInfo.CurrentCulture, out decimal valor) ||
                    decimal.TryParse(texto, NumberStyles.Number, CultureInfo.InvariantCulture, out valor))
                {
                    if (valor >= 0)
                    {
                        return valor;
                    }
                }

                Console.WriteLine("Ingresa un número mayor o igual a 0.");
            }
        }
    }
}
