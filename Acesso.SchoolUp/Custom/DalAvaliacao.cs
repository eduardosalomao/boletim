using Acesso.SchoolUp.Contexts;
using Modelo.SchoolUp.Custom;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Acesso.SchoolUp.Custom
{
    public class DalAvaliacao
    {
        public List<CmAvaliacao> ObterTodos(Guid? idTurma, Guid? idSubPeriodo)
        {
            using (SchoolContext contexto = new SchoolContext())
            {
                List<CmAvaliacao> lista = (from a in contexto.Avaliacao
                                           join sp in contexto.SubPeriodo on a.IdSubPeriodo equals sp.Id
                                           join pr in contexto.Periodo on sp.IdPeriodo equals pr.Id
                                           join pd in contexto.ProfessorDisciplina on a.IdProfessorDisciplina equals pd.Id
                                           join ta in contexto.TipoAvaliacao on a.IdTipoAvaliacao equals ta.Id
                                           join d in contexto.Disciplina on pd.IdDisciplina equals d.Id
                                           join ep in contexto.EscolaProfessor on pd.IdEscolaProfessor equals ep.Id
                                           join p in contexto.Professor on ep.IdProfessor equals p.Id
                                           join t in contexto.Turma on a.IdTurma equals t.Id
                                           where (idTurma == null || idTurma == Guid.Empty || a.IdTurma == idTurma.Value) 
                                           && (idSubPeriodo == null || idSubPeriodo == Guid.Empty || a.IdSubPeriodo == idSubPeriodo) && a.Excluido == false
                                           && !sp.Excluido && !pr.Excluido && !pd.Excluido && !ta.Excluido && !d.Excluido && !ep.Excluido && !p.Excluido && !t.Excluido
                                           && ta.Codigo != "MEDIA"
                                           orderby ta.Codigo + a.Sigla
                                           select new CmAvaliacao()
                                           {
                                               Id = a.Id,
                                               IdSubPeriodo = a.IdSubPeriodo,
                                               IdPeriodo = pr.Id,
                                               IdProfessorDisciplina = a.IdProfessorDisciplina,
                                               IdTipoAvaliacao = a.IdTipoAvaliacao,
                                               IdTurma = a.IdTurma,
                                               AulasDadas = a.AulasDadas,
                                               AulasPrevistas = a.AulasPrevistas,
                                               De = a.De,
                                               Ate = a.Ate,
                                               Peso = a.Peso,
                                               Sigla = a.Sigla,
                                               NomeAvaliacao = ta.Nome,
                                               NomeDisciplina = d.Nome,
                                               NomeProfessor = p.Nome,
                                               NomeSubPeriodo = sp.Nome,
                                               NomePeriodo = pr.Nome,
                                               NomeTurma = t.Nome,
                                               Excluido = a.Excluido
                                           })?.OrderBy(o => o.De.Value)?.OrderBy(o => o.Ate)?.ToList();
                return lista;
            }
        }

        public List<CmNotas> ObterTodos(Guid idAvaliacao)
        {
            using (SchoolContext contexto = new SchoolContext())
            {
                List<CmNotas> lista = (from n in contexto.Notas
                                       join i in contexto.Inscricao on n.IdInscricao equals i.Id
                                       join al in contexto.Aluno on i.IdAluno equals al.Id
                                       join av in contexto.Avaliacao on n.IdAvaliacao equals av.Id
                                       join b in contexto.SubPeriodo on av.IdSubPeriodo equals b.Id
                                       join t in contexto.Turma on i.IdTurma equals t.Id
                                       join pd in contexto.ProfessorDisciplina on av.IdProfessorDisciplina equals pd.Id
                                       join d in contexto.Disciplina on pd.IdDisciplina equals d.Id
                                       where n.IdAvaliacao == idAvaliacao && i.Excluido == false && n.Excluido == false
                                       && al.Excluido == false && av.Excluido == false
                                       orderby al.Nome
                                       select new CmNotas()
                                       {
                                           Id = n.Id,
                                           IdAvaliacao = n.IdAvaliacao,
                                           IdInscricao = n.IdInscricao,
                                           NomeAluno = al.Nome,
                                           IdDisciplina = d.Id,
                                           Disciplina = d.Nome,
                                           IdTurma = t.Id,
                                           Turma = t.Nome,
                                           IdBimestre = b.Id,
                                           Bimestre = b.Nome,
                                           MatriculaAluno = al.Matricula,
                                           Faltas = n.Faltas,
                                           Nota = n.Nota,
                                           NotaRecuperacao = n.NotaRecuperacao,
                                           Excluido = n.Excluido
                                       }).ToList();
                return lista;
            }
        }


        public List<CmNotas> ObterTodosAluno(Guid idAluno)
        {
            using (SchoolContext contexto = new SchoolContext())
            {
                List<CmNotas> lista = (from n in contexto.Notas
                                       join i in contexto.Inscricao on n.IdInscricao equals i.Id
                                       join al in contexto.Aluno on i.IdAluno equals al.Id
                                       join av in contexto.Avaliacao on n.IdAvaliacao equals av.Id
                                       join b in contexto.SubPeriodo on av.IdSubPeriodo equals b.Id
                                       join t in contexto.Turma on i.IdTurma equals t.Id
                                       join pd in contexto.ProfessorDisciplina on av.IdProfessorDisciplina equals pd.Id
                                       join d in contexto.Disciplina on pd.IdDisciplina equals d.Id
                                       where al.Id == idAluno && i.Excluido == false && n.Excluido == false
                                       && al.Excluido == false && av.Excluido == false && b.Excluido == false
                                       orderby al.Nome
                                       select new CmNotas()
                                       {
                                           Id = n.Id,
                                           IdAvaliacao = n.IdAvaliacao,
                                           IdInscricao = n.IdInscricao,
                                           NomeAluno = al.Nome,
                                           IdDisciplina = d.Id,
                                           Disciplina = d.Nome,
                                           IdTurma = t.Id,
                                           Turma = t.Nome,
                                           IdBimestre = b.Id,
                                           Bimestre = b.Nome,
                                           MatriculaAluno = al.Matricula,
                                           Faltas = n.Faltas,
                                           Nota = n.Nota,
                                           NotaRecuperacao = n.NotaRecuperacao,
                                           Excluido = n.Excluido
                                       }).ToList();
                return lista;
            }
        }

        public List<CmNotas> ObterTodosAluno(Guid idAluno, Guid? idPeriodo, Guid? idBimestre)
        {
            using (SchoolContext contexto = new SchoolContext())
            {
                List<CmNotas> lista = (from n in contexto.Notas
                                       join i in contexto.Inscricao on n.IdInscricao equals i.Id
                                       join al in contexto.Aluno on i.IdAluno equals al.Id
                                       join av in contexto.Avaliacao on n.IdAvaliacao equals av.Id
                                       join b in contexto.SubPeriodo on av.IdSubPeriodo equals b.Id
                                       join t in contexto.Turma on i.IdTurma equals t.Id
                                       join pd in contexto.ProfessorDisciplina on av.IdProfessorDisciplina equals pd.Id
                                       join d in contexto.Disciplina on pd.IdDisciplina equals d.Id
                                       join p in contexto.Periodo on b.IdPeriodo equals p.Id
                                       where al.Id == idAluno && (idBimestre == null || b.Id == idBimestre.Value) && (idPeriodo == null || p.Id == idPeriodo.Value)
                                       && i.Excluido == false && n.Excluido == false && pd.Excluido == false && t.Excluido == false
                                       && al.Excluido == false && av.Excluido == false && b.Excluido == false && p.Excluido == false
                                       orderby al.Nome
                                       select new CmNotas()
                                       {
                                           Id = n.Id,
                                           IdAvaliacao = n.IdAvaliacao,
                                           IdInscricao = n.IdInscricao,
                                           NomeAluno = al.Nome,
                                           IdDisciplina = d.Id,
                                           Disciplina = d.Nome,
                                           IdTurma = t.Id,
                                           Turma = t.Nome,
                                           IdBimestre = b.Id,
                                           Bimestre = b.Nome,
                                           IdPeriodo = p.Id,
                                           Periodo = p.Nome,
                                           MatriculaAluno = al.Matricula,
                                           Faltas = n.Faltas,
                                           Nota = n.Nota,
                                           NotaRecuperacao = n.NotaRecuperacao,
                                           Excluido = n.Excluido
                                       }).ToList();
                return lista;
            }
        }

        public List<CmAvaliacao> ObterTodosDisciplina(Guid idTurma, Guid idSubPeriodo)
        {
            using (SchoolContext contexto = new SchoolContext())
            {
                List<CmAvaliacao> lista = (from a in contexto.Avaliacao
                                           join sp in contexto.SubPeriodo on a.IdSubPeriodo equals sp.Id
                                           join pd in contexto.ProfessorDisciplina on a.IdProfessorDisciplina equals pd.Id
                                           join ta in contexto.TipoAvaliacao on a.IdTipoAvaliacao equals ta.Id
                                           join d in contexto.Disciplina on pd.IdDisciplina equals d.Id
                                           join ep in contexto.EscolaProfessor on pd.IdEscolaProfessor equals ep.Id
                                           join p in contexto.Professor on ep.IdProfessor equals p.Id
                                           join t in contexto.Turma on a.IdTurma equals t.Id
                                           where a.IdTurma == idTurma && a.IdSubPeriodo == idSubPeriodo && a.Excluido == false
                                           && ta.Codigo == "MEDIA"
                                           orderby d.Nome
                                           select new CmAvaliacao()
                                           {
                                               Id = a.Id,
                                               IdSubPeriodo = a.IdSubPeriodo,
                                               IdProfessorDisciplina = a.IdProfessorDisciplina,
                                               IdTipoAvaliacao = a.IdTipoAvaliacao,
                                               IdTurma = a.IdTurma,
                                               AulasDadas = a.AulasDadas,
                                               AulasPrevistas = a.AulasPrevistas,
                                               De = a.De,
                                               Ate = a.Ate,
                                               Peso = a.Peso,
                                               Sigla = a.Sigla,
                                               NomeAvaliacao = ta.Nome,
                                               NomeDisciplina = d.Nome,
                                               NomeProfessor = p.Nome,
                                               NomeSubPeriodo = sp.Nome,
                                               NomeTurma = t.Nome,
                                               Excluido = a.Excluido
                                           }).ToList();
                return lista;
            }
        }
    }
}
