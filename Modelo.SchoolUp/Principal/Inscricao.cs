using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Modelo.SchoolUp.Principal
{
    public class Inscricao
    {
        public Inscricao()
        {
            Notas = new HashSet<Notas>();
        }

        public Guid Id { get; set; }
        public Guid IdAluno { get; set; }
        public Guid? IdTurma { get; set; }
        public Guid IdDisciplina { get; set; }
        public bool IsDependencia { get; set; }
        public DateTime DataInscricao { get; set; }
        public bool Excluido { get; set; }

        [JsonIgnore]
        public Aluno IdAlunoNavigation { get; set; }
        [JsonIgnore]
        public Disciplina IdDisciplinaNavigation { get; set; }
        [JsonIgnore]
        public Turma IdTurmaNavigation { get; set; }
        [JsonIgnore]
        public ICollection<Notas> Notas { get; set; }
    }
}
