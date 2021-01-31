using Modelo.SchoolUp.Recursos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Modelo.SchoolUp.Principal
{
    public class Aluno
    {
        public Aluno()
        {
            AlunoTurma = new HashSet<AlunoTurma>();
            Diario = new HashSet<Diario>();
            Inscricao = new HashSet<Inscricao>();
            ResponsavelAluno = new HashSet<ResponsavelAluno>();
        }

        public Guid Id { get; set; }
        public Guid? IdUser { get; set; }
        public Guid IdEscola { get; set; }
        [Display(Name = "Nome do Aluno")]
        [StringLength(70, MinimumLength = 3, ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoTamanhoEntre")]
        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        public string Nome { get; set; }
        [Display(Name = "Email do Responsável")]
        [RegularExpression(@"^[a-zA-Z]+(([\'\,\.\- ][a-zA-Z ])?[a-zA-Z]*)*\s+<(\w[-._\w]*\w@\w[-._\w]*\w\.\w{2,3})>$|^(\w[-._\w]*\w@\w[-._\w]*\w\.\w{2,3})$", ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoInvalido")]
        [StringLength(70, MinimumLength = 3, ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoTamanhoEntre")]
        public string Email { get; set; }
        public Guid? Imagem { get; set; }
        [Display(Name = "CPF do Responsável")]
        [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$", ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoInvalido")]
        //[Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        public string Cpf { get; set; }
        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        [Display(Name = "Data de Nascimento do Aluno")]
        [DataType(DataType.Date)]
        public DateTime DataNascimento { get; set; }
        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        [StringLength(20, MinimumLength = 1, ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoTamanhoEntre")]
        [Display(Name = "Matrícula")]
        public string Matricula { get; set; }
        public bool? Ativo { get; set; }
        public bool Excluido { get; set; }

        [JsonIgnore]
        public Escola IdEscolaNavigation { get; set; }
        [JsonIgnore]
        public ICollection<AlunoTurma> AlunoTurma { get; set; }
        [JsonIgnore]
        public ICollection<Diario> Diario { get; set; }
        [JsonIgnore]
        public ICollection<Inscricao> Inscricao { get; set; }
        [JsonIgnore]
        public ICollection<ResponsavelAluno> ResponsavelAluno { get; set; }
    }
}
