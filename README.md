# Sistema de gestión de biblioteca

Práctica de la Semana 4, Tarea 1 de Aplicaciones Web. Sexto nivel de Ingeniería en Software, UNIANDES. Docente Ing. Luis Llerena.

El sistema permite registrar, listar, consultar, editar y eliminar autores, categorías, libros, lectores y préstamos. Para registrar un préstamo se necesita un libro y un lector. La fecha de devolución puede quedar pendiente y completarse después desde Editar.

## Cómo ejecutar el proyecto

1. Inicia **MySQL** en el panel de XAMPP. La práctica se probó con el SDK .NET 9.0.311 y MariaDB 10.4.32.
2. Abre PowerShell en la carpeta `BibliotecaSemana4` y ejecuta estos comandos.

```powershell
dotnet restore
dotnet build
dotnet tool restore
dotnet tool run dotnet-ef database update
dotnet run --launch-profile http
```

3. Abre [http://localhost:5184](http://localhost:5184).

La conexión predeterminada usa `127.0.0.1:3306`, la base `biblioteca_semana4` y el usuario local `root` de XAMPP. Si tu instalación usa contraseña u otro usuario, crea `appsettings.Local.json` con el siguiente formato. Git ignora ese archivo.

```json
{
  "ConnectionStrings": {
    "Biblioteca": "Server=127.0.0.1;Port=3306;Database=biblioteca_semana4;User=TU_USUARIO;Password=TU_CONTRASEÑA;"
  }
}
```

## Entidades y relaciones

| Tabla | Cinco columnas |
| --- | --- |
| Autores | Id, Nombre, Apellido, Nacionalidad, FechaNacimiento |
| Categorias | Id, Nombre, Descripcion, Codigo, Activa |
| Libros | Id, Titulo, AutorId, CategoriaId, AnioPublicacion |
| Lectores | Id, Nombre, Apellido, Correo, Telefono |
| Prestamos | Id, LibroId, LectorId, FechaPrestamo, FechaDevolucion |

Un autor puede tener varios libros y una categoría puede agrupar varios libros. Un libro puede aparecer en varios préstamos y un lector puede registrar varios préstamos. `BibliotecaContext` configura estas cuatro relaciones de uno a muchos. Las claves foráneas impiden eliminar un registro mientras tenga registros que dependan de él.

## Cómo se utilizó scaffolding

Primero definí los modelos y `BibliotecaContext`. Después ejecuté `dotnet-aspnet-codegenerator` 9.0.0 para generar cinco controladores MVC y las vistas `Index`, `Details`, `Create`, `Edit` y `Delete` para cada entidad. El siguiente bloque registra las invocaciones de la generación inicial.

```powershell
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 9.0.13
dotnet build --no-restore
$entidades = [ordered]@{ Autor = 'Autores'; Categoria = 'Categorias'; Libro = 'Libros'; Lector = 'Lectores'; Prestamo = 'Prestamos' }
foreach ($modelo in $entidades.Keys) {
    $controlador = $entidades[$modelo] + 'Controller'
    dotnet tool run dotnet-aspnet-codegenerator controller -name $controlador -m "BibliotecaSemana4.Models.$modelo" -dc BibliotecaSemana4.Data.BibliotecaContext -outDir Controllers -udl -scripts --no-build
}
dotnet remove package Microsoft.EntityFrameworkCore.SqlServer
```

El generador pidió esa referencia temporal para validar sus dependencias. Durante la generación detectó el contexto de Pomelo y la aplicación usa MariaDB. Adapté las vistas generadas al español y los desplegables muestran nombres en lugar de identificadores.

En los listados, SweetAlert2 pide confirmación antes de enviar el formulario POST con el token antifalsificación. El servidor muestra el resultado después de guardar. En Préstamos, el botón **Actualizar listado** usa jQuery para llamar a `Todos`. La vista parcial Razor genera las filas, codifica los datos y crea los enlaces de cada registro.

Referencias de clase [Factura](https://github.com/lleroc/Sexto_om27/tree/f7e80b2241a0cd1471d1cd98bff473ce3b3ad0d6/Factura) y [Provincias](https://github.com/lleroc/Sexto_om27/tree/f7e80b2241a0cd1471d1cd98bff473ce3b3ad0d6/Provincias), en especial los ejemplos de Cantón y Habitante.

