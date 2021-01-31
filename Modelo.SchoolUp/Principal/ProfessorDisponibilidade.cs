using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Modelo.SchoolUp.Principal
{
    public class ProfessorDisponibilidade
    {
        public Guid Id { get; set; }
        public Guid? IdProfessor { get; set; }
        public int Dia { get; set; }
        public TimeSpan Inicio { get; set; }
        public TimeSpan Termino { get; set; }

        [JsonIgnore]
        public Professor IdProfessorNavigation { get; set; }
    }
}
