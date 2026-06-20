namespace BibliotecaAPI
{
    public class LogueaPeticionMiddleware
    {
        private readonly RequestDelegate next;

        // El constructor de la clase recibe un parámetro de tipo RequestDelegate, que representa el siguiente middleware en la cadena de middlewares. Esto permite que el middleware actual pueda llamar al siguiente middleware después de realizar su tarea.
        public LogueaPeticionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        // Para que esta clase sea un middleware, es necesario que tenga un método llamado Invoke o InvokeAsync, el cual recibe un parámetro de tipo HttpContext. Este método es el encargado de procesar la petición HTTP y generar la respuesta HTTP.
        public async Task InvokeAsync(HttpContext httpcontext)
        {
            // Viene la petición
            var logger = httpcontext.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogInformation($"Petición: {httpcontext.Request.Method} {httpcontext.Request.Path}");

            // Es necesario pasar el contexto al Invoke del RequestDelegate
            await next.Invoke(httpcontext);

            // Se va la respuesta
            logger.LogInformation($"Respuesta: {httpcontext.Response.StatusCode}");
        }
    }

    // Para simplificar cómo utilizamos este middleware (LogueaPeticionMiddleware), el estándar en ASP.NET Core es que creemos una clase estática que exponga un método de extensión para IApplicationBuilder, lo que nos permitirá usar este middleware de una forma más sencilla y limpia en el Program.cs. Se crea en el mismo archivo porque básicament es una clase de ayuda para usar el middleware.
    public static class LogueaPeticionMiddlewareExtensions
    {
        // Se utiliza el this en el parámetro IApplicationBuilder para indicar que este método es un método de extensión para la interfaz IApplicationBuilder, lo que nos permitirá usar este método como si fuera un método nativo de IApplicationBuilder. No es obligatorio utilizar Use en el nombre del método, pero es un estándar, así que se recomienda respetarlo
        public static IApplicationBuilder UseLogueaPeticion(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LogueaPeticionMiddleware>();
        }
    }
}
