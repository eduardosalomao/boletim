using Acesso.SchoolUp.Contexts;
using Modelo.SchoolUp.Custom;
using Modelo.SchoolUp.Principal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Acesso.SchoolUp.Custom
{
    public class DalSubPeriodo
    {
        public List<CmSubPeriodo> ObterTodos(Guid idEscola)
        {
            using (SchoolContext contexto = new SchoolContext())
            {
                List<CmSubPeriodo> lista = (from s in contexto.SubPeriodo
                                            join p in contexto.Periodo on s.IdPeriodo equals p.Id
                                            where p.IdEscola.Equals(idEscola) && !s.Excluido
                                            orderby p.Nome descending, s.Nome
                                            select new CmSubPeriodo()
                                            {
                                                Id = s.Id,
                                                IdPeriodo = s.IdPeriodo,
                                                Nome = s.Nome,
                                                NomePeriodo = p.Nome,
                                                Codigo = s.Codigo,
                                                De = s.De,
                                                Ate = s.Ate
                                       }).ToList();
                return lista;
            }
        }
    }
}
