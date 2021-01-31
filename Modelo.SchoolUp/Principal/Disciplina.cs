using Modelo.SchoolUp.Recursos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Modelo.SchoolUp.Principal
{
    public class Disciplina
    {
        public Disciplina()
        {
            Diario = new HashSet<Diario>();
            DisciplinaHorario = new HashSet<DisciplinaHorario>();
            Inscricao = new HashSet<Inscricao>();
            ProfessorDisciplina = new HashSet<ProfessorDisciplina>();
        }

        public Guid Id { get; set; }
        [Display(Name = "Ensino")]
        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        public Guid? IdEnsino { get; set; }
        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        public Guid? IdEscola { get; set; }
        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        [StringLength(50, MinimumLength = 3, ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoTamanhoEntre")]
        public string Nome { get; set; }
        public string Codigo { get; set; }
        [Display(Name = "Área")]
        public Guid? IdArea { get; set; }
        public bool Excluido { get; set; }

        [JsonIgnore]
        public AreaConhecimento IdAreaNavigation { get; set; }
        [JsonIgnore]
        public Ensino IdEnsinoNavigation { get; set; }
        [JsonIgnore]
        public Escola IdEscolaNavigation { get; set; }
        [JsonIgnore]
        public ICollection<Diario> Diario { get; set; }
        [JsonIgnore]
        public ICollection<DisciplinaHorario> DisciplinaHorario { get; set; }
        [JsonIgnore]
        public ICollection<Inscricao> Inscricao { get; set; }
        [JsonIgnore]
        public ICollection<ProfessorDisciplina> ProfessorDisciplina { get; set; }
    }
}
