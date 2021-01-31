using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Modelo.SchoolUp.Custom
{
    public class CmSerie
    {
        public Guid Id { get; set; }
        public Guid? IdEnsino { get; set; }
        [Display(Name = "Ensino")]
        public string NomeEnsino { get; set; }
        public string Nome { get; set; }
        public string Codigo { get; set; }
        public bool? Ativo { get; set; }
    }
}
