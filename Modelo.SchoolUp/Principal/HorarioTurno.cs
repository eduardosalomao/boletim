using Modelo.SchoolUp.Recursos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Modelo.SchoolUp.Principal
{
    public class HorarioTurno
    {
        public HorarioTurno()
        {
            DisciplinaHorario = new HashSet<DisciplinaHorario>();
        }

        public Guid Id { get; set; }
        [Display(Name = "Turno")]
        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        public Guid IdTurno { get; set; }
        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        public string Nome { get; set; }
        public string Codigo { get; set; }
        [Display(Name = "Início do Tempo")]
        [DataType(DataType.Time)]
        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        public TimeSpan? Inicio { get; set; }
        [Display(Name = "Fim do Tempo")]
        [DataType(DataType.Time)]
        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        public TimeSpan? Termino { get; set; }
        public bool? Ativo { get; set; }

        [JsonIgnore]
        public Turno IdTurnoNavigation { get; set; }
        [JsonIgnore]
        public ICollection<DisciplinaHorario> DisciplinaHorario { get; set; }
    }
}
