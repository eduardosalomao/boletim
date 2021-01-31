using Modelo.SchoolUp.Recursos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Modelo.SchoolUp.Principal
{
    public class Avaliacao
    {
        public Avaliacao()
        {
            Notas = new HashSet<Notas>();
        }

        public Guid Id { get; set; }
        public Guid IdSubPeriodo { get; set; }
        [Display(Name = "Professor/Disciplina")]
        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        public Guid IdProfessorDisciplina { get; set; }
        [Display(Name = "Avaliação")]
        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        public Guid IdTipoAvaliacao { get; set; }
        public Guid IdTurma { get; set; }
        public int? AulasPrevistas { get; set; }
        public int? AulasDadas { get; set; }
        public decimal Peso { get; set; }
        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        public string Sigla { get; set; }
        [Display(Name = "Início")]
        [DataType(DataType.Text)]
        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        public DateTime? De { get; set; }
        [Display(Name = "Término")]
        [DataType(DataType.Text)]
        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        public DateTime? Ate { get; set; }
        public bool Excluido { get; set; }

        [JsonIgnore]
        public ProfessorDisciplina IdProfessorDisciplinaNavigation { get; set; }
        [JsonIgnore]
        public SubPeriodo IdSubPeriodoNavigation { get; set; }
        [JsonIgnore]
        public TipoAvaliacao IdTipoAvaliacaoNavigation { get; set; }
        [JsonIgnore]
        public Turma IdTurmaNavigation { get; set; }
        [JsonIgnore]
        public ICollection<Notas> Notas { get; set; }
    }
}
