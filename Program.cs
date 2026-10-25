using SistemaInventarioComposicion.Gestores;
using SistemaInventarioComposicion.Servicios;

namespace SistemaInventarioComposicion
{
    class Program
    {
        static void Main(string[] args)
        {
            // El punto de entrada compone los módulos y entrega la tienda al menú de consola.
            var tienda = new Tienda(new GestorDeInventario(), new Reportes());
            var menu = new MenuInventario(tienda);
            menu.Iniciar();
        }
    }
}
