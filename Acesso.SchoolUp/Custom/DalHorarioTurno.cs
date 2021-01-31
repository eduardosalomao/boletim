using Acesso.SchoolUp.Contexts;
using Modelo.SchoolUp.Custom;
using Modelo.SchoolUp.Principal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Acesso.SchoolUp.Custom
{
    public class DalHorarioTurno
    {
        public List<CmHorarioTurno> ObterTodos(Guid idEscola)
        {
            using (SchoolContext contexto = new SchoolContext())
            {
                List<CmHorarioTurno> lista = (from h in contexto.HorarioTurno
                                            join t in contexto.Turno on h.IdTurno equals t.Id
                                            where t.IdEscola.Equals(idEscola) && h.Ativo.HasValue && h.Ativo.Value
                                            orderby t.Nome, h.Nome
                                            select new CmHorarioTurno()
                                            {
                                                Id = h.Id,
                                                IdTurno = t.Id,
                                                Nome = h.Nome,
                                                NomeTurno = t.Nome,
                                                Codigo = h.Codigo,
                                                Inicio = h.Inicio,
                                                Termino = h.Termino
                                       }).ToList();
                return lista;
            }
        }
    }
}
