using System;
using SistemaInventarioComposicion.Modelos;
using SistemaInventarioComposicion.Gestores;
using SistemaInventarioComposicion.Servicios;

namespace SistemaInventarioComposicion
{
    // PATRÓN DE COMPOSICIÓN: La clase Tienda "está compuesta por" un Gestor y un Servicio de Reportes
    public class Tienda
    {
        private GestorDeInventario _gestor;
        private Reportes _reportes;

        public Tienda()
        {
            // Instanciamos los componentes modulares dentro de la clase constructora
            _gestor = new GestorDeInventario();
            _reportes = new Reportes();
        }

        public void OperarInventario()
        {
            Console.WriteLine("Iniciando Sistema de Inventario...\n");

            _gestor.AgregarProducto(new Producto(1, "Laptop Dell", 850.00m, 10));
            _gestor.AgregarProducto(new Producto(2, "Monitor Samsung 24\"", 150.00m, 25));
            _gestor.AgregarProducto(new Producto(3, "Teclado Mecánico", 45.00m, 50));

            _reportes.GenerarReporteInventario(_gestor.ObtenerProductos());

            _gestor.ModificarCantidad(2, 15);
            _gestor.EliminarProducto(3);

            _reportes.GenerarReporteInventario(_gestor.ObtenerProductos());
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Tienda miTienda = new Tienda();
            miTienda.OperarInventario();
        }
    }
}