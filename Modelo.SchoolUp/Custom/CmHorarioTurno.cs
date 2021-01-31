using Modelo.SchoolUp.Principal;
using Modelo.SchoolUp.Recursos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Modelo.SchoolUp.Custom
{
    public class CmHorarioTurno
    {
        public Guid Id { get; set; }
        [Display(Name = "Turno")]
        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        public Guid IdTurno { get; set; }
        [Display(Name = "Turno")]
        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        public string NomeTurno { get; set; }
        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        public string Nome { get; set; }
        public string Codigo { get; set; }
        [Display(Name = "Início do Tempo")]
        [DataType(DataType.Time)]
        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        //[DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan? Inicio { get; set; }
        [Display(Name = "Fim do Tempo")]
        [DataType(DataType.Time)]
        //[DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        public TimeSpan? Termino { get; set; }
        public bool? Ativo { get; set; }

        public Turno IdTurnoNavigation { get; set; }
    }
}
