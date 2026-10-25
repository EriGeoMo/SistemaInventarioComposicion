using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using SistemaInventarioComposicion.Modelos;

namespace SistemaInventarioComposicion.Gui.Services;

public class InventarioStore
{
    public string GetPath()
    {
        // Buscar la raíz del repositorio (donde se encuentra el .csproj principal)
        var dir = AppContext.BaseDirectory;
        var current = new DirectoryInfo(dir);
        for (int i = 0; i < 10; i++)
        {
            if (current == null) break;
            var csproj = Path.Combine(current.FullName, "SistemaInventarioComposicion.csproj");
            if (File.Exists(csproj))
            {
                return Path.Combine(current.FullName, "inventario.json");
            }
            current = current.Parent;
        }
        // Fallback: directorio actual
        var cwd = Directory.GetCurrentDirectory();
        if (File.Exists(Path.Combine(cwd, "SistemaInventarioComposicion.csproj")))
            return Path.Combine(cwd, "inventario.json");
        // Último recurso: AppContext.BaseDirectory
        return Path.Combine(AppContext.BaseDirectory, "inventario.json");
    }

    public List<Producto> Cargar()
    {
        var path = GetPath();
        if (!File.Exists(path)) return new List<Producto>();
        var json = File.ReadAllText(path);
        var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var data = JsonSerializer.Deserialize<List<ProductoDto>>(json, opciones);
        if (data == null) return new List<Producto>();
        var lista = new List<Producto>();
        foreach (var d in data)
        {
            lista.Add(new Producto(d.Id, d.Nombre ?? string.Empty, d.Precio, d.Cantidad));
        }
        return lista;
    }

    // InventarioStore adapta la colección del dominio al JSON persistido en la raíz.
    public void Guardar(IReadOnlyList<Producto> productos)
    {
        var path = GetPath();
        var lista = new List<ProductoDto>();
        foreach (var p in productos)
        {
            lista.Add(new ProductoDto { Id = p.Id, Nombre = p.Nombre, Precio = p.Precio, Cantidad = p.Cantidad });
        }
        var opciones = new JsonSerializerOptions { WriteIndented = true };
        var json = JsonSerializer.Serialize(lista, opciones);
        File.WriteAllText(path, json);
    }

    private class ProductoDto
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
    }
}
