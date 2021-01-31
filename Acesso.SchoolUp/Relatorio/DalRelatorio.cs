using Acesso.Nucleo.Contexts;
using Acesso.SchoolUp.Contexts;
using Modelo.SchoolUp.Relatorio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Acesso.SchoolUp.Relatorio
{
    public class DalRelatorio
    {
        public List<RptAcessoResponsavel> ObterAcessoResponsavel(RptAcessoResponsavel mdlRptAcessoResponsavel)
        {
            List<RptAcessoResponsavel> lstRptAcessoResponsavel = new List<RptAcessoResponsavel>();
            using (SchoolContext contexto = new SchoolContext())
            {
                lstRptAcessoResponsavel = (from a in contexto.Aluno
                                              join at in contexto.AlunoTurma on a.Id equals at.IdAluno
                                              join t in contexto.Turma on at.IdTurma equals t.Id
                                              join p in contexto.Periodo on t.IdPeriodo equals p.Id
                                              join ra in contexto.ResponsavelAluno on a.Id equals ra.IdAluno
                                              join r in contexto.Responsavel on ra.IdResponsavel equals r.Id
                                              where p.IdEscola == mdlRptAcessoResponsavel.IdEscola 
                                              && (String.IsNullOrEmpty(mdlRptAcessoResponsavel.IdTurma)
                                                || t.Id.ToString() == mdlRptAcessoResponsavel.IdTurma)
                                              && (String.IsNullOrEmpty(mdlRptAcessoResponsavel.IdAluno) 
                                                || a.Id.ToString() == mdlRptAcessoResponsavel.IdAluno)
                                              && !r.Excluido && !ra.Excluido 
                                              && !a.Excluido && !at.Excluido && !t.Excluido
                                              select new RptAcessoResponsavel()
                                              {
                                                  IdAluno = a.Id.ToString(),
                                                  IdResponsavel = r.Id.ToString(),
                                                  NomeAluno = a.Nome,
                                                  Turma = t.Nome,
                                                  NomeResponsavel = r.Nome,                                                  
                                                  IdUser = r.IdUser.Value.ToString() ?? ""
                                              })?.ToList();
            }

            if (lstRptAcessoResponsavel?.Any() != true)
            {
                return new List<RptAcessoResponsavel>();
            }

            using (PadraoContext contextoPadrao = new PadraoContext())
            {
                List<RptAcessoResponsavel> listaAcessoResponsavel = new List<RptAcessoResponsavel>();
                if (mdlRptAcessoResponsavel.IsAgrupar)
                {
                    listaAcessoResponsavel = (from f in contextoPadrao.Funcoes
                                              join h in contextoPadrao.Historico on f.IdFuncao equals h.IdFuncao
                                              join a in contextoPadrao.Acoes on h.IdAcao equals a.IdAcao
                                              join u in contextoPadrao.Usuario on h.IdUsuario.ToUpper() equals u.IdUsuario.ToUpper()
                                              join au in contextoPadrao.AspNetUsers on u.UserId.ToUpper() equals au.Id.ToUpper()
                                              join pu in contextoPadrao.PerfilUsuario on u.IdUsuario.ToUpper() equals pu.IdUsuario.ToUpper()
                                              join p in contextoPadrao.Perfis on pu.IdPerfil.ToUpper() equals p.IdPerfil.ToUpper()
                                              join r in lstRptAcessoResponsavel on au.Id.ToUpper() equals r.IdUser.ToUpper()
                                              where f.Codigo == "SCHOOLUP_ACESSO" && a.Codigo == "LOGIN" &&
                                              (p.Codigo == "SCHOOLUP_ALUNO" || p.Codigo == "SCHOOLUP_RESPONSAVEL")
                                              && !u.Excluido
                                              && (mdlRptAcessoResponsavel.Inicio == null
                                              || DateTime.Compare(h.DataAlteracao.Date, mdlRptAcessoResponsavel.Inicio.Value.Date) > -1)
                                              && (mdlRptAcessoResponsavel.Fim == null
                                              || DateTime.Compare(mdlRptAcessoResponsavel.Fim.Value.Date, h.DataAlteracao.Date) > -1)
                                              group new { r, au, h } by new
                                              {
                                                  IdAluno = r.IdAluno,
                                                  IdResponsavel = r.IdResponsavel,
                                                  IdUser = r.IdUser,
                                                  Turma = r.Turma,
                                                  NomeAluno = r.NomeAluno,
                                                  NomeResponsavel = r.NomeResponsavel,
                                                  Login = au.UserName
                                              } into g
                                              orderby g.Select(s => s.r.NomeAluno).FirstOrDefault()
                                              select new RptAcessoResponsavel()
                                              {
                                                  IsAgrupar = g.Select(s => true).FirstOrDefault(),
                                                  IdAluno = g.Select(s => s.r.IdAluno).FirstOrDefault(),
                                                  IdResponsavel = g.Select(s => s.r.IdResponsavel).FirstOrDefault(),
                                                  IdUser = g.Select(s => s.r.IdUser).FirstOrDefault(),
                                                  Turma = g.Select(s => s.r.Turma).FirstOrDefault(),
                                                  NomeAluno = g.Select(s => s.r.NomeAluno).FirstOrDefault(),
                                                  NomeResponsavel = g.Select(s => s.r.NomeResponsavel).FirstOrDefault(),
                                                  Login = g.Select(s => s.au.UserName).FirstOrDefault(),
                                                  DataAcessoMaxima = g.Max(m => m.h.DataAlteracao),
                                                  DataAcessoMinima = g.Min(m => m.h.DataAlteracao),
                                                  NumeroAcessos = g.Count()
                                              })?.ToList();

                }
                else
                {
                    listaAcessoResponsavel = (from f in contextoPadrao.Funcoes
                                              join h in contextoPadrao.Historico on f.IdFuncao equals h.IdFuncao
                                              join a in contextoPadrao.Acoes on h.IdAcao equals a.IdAcao
                                              join u in contextoPadrao.Usuario on h.IdUsuario.ToUpper() equals u.IdUsuario.ToUpper()
                                              join au in contextoPadrao.AspNetUsers on u.UserId.ToUpper() equals au.Id.ToUpper()
                                              join pu in contextoPadrao.PerfilUsuario on u.IdUsuario.ToUpper() equals pu.IdUsuario.ToUpper()
                                              join p in contextoPadrao.Perfis on pu.IdPerfil.ToUpper() equals p.IdPerfil.ToUpper()
                                              join r in lstRptAcessoResponsavel on au.Id.ToUpper() equals r.IdUser.ToUpper()
                                              where f.Codigo == "SCHOOLUP_ACESSO" && a.Codigo == "LOGIN" &&
                                              (p.Codigo == "SCHOOLUP_ALUNO" || p.Codigo == "SCHOOLUP_RESPONSAVEL")
                                              && !u.Excluido
                                              && (mdlRptAcessoResponsavel.Inicio == null
                                              || DateTime.Compare(h.DataAlteracao.Date, mdlRptAcessoResponsavel.Inicio.Value.Date) > -1)
                                              && (mdlRptAcessoResponsavel.Fim == null
                                              || DateTime.Compare(mdlRptAcessoResponsavel.Fim.Value.Date, h.DataAlteracao.Date) > -1)
                                              orderby h.DataAlteracao descending, r.NomeAluno
                                              select new RptAcessoResponsavel()
                                              {
                                                  IsAgrupar = false,
                                                  IdAluno = r.IdAluno,
                                                  IdResponsavel = r.IdResponsavel,
                                                  IdUser = r.IdUser,
                                                  Turma = r.Turma,
                                                  NomeAluno = r.NomeAluno,
                                                  NomeResponsavel = r.NomeResponsavel,
                                                  Login = au.UserName,
                                                  DataAcesso = h.DataAlteracao
                                              })?.ToList();

                }

                return listaAcessoResponsavel;
            }

        }
    }
}
