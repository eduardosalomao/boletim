using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Modelo.SchoolUp.Principal
{
    public class EscolaUsuario
    {
        public Guid Id { get; set; }
        public Guid IdEscola { get; set; }
        public Guid IdUser { get; set; }

        [JsonIgnore]
        public Escola IdEscolaNavigation { get; set; }
    }
}
