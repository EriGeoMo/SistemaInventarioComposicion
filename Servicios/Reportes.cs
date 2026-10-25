using System;
using System.Collections.Generic;
using System.Linq;
using SistemaInventarioComposicion.Modelos;

namespace SistemaInventarioComposicion.Servicios
{
    public class Reportes
    {
        // GenerarReporteInventario conserva la salida de consola usada por la aplicación original.
        public void GenerarReporteInventario(List<Producto> productos)
        {
            Console.WriteLine("\n--- REPORTE DE INVENTARIO ACTUAL ---");
            if (productos.Count == 0)
            {
                Console.WriteLine("El inventario está vacío.");
            }
            else
            {
                foreach (var p in productos)
                {
                    Console.WriteLine($"ID: {p.Id} | Producto: {p.Nombre} | Precio: ${p.Precio} | Stock: {p.Cantidad}");
                }
            }
            Console.WriteLine("------------------------------------\n");
        }

        // CalcularReporte concentra las métricas que la consola y la GUI reutilizan.
        public ReporteInventario CalcularReporte(IReadOnlyList<Producto> productos)
        {
            var reporte = new ReporteInventario
            {
                TotalProductos = productos.Count
            };

            if (productos.Count == 0)
            {
                return reporte;
            }

            reporte.TotalItems = productos.Sum(p => p.Cantidad);
            reporte.ValorTotal = productos.Sum(p => p.Precio * p.Cantidad);

            var masCaro = productos.OrderByDescending(p => p.Precio).First();
            var masBarato = productos.OrderBy(p => p.Precio).First();
            reporte.PrecioMasAlto = masCaro.Precio;
            reporte.PrecioMasBajo = masBarato.Precio;
            reporte.ProductoMasCaro = masCaro;
            reporte.ProductoMasBarato = masBarato;
            reporte.ProductosStockBajo = productos
                .Where(p => p.Cantidad <= 5)
                .OrderBy(p => p.Cantidad)
                .ToList();

            return reporte;
        }
    }

    // ReporteInventario transporta los resultados del reporte sin acoplarlos a una interfaz.
    public class ReporteInventario
    {
        public int TotalProductos { get; set; }
        public int TotalItems { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal PrecioMasAlto { get; set; }
        public decimal PrecioMasBajo { get; set; }
        public Producto? ProductoMasCaro { get; set; }
        public Producto? ProductoMasBarato { get; set; }
        public List<Producto> ProductosStockBajo { get; set; } = new();
    }
}