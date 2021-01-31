using Modelo.SchoolUp.Recursos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Modelo.SchoolUp.Principal
{
    public class Periodo
    {
        public Periodo()
        {
            SubPeriodo = new HashSet<SubPeriodo>();
            Turma = new HashSet<Turma>();
        }

        public Guid Id { get; set; }
        public Guid? IdEscola { get; set; }
        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        [StringLength(20, MinimumLength = 1, ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoTamanhoEntre")]
        public string Nome { get; set; }
        public string Codigo { get; set; }
        [Display(Name = "Média para Aprovação")]
        [DataType(DataType.Text)]
        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        [RegularExpression(@"^\d{1,2},\d{1}$", ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoInvalido")]
        public decimal MediaAprovacao { get; set; }
        [Display(Name = "Média Aprovação com Prova Final")]
        public decimal? MediaAprovacaoFinal { get; set; }
        [Display(Name = "Média Aprovação com Prova Final")]
        public decimal? MediaReprovacao { get; set; }
        [Display(Name = "Início do Período")]
        [DataType(DataType.Date)]
        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        public DateTime? De { get; set; }
        [Display(Name = "Fim do Período")]
        [DataType(DataType.Date)]
        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        public DateTime? Ate { get; set; }
        public bool Excluido { get; set; }

        [JsonIgnore]
        public Escola IdEscolaNavigation { get; set; }
        [JsonIgnore]
        public ICollection<SubPeriodo> SubPeriodo { get; set; }
        [JsonIgnore]
        public ICollection<Turma> Turma { get; set; }
    }
}
