using BibliotecaAPI.Datos;
using BibliotecaAPI.Entidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPI.Controllers
{
    [ApiController]
    [Route("api/libros")]
    public class LibrosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LibrosController(ApplicationDbContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<Libro>> Get() => await _context.Libros.ToListAsync();

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Libro>> Get(int id)
        {
            Libro? libro = await _context.Libros
                //! Se consulta la tabla de Autores a partir de los libros
                //! Al incluir las entidades relacionadas, esto devolverá un error de referencia circular (porque Autor tiene una propiedad de navegación hacia Libro y Libro hacia Autor y así hasta que se llena el stack), para evitarlo se puede configurar el JSON Serializer para que ignore las referencias circulares. La manera más profesional de solucionarlo es crear DTOs para evitar exponer las entidades directamente..
                .Include(x => x.Autor)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (libro is null) return NotFound();

            return libro;
        }

        [HttpPost]
        public async Task<ActionResult> Post(Libro libro)
        {
            var existeAutor = await _context.Autores.AnyAsync(x => x.Id == libro.AutorId);

            if (!existeAutor)
            {
                ModelState.AddModelError(nameof(libro.AutorId), $"El autor de id {libro.AutorId} no existe");
                return ValidationProblem();
                //return BadRequest($"El autor de id {libro.AutorId} no existe");
            }

            _context.Add(libro);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, Libro libro)
        {
            if (id != libro.Id) return BadRequest("Los ids deben de coincidir");

            var existeAutor = await _context.Autores.AnyAsync(x => x.Id == libro.AutorId);

            if (!existeAutor) return BadRequest($"El autor de id {libro.AutorId} no existe");

            _context.Update(libro);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var registrosBorrados = await _context.Libros.Where(x => x.Id == id).ExecuteDeleteAsync();

            if (registrosBorrados == 0) return NotFound();

            return Ok();
        }
    }
}
