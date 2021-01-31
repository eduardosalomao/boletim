using Acesso.SchoolUp.Contexts;
using Modelo.SchoolUp.Custom;
using Modelo.SchoolUp.Principal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Acesso.SchoolUp.Custom
{
    public class DalSerie
    {
        public List<CmSerie> ObterTodos(Guid idEscola)
        {
            using (SchoolContext contexto = new SchoolContext())
            {
                List<CmSerie> lista = (from s in contexto.Serie
                                       join e in contexto.Ensino on s.IdEnsino equals e.Id into el
                                       from e in el.DefaultIfEmpty()
                                       where e.IdEscola.Equals(idEscola) && s.Ativo.HasValue && s.Ativo.Value
                                       orderby s.Nome
                                       select new CmSerie()
                                       {
                                           Id = s.Id,
                                           IdEnsino = s.IdEnsino,
                                           Nome = s.Nome,
                                           NomeEnsino = e.Nome,
                                           Codigo = s.Codigo
                                       }).ToList();
                return lista;
            }
        }

        public List<Serie> ObterTodosCombo(Guid idEscola)
        {
            using (SchoolContext contexto = new SchoolContext())
            {
                List<Serie> lista = (from s in contexto.Serie
                                       join e in contexto.Ensino on s.IdEnsino equals e.Id into el
                                       from e in el.DefaultIfEmpty()
                                       where e.IdEscola.Equals(idEscola) && s.Ativo.HasValue && s.Ativo.Value
                                       orderby s.Nome
                                       select new Serie() { Id = s.Id, Nome = $"{s.Nome} - {e.Nome}" }
                                       ).ToList();
                return lista;
            }
        }
    }
}
