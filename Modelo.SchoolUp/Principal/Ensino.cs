using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Modelo.SchoolUp.Principal
{
    public class Ensino
    {
        public Ensino()
        {
            Serie = new HashSet<Serie>();
            Disciplina = new HashSet<Disciplina>();
        }

        public Guid Id { get; set; }
        public Guid? IdEscola { get; set; }
        public string Nome { get; set; }
        public string Codigo { get; set; }
        public bool? Ativo { get; set; }

        [JsonIgnore]
        public ICollection<Serie> Serie { get; set; }
        [JsonIgnore]
        public ICollection<Disciplina> Disciplina { get; set; }
    }
}
