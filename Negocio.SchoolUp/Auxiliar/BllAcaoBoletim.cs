using Modelo.Nucleo.Models;
using Modelo.SchoolUp.Enumeracao;
using Negocio.Nucleo.Geral;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Negocio.SchoolUp.Auxiliar
{
    public class BllAcaoBoletim : BllAcao
    {
        public static List<Acoes> ListaAcaoBoletim { get; set; }
        public static string IdAcaoAcessarGrade { get; set; }
        public static string IdAcaoEditarGrade { get; set; }

        public BllAcaoBoletim()
        {
            List<Acoes> acoes = ObterTodos();
            IdAcaoAcessarGrade = ListaAcao.Where(i => i.Codigo == EAcoesBoletim.BOLETIMGRADE.ToString()).FirstOrDefault().IdAcao;
            IdAcaoEditarGrade = ListaAcao.Where(i => i.Codigo == EAcoesBoletim.BOLETIMGRADE.ToString()).FirstOrDefault().IdAcao;
        }
    }
}
