using System;

namespace Modelo.SchoolUp.Principal
{
    public partial class Diario
    {
        public Guid Id { get; set; }
        public Guid IdProfessor { get; set; }
        public Guid IdDisciplina { get; set; }
        public Guid IdTurma { get; set; }
        public Guid IdAluno { get; set; }
        public DateTime Data { get; set; }
        public bool Presente { get; set; }
        public string Observacao { get; set; }

        public Aluno IdAlunoNavigation { get; set; }
        public Disciplina IdDisciplinaNavigation { get; set; }
        public Professor IdProfessorNavigation { get; set; }
        public Turma IdTurmaNavigation { get; set; }
    }
}
