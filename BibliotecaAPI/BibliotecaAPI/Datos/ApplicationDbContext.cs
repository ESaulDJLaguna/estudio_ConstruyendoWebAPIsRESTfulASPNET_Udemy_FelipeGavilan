using BibliotecaAPI.Entidades;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPI.Datos
{
    //! Básicamente el DbContext es la pieza central de Entity Framework Core. Es a través de esta clase que se configura cuáles serán las tablas de la base de datos y también otras configuraciones, como por ejemplo, el conection string o que realmente se utilizará una base de datos SQL Server. Aquí se tienen las configuraciones fundamentes de EFCore.
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            //! Esto me permite realizar ciertas configuraciones de EFC fuera de la clase, por ejemplo el connection string o el tipo de base de datos que se va a utilizar, etc. Esto es una buena práctica, ya que me permite mantener esta clase más limpia y también me permite realizar pruebas unitarias sobre esta clase, ya que puedo inyectar un DbContextOptions con una configuración específica para mis pruebas unitarias.
            DbContextOptions options
            ) : base(options)
        {
        }

        public DbSet<Autor> Autores { get; set; }
        public DbSet<Libro> Libros { get; set; }
    }
}
