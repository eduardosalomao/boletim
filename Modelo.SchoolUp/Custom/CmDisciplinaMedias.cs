using System;
using System.Collections.Generic;

namespace Modelo.SchoolUp.Custom
{
    public class CmDisciplinaMedias
    {
        public Guid IdSubPeriodo { get; set; }
        public Guid IdTurma { get; set; }
        public Guid IdDisciplina { get; set; }
        public string NomeDisciplina { get; set; }
        public Guid IdProfessor { get; set; }
        public string NomeProfessor { get; set; }
        public List<decimal?> ListaMediaNotas { get; set; }
        public List<int?> ListaMediaFaltas { get; set; }
        public decimal? MediaNotas { get; set; }
        public decimal? MediaNotasRecuperacao { get; set; }
        public decimal? MediaAposRecuperacao { get; set; }
        public double? MediaFaltas { get; set; }
        public decimal? TotalNotas { get; set; }
        public decimal? TotalNotasRecuperacao { get; set; }
        public double? TotalFaltas { get; set; }
        public decimal? TotalAposRecuperacao { get; set; }
        public int TotalAlunos { get; set; }
    }
}
