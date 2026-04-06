# JiCpu
# JiCpu — Actualización 1.0.3

Versión: 1.0.1
Licencia: MIT (libre para usar, modificar y distribuir)

Resumen
-------
Mejoras de interfaz y correcciones de servicios WMI. Se añadieron controles y se refactorizó la carga de módulos para ofrecer una apariencia consistente en las pestañas CPU, GPU, RAM y Mainboard.

Principales cambios en 1.0.3
---------------------------
 
- Visual y UX:
-  Mejor disposición y estilos en las pestañas.
   
- Código y correcciones:
  - `Servicios/RamService.cs` corregido para evitar errores de conversión y se mantiene bajo el namespace `Services`.
  - `Servicios/MainboardService.cs` mejorado para mapear valores usando `Win32_BaseBoard`, `Win32_SystemSlot`, `Win32_Processor` y `Win32_MotherboardDevice`.
  - `Modelos/Mainboard.cs` cambiado a `public` para evitar incoherencias de accesibilidad.
  - `Form1` y su designer actualizados para mostrar información de la placa base.

- Compatibilidad:
  - Proyecto orientado a .NET 10 y C# 14.

Archivos añadidos/modificados
----------------------------
- Añadidos:
  - Mainboard `JiCpu/Servicios/MainboardService.cs`
  - Graphics `JiCpu/modelos/Graphics.cs`
  - GraphicsServices `JiCpu/Servicios/GraphicsService.cs`
 
- Modificados:
  - `JiCpu/Servicios/RamService.cs` — namespace `Services` y cambios para evitar errores de conversión.


Notas de diseño y extensión
--------------------------
- Para mostrar detalles extendidos al hacer clic en una tarjeta, se puede añadir un `Panel` lateral en `Form1` y mostrar/animar
- se modifico el `Form1` para agregar los funciones de Graphics

Correcciones y consideraciones
-----------------------------
- Se corrigieron errores previos relacionados con `RamService` y conversiones WMI.


Cómo compilar y ejecutar
------------------------
- Abrir la solución en Visual Studio (o usar `dotnet build` desde la raíz del proyecto).
- Proyecto objetivo: `.NET 10`.
- Si estás depurando con hot-reload activado, puedes aplicar cambios en caliente; de lo contrario, reconstruye y reinicia la aplicación.


Notas
-----
- WMI requiere ejecutar en Windows y puede necesitar privilegios para acceder a ciertos datos.
- Si desea, se puede añadir una vista detallada lateral que muestre más campos al seleccionar una tarjeta.
  
Licencia
--------
Este proyecto se publica bajo la licencia MIT. Puedes copiar, modificar y redistribuir libremente este código.

Contacto y siguientes pasos
--------------------------
- Panel lateral con detalles ampliados al hacer click en una tarjeta.
- Animaciones adicionales (entrada/salida de panel lateral, hover con sombra, etc.).
