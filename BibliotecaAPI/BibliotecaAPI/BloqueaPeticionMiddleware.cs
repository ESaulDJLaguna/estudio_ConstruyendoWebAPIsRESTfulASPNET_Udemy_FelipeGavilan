namespace BibliotecaAPI
{
    public class BloqueaPeticionMiddleware
    {
        private readonly RequestDelegate next;

        public BloqueaPeticionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path == "/bloqueado")
            {
                context.Response.StatusCode = 403;
                // Se genera un corto circuito, lo que significa que no se llamará al siguiente middleware y la respuesta se enviará inmediatamente con el código de estado 403.
                await context.Response.WriteAsync("Acceso denegado");
            }
            else
            {
                await next.Invoke(context);
            }
        }
    }

    public static class BloqueaPeticionMiddlewareExtensions
    {
        public static IApplicationBuilder UseBloqueaPeticion(this IApplicationBuilder builder) => builder.UseMiddleware<BloqueaPeticionMiddleware>();
    }
}
