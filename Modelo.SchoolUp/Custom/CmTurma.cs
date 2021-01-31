using Modelo.SchoolUp.Principal;
using Modelo.SchoolUp.Recursos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Modelo.SchoolUp.Custom
{
    public class CmTurma
    {
        public CmTurma()
        {
            AlunoTurma = new HashSet<AlunoTurma>();
            DisciplinaHorario = new HashSet<DisciplinaHorario>();
        }

        public Guid Id { get; set; }
        [Display(Name = "Período")]
        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        public Guid IdPeriodo { get; set; }
        [Display(Name = "Período")]
        public string NomePeriodo { get; set; }
        [Display(Name = "Turno")]
        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        public Guid? IdTurno { get; set; }
        [Display(Name = "Turno")]
        public string NomeTurno { get; set; }
        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        [StringLength(20, MinimumLength = 3, ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoTamanhoEntre")]
        public string Nome { get; set; }
        [Display(Name = "Série")]
        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        public Guid IdSerie { get; set; }
        [Display(Name = "Série")]
        public string NomeSerie { get; set; }
        [Display(Name = "Número de Alunos")]
        [RegularExpression(@"^\d{1,3}$", ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoInvalido")]
        public int? NumeroAlunos { get; set; }
        public string Codigo { get; set; }
        public bool Excluido { get; set; }

        public Periodo IdPeriodoNavigation { get; set; }
        public Serie IdSerieNavigation { get; set; }
        public Turno IdTurnoNavigation { get; set; }
        public ICollection<AlunoTurma> AlunoTurma { get; set; }
        public ICollection<DisciplinaHorario> DisciplinaHorario { get; set; }
    }
}
