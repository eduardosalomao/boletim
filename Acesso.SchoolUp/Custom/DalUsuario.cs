using Acesso.Nucleo.Contexts;
using Acesso.SchoolUp.Contexts;
using Modelo.Nucleo.Models;
using Modelo.SchoolUp.Custom;
using Modelo.SchoolUp.Principal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Acesso.SchoolUp.Custom
{
    public class DalUsuario
    {
        public CmUsuario Obter(Guid idUsuario)
        {
            using (SchoolContext contexto = new SchoolContext())
            {
                List<string> perfis = new List<string>();
                Usuario usuario;
                using (PadraoContext padrao = new PadraoContext())
                {
                    usuario = (from u in padrao.Usuario
                               where u.UserId == idUsuario.ToString() && u.Ativo && !u.Excluido && !u.Reservado
                               select new Usuario()
                               {
                                   IdUsuario = u.IdUsuario,
                                   Nome = u.Nome
                               })?.FirstOrDefault();

                    perfis = (from pu in padrao.PerfilUsuario
                                           join p in padrao.Perfis on pu.IdPerfil equals p.IdPerfil
                                           where pu.IdUsuario == idUsuario.ToString() && p.Ativo
                                           select p.Codigo)?.Distinct()?.ToList();
                    
                }

                CmUsuario cmUsuario = (from eu in contexto.EscolaUsuario
                                       where eu.IdUser == idUsuario
                                       select new CmUsuario()
                                       {
                                           IdEscola = eu.IdEscola,
                                           Nome = usuario.Nome
                                       })?.FirstOrDefault();

                if (cmUsuario != null)
                {
                    cmUsuario.PerfilCodigo = perfis;

                    return cmUsuario;
                }

                cmUsuario = (from r in contexto.Responsavel
                             join ra in contexto.ResponsavelAluno on r.Id equals ra.IdResponsavel
                             join a in contexto.Aluno on ra.IdAluno equals a.Id
                             where r.IdUser == idUsuario && !r.Excluido && !a.Excluido && !ra.Excluido && a.Ativo.HasValue && a.Ativo.Value
                             select new CmUsuario()
                             {
                                 IdEscola = a.IdEscola,
                                 Nome = r.Email
                             })?.FirstOrDefault();


                List<CmAluno> listaAlunos = (from r in contexto.Responsavel
                             join ra in contexto.ResponsavelAluno on r.Id equals ra.IdResponsavel
                             join a in contexto.Aluno on ra.IdAluno equals a.Id
                             join at in contexto.AlunoTurma on a.Id equals at.IdAluno
                             where r.IdUser == idUsuario && !r.Excluido && !a.Excluido && !ra.Excluido && a.Ativo.HasValue && a.Ativo.Value && !at.Excluido
                                           select new CmAluno() { Id = a.Id,
                                                                  Nome = a.Nome,
                                                                  IdTurma = at.IdTurma,
                                                                  IdEscola = a.IdEscola
                                                    } )?.ToList();

                cmUsuario.Alunos = listaAlunos;
                cmUsuario.PerfilCodigo = perfis;

                return cmUsuario;
            }
        }

        public AspNetUsers Obter(string login)
        {
                AspNetUsers usuario;
                using (PadraoContext padrao = new PadraoContext())
                {
                    usuario = (from u in padrao.AspNetUsers
                               where u.UserName == login
                               select u)?.FirstOrDefault();

                }

                return usuario;
        }

    }
}
