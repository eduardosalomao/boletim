using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Modelo.SchoolUp.Principal
{
    public class TipoAvaliacao
    {
        public TipoAvaliacao()
        {
            Avaliacao = new HashSet<Avaliacao>();
        }
        public Guid Id { get; set; }
        public Guid? IdEscola { get; set; }
        public string Nome { get; set; }
        public int? TempoPadraoMinutos { get; set; }
        public string Descricao { get; set; }
        public string Codigo { get; set; }
        public bool Excluido { get; set; }

        public Escola IdEscolaNavigation { get; set; }

        [JsonIgnore]
        public ICollection<Avaliacao> Avaliacao { get; set; }
    }
}
