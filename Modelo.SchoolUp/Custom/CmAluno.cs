using Modelo.SchoolUp.Principal;
using Modelo.SchoolUp.Recursos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Modelo.SchoolUp.Custom
{
    public class CmAluno
    {
        public Guid Id { get; set; }
        public Guid? IdUser { get; set; }
        public Guid IdEscola { get; set; }
        public Guid IdTurma { get; set; }
        public bool IsSelected { get; set; }
        [StringLength(70, MinimumLength = 3, ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoTamanhoEntre")]
        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        public string Nome { get; set; }
        [RegularExpression(@"^[a-zA-Z]+(([\'\,\.\- ][a-zA-Z ])?[a-zA-Z]*)*\s+<(\w[-._\w]*\w@\w[-._\w]*\w\.\w{2,3})>$|^(\w[-._\w]*\w@\w[-._\w]*\w\.\w{2,3})$", ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoInvalido")]
        [StringLength(70, MinimumLength = 3, ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoTamanhoEntre")]
        public string Email { get; set; }
        public Guid? Imagem { get; set; }
        [Display(Name = "CPF do Responsável")]
        [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$", ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoInvalido")]
        //[Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        public string Cpf { get; set; }
        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        [Display(Name = "Data de Nascimento")]
        [DataType(DataType.Date)]
        public DateTime DataNascimento { get; set; }
        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        [StringLength(20, MinimumLength = 1, ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoTamanhoEntre")]
        [Display(Name = "Matrícula")]
        public string Matricula { get; set; }
        public bool? Ativo { get; set; }
        public bool Excluido { get; set; }

        public Escola IdEscolaNavigation { get; set; }
        public ICollection<AlunoTurma> AlunoTurma { get; set; }
        public ICollection<Inscricao> Inscricao { get; set; }
        public ICollection<ResponsavelAluno> ResponsavelAluno { get; set; }
    }
}
