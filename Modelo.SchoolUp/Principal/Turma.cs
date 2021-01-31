using Modelo.SchoolUp.Recursos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Modelo.SchoolUp.Principal
{
    public class Turma
    {
        public Turma()
        {
            AlunoTurma = new HashSet<AlunoTurma>();
            Avaliacao = new HashSet<Avaliacao>();
            Diario = new HashSet<Diario>();
            DisciplinaHorario = new HashSet<DisciplinaHorario>();
            Inscricao = new HashSet<Inscricao>();
        }

        public Guid Id { get; set; }
        [Display(Name = "Período")]
        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        public Guid IdPeriodo { get; set; }
        [Display(Name = "Turno")]
        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        public Guid? IdTurno { get; set; }
        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        [StringLength(20, MinimumLength = 3, ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoTamanhoEntre")]
        public string Nome { get; set; }
        [Display(Name = "Série")]
        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        public Guid IdSerie { get; set; }
        [Display(Name = "Número de Alunos")]
        [RegularExpression(@"^\d{1,3}$", ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoInvalido")]
        public int? NumeroAlunos { get; set; }
        public string Codigo { get; set; }
        public bool Excluido { get; set; }

        [JsonIgnore]
        public Periodo IdPeriodoNavigation { get; set; }
        [JsonIgnore]
        public Serie IdSerieNavigation { get; set; }
        [JsonIgnore]
        public Turno IdTurnoNavigation { get; set; }
        [JsonIgnore]
        public ICollection<AlunoTurma> AlunoTurma { get; set; }
        [JsonIgnore]
        public ICollection<Diario> Diario { get; set; }
        [JsonIgnore]
        public ICollection<DisciplinaHorario> DisciplinaHorario { get; set; }
        [JsonIgnore]
        public ICollection<Inscricao> Inscricao { get; set; }
        [JsonIgnore]
        public ICollection<Avaliacao> Avaliacao { get; set; }
    }
}
