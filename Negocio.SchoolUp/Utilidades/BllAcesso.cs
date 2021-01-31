using Acesso.SchoolUp.Custom;
using System;
using System.Collections.Generic;
using System.Text;

namespace Negocio.SchoolUp.Utilidades
{
    public class BllAcesso
    {

        public bool ExistLogin(string login)
        {
            return !String.IsNullOrEmpty(new DalUsuario().Obter(login)?.Id);
        }
    }
}
