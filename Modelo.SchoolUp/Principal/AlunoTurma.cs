using Modelo.SchoolUp.Recursos;
using Modelo.SchoolUp.Validation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Modelo.SchoolUp.Principal
{
    public class AlunoTurma
    {
        public Guid Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        [Display(Name = "Aluno")]
        [IsGuidEmpty(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        public Guid IdAluno { get; set; }
        [IsGuidEmpty(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        [Display(Name = "Turma")]
        public Guid IdTurma { get; set; }
        public bool Excluido { get; set; }

        [JsonIgnore]
        public Turma IdTurmaNavigation { get; set; }
        [JsonIgnore]
        public Aluno IdAlunoNavigation { get; set; }
    }
}
