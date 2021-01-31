using Microsoft.AspNetCore.Mvc;
using Modelo.Nucleo.Geral;
using Modelo.SchoolUp.Principal;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Modelo.SchoolUp.Custom;
using Modelo.SchoolUp.Recursos;
using System.Collections.Generic;
using Modelo.SchoolUp.Enumeracao;

namespace MVC.SchoolUp.Controllers
{
    public class ResponsavelController : MasterController
    {
        public ResponsavelController() : base()
        {
        }

        public IActionResult GetSubPeriodo(string idPeriodo)
        {
            var idUsuario = HttpContext?.Session?.GetString("IdUsuario");
            var idEscola = HttpContext?.Session?.GetString("IdEscola");
            Guid idUsuarioGuid;
            if (!Guid.TryParse(idUsuario, out idUsuarioGuid))
            {
                TempData["PaginaAnterior"] = String.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, Request.Path, Request.QueryString);
                TempData["MensagemErro"] = "Sessão expirada";
                return RedirectToAction("Login", "Acesso");
            }

            GlResposta<SubPeriodo> resultSubPeriodo = GetAux<SubPeriodo>("Servico/ObterSubPeriodo", idPeriodo);
            if (resultSubPeriodo.Succeeded == false)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = resultSubPeriodo.Mensagem;
                return RedirectToAction("Erro", "Home");
            }

            return Ok(resultSubPeriodo);
        }

        public IActionResult GetTurmasAluno(string idPeriodo, string idTurmaAluno)
        {
            var idUsuario = HttpContext?.Session?.GetString("IdUsuario");
            var idEscola = HttpContext?.Session?.GetString("IdEscola");
            Guid idUsuarioGuid;
            if (!Guid.TryParse(idUsuario, out idUsuarioGuid))
            {
                TempData["PaginaAnterior"] = String.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, Request.Path, Request.QueryString);
                TempData["MensagemErro"] = "Sessão expirada";
                return RedirectToAction("Login", "Acesso");
            }

            GlResposta<Turma> resultTurma = GetAux<Turma>("Servico/ObterTurma", idPeriodo);
            resultTurma.Dados = resultTurma.Dados.Where(w => idTurmaAluno.Contains(w.Id.ToString())).ToList();
            if (resultTurma.Succeeded == false)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = resultTurma.Mensagem;
                return RedirectToAction("Erro", "Home");
            }

            return Ok(resultTurma.Dados);
        }

        public IActionResult GetTurmas(string idPeriodo, string idAluno)
        {
            var idUsuario = HttpContext?.Session?.GetString("IdUsuario");
            var idEscola = HttpContext?.Session?.GetString("IdEscola");
            Guid idUsuarioGuid;
            if (!Guid.TryParse(idUsuario, out idUsuarioGuid))
            {
                TempData["PaginaAnterior"] = String.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, Request.Path, Request.QueryString);
                TempData["MensagemErro"] = "Sessão expirada";
                return RedirectToAction("Login", "Acesso");
            }

            GlResposta<CmTurmaAluno> resultTurma = GetAux<CmTurmaAluno>("Servico/ObterTurmaAluno", new string[] { idAluno, idPeriodo });
            if (resultTurma.Succeeded == false)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = resultTurma.Mensagem;
                return RedirectToAction("Erro", "Home");
            }

            return Ok(resultTurma.Dados);
        }

        public IActionResult GetCalendario(string idTurma, string idSubPeriodo)
        {
            var idUsuario = HttpContext?.Session?.GetString("IdUsuario");
            var idEscola = HttpContext?.Session?.GetString("IdEscola");
            Guid idUsuarioGuid;
            if (!Guid.TryParse(idUsuario, out idUsuarioGuid))
            {
                TempData["PaginaAnterior"] = String.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, Request.Path, Request.QueryString);
                TempData["MensagemErro"] = "Sessão expirada";
                return RedirectToAction("Login", "Acesso");
            }

            //if (String.IsNullOrEmpty(idTurma))
            //{
            //    ViewBag.IsSucesso = "false";
            //    ViewBag.LblMensagem = "Selecione uma turma";

            //    return PartialView("PartialGradeCalendario", new GlResposta<CmAvaliacao>() { Mensagem = Mensagens.SemRegistroEncontrado });
            //}

            //if (String.IsNullOrEmpty(idSubPeriodo))
            //{
            //    ViewBag.IsSucesso = "false";
            //    ViewBag.LblMensagem = "Selecione um bimestre";

            //    return PartialView("PartialGradeCalendario", new GlResposta<CmAvaliacao>() { Mensagem = Mensagens.SemRegistroEncontrado });
            //}

            GlResposta<CmAvaliacao> result = Get<CmAvaliacao>("Avaliacao/ObterGrade", new string[] { idTurma, idSubPeriodo });
            ViewBag.IsSucesso = "";
            if (result == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }
            if (!result.Succeeded)
            {
                ViewBag.IsSucesso = "false";
                ViewBag.LblMensagem = result.Mensagem;
            }
            else
            {
                if (result.Dados == null || result.Dados.Count == 0)
                {
                    ViewBag.IsSucesso = "true";
                    ViewBag.LblMensagem = result.Mensagem;
                }
            }

            return PartialView("PartialCalendario", result);
        }

        public IActionResult GetHorario(string idTurma)
        {
            var idUsuario = HttpContext?.Session?.GetString("IdUsuario");
            var idEscola = HttpContext?.Session?.GetString("IdEscola");
            Guid idUsuarioGuid;
            if (!Guid.TryParse(idUsuario, out idUsuarioGuid))
            {
                TempData["PaginaAnterior"] = String.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, Request.Path, Request.QueryString);
                TempData["MensagemErro"] = "Sessão expirada";
                return RedirectToAction("Login", "Acesso");
            }

            if (String.IsNullOrEmpty(idTurma))
            {
                ViewBag.IsSucesso = "false";
                ViewBag.LblMensagem = "Selecione uma turma";

                return PartialView("PartialGrade", new GlResposta<CmDisciplinaHorario>() { Mensagem = Mensagens.SemRegistroEncontrado });
            }

            GlResposta<CmDisciplinaHorario> result = Get<CmDisciplinaHorario>("Disciplina/ObterGrade", idTurma);
            if (result == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }
            if (!result.Succeeded)
            {
                ViewBag.IsSucesso = "false";
                ViewBag.LblMensagem = result.Mensagem;
            }
            else
            {
                if (result.Dados == null || result.Dados.Count == 0)
                {
                    ViewBag.IsSucesso = "true";
                    ViewBag.LblMensagem = result.Mensagem;
                }
            }

            List<CmDisciplinaHorario> listaDisciplinaHorario = new List<CmDisciplinaHorario>();

            foreach (var item in DiasSemana.ObterDiasSemana().OrderBy(o => o.dia))
            {
                var listaDisciplinaHorarioDiasSemana = result?.Dados?.Where(i => i.Dia == item.dia && (!i.Excluido.HasValue || i.Excluido == false))?.ToList();
                if (listaDisciplinaHorarioDiasSemana?.Any() == true)
                {
                    listaDisciplinaHorario.AddRange(listaDisciplinaHorarioDiasSemana);
                }
            }

            result.Dados = listaDisciplinaHorario.Where(i => i.Id.HasValue)?.ToList();

            if (result.Dados == null || result.Dados.Count == 0)
            {
                ViewBag.IsSucesso = "true";
                ViewBag.LblMensagem = result.Mensagem;
            }

            return PartialView("PartialHorario", result);
        }

        [HttpGet("Responsavel/Calendario/{idTurma}/{idSubPeriodo}")]
        [HttpGet("Responsavel/Calendario")]
        public IActionResult Calendario(Guid? idTurma, Guid? idSubPeriodo)
        {
            NomeFuncao = "SCHOOLUP_RCALEND";
            ViewBag.IsSucesso = "";
            var idUsuario = HttpContext?.Session?.GetString("IdUsuario");
            var idEscola = HttpContext?.Session?.GetString("IdEscola");
            Guid idUsuarioGuid;
            if (!Guid.TryParse(idUsuario, out idUsuarioGuid))
            {
                TempData["PaginaAnterior"] = String.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, Request.Path, Request.QueryString);
                TempData["MensagemErro"] = "Sessão expirada";
                return RedirectToAction("Login", "Acesso");
            }

            GlResposta<string> resultPermissao = VerificarPermissao();
            if (resultPermissao?.Succeeded != true)
            {
                ViewBag.IsSucesso = "false";
                ViewBag.LblMensagem = resultPermissao.Mensagem;

                return View();
            }

            GlResposta<Periodo> resultPeriodo = GetAux<Periodo>("Servico/ObterPeriodo", idEscola);
            if (resultPeriodo.Dados != null)
            {
                ViewBag.IdPeriodo = new SelectList(resultPeriodo.Dados?.ToList(), "Id", "Nome");
                Guid idPeriodo = resultPeriodo.Dados.FirstOrDefault().Id;
                GlResposta<Turma> resultTurma = GetAux<Turma>("Servico/ObterTurma", idPeriodo.ToString());
                if (resultTurma.Dados != null)
                {
                    if (idTurma == null)
                    {
                        string idAlunoSelecionado = HttpContext?.Session?.GetString("IdAlunoSelecionado");
                        if (!String.IsNullOrWhiteSpace(idAlunoSelecionado))
                        {
                            List<CmAluno> alunoSelecionado = ListaAlunos.Where(i => i.Id.ToString() == idAlunoSelecionado).ToList();
                            ViewBag.IdTurma = new SelectList(resultTurma.Dados.Where(a => alunoSelecionado.Any(c => c.IdTurma == a.Id)).ToList(), "Id", "Nome");
                            idTurma = resultTurma.Dados.Where(a => alunoSelecionado.Any(c => c.IdTurma == a.Id))?.FirstOrDefault()?.Id ?? Guid.Empty;
                            ViewBag.IdTurmaAluno = String.Join(",", alunoSelecionado.Select(s => s.IdTurma)) ?? "";
                        }
                        else
                        {
                            ViewBag.IdTurma = new SelectList(resultTurma.Dados?.ToList(), "Id", "Nome");
                        }
                    }
                    else
                    {
                        ViewBag.IdTurma = new SelectList(resultTurma.Dados?.ToList(), "Id", "Nome", idTurma.Value.ToString());
                    }
                }

                GlResposta<SubPeriodo> resultSubPeriodo = GetAux<SubPeriodo>("Servico/ObterSubPeriodo", idPeriodo.ToString());
                if (resultSubPeriodo.Dados != null)
                {
                    if (idSubPeriodo != null)
                    {
                        ViewBag.IdSubPeriodo = new SelectList(resultSubPeriodo.Dados?.ToList(), "Id", "Nome", idSubPeriodo.Value);
                    }
                    else if (String.IsNullOrEmpty(resultSubPeriodo.Id))
                    {
                        ViewBag.IdSubPeriodo = new SelectList(resultSubPeriodo.Dados?.ToList(), "Id", "Nome");
                    }
                    else
                    {
                        ViewBag.IdSubPeriodo = new SelectList(resultSubPeriodo.Dados?.ToList(), "Id", "Nome", resultSubPeriodo.Id);
                    }
                }
            }

            ViewBag.IsSucesso = "";

            return View();
        }

        public IActionResult Horario(Guid? idTurma)
        {
            NomeFuncao = "SCHOOLUP_RHORARIO";
            ViewBag.IsSucesso = "";

            var idUsuario = HttpContext?.Session?.GetString("IdUsuario");
            var idEscola = HttpContext?.Session?.GetString("IdEscola");
            Guid idUsuarioGuid;
            if (!Guid.TryParse(idUsuario, out idUsuarioGuid))
            {
                TempData["PaginaAnterior"] = String.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, Request.Path, Request.QueryString);
                TempData["MensagemErro"] = "Sessão expirada";
                return RedirectToAction("Login", "Acesso");
            }

            GlResposta<string> resultPermissao = VerificarPermissao();
            if (resultPermissao?.Succeeded != true)
            {
                ViewBag.IsSucesso = "false";
                ViewBag.LblMensagem = resultPermissao.Mensagem;

                return View();
            }

            GlResposta<Periodo> resultPeriodo = GetAux<Periodo>("Servico/ObterPeriodo", idEscola);
            if (resultPeriodo.Dados != null)
            {
                ViewBag.IdPeriodo = new SelectList(resultPeriodo.Dados?.ToList(), "Id", "Nome");
                string idAlunoSelecionado = HttpContext?.Session?.GetString("IdAlunoSelecionado");
                ViewBag.IdAluno = idAlunoSelecionado;
                GlResposta<CmTurmaAluno> resultTurma = GetAux<CmTurmaAluno>("Servico/ObterTurmaAluno", new string[] { idAlunoSelecionado, resultPeriodo.Dados?.ToList().FirstOrDefault().Id.ToString() });

                if (resultTurma.Dados != null)
                {
                    if (idTurma == null)
                    {
                        if (!String.IsNullOrWhiteSpace(idAlunoSelecionado))
                        {
                            CmAluno alunoSelecionado = ListaAlunos.Where(i => i.Id.ToString() == idAlunoSelecionado).FirstOrDefault();
                            ViewBag.IdTurma = new SelectList(resultTurma.Dados?.ToList(), "IdTurma", "TurmaNome", alunoSelecionado.IdTurma.ToString());
                            TempData["OcultarPanel"] = "true";
                        }
                        else
                        {
                            ViewBag.IdTurma = new SelectList(resultTurma.Dados?.ToList(), "IdTurma", "TurmaNome");
                        }
                    }
                    else
                    {
                        ViewBag.IdTurma = new SelectList(resultTurma.Dados?.ToList(), "IdTurma", "TurmaNome", idTurma.Value.ToString());
                    }
                }
            }

            GlResposta<CmDisciplinaHorario> result = new GlResposta<CmDisciplinaHorario>();
            ViewBag.IsSucesso = "";

            return View(result);
        }
    }
}
