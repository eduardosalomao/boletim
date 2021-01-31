using Modelo.SchoolUp.Recursos;
using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo.SchoolUp.Custom
{
    public class CmPrimeiroAcesso
    {

        public string IdEscola { get; set; }

        [Display(Name = "Turma")]
        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        public Guid IdTurma { get; set; }

        [Display(Name = "Aluno")]
        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        public Guid IdAluno { get; set; }

        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        [StringLength(20, MinimumLength = 1, ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoTamanhoEntre")]
        [Display(Name = "Matrícula")]
        public string Matricula { get; set; }

        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        [Display(Name = "Data de Nascimento")]
        [DataType(DataType.Date)]
        public DateTime DataNascimento { get; set; }

        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        [RegularExpression(@"^[a-zA-Z]+(([\'\,\.\- ][a-zA-Z ])?[a-zA-Z]*)*\s+<(\w[-._\w]*\w@\w[-._\w]*\w\.\w{2,3})>$|^(\w[-._\w]*\w@\w[-._\w]*\w\.\w{2,3})$", ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoInvalido")]
        [StringLength(70, MinimumLength = 3, ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoTamanhoEntre")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        [RegularExpression(@"^(?=.{8,16})(?=.*?[A-Z])(?=.*?[a-z])(?=.*?\d)(?=.*?\W).*$", ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "SenhaRegex")]
        public string Senha { get; set; }
        
        [Display(Name = "Confirmar Senha")]
        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        [Compare("Senha", ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "SenhasDiferentes")]
        public string ConfirmaSenha { get; set; }
    }
}