using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Modelo.SchoolUp.Custom
{
    public class CmDisciplinaHorario
    {
        public Guid? Id { get; set; }
        public Guid? IdDisciplina { get; set; }
        public string nomeDisciplina { get; set; } = "Nenhuma";
        public Guid? IdTurma { get; set; }
        public string nomeTurma { get; set; }
        public Guid? IdHorarioTurno { get; set; }
        public string nomeHorarioTurno { get; set; }
        public Guid? IdProfessor { get; set; }
        public string nomeProfessor { get; set; } = "Nenhum";
        public int? Dia { get; set; }
        public string nomeDia { get; set; }
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan? Inicio { get; set; }
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan? Termino { get; set; }
        public bool? Excluido { get; set; }
    }
}
