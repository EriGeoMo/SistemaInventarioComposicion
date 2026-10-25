# Documentación del Proyecto: Sistema de Gestión de Inventarios Patrón de Composición

## Integrantes (Grupo 4)
* Guillermo Alexander Rodríguez Cortez
* Salvador Isaías Juárez Alcántara
* Luis Eduardo Guiroa Linares
* Erick Giovanni Monroy López
* José Raúl Moreno Mejía
* Vilma Melissa Alvarado Parada
* Leonel Antonio Hernández Pérez

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
1. Asegúrese de tener instalado el SDK de .NET.
2. Clone el repositorio o descargue los archivos del proyecto, asegurándose de estar en la rama que contiene la interfaz (ej. `JA22005` o `Gui`).
3. Abra una terminal Símbolo del sistema, PowerShell o la terminal de VS Code y navegue hasta el directorio raíz del proyecto.
4. Para ejecutar el sistema con su interfaz gráfica, utilice el siguiente comando:
   `dotnet run --project Gui`
5. El sistema abrirá la ventana de "Gestión de Inventario" donde podrá realizar las operaciones CRUD, visualizar el listado y comprobar el cálculo automático del stock y valor total a través de nuestros componentes delegados.
