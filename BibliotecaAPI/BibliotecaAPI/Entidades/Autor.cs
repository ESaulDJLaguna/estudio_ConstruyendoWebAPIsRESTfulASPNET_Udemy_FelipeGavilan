using BibliotecaAPI.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Entidades
{
    public class Autor
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")] // El atributo [Required] indica que el campo es obligatorio (es una validación de ASP.NET Core)
        [StringLength(150, ErrorMessage = "El campo {0} debe tener {1} caracteres o menos")]
        [PrimeraLetraMayuscula]
        public required string Nombre { get; set; } // El modificador 'required' es una característica de C# 11 que indica que esta propiedad debe ser inicializada al crear una instancia de la clase. Esto es útil para garantizar que se proporcionen valores para propiedades esenciales, como 'Nombre' en este caso.

        public List<Libro> Libros { get; set; } = new List<Libro>();
    }
}
