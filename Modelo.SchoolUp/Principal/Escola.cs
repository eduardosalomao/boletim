using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Modelo.SchoolUp.Principal
{
    public class Escola
    {
        public Escola()
        {
            Aluno = new HashSet<Aluno>();
            Disciplina = new HashSet<Disciplina>();
            EscolaProfessor = new HashSet<EscolaProfessor>();
            EscolaUsuario = new HashSet<EscolaUsuario>();
            Periodo = new HashSet<Periodo>();
            TipoAvaliacao = new HashSet<TipoAvaliacao>();
            Turno = new HashSet<Turno>();
        }

        public Guid Id { get; set; }
        public Guid IdEmpresa { get; set; }
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
        public Empresa IdEmpresaNavigation { get; set; }
        [JsonIgnore]
        public ICollection<Aluno> Aluno { get; set; }
        [JsonIgnore]
        public ICollection<Disciplina> Disciplina { get; set; }
        [JsonIgnore]
        public ICollection<EscolaProfessor> EscolaProfessor { get; set; }
        [JsonIgnore]
        public ICollection<EscolaUsuario> EscolaUsuario { get; set; }
        [JsonIgnore]
        public ICollection<Periodo> Periodo { get; set; }
        [JsonIgnore]
        public ICollection<TipoAvaliacao> TipoAvaliacao { get; set; }
        [JsonIgnore]
        public ICollection<Turno> Turno { get; set; }
    }
}
