using Acesso.SchoolUp.Contexts;
using Modelo.SchoolUp.Custom;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Acesso.SchoolUp.Custom
{
    public class DalTurma
    {
        public List<CmTurma> ObterTodos(Guid idEscola)
        {
            using (SchoolContext contexto = new SchoolContext())
            {
                List<CmTurma> lista = (from t in contexto.Turma
                                            join s in contexto.Serie on t.IdSerie equals s.Id
                                            join p in contexto.Periodo on t.IdPeriodo equals p.Id into pl from p in pl.DefaultIfEmpty()
                                            join tn in contexto.Turno on t.IdTurno equals tn.Id into tnl from tn in tnl.DefaultIfEmpty()
                                            where p.IdEscola.Equals(idEscola) && !t.Excluido && !p.Excluido && s.Ativo.HasValue && s.Ativo.Value && tn.Ativo.HasValue && tn.Ativo.Value
                                       orderby p.Nome descending, t.Nome
                                            select new CmTurma()
                                            {
                                                Id = t.Id,
                                                IdPeriodo = p.Id,
                                                IdSerie = s.Id,
                                                IdTurno = tn.Id,
                                                Nome = t.Nome,
                                                NomePeriodo = p.Nome,
                                                NomeSerie = s.Nome,
                                                NomeTurno = tn.Nome,
                                                NumeroAlunos = t.NumeroAlunos
                                            }
                                            ).ToList();
                return lista;
            }
        }

        public List<CmTurmaAluno> ObterAlunos(Guid idTurma)
        {
            using (SchoolContext contexto = new SchoolContext())
            {
                List<CmTurmaAluno> lista = (from t in contexto.Turma
                                       join at in contexto.AlunoTurma on t.Id equals at.IdTurma 
                                       join a in contexto.Aluno on at.IdAluno equals a.Id
                                       where t.Id == idTurma && !t.Excluido && !a.Excluido && !at.Excluido
                                       orderby a.Nome
                                       select new CmTurmaAluno()
                                       {
                                           Id = at.Id,
                                           IdAluno = a.Id,
                                           IdAlunoTurma = at.Id,
                                           AlunoNome = a.Nome,
                                           AlunoMatricula = a.Matricula,
                                           AlunoDataNascimento = a.DataNascimento
                                       }).ToList();
                return lista;
            }
        }

    }
}
