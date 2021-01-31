using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Modelo.SchoolUp.Principal
{
    public class EscolaProfessor
    {
        public EscolaProfessor()
        {
            ProfessorDisciplina = new HashSet<ProfessorDisciplina>();
        }

        public Guid Id { get; set; }
        public Guid IdEscola { get; set; }
        public Guid IdProfessor { get; set; }
        public string Matricula { get; set; }
        public DateTime? De { get; set; }
        public DateTime? Ate { get; set; }
        public bool Excluido { get; set; }

        [JsonIgnore]
        public Escola IdEscolaNavigation { get; set; }
        [JsonIgnore]
        public Professor IdProfessorNavigation { get; set; }
        [JsonIgnore]
        public ICollection<ProfessorDisciplina> ProfessorDisciplina { get; set; }
    }
}
