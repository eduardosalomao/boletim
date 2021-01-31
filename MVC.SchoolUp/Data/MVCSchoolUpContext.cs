using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Modelo.SchoolUp.Principal;

namespace MVC.SchoolUp.Models
{
    public class MVCSchoolUpContext : DbContext
    {
        public MVCSchoolUpContext (DbContextOptions<MVCSchoolUpContext> options)
            : base(options)
        {
        }

        public DbSet<Modelo.SchoolUp.Principal.Disciplina> Disciplina { get; set; }

        public DbSet<Modelo.SchoolUp.Principal.Professor> Professor { get; set; }

        public DbSet<Modelo.SchoolUp.Principal.Serie> Serie { get; set; }

        public DbSet<Modelo.SchoolUp.Principal.Periodo> Periodo { get; set; }

        public DbSet<Modelo.SchoolUp.Principal.Aluno> Aluno { get; set; }

        public DbSet<Modelo.SchoolUp.Principal.Turma> Turma { get; set; }

        public DbSet<Modelo.SchoolUp.Principal.SubPeriodo> SubPeriodo { get; set; }

        public DbSet<Modelo.SchoolUp.Principal.HorarioTurno> HorarioTurno { get; set; }
    }
}
