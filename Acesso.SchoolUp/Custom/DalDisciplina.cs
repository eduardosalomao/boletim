using Acesso.SchoolUp.Contexts;
using Modelo.SchoolUp.Custom;
using Modelo.SchoolUp.Principal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Acesso.SchoolUp.Custom
{
    public class DalDisciplina
    {
        public List<CmDisciplina> ObterTodos(Guid idEscola)
        {
            using (SchoolContext contexto = new SchoolContext())
            {
                List<CmDisciplina> lista = (from d in contexto.Disciplina
                                            join a in contexto.AreaConhecimento on d.IdArea equals a.Id into al
                                            from a in al.DefaultIfEmpty()
                                            join e in contexto.Ensino on d.IdEnsino equals e.Id into el
                                            from e in el.DefaultIfEmpty()
                                            where d.IdEscola.Equals(idEscola) && !d.Excluido
                                            orderby d.Nome
                                            select new CmDisciplina()
                                            {
                                                Id = d.Id,
                                                IdEscola = d.IdEscola,
                                                IdArea = d.IdArea,
                                                IdEnsino = d.IdEnsino,
                                                Nome = d.Nome,
                                                NomeArea = a.Nome,
                                                NomeEnsino = e.Nome
                                            }
                                            ).ToList();
                return lista;
            }
        }

        public List<CmDisciplinaMedias> ObterMediasPorPeriodo(Guid idPeriodo)
        {
            using (SchoolContext contexto = new SchoolContext())
            {
                List<CmDisciplinaMedias> lista = (from p in contexto.Periodo
                                                  join sp in contexto.SubPeriodo on p.Id equals sp.IdPeriodo
                                                  join a in contexto.Avaliacao on sp.Id equals a.IdSubPeriodo
                                                  join pd in contexto.ProfessorDisciplina on a.IdProfessorDisciplina equals pd.Id
                                                  join d in contexto.Disciplina on pd.IdDisciplina equals d.Id
                                                  join n in contexto.Notas on a.Id equals n.IdAvaliacao
                                                  join i in contexto.Inscricao on n.IdInscricao equals i.Id
                                                  join al in contexto.Aluno on i.IdAluno equals al.Id
                                                  join ta in contexto.TipoAvaliacao on a.IdTipoAvaliacao equals ta.Id
                                                  where p.Id == idPeriodo && !p.Excluido && !sp.Excluido && !pd.Excluido && !d.Excluido
                                                      && !a.Excluido && !n.Excluido && !i.Excluido && !al.Excluido && ta.Codigo == "MEDIA"
                                                            && (n.Nota.HasValue || n.NotaRecuperacao.HasValue || n.Faltas.HasValue)
                                                  group new { sp, d, a, n } by new { IdBimestre = sp.Id, IdDisciplina = d.Id, a.IdTurma }
                                                  into g
                                                  orderby g.Select(s => s.d.Nome).FirstOrDefault()
                                                  select new CmDisciplinaMedias()
                                                  {
                                                      IdDisciplina = g.Select(s => s.d.Id).FirstOrDefault(),
                                                      NomeDisciplina = g.Select(s => s.d.Nome).FirstOrDefault(),
                                                      IdTurma = g.Select(s => s.a.IdTurma).FirstOrDefault(),
                                                      IdSubPeriodo = g.Select(s => s.sp.Id).FirstOrDefault(),
                                                      MediaNotasRecuperacao = g.Average(m => (m.n.NotaRecuperacao)),
                                                      MediaFaltas = g.Average(m => m.n.Faltas),
                                                      MediaNotas = g.Average(m => (m.n.Nota))
                                                  }
                                            )?.ToList();
                return lista;
            }
        }

        public List<CmDisciplinaMedias> ObterMediasPorTurma(Guid idTurma)
        {
            using (SchoolContext contexto = new SchoolContext())
            {
                List<CmDisciplinaMedias> lista = (from p in contexto.Periodo
                                                  join sp in contexto.SubPeriodo on p.Id equals sp.IdPeriodo
                                                  join a in contexto.Avaliacao on sp.Id equals a.IdSubPeriodo
                                                  join pd in contexto.ProfessorDisciplina on a.IdProfessorDisciplina equals pd.Id
                                                  join d in contexto.Disciplina on pd.IdDisciplina equals d.Id
                                                  join n in contexto.Notas on a.Id equals n.IdAvaliacao
                                                  join i in contexto.Inscricao on n.IdInscricao equals i.Id
                                                  join al in contexto.Aluno on i.IdAluno equals al.Id
                                                  join ta in contexto.TipoAvaliacao on a.IdTipoAvaliacao equals ta.Id
                                                  where a.IdTurma == idTurma && !p.Excluido && !sp.Excluido && !pd.Excluido && !d.Excluido
                                                      && !a.Excluido && !n.Excluido && !i.Excluido && !al.Excluido && ta.Codigo == "MEDIA"
                                                      && (n.Nota.HasValue || n.NotaRecuperacao.HasValue || n.Faltas.HasValue)
                                                  group new { sp, d, a, n } by new { IdBimestre = sp.Id, IdDisciplina = d.Id, a.IdTurma }
                                                  into g
                                                  orderby g.Select(s => s.d.Nome).FirstOrDefault()
                                                  select new CmDisciplinaMedias()
                                                  {
                                                      IdDisciplina = g.Select(s => s.d.Id).FirstOrDefault(),
                                                      NomeDisciplina = g.Select(s => s.d.Nome).FirstOrDefault(),
                                                      IdTurma = g.Select(s => s.a.IdTurma).FirstOrDefault(),
                                                      IdSubPeriodo = g.Select(s => s.sp.Id).FirstOrDefault(),
                                                      TotalAlunos = g.Count(),
                                                      TotalNotasRecuperacao = g.Sum(m => m.n.NotaRecuperacao),
                                                      TotalNotas = g.Sum(m => m.n.Nota),
                                                      TotalAposRecuperacao = g.Sum(m => (m.n.Nota ?? 0) > (m.n.NotaRecuperacao ?? 0) ? (m.n.Nota ?? 0) : (m.n.NotaRecuperacao ?? 0)),
                                                      MediaNotas = g.Average(m => (m.n.Nota ?? 0) > (m.n.NotaRecuperacao ?? 0) ? (m.n.Nota ?? 0) : (m.n.NotaRecuperacao ?? 0))
                                                  }
                                            )?.ToList();
                return lista;
            }
        }

        public List<CmDisciplinaMedias> ObterMediasPorAluno(Guid idAluno, Guid idPeriodo)
        {
            using (SchoolContext contexto = new SchoolContext())
            {
                List<CmDisciplinaMedias> lista = (from p in contexto.Periodo
                                                  join sp in contexto.SubPeriodo on p.Id equals sp.IdPeriodo
                                                  join a in contexto.Avaliacao on sp.Id equals a.IdSubPeriodo
                                                  join pd in contexto.ProfessorDisciplina on a.IdProfessorDisciplina equals pd.Id
                                                  join d in contexto.Disciplina on pd.IdDisciplina equals d.Id
                                                  join n in contexto.Notas on a.Id equals n.IdAvaliacao
                                                  join i in contexto.Inscricao on n.IdInscricao equals i.Id
                                                  join al in contexto.Aluno on i.IdAluno equals al.Id
                                                  join ta in contexto.TipoAvaliacao on a.IdTipoAvaliacao equals ta.Id
                                                  where al.Id == idAluno && p.Id == idPeriodo && !p.Excluido && !sp.Excluido && !pd.Excluido && !d.Excluido
                                                      && !a.Excluido && !n.Excluido && !i.Excluido && !al.Excluido && ta.Codigo == "MEDIA"
                                                      && (n.Nota.HasValue || n.NotaRecuperacao.HasValue || n.Faltas.HasValue)
                                                  group new { sp, d, a, n } by new { IdBimestre = sp.Id, IdDisciplina = d.Id, a.IdTurma }
                                                  into g
                                                  orderby g.Select(s => s.d.Nome).FirstOrDefault()
                                                  select new CmDisciplinaMedias()
                                                  {
                                                      IdDisciplina = g.Select(s => s.d.Id).FirstOrDefault(),
                                                      NomeDisciplina = g.Select(s => s.d.Nome).FirstOrDefault(),
                                                      IdTurma = g.Select(s => s.a.IdTurma).FirstOrDefault(),
                                                      IdSubPeriodo = g.Select(s => s.sp.Id).FirstOrDefault(),
                                                      MediaFaltas = g.Select(s => s.n.Faltas).FirstOrDefault(),
                                                      MediaNotasRecuperacao = g.Select(s => s.n.NotaRecuperacao).FirstOrDefault(),
                                                      MediaNotas = g.Select(s => s.n.Nota).FirstOrDefault(),
                                                      MediaAposRecuperacao = g.Select(s => ((s.n.Nota ?? 0) > (s.n.NotaRecuperacao ?? 0)) ? (s.n.Nota ?? 0) : (s.n.NotaRecuperacao ?? 0)).FirstOrDefault()
                                                  }
                                            )?.ToList();


                return lista;
            }
        }

        public List<Disciplina> ObterTodosDisciplinas(Guid idTurma)
        {
            using (SchoolContext contexto = new SchoolContext())
            {
                List<Disciplina> lista = (from dh in contexto.DisciplinaHorario
                                          join d in contexto.Disciplina on dh.IdDisciplina equals d.Id
                                          where dh.IdTurma == idTurma && dh.Excluido == false && !d.Excluido
                                          select d).Distinct().ToList();
                return lista;
            }
        }

        public List<CmProfessorDisciplina> ObterTodosProfessorComDisciplina(Guid idProfessor, Guid idEscola)
        {
            using (SchoolContext contexto = new SchoolContext())
            {
                List<CmProfessorDisciplina> lista = (from ep in contexto.EscolaProfessor
                                                     join pd in contexto.ProfessorDisciplina on ep.Id equals pd.IdEscolaProfessor
                                                     join d in contexto.Disciplina on pd.IdDisciplina equals d.Id
                                                     where ep.IdEscola == idEscola && ep.IdProfessor == idProfessor && ep.Excluido == false
                                                     && pd.Excluido == false && d.Excluido == false
                                                     orderby d.Nome
                                                     select new CmProfessorDisciplina()
                                                     {
                                                         Id = pd.Id,
                                                         IdDisciplina = d.Id,
                                                         NomeDisciplina = d.Nome,
                                                         IdProfessor = ep.IdProfessor,
                                                         IdProfessorEscola = ep.Id,
                                                         Excluido = pd.Excluido
                                                     }).Distinct().ToList();
                return lista;
            }
        }
    }
}
