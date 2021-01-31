using Modelo.SchoolUp.Recursos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Modelo.SchoolUp.Principal
{
    public class Serie
    {
        public Serie()
        {
            Turma = new HashSet<Turma>();
        }

        public Guid Id { get; set; }
        [Display(Name = "Ensino")]
        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        public Guid? IdEnsino { get; set; }
        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        [StringLength(50, MinimumLength = 3, ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoTamanhoEntre")]
        public string Nome { get; set; }
        public string Codigo { get; set; }
        public bool? Ativo { get; set; }

        [JsonIgnore]
        public Ensino IdEnsinoNavigation { get; set; }
        [JsonIgnore]
        public ICollection<Turma> Turma { get; set; }
    }
}
