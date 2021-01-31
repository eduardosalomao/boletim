using System;
using System.Collections.Generic;
using System.Text;

namespace Modelo.SchoolUp.Enumeracao
{
    public enum EDiasSemana
    {
        Segunda,
        Terça,
        Quarta,
        Quinta,
        Sexta,
        Sábado,
        Domingo
    }

    public class DiasSemana
    {
        public int dia { get; set; }
        public string nomeDia { get; set; }

        public DiasSemana(int pDia, string pNomeDia)
        {
            dia = pDia;
            nomeDia = pNomeDia;
        }

        public static List<DiasSemana> ObterDiasSemana()
        {
            return new List<DiasSemana>() { new DiasSemana(0, "Segunda"), new DiasSemana(1, "Terça"), new DiasSemana(2, "Quarta"), new DiasSemana(3, "Quinta"), new DiasSemana(4, "Sexta"), new DiasSemana(5, "Sábado"), new DiasSemana(6, "Domingo") };
        }
    }
}
