using Acesso.SchoolUp.Contexts;
using Modelo.SchoolUp.Custom;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Acesso.SchoolUp.Custom
{
    public class DalAluno
    {
        public List<CmTurmaAluno> ObterPorResponsavel(Guid idUsuario)
        {
            using (SchoolContext contexto = new SchoolContext())
            {
                var aluno = (from r in contexto.Responsavel
                                join ra in contexto.ResponsavelAluno on r.Id equals ra.IdResponsavel
                                join a in contexto.Aluno on ra.IdAluno equals a.Id
                                join at in contexto.AlunoTurma on a.Id equals at.IdAluno
                                join t in contexto.Turma on at.IdTurma equals t.Id
                                where r.IdUser == idUsuario
                                && !r.Excluido && !ra.Excluido && !a.Excluido && !at.Excluido && !t.Excluido
                                select new CmTurmaAluno()
                                {
                                    IdAluno = a.Id,
                                    IdTurma = t.Id,
                                    AlunoNome = a.Nome,
                                    IdPeriodo = t.IdPeriodo
                                })?.ToList();

                return aluno;
            }
        }

        public List<CmTurmaAluno> ObterTurma(Guid idAluno, Guid idPeriodo)
        {
            using (SchoolContext contexto = new SchoolContext())
            {
                var aluno = (from a in contexto.Aluno 
                             join at in contexto.AlunoTurma on a.Id equals at.IdAluno
                             join t in contexto.Turma on at.IdTurma equals t.Id
                             where a.Id == idAluno && t.IdPeriodo == idPeriodo
                             && !a.Excluido && !at.Excluido && !t.Excluido
                             select new CmTurmaAluno()
                             {
                                 IdAluno = a.Id,
                                 IdPeriodo = t.IdPeriodo,
                                 IdTurma = t.Id,
                                 AlunoNome = a.Nome,
                                 TurmaNome = t.Nome
                             })?.ToList();

                return aluno;
            }
        }

        public List<CmTurmaAluno> ObterTurmas(Guid idAluno)
        {
            using (SchoolContext contexto = new SchoolContext())
            {
                var aluno = (from a in contexto.Aluno
                             join at in contexto.AlunoTurma on a.Id equals at.IdAluno
                             join t in contexto.Turma on at.IdTurma equals t.Id
                             join p in contexto.Periodo on t.IdPeriodo equals p.Id
                             where a.Id == idAluno
                             && !a.Excluido && !at.Excluido && !t.Excluido
                             select new CmTurmaAluno()
                             {
                                 IdAluno = a.Id,
                                 IdTurma = t.Id,
                                 AlunoNome = a.Nome,
                                 TurmaNome = t.Nome,
                                 IdPeriodo = p.Id,
                                 PeriodoNome = p.Nome
                             })?.ToList();

                return aluno;
            }
        }
    }
}
