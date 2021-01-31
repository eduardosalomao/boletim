using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Modelo.SchoolUp.Custom
{
    public class CmDisciplina
    {
        public Guid Id { get; set; }
        public Guid? IdEnsino { get; set; }
        [Display(Name = "Ensino")]
        public string NomeEnsino { get; set; }
        public Guid? IdEscola { get; set; }
        public string Nome { get; set; }
        public string Codigo { get; set; }
        public Guid? IdArea { get; set; }
        [Display(Name = "Área")]
        public string NomeArea { get; set; }
        public bool Excluido { get; set; }

    }
}
