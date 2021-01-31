using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Modelo.SchoolUp.Principal
{
    public class AreaConhecimento
    {
        public AreaConhecimento()
        {
            Disciplina = new HashSet<Disciplina>();
        }

        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Codigo { get; set; }
        public bool Excluido { get; set; }

        [JsonIgnore]
        public ICollection<Disciplina> Disciplina { get; set; }
    }
}
