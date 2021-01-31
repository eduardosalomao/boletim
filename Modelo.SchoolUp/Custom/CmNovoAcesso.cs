using Modelo.SchoolUp.Recursos;
using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo.SchoolUp.Custom
{
    public class CmNovoAcesso
    {

        public string IdEscola { get; set; }

        public string IdUsuario { get; set; }

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
    }
}