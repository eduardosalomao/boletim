using Modelo.SchoolUp.Recursos;
using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo.SchoolUp.Custom
{
    public class CmSenha
    {
        [Key]
        public Guid IdUsuario { get; set; }

        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        [RegularExpression(@"^(?=.{8,16})(?=.*?[A-Z])(?=.*?[a-z])(?=.*?\d)(?=.*?\W).*$", ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "SenhaRegex")]
        public string Senha { get; set; }

        [Display(Name = "Confirmar Senha")]
        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        [Compare("Senha", ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "SenhasDiferentes")]
        public string ConfirmaSenha { get; set; }

        [Display(Name = "Senha Atual")]
        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        //[RegularExpression(@"^(?=.{8,16})(?=.*?[A-Z])(?=.*?[a-z])(?=.*?\d)(?=.*?\W).*$", ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "SenhaRegex")]
        public string SenhaAntiga { get; set; }
    }
}