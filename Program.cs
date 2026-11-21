using BibliotecaSemana4.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true)
    .AddEnvironmentVariables();

var connection = builder.Configuration.GetConnectionString("Biblioteca")
    ?? throw new InvalidOperationException("Falta configurar la conexión Biblioteca.");

builder.Services.AddDbContext<BibliotecaContext>(options =>
    options.UseMySql(connection, new MariaDbServerVersion(new Version(10, 4, 32))));
builder.Services.AddControllersWithViews(options =>
{
    var mensajes = options.ModelBindingMessageProvider;
    mensajes.SetMissingBindRequiredValueAccessor(campo => $"El campo {campo} es obligatorio.");
    mensajes.SetValueMustNotBeNullAccessor(_ => "Este campo es obligatorio.");
    mensajes.SetAttemptedValueIsInvalidAccessor((_, campo) => $"El valor de {campo} no es válido.");
    mensajes.SetValueMustBeANumberAccessor(campo => $"El campo {campo} debe ser numérico.");
});

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseRouting();
app.MapStaticAssets();
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();
app.Run();
