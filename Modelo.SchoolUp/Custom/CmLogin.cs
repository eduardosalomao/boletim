using Modelo.SchoolUp.Recursos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;

namespace Modelo.SchoolUp.Custom
{
    public class CmLogin
    {
        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        public string Login { get; set; }

        [Required(ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoObrigatorio")]
        public string Senha { get; set; }
        public string MensagemErro { get; set; }
    }
}