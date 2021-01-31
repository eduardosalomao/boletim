using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Modelo.SchoolUp.Principal
{
    public class TipoRelacao
    {
        public TipoRelacao()
        {
            ResponsavelAluno = new HashSet<ResponsavelAluno>();
        }

        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Codigo { get; set; }
        public bool Excluido { get; set; }

        [JsonIgnore]
        public ICollection<ResponsavelAluno> ResponsavelAluno { get; set; }
    }
}
