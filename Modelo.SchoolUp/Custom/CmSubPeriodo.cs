using Modelo.SchoolUp.Principal;
using Modelo.SchoolUp.Recursos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Modelo.SchoolUp.Custom
{
    public class CmSubPeriodo
    {
        public CmSubPeriodo()
        {
            Avaliacao = new HashSet<Avaliacao>();
        }

        public Guid Id { get; set; }
        [Display(Name = "Período")]
        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        public Guid IdPeriodo { get; set; }
        [Display(Name = "Período")]
        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        public String NomePeriodo { get; set; }
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

        public Periodo IdPeriodoNavigation { get; set; }
        public ICollection<Avaliacao> Avaliacao { get; set; }
    }
}
