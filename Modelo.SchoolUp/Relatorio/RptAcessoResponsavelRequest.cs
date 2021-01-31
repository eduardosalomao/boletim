using System;
using System.Collections.Generic;
using System.Text;

namespace Modelo.SchoolUp.Relatorio
{
    public class RptAcessoResponsavelRequest
    {
        public string IdEscola { get; set; }
        public string IdAluno { get; set; }
        public string IdTurma { get; set; }
        public string Inicio { get; set; }
        public string Fim { get; set; }
    }
}
