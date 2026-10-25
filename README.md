# Sistema de Gestión de Inventarios con Componentes Reutilizables

## Integrantes (Grupo 4)
* Guillermo Alexander Rodríguez Cortez
* Salvador Isaías Juárez Alcántara
* Luis Eduardo Guiroa Linares
* Erick Giovanni Monroy López
* José Raúl Moreno Mejía
* Vilma Melissa Alvarado Parada
* Leonel Antonio Hernández Pérez


## 1. Descripción general
El proyecto implementa un sistema de gestión de inventarios en C# con dos clientes:

*   **Aplicación de consola:** menú interactivo para administrar productos y generar reportes.
*   **Interfaz gráfica:** aplicación Avalonia con pestañas de inventario y reportes.

Ambos clientes reutilizan el mismo backend modular: `Producto`, `GestorDeInventario`, `Reportes` y `Tienda`. Comparten el código de las clases, pero cada uno crea su propia instancia de `Tienda` al ejecutarse.

## 2. Enfoque y patrón de composición
Para el desarrollo del **Tema 1** se implementó el **Patrón de Composición** en C#. El objetivo es construir clases complejas a partir de componentes más simples mediante una relación "tiene un" (has-a), en lugar de herencia (is-a).

En este sistema:

*   La clase contenedora principal es `Tienda`.
*   `Tienda` "está compuesta por" un `GestorDeInventario` y un servicio de `Reportes`.
*   La tienda delega las operaciones de inventario y la generación de reportes en sus componentes.
*   La consola y la GUI consumen `Tienda`, por lo que no necesitan instanciar directamente el gestor.

```csharp
var tienda = new Tienda(new GestorDeInventario(), new Reportes());
```

## 3. Arquitectura y componentes

| Componente | Responsabilidad | Usado por |
|---|---|---|
| `Modelos/Producto.cs` | Define los datos de un producto: `Id`, `Nombre`, `Precio` y `Cantidad`. | Consola y GUI |
| `Gestores/GestorDeInventario.cs` | Encapsula la colección `List<Producto>` y las operaciones de agregar, eliminar, modificar y actualizar. | `Tienda` |
| `Servicios/Reportes.cs` | Calcula las métricas del inventario y conserva la impresión del reporte en consola. | `Tienda` |
| `Tienda.cs` | Contenedor que compone `GestorDeInventario` y `Reportes`; expone operaciones de alto nivel. | Consola y GUI |
| `MenuInventario.cs` | Adaptador de consola: muestra el menú y delega cada acción en `Tienda`. | Consola |
| `Program.cs` | Composition root de la consola: crea los componentes y arranca el menú. | Consola |
| `Gui/ViewModels/MainWindowViewModel.cs` | Adaptador de la GUI: usa `Tienda`, prepara los datos para las vistas y coordina la persistencia. | GUI |
| `Gui/Services/InventarioStore.cs` | Lee y escribe el inventario en el `inventario.json` de la raíz. | GUI |
| `Gui/Views/MainWindow.axaml` | Define la ventana principal, la pestaña de inventario y la pestaña de reportes. | GUI |

## 4. Flujo de interacción

### Aplicación de consola
1. `Program.Main` crea `GestorDeInventario` y `Reportes`.
2. Ambos se inyectan en `Tienda`.
3. `MenuInventario` recibe la tienda y muestra el menú.
4. Cada opción del menú delega en `Tienda` y esta, a su vez, en sus componentes.

### Interfaz gráfica
1. `MainWindow` crea `MainWindowViewModel`.
2. El ViewModel instancia `Tienda` y `InventarioStore`.
3. Las acciones de la GUI se envían a `Tienda`.
4. `Tienda` delega en `GestorDeInventario` y `Reportes`.
5. Los resultados se adaptan a las colecciones y propiedades que muestran las vistas.

### Reportes
*   `Reportes.GenerarReporteInventario(...)` imprime el reporte en consola.
*   `Reportes.CalcularReporte(...)` devuelve un objeto `ReporteInventario` con totales, producto más caro/barato y productos con stock bajo.
*   La GUI reutiliza `CalcularReporte` y no duplica las reglas del reporte.

### Persistencia
*   La GUI usa `InventarioStore` para guardar y cargar datos.
*   El archivo utilizado es `inventario.json`, ubicado en la raíz del repositorio.
*   La consola trabaja en memoria; no guarda datos entre ejecuciones.

## 5. Funcionalidades

### Consola
El menú permite:

1. Agregar producto.
2. Eliminar producto.
3. Modificar cantidad.
4. Actualizar producto.
5. Generar reporte.
6. Salir.

### GUI
La pestaña **Inventario** permite:

*   Agregar productos.
*   Editar productos, incluyendo nombre, precio y cantidad.
*   Eliminar productos.
*   Recargar los registros desde `inventario.json`.
*   Validar IDs duplicados y valores numéricos.
*   Guardar automáticamente al agregar, actualizar o eliminar.

La pestaña **Reportes** muestra:

*   Total de productos registrados.
*   Unidades totales.
*   Valor total del inventario.
*   Producto más caro y más barato.
*   Productos con stock bajo (≤ 5 unidades).
*   Inventario completo.

## 6. Persistencia

La GUI localiza la raíz del repositorio mediante `InventarioStore.GetPath()` y utiliza el archivo `inventario.json` de la raíz.

Formato del archivo:

```json
[
  {
    "Id": 1,
    "Nombre": "Laptop HP",
    "Precio": 260,
    "Cantidad": 26
  }
]
```

La carga manual se realiza con el botón **Recargar registros**. Las altas, actualizaciones y eliminaciones se guardan automáticamente.

## 7. Tecnologías utilizadas

*   C# y .NET 10 (`net10.0`).
*   Avalonia 12.1.4 para la interfaz gráfica.
*   Avalonia.Controls.DataGrid para las tablas.
*   CommunityToolkit.Mvvm 8.4.2 para el patrón MVVM.
*   System.Text.Json para la persistencia.
*   `List<Producto>` como colección del inventario.

## 8. Instrucciones de ejecución

### Requisitos
*   Visual Studio con soporte para .NET 10 y la carga de trabajo **.NET desktop development**.
*   SDK de .NET 10.

### Aplicación de consola
1. Abrir el proyecto `SistemaInventarioComposicion` en Visual Studio.
2. Ejecutar la clase principal `Program.cs`.
3. El programa inicia el menú interactivo de inventario.

También se puede ejecutar desde la raíz del repositorio:

```bash
dotnet run
```

### Interfaz gráfica
Opción 1, desde la raíz del repositorio utilizando el siguiente comando en Visual Studio:

```bash
dotnet run --project Gui
```

Opción 2, mediante el ejecutable `.exe`:

El archivo ejecutable (.exe) de la aplicación se encuentra en la carpeta principal del proyecto, dentro de una carpeta dedicada llamada `EJECUTABLE`.

<img width="529" height="234" alt="image" src="https://github.com/user-attachments/assets/9855b12e-b612-44e8-a0ed-e28887eb8b1a" />

<img width="538" height="129" alt="image" src="https://github.com/user-attachments/assets/550dac0e-1e56-413d-aeb3-524a668eab8d" />

## 9. Decisiones de diseño y reutilización

*   **Composición sobre herencia:** `Tienda` agrupa y delega en `GestorDeInventario` y `Reportes`.
*   **Separación de responsabilidades:** los modelos, la lógica de inventario, los reportes y la interfaz están en componentes distintos.
*   **Reutilización real:** la GUI enlaza los archivos del backend desde su `.csproj`:

```xml
<Compile Include="../Modelos/Producto.cs" Link="Modelos/Producto.cs" />
<Compile Include="../Gestores/GestorDeInventario.cs" Link="Gestores/GestorDeInventario.cs" />
<Compile Include="../Servicios/Reportes.cs" Link="Servicios/Reportes.cs" />
<Compile Include="../Tienda.cs" Link="Tienda.cs" />
```

*   **Sin duplicación de reportes:** la GUI obtiene las métricas mediante `Tienda.GenerarReporte()`, que delega en `Reportes.CalcularReporte(...)`.
*   **Adaptadores de interfaz:** `MenuInventario` y `MainWindowViewModel` adaptan la misma lógica de negocio a consola y GUI.
*   **Persistencia separada:** `InventarioStore` se encarga del JSON y no contamina la lógica del inventario.
