var builder = WebApplication.CreateBuilder(args);

//! Se utiliza Configuration para acceder a los proveedores de conexión
var cadenaDeConexion = builder.Configuration.GetValue<string>("CadenaDeConexion");

//! Inicio del área de servicios

//! Fin del área de servicios

var app = builder.Build();

/* Inicio del área de los middlewares
 * - Un middleware se refiere a un conjunto de procesos que se van a correr cada vez que recibamos una petición HTTP en nuestra aplicación.
*/

app.MapGet("/", () => cadenaDeConexion);

//! Fin del área de los middlewares

app.Run();
