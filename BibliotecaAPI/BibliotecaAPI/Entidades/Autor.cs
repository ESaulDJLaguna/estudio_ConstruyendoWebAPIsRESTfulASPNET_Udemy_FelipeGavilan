using BibliotecaAPI.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Entidades
{
    public class Autor// : IValidatableObject
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")] // El atributo [Required] indica que el campo es obligatorio (es una validación de ASP.NET Core)
        [StringLength(150, ErrorMessage = "El campo {0} debe tener {1} caracteres o menos")]
        [PrimeraLetraMayuscula]
        public required string Nombre { get; set; } // El modificador 'required' es una característica de C# 11 que indica que esta propiedad debe ser inicializada al crear una instancia de la clase. Esto es útil para garantizar que se proporcionen valores para propiedades esenciales, como 'Nombre' en este caso.

        public List<Libro> Libros { get; set; } = new List<Libro>();

        /*! Ejemplo con múltiples errores de validación por modelo
        public int Edad { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(!string.IsNullOrEmpty(Nombre))
            {
                var primeraLetra = Nombre[0].ToString();
                if (primeraLetra != primeraLetra.ToUpper())
                {
                    yield return new ValidationResult("La primera letra debe ser mayúscula - por modelo", new string[] { nameof(Nombre) });
                }
            }

            if(Nombre.Length < 5)
            {
                yield return new ValidationResult($"El {nameof(Nombre)} no debe tener más de 5 caracteres", new string[] { nameof(Nombre) });
            }

            if(Edad < 18)
            {
                yield return new ValidationResult("Debe ser mayor de edad", new[] { nameof(Edad) });
            }
        }
        */

        /* Otras validaciones por defecto
        [Range(18, 120, ErrorMessage = "La {0} debe ser mayor o igual que {1} pero menor o igual a {2}")]
        public int Edad { get; set; }
        [CreditCard] // Solo valida el formato de la tarjeta de crédito
        public string? TarjetaDeCredito { get; set; }
        [Url]
        public string? URL { get; set; }
        */
    }
}
