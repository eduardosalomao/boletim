using Acesso.SchoolUp.Contexts;
using Modelo.SchoolUp.Custom;
using Modelo.SchoolUp.Principal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Acesso.SchoolUp.Custom
{
    public class DalProfessor
    {
        public List<CmProfessorDisciplina> ObterTodosProfessorComDisciplina(Guid idEscola)
        {
            using (SchoolContext contexto = new SchoolContext())
            {
                List<CmProfessorDisciplina> lista = (from pd in contexto.ProfessorDisciplina
                                                     join d in contexto.Disciplina on pd.IdDisciplina equals d.Id
                                                     join ep in contexto.EscolaProfessor on pd.IdEscolaProfessor equals ep.Id
                                                     join p in contexto.Professor on ep.IdProfessor equals p.Id
                                                    where ep.IdEscola == idEscola && d.IdEscola == idEscola && ep.Excluido == false && 
                                                    p.Ativo == true && p.Excluido == false && d.Excluido == false && pd.Excluido == false
                                                    orderby p.Nome, d.Nome
                                                    select new CmProfessorDisciplina()
                                                    {
                                                        Id = pd.Id,
                                                        IdProfessor = p.Id,
                                                        IdDisciplina = pd.IdDisciplina,
                                                        NomeProfessor = p.Nome,
                                                        NomeProfessorDisciplina = p.Nome + " - " + d.Nome
                                                    }).Distinct().ToList();
                return lista;
            }
        }

        public List<CmProfessorDisciplina> ObterTodosProfessorPorTurma(Guid idTurma)
        {
            using (SchoolContext contexto = new SchoolContext())
            {
                List<CmProfessorDisciplina> lista = (from pd in contexto.ProfessorDisciplina
                                                     join d in contexto.Disciplina on pd.IdDisciplina equals d.Id
                                                     join ep in contexto.EscolaProfessor on pd.IdEscolaProfessor equals ep.Id
                                                     join p in contexto.Professor on ep.IdProfessor equals p.Id
                                                     join dh in contexto.DisciplinaHorario on new { IdDisciplina = d.Id, IdProfessor = p.Id } equals new { IdDisciplina = dh.IdDisciplina.Value, IdProfessor = dh.IdProfessor.Value }
                                                     where dh.IdTurma == idTurma && ep.Excluido == false && dh.Excluido == false &&
                                                     p.Ativo == true && p.Excluido == false && d.Excluido == false && pd.Excluido == false
                                                     orderby p.Nome, d.Nome
                                                     select new CmProfessorDisciplina()
                                                     {
                                                         Id = pd.Id,
                                                         IdProfessor = p.Id,
                                                         IdDisciplina = pd.IdDisciplina,
                                                         NomeProfessor = p.Nome,
                                                         NomeProfessorDisciplina = p.Nome + " - " + d.Nome
                                                     })?.Distinct()?.ToList();
                return lista?.OrderBy(o => o.NomeProfessorDisciplina)?.ToList();
            }
        }

        public List<CmProfessorDisciplina> ObterTodosProfessorPorDisciplina(Guid idDisciplina)
        {
            using (SchoolContext contexto = new SchoolContext())
            {
                List<CmProfessorDisciplina> lista = (from pd in contexto.ProfessorDisciplina
                                                     join ep in contexto.EscolaProfessor on pd.IdEscolaProfessor equals ep.Id
                                                     join p in contexto.Professor on ep.IdProfessor equals p.Id
                                                     where pd.IdDisciplina == idDisciplina && ep.Excluido == false &&
                                                     p.Ativo == true && p.Excluido == false && pd.Excluido == false
                                                     orderby p.Nome
                                                     select new CmProfessorDisciplina()
                                                     {
                                                         IdProfessor = p.Id,
                                                         NomeProfessor = p.Nome
                                                     }).Distinct().ToList();
                return lista;
            }
        }
    }
}
