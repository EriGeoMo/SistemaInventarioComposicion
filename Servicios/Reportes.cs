using System;
using System.Collections.Generic;
using SistemaInventarioComposicion.Modelos;

namespace SistemaInventarioComposicion.Servicios
{
    public class Reportes
    {
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
    }
}