using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Modelo.SchoolUp.Relatorio
{
    public class RptAcessoResponsavel
    {
        public Guid IdEscola { get; set; }
        [Display(Name = "Data Inicial")]
        [DataType(DataType.Date)]
        public DateTime? Inicio { get; set; }
        [Display(Name = "Data Final")]
        [DataType(DataType.Date)]
        public DateTime? Fim { get; set; }
        public string IdResponsavel { get; set; }
        public string  NomeResponsavel { get; set; }
        [Display(Name = "Aluno")]
        public string IdAluno { get; set; }
        public string IdTurma { get; set; }
        public string IdUser { get; set; }
        public string Login { get; set; }
        public string Turma { get; set; }
        [Display(Name = "Aluno")]
        public string NomeAluno { get; set; }
        [Display(Name = "Data de Acesso")]
        [DataType(DataType.Date)]
        public DateTime DataAcesso { get; set; }
        [Display(Name = "Primeiro Acesso")]
        [DataType(DataType.Date)]
        public DateTime DataAcessoMinima { get; set; }
        [Display(Name = "Último Acesso")]
        [DataType(DataType.Date)]
        public DateTime DataAcessoMaxima { get; set; }
        [Display(Name = "Número de Acessos")]
        public int NumeroAcessos { get; set; }
        [Display(Name = "Agrupar")]
        public bool IsAgrupar { get; set; }
    }
}
