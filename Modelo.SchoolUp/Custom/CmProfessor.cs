using Modelo.SchoolUp.Principal;
using Modelo.SchoolUp.Recursos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Modelo.SchoolUp.Custom
{
    public class CmProfessor
    {
        public CmProfessor()
        {
            EscolaProfessor = new HashSet<EscolaProfessor>();
            ProfessorDisponibilidade = new HashSet<ProfessorDisponibilidade>();
        }

        public Guid Id { get; set; }
        public Guid? IdUser { get; set; }
        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        [StringLength(70, MinimumLength = 3, ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoTamanhoEntre")]
        public string Nome { get; set; }
        [RegularExpression(@"^[a-zA-Z]+(([\'\,\.\- ][a-zA-Z ])?[a-zA-Z]*)*\s+<(\w[-._\w]*\w@\w[-._\w]*\w\.\w{2,3})>$|^(\w[-._\w]*\w@\w[-._\w]*\w\.\w{2,3})$", ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoInvalido")]
        [StringLength(70, MinimumLength = 3, ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoTamanhoEntre")]
        public string Email { get; set; }
        public Guid? Imagem { get; set; }
        public bool? Ativo { get; set; }
        public bool Excluido { get; set; }

        public ICollection<EscolaProfessor> EscolaProfessor { get; set; }
        public ICollection<ProfessorDisponibilidade> ProfessorDisponibilidade { get; set; }
    }
}
