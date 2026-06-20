using BibliotecaAPI.Entidades;

namespace BibliotecaAPI
{
    public class RepositorioValoresOracle : IRepositorioValores
    {
        List<Valor> _valores;
        public RepositorioValoresOracle()
        {
            _valores = new List<Valor>
            {
                new Valor{ Id = 4, Nombre = "Valor Oracle 1" },
                new Valor{ Id = 5, Nombre = "Valor Oracle 3" },
                new Valor{ Id = 6, Nombre = "Valor Oracle 3" },
            };
        }
        
        public IEnumerable<Valor> ObtenerValores()
        {
            return _valores;
        }

        public void InsertarValor(Valor valor)
        {
            _valores.Add(valor);
        }
    }
}
