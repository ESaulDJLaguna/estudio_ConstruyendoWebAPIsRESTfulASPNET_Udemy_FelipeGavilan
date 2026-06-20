using BibliotecaAPI.Datos;
using BibliotecaAPI.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPI.Controllers
{
    [ApiController] // Es un atributo el cuál nos permite indicar que este controlador se refiere a un controlador de una Web API. Esto nos va a permitir, por ejemplo, automáticamente realizar validaciones sobre la data que envía el usuario
    [Route("api/autores")]
    public class AutoresController : ControllerBase // ControllerBase es una clase base que tiene funcionalidad auxiliar que permite trabajar de una manera sencilla con un web API, por ejemplo, tiene métodos auxiliares para devolver respuestas HTTP.
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AutoresController> logger;

        public AutoresController(ApplicationDbContext context, ILogger<AutoresController> logger)
        {
            _context = context;
            this.logger = logger;
        }

        // Es posible definir que una acción responda a varias rutas. En este ejemplo utilizamos [HttpGet("/listado-de-autores")] y [HttpGet]
        [HttpGet("/listado-de-autores")] // Es posible ignorar la ruta (api/autores) y darle una ruta personalizada a una acción. Para eso el nombre en la plantilla debe iniciar por /. Por lo tanto, la ruta para esta acción sería: https://midominio.com/listado-de-autores
        [HttpGet]
        public async Task<IEnumerable<Autor>> Get()
        {
            logger.LogTrace("Obteniendo el listado de autores - Trace");
            logger.LogDebug("Obteniendo el listado de autores - Debug");
            logger.LogInformation("Obteniendo el listado de autores - Information");
            logger.LogWarning("Obteniendo el listado de autores - Warning");
            logger.LogError("Obteniendo el listado de autores - Error");
            logger.LogCritical("Obteniendo el listado de autores - Critical");
            return await _context.Autores.ToListAsync();
        }

        [HttpGet("primero")] // api/autores/primero
        public async Task<Autor> GetPrimerAutor() => await _context.Autores.FirstAsync();

        // {id:int} es una plantilla de ruta, lo que significa que el valor que se envíe en esa parte de la URL va a ser interpretado como un entero y va a ser asignado al parámetro 'id' del método Get(int id). Hay que tener en cuenta que no lleva espacio entre id e int, sino que es id:int
        [HttpGet("{id:int}")] // api/autores/id?incluirLibros=true|false
        // ActionResult: es un tipo de retorno que representa el resultado de una acción en un controlador. Permite devolver diferentes tipos de respuestas HTTP, como Ok(), NotFound(), BadRequest(), etc., dependiendo del resultado de la operación realizada en el método. Permite devolver cualuier tipo de código de estatus HTTP (404, 500, etc.) o un Autor.
        //public async Task<ActionResult<Autor>> Get([FromRoute] int id, [FromQuery] bool incluirLibros) // incluirLibros se envía a través de un query string
        public async Task<ActionResult<Autor>> Get([FromRoute] int id, [FromHeader] bool incluirLibros)
        {
            Autor? autor = await _context.Autores.Include(x => x.Libros).FirstOrDefaultAsync(x => x.Id == id);

            if (autor is null) return NotFound();

            return autor;
        }

        /*
         * Los parámetros de ruta nos permiten definir variables a nivel de la URL. Esto nos permite mayor libertad a la hora de configurar las rutas de nuestras acciones.
         * El tipo de parámetro string no existe para los tipos de variable de ruta, para eso se utiliza 'alpha'. Si no se le coloca un tipo de dato (al cual se le conoce como restricción de variable de ruta), podremos utilizar un string cualquiera (es decir, se aceptan símbolos, números, letras, etc.)
         */
        [HttpGet("{nombre:alpha}")]
        public async Task<IEnumerable<Autor>> Get(string nombre) => await _context.Autores.Where(x => x.Nombre.Contains(nombre)).ToListAsync();

        // Una acción puede tener más de un parámetro de ruta y también se puede indicar que los últimos son opcionales (utilizando el símbolo ?), en caso de ser necesario, se les puede dar un valor por defecto (para ello el símbolo ? en "string? parametro2" ya no es necesario)
        //[HttpGet("{parametro1}/{parametro2}")] // api/autores/valor1/valor2 => parametro2 es obligatorio
        //[HttpGet("{parametro1}/{parametro2?}")] // api/autores/valor1/valor2 => parametro2 es opcional, si no se envía en la URL el valor de parametro2 es null
        //public ActionResult Get(string parametro1, string? parametro2) // Parámetro opcional (si no se envia en la URL el valor es null)
        //public ActionResult Get(string parametro1, string parametro2 = "Valor por defecto") // Parámetro con valor por defecto
        //{
        //return Ok(new { parametro1, parametro2 });
        //}


        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Autor autor)
        {
            //! Esto NO está agregando en la tabla autores el autor, sino que lo que está haciendo es marcando el objeto 'autor' para que sea agregado en el futuro cuando guardemos los cambios
            _context.Add(autor);
            //! await básicamente permite lanzar esta operación (que es mandar el query de insert) y no quedar bloqueados esperando la respuesta a esa operación, sino que el WebAPI es libre de seguir haciendo otras cosas en lo que dicha respuesta llega.
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("{id:int}")] // api/autores/id
        public async Task<ActionResult> Put(int id, Autor autor)
        {
            if (id != autor.Id) return BadRequest("Los ids deben de coincidir");

            _context.Update(autor);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            // Se va a la base de datos y con Where se buscan todos los autores con el id indicado. ExecuteDeleteAsync realiza el borrado de todos esos registros y devuelve la cantidad de registros borrados
            int registrosBorrados = await _context.Autores.Where(x => x.Id == id).ExecuteDeleteAsync();

            if (registrosBorrados == 0) return NotFound();

            return Ok();
        }
    }
}
