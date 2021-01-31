using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Modelo.SchoolUp.Principal
{
    public class ResponsavelAluno
    {
        public Guid Id { get; set; }
        public Guid IdResponsavel { get; set; }
        public Guid IdAluno { get; set; }
        public Guid IdRelacao { get; set; }
        public bool IsResponsavelFinanceiro { get; set; }
        public bool Excluido { get; set; }

        [JsonIgnore]
        public Aluno IdAlunoNavigation { get; set; }
        [JsonIgnore]
        public TipoRelacao IdRelacaoNavigation { get; set; }
        [JsonIgnore]
        public Responsavel IdResponsavelNavigation { get; set; }
    }
}
