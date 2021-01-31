using Modelo.SchoolUp.Principal;
using System;
using System.Collections.Generic;

namespace Modelo.SchoolUp.Custom
{
    public class CmUsuario
    {
        public string Nome { get; set; }
        public Guid IdEscola { get; set; }
        public List<string> PerfilCodigo { get; set; }
        public List<CmAluno> Alunos { get; set; }
    }
}