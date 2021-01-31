using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Modelo.SchoolUp.Custom
{
    public class CmAvaliacao
    {
        public CmAvaliacao()
        {
        }

        public Guid Id { get; set; }
        public Guid IdPeriodo { get; set; }
        public Guid IdSubPeriodo { get; set; }
        [Display(Name = "Bimestre")]
        public string NomeSubPeriodo { get; set; }
        [Display(Name = "Período")]
        public string NomePeriodo { get; set; }
        public Guid IdProfessorDisciplina { get; set; }
        [Display(Name = "Professor")]
        public string NomeProfessor { get; set; }
        [Display(Name = "Disciplina")]
        public string NomeDisciplina { get; set; }
        public Guid IdTipoAvaliacao { get; set; }
        [Display(Name = "Avaliação")]
        public string NomeAvaliacao { get; set; }
        public Guid IdTurma { get; set; }
        [Display(Name = "Turma")]
        public string NomeTurma { get; set; }
        public int? AulasPrevistas { get; set; }
        public int? AulasDadas { get; set; }
        public decimal Peso { get; set; }
        public string Sigla { get; set; }
        [Display(Name = "Início")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        [DataType(DataType.DateTime)]
        public DateTime? De { get; set; }
        [Display(Name = "Término")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        [DataType(DataType.DateTime)]
        public DateTime? Ate { get; set; }
        public bool Excluido { get; set; }
    }
}
