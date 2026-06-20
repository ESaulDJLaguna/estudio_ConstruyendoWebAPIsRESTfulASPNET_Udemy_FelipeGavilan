using Azure.Core;
using BibliotecaAPI;
using BibliotecaAPI.Datos;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

//! Área de servicios

// .AddControllers(): Me permite habilitar la funcionalidad de los controladores en mi aplicación.
// AddJsonOptions(): Permite ignorar referencias circulares al serializar objetos a JSON. Esto es útil cuando tienes entidades que se refieren entre sí, como en el caso de Autor y Libro, donde Autor tiene una colección de Libros y Libro tiene una referencia a Autor. Sin esta configuración, al intentar serializar un objeto que tiene referencias circulares, se produciría un error de stack overflow. Al configurar el ReferenceHandler.IgnoreCycles, se le indica al serializador que ignore estas referencias circulares y evite el error.
builder.Services.AddControllers().AddJsonOptions(opciones => opciones.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

//! AddDbContext es una función especial para registrar el DbContext como un servicio
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//! Fin del área de servicios

var app = builder.Build();

//! Área de Middlewares

// Básicamente se le está diciendo que cuando reciba una petición HTTP a mi Web API, le vamos a mandar dicha petición al sistema de controladores, para que sea un controlador el que dé respuesta a dicha petición.
app.MapControllers();

app.Run();
//! Fin del área de Middlewares
