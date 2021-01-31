using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Modelo.SchoolUp.Principal
{
    public class Notas
    {
        public Guid Id { get; set; }
        public Guid IdInscricao { get; set; }
        public Guid IdAvaliacao { get; set; }
        public decimal? Nota { get; set; }
        public decimal? NotaRecuperacao { get; set; }
        public int? Faltas { get; set; }
        public bool Excluido { get; set; }

        [JsonIgnore]
        public Avaliacao IdAvaliacaoNavigation { get; set; }
        [JsonIgnore]
        public Inscricao IdInscricaoNavigation { get; set; }
    }
}
