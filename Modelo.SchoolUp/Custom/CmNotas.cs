using Modelo.SchoolUp.Recursos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Modelo.SchoolUp.Custom
{
    public class CmNotas
    {
        public Guid Id { get; set; }
        [Display(Name = "Nome")]
        public string NomeAluno { get; set; }
        public string Turma { get; set; }
        public string Disciplina { get; set; }
        public string Bimestre { get; set; }
        public string Periodo { get; set; }
        public Guid IdTurma { get; set; }
        public Guid IdDisciplina { get; set; }
        public Guid IdBimestre { get; set; }
        public Guid IdPeriodo { get; set; }
        [Display(Name = "Matrícula")]
        public string MatriculaAluno { get; set; }
        public Guid IdInscricao { get; set; }
        public Guid IdAvaliacao { get; set; }
        [DataType(DataType.Text)]
        [RegularExpression(@"^\d{1,2},\d{1}$", ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoInvalido")]
        public decimal? Nota { get; set; }
        [Display(Name = "Recuperação")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^\d{1,2},\d{1}$", ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoInvalido")]
        public decimal? NotaRecuperacao { get; set; }
        [DataType(DataType.Text)]
        [RegularExpression(@"^\d{1,2}$", ErrorMessageResourceType = typeof(Mensagens), ErrorMessageResourceName = "CampoInvalido")]
        public int? Faltas { get; set; }
        public bool Excluido { get; set; }
    }
}
