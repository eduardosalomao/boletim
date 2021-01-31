using Modelo.SchoolUp.Principal;
using System.Collections.Generic;

namespace Modelo.SchoolUp.Custom
{
    public class CmPrincipalAdmin
    {
        public Periodo PeriodoAtual { get; set; }
        public List<SubPeriodo> Bimestres { get; set; }
        public List<Turma> Turmas { get; set; }
        public List<CmDisciplinaMedias> Disciplinas { get; set; }
        public List<CmDisciplinaMedias> DisciplinasRadar { get; set; }
        public string IdAluno { get; set; }
        public string Perfil { get; set; }
        public string IdDdlPeriodo { get; set; }
        public string IdDdlTurma { get; set; }
        public string IdDdlAluno { get; set; }
    }
}
