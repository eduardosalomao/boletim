using Modelo.SchoolUp.Recursos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Modelo.SchoolUp.Principal
{
    public class SubPeriodo
    {
        public SubPeriodo()
        {
            Avaliacao = new HashSet<Avaliacao>();
        }

        public Guid Id { get; set; }
        [Display(Name = "Período")]
        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        public Guid IdPeriodo { get; set; }
        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        public string Nome { get; set; }
        public string Codigo { get; set; }
        [Display(Name = "Início do Bimestre")]
        [DataType(DataType.Date)]
        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        public DateTime? De { get; set; }
        [Display(Name = "Fim do Bimestre")]
        [DataType(DataType.Date)]
        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        public DateTime? Ate { get; set; }
        public bool Excluido { get; set; }

        [JsonIgnore]
        public Periodo IdPeriodoNavigation { get; set; }
        [JsonIgnore]
        public ICollection<Avaliacao> Avaliacao { get; set; }
    }
}
