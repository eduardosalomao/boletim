using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Modelo.SchoolUp.Principal
{
    public class Responsavel
    {
        public Responsavel()
        {
            ResponsavelAluno = new HashSet<ResponsavelAluno>();
        }

        public Guid Id { get; set; }
        public Guid? IdUser { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Identidade { get; set; }
        public string OrgaoIdentidade { get; set; }
        public string UfIdentidade { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string WhatsApp { get; set; }
        public bool Excluido { get; set; }

        [JsonIgnore]
        public ICollection<ResponsavelAluno> ResponsavelAluno { get; set; }
    }
}
