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

        public AutoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<Autor>> Get()
        {
            return await _context.Autores.ToListAsync();
        }

        // {id:int} es una plantilla de ruta, lo que significa que el valor que se envíe en esa parte de la URL va a ser interpretado como un entero y va a ser asignado al parámetro 'id' del método Get(int id). Hay que tener en cuenta que no lleva espacio entre id e int, sino que es id:int
        [HttpGet("{id:int}")] // api/autores/id
        // ActionResult: es un tipo de retorno que representa el resultado de una acción en un controlador. Permite devolver diferentes tipos de respuestas HTTP, como Ok(), NotFound(), BadRequest(), etc., dependiendo del resultado de la operación realizada en el método. Permite devolver cualuier tipo de código de estatus HTTP (404, 500, etc.) o un Autor.
        //public async Task<ActionResult<Autor>> Get([FromRoute] int id, [FromQuery] bool incluirLibros) // incluirLibros se envía a través de un query string
        public async Task<ActionResult<Autor>> Get([FromRoute] int id)
        {
            Autor? autor = await _context.Autores.Include(x => x.Libros).FirstOrDefaultAsync(x => x.Id == id);

            if (autor is null) return NotFound();

            return autor;
        }

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
