using System.Collections.Generic;

namespace Entity
{
    public class Categorias
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public IList<Produtos> Produtos { get; set; }
    }
}
