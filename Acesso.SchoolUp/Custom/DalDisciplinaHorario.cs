using Acesso.SchoolUp.Contexts;
using Modelo.SchoolUp.Custom;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Acesso.SchoolUp.Custom
{
    public class DalDisciplinaHorario
    {
        public List<CmDisciplinaHorario> ObterTodos(Guid idTurma)
        {
            using (SchoolContext contexto = new SchoolContext())
            {
                List<CmDisciplinaHorario> lista = (from ht in contexto.HorarioTurno.Where(i => !i.Ativo.HasValue || i.Ativo.Value)
                                                   join tn in contexto.Turno on ht.IdTurno equals tn.Id
                                                   join tm in contexto.Turma on tn.Id equals tm.IdTurno
                                                   join dh in contexto.DisciplinaHorario.Where(i => !i.Excluido) 
                                                   on new { IdTurno = ht.Id, IdTurma = tm.Id } equals new { IdTurno = dh.IdHorarioTurno.Value, IdTurma = dh.IdTurma  } into dhl
                                                   from dh in dhl.DefaultIfEmpty()
                                                       //join t in contexto.Turma on dh.IdTurma equals t.Id into tl
                                                       //from t in tl.DefaultIfEmpty()
                                                       //join pe in contexto.Periodo on tm.IdPeriodo equals pe.Id into pel
                                                       //from pe in pel.DefaultIfEmpty()
                                                   join d in contexto.Disciplina on dh.IdDisciplina.Value equals d.Id into dl
                                                   from d in dl.DefaultIfEmpty()
                                                   join p in contexto.Professor on dh.IdProfessor.Value equals p.Id into pl
                                                   from p in pl.DefaultIfEmpty()
                                                   where tm.Id == idTurma
                                                   //(!ht.Ativo.HasValue || ht.Ativo.Value) && (tm.Id == idTurma                                                    
                                                   //|| (dh.IdTurma == null && (ht.IdTurno == (
                                                   //from tw in contexto.Turma where tw.Id == idTurma select tw.IdTurno.Value).FirstOrDefault())))
                                                   orderby ht.Nome //, dh.Dia
                                                   select new CmDisciplinaHorario()
                                                   {
                                                       Id = dh.Id,
                                                       Dia = dh.Dia,
                                                       IdDisciplina = dh.IdDisciplina,
                                                       nomeDisciplina = d.Nome,
                                                       IdHorarioTurno = ht.Id,
                                                       nomeHorarioTurno = ht.Nome,
                                                       IdTurma = tm.Id,
                                                       nomeTurma = tm.Nome,
                                                       IdProfessor = p.Id,
                                                       nomeProfessor = p.Nome,
                                                       Inicio = ht.Inicio.Value,
                                                       Termino = ht.Termino.Value
                                                   }).ToList();
                return lista;
            }
        }

    }
}