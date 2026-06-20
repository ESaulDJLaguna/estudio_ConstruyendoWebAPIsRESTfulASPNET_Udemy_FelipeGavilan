using Azure.Core;
using BibliotecaAPI;
using BibliotecaAPI.Datos;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

//! Área de servicios


builder.Services.AddTransient<ServicioTransient>();
builder.Services.AddScoped<ServicioScoped>();
builder.Services.AddSingleton<ServicioSingleton>();

builder.Services.AddSingleton<IRepositorioValores, RepositorioValoresOracle>();

// .AddControllers(): Me permite habilitar la funcionalidad de los controladores en mi aplicación.
// AddJsonOptions(): Permite ignorar referencias circulares al serializar objetos a JSON. Esto es útil cuando tienes entidades que se refieren entre sí, como en el caso de Autor y Libro, donde Autor tiene una colección de Libros y Libro tiene una referencia a Autor. Sin esta configuración, al intentar serializar un objeto que tiene referencias circulares, se produciría un error de stack overflow. Al configurar el ReferenceHandler.IgnoreCycles, se le indica al serializador que ignore estas referencias circulares y evite el error.
builder.Services.AddControllers().AddJsonOptions(opciones => opciones.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

//! AddDbContext es una función especial para registrar el DbContext como un servicio
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//! Fin del área de servicios

var app = builder.Build();

//! Área de Middlewares

// El método 'Use' se usa para crear un middleware. Next básicamente nos permitirá invocar al siguiente middlweare. Este sería la forma de crear un middleware directamente en el Program.cs, sin necesidad de crear una clase aparte para el middleware.
/*
app.Use(async (httpcontext, next) =>
{
    // Viene la petición
    var logger = httpcontext.RequestServices.GetRequiredService<ILogger<Program>>();
    logger.LogInformation($"Petición: {httpcontext.Request.Method} {httpcontext.Request.Path}");

    // next.Invoke() es lo que permite que el siguiente middleware en la cadena se ejecute. Si no se llama a next.Invoke(), el flujo de la aplicación se detendrá en este middleware y no se ejecutarán los siguientes middlewares.
    await next.Invoke();

    // Se va la respuesta
    logger.LogInformation($"Respuesta: {httpcontext.Response.StatusCode}");
});
*/
//! Este es exactamente el mismo middleware que el anterior, pero con una sintaxis más simplificada gracias a que se creó una clase propia para el middleware, lo que permite que el código sea más limpio y fácil de mantener.
//app.UseMiddleware<LogueaPeticionMiddleware>();

// Esta es la forma más simplificada de usar el middleware, gracias a que se creó una clase de extensión (LogueaPeticionMiddlewareExtensions) para el middleware, lo que permite que el código sea aún más limpio y fácil de mantener.
app.UseLogueaPeticion();

/*
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/bloqueado")
    {
        context.Response.StatusCode = 403;
        // Se genera un corto circuito, lo que significa que no se llamará al siguiente middleware y la respuesta se enviará inmediatamente con el código de estado 403.
        await context.Response.WriteAsync("Acceso denegado");
    }
    else
    {
        await next.Invoke();
    }
});
*/
app.UseBloqueaPeticion();

// Básicamente se le está diciendo que cuando reciba una petición HTTP a mi Web API, le vamos a mandar dicha petición al sistema de controladores, para que sea un controlador el que dé respuesta a dicha petición.
app.MapControllers();

app.Run();
//! Fin del área de Middlewares
