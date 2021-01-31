using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo.SchoolUp.Custom
{
    public class CmTurmaAluno
    {
        public CmTurmaAluno()
        {
        }

        public Guid Id { get; set; }
        public Guid IdAluno { get; set; }
        public Guid? IdPeriodo { get; set; }
        public Guid IdTurma { get; set; }
        public Guid IdAlunoTurma { get; set; }
        [Display(Name = "Nome")]
        public string TurmaNome { get; set; }
        public string PeriodoNome { get; set; }
        public string AlunoNome { get; set; }
        [Display(Name = "Matrícula")]
        public string AlunoMatricula { get; set; }
        [Display(Name = "Data Nascimento")]
        [DataType(DataType.Date)]
        public DateTime? AlunoDataNascimento { get; set; }
    }
}
