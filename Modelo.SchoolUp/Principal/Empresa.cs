using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Modelo.SchoolUp.Principal
{
    public class Empresa
    {
        public Empresa()
        {
            Escola = new HashSet<Escola>();
        }

        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string NomeCurto { get; set; }
        public string RazaoSocial { get; set; }
        public string Cnpj { get; set; }
        public string Codigo { get; set; }
        public string Site { get; set; }
        public string Email { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Telefone { get; set; }
        public string WhatsApp { get; set; }
        public Guid? Imagem { get; set; }
        public bool? Ativo { get; set; }
        public bool Excluido { get; set; }

        [JsonIgnore]
        public ICollection<Escola> Escola { get; set; }
    }
}
