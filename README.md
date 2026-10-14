# Documentación del Proyecto: Sistema de Gestión de Inventarios Patrón de Composición

## Integrantes (Grupo 4)
* Guillermo Alexander Rodríguez Cortez[cite: 62]
* Salvador Isaías Juárez Alcántara[cite: 62]
* Luis Eduardo Guiroa Linares[cite: 62]
* Erick Giovanni Monroy López[cite: 62]
* José Raúl Moreno Mejía[cite: 62]
* Vilma Melissa Alvarado Parada[cite: 62]
* Leonel Antonio Hernández Pérez[cite: 62]

## 1. Enfoque Utilizado
Para el desarrollo del **Tema 1**, se implementó el **Patrón de Composición** en C#. El objetivo principal es construir clases complejas a partir de otras más simples, estableciendo una relación de tipo "tiene un" has-a en lugar de herencia is-a.

En este sistema:
*   La clase contenedora principal es `Tienda`.
*   La clase `Tienda` "está compuesta por" un `GestorDeInventario` y un servicio de `Reportes`.
*   Esto permite delegar responsabilidades: la tienda no maneja directamente la lista de productos ni imprime en consola; delega esas tareas a sus componentes.

## 2. Decisiones Tomadas y Modularidad
Para cumplir con los criterios de "Reutilización y Modularidad" y "Calidad del Código" de la rúbrica, el proyecto se estructuró en carpetas lógicas:

*   **Carpeta `Modelos`:** Contiene `Producto.cs`. Es una clase de datos pura que define las propiedades de los artículos en venta.
*   **Carpeta `Gestores`:** Contiene `GestorDeInventario.cs`. Esta clase encapsula la lógica de negocio usando una colección `List<Producto>` para agregar, eliminar y modificar elementos, manteniendo la lista protegida del exterior.
*   **Carpeta `Servicios`:** Contiene `Reportes.cs`. Un componente dedicado exclusivamente a la visualización de datos en consola, separando la lógica de presentación de la lógica de negocio.

Esta separación facilita el mantenimiento. Si en el futuro se desea cambiar la forma en que se muestran los reportes por ejemplo, exportar a PDF en lugar de imprimir en consola, solo se debe modificar la clase `Reportes` sin afectar el resto del sistema.

## 3. Instrucciones de Ejecución
1. Asegúrese de tener instalado el SDK de .NET versión 8.0 o superior.
2. Clone el repositorio o descargue los archivos del proyecto.
3. Abra una terminal símbolo del sistema o PowerShell y navegue hasta el directorio raíz del proyecto donde se encuentra el archivo `.csproj`.
4. Ejecute el siguiente comando para compilar y probar la aplicación:
   `dotnet run`
5. El sistema mostrará en consola la simulación de las operaciones de inventario y la generación de reportes.
