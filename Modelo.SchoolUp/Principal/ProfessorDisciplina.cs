using Modelo.SchoolUp.Recursos;
using Modelo.SchoolUp.Validation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Modelo.SchoolUp.Principal
{
    public class ProfessorDisciplina
    {
        public ProfessorDisciplina()
        {
            Avaliacao = new HashSet<Avaliacao>();
        }

        public Guid Id { get; set; }
        [IsGuidEmpty(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        public Guid IdEscolaProfessor { get; set; }
        [Display(Name = "Disciplina")]
        [IsGuidEmpty(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        public Guid IdDisciplina { get; set; }
        public DateTime? De { get; set; }
        public DateTime? Ate { get; set; }
        public bool Excluido { get; set; }

        [JsonIgnore]
        public Disciplina IdDisciplinaNavigation { get; set; }
        [JsonIgnore]
        public EscolaProfessor IdEscolaProfessorNavigation { get; set; }
        [JsonIgnore]
        public ICollection<Avaliacao> Avaliacao { get; set; }
    }
}
