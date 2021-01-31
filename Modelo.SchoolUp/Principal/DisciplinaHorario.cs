using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Modelo.SchoolUp.Principal
{
    public class DisciplinaHorario
    {
        public Guid Id { get; set; }
        public Guid? IdDisciplina { get; set; }
        public Guid? IdProfessor { get; set; }
        public Guid IdTurma { get; set; }
        public Guid? IdHorarioTurno { get; set; }
        public int Dia { get; set; }
        public TimeSpan? Inicio { get; set; }
        public TimeSpan? Termino { get; set; }
        public bool Excluido { get; set; }

        [JsonIgnore]
        public Disciplina IdDisciplinaNavigation { get; set; }
        [JsonIgnore]
        public HorarioTurno IdHorarioTurnoNavigation { get; set; }
        [JsonIgnore]
        public Professor IdProfessorNavigation { get; set; }
        [JsonIgnore]
        public Turma IdTurmaNavigation { get; set; }
    }
}
