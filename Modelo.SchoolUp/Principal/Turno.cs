using Modelo.SchoolUp.Recursos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Modelo.SchoolUp.Principal
{
    public class Turno
    {
        public Turno()
        {
            HorarioTurno = new HashSet<HorarioTurno>();
            Turma = new HashSet<Turma>();
        }

        public Guid Id { get; set; }
        public Guid? IdEscola { get; set; }
        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        public string Nome { get; set; }
        public string Codigo { get; set; }
        [Display(Name = "Início do Turno")]
        [DataType(DataType.Time)]
        public TimeSpan? Inicio { get; set; }
        [Display(Name = "Fim do Turno")]
        [DataType(DataType.Time)]
        public TimeSpan? Termino { get; set; }
        public bool? Ativo { get; set; }

        [JsonIgnore]
        public Escola IdEscolaNavigation { get; set; }
        [JsonIgnore]
        public ICollection<HorarioTurno> HorarioTurno { get; set; }
        [JsonIgnore]
        public ICollection<Turma> Turma { get; set; }
    }
}
