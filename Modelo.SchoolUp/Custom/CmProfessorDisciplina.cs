using Modelo.SchoolUp.Principal;
using Modelo.SchoolUp.Recursos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Modelo.SchoolUp.Custom
{
    public class CmProfessorDisciplina
    {
        public Guid? Id { get; set; }
        public Guid? IdProfessorEscola { get; set; }
        public Guid? IdProfessor { get; set; }
        public Guid? IdDisciplina { get; set; }
        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        [StringLength(70, MinimumLength = 3, ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoTamanhoEntre")]
        [Display(Name = "Disciplina")]
        public string NomeDisciplina { get; set; }
        public string NomeProfessorDisciplina { get; set; }
        public string NomeProfessor { get; set; }
        public bool Excluido { get; set; }
    }
}
