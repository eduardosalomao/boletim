﻿using Microsoft.AspNetCore.Mvc;
using Modelo.Nucleo.Geral;
using Modelo.SchoolUp.Principal;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Modelo.SchoolUp.Custom;
using Modelo.SchoolUp.Recursos;

namespace MVC.SchoolUp.Controllers
{
    public class TurmaController : MasterController
    {
        public TurmaController() : base()
        {
            NomeFuncao = "SCHOOLUP_TURMA";
        }

        public IActionResult GetTurmas(string idPeriodo)
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
            if (resultTurma.Succeeded == false)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = resultTurma.Mensagem;
                return RedirectToAction("Erro", "Home");
            }

            return Ok(resultTurma.Dados);
        }

        [HttpGet("GetAluno")]
        public IActionResult GetAluno(string filtro)
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

            GlResposta<Aluno> result = Get<Aluno>("Aluno/Filtrar", filtro + "/" + idEscola);
            if (result.Succeeded == false)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = result.Mensagem;
                return RedirectToAction("Erro", "Home");
            }

            return Ok(result.Dados);
        }

        public IActionResult GetGrade(string idTurma)
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

                return PartialView("PartialGrade", new GlResposta<CmTurmaAluno>() { Mensagem = Mensagens.SemRegistroEncontrado });
            }

            GlResposta<CmTurmaAluno> result = Get<CmTurmaAluno>("Turma/ObterGrade", idTurma);
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

            return PartialView("PartialGrade", result);
        }

        public IActionResult Grade()
        {
            NomeFuncao = "SCHOOLUP_TEMPOTURMA";
            ViewBag.IsSucesso = "";
            GlResposta<string> resultPermissao = VerificarPermissao();
            if (resultPermissao?.Succeeded != true)
            {
                ViewBag.IsSucesso = "false";
                ViewBag.LblMensagem = resultPermissao.Mensagem;

                return View();
            }
            CarregarMenuObsoleto();
            var idUsuario = HttpContext?.Session?.GetString("IdUsuario");
            var idEscola = HttpContext?.Session?.GetString("IdEscola");
            Guid idUsuarioGuid;
            if (!Guid.TryParse(idUsuario, out idUsuarioGuid))
            {
                TempData["PaginaAnterior"] = String.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, Request.Path, Request.QueryString);
                TempData["MensagemErro"] = "Sessão expirada";
                return RedirectToAction("Login", "Acesso");
            }
            string idTurma = String.Empty;
            GlResposta<Periodo> resultPeriodo = GetAux<Periodo>("Servico/ObterPeriodo", idEscola);
            if (resultPeriodo.Dados != null)
            {
                ViewBag.IdPeriodo = new SelectList(resultPeriodo.Dados?.ToList(), "Id", "Nome"); GlResposta<Turma> resultTurma = GetAux<Turma>("Servico/ObterTurma", resultPeriodo.Dados?.ToList().FirstOrDefault().Id.ToString());
                if (resultTurma.Dados != null)
                {
                    ViewBag.IdTurma = new SelectList(resultTurma.Dados?.ToList(), "Id", "Nome");
                    idTurma = resultTurma.Dados?.ToList().FirstOrDefault().Id.ToString();
                }
            }

            ViewBag.IsSucesso = "";

            return View();
        }

        [HttpPost("Matricular")]
        public IActionResult Matricular(string idTurma, string idAluno)
        {
            GlResposta<string> resultPermissao = VerificarPermissao();
            if (resultPermissao?.Succeeded != true)
            {
                ViewBag.IsSucesso = "false";
                ViewBag.LblMensagem = resultPermissao.Mensagem;

                return View();
            }
            CarregarMenuObsoleto();
            var idUsuario = HttpContext?.Session?.GetString("IdUsuario");
            Guid idUsuarioGuid;
            if (!Guid.TryParse(idUsuario, out idUsuarioGuid))
            {
                TempData["PaginaAnterior"] = String.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, Request.Path, Request.QueryString);
                TempData["MensagemErro"] = "Sessão expirada";
                return RedirectToAction("Login", "Acesso");
            }

            AlunoTurma alunoTurma = new AlunoTurma() {
                Id = Guid.NewGuid(),
                IdAluno = String.IsNullOrEmpty(idAluno) ? Guid.Empty : Guid.Parse(idAluno),
                IdTurma = String.IsNullOrEmpty(idTurma) ? Guid.Empty : Guid.Parse(idTurma),
                Excluido = false
            };

            GlResposta<AlunoTurma> result = Post<AlunoTurma>("Turma/MatricularAluno", alunoTurma);

            if (result == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }

            ViewBag.IsSucesso = result.Succeeded.ToString();
            ViewBag.LblMensagem = result.Mensagem;

            return PartialView("JanelaMensagem");
        }

        public IActionResult Index()
        {
            ViewBag.IsSucesso = "";
            GlResposta<string> resultPermissao = VerificarPermissao();
            if (resultPermissao?.Succeeded != true)
            {
                ViewBag.IsSucesso = "false";
                ViewBag.LblMensagem = resultPermissao.Mensagem;

                return View();
            }
            CarregarMenuObsoleto();
            var idUsuario = HttpContext?.Session?.GetString("IdUsuario");
            var idEscola = HttpContext?.Session?.GetString("IdEscola");
            Guid idUsuarioGuid;
            if (!Guid.TryParse(idUsuario, out idUsuarioGuid))
            {
                TempData["PaginaAnterior"] = String.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, Request.Path, Request.QueryString);
                TempData["MensagemErro"] = "Sessão expirada";
                return RedirectToAction("Login", "Acesso");
            }
            GlResposta<CmTurma> result = Get<CmTurma>("Turma/ObterTodos", idEscola);
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
                else
                {
                    result.Dados = result.Dados.OrderByDescending(o => o.NomePeriodo).ThenBy(t => t.Nome).ToList();
                }
            }

            return View(result);
        }

        public IActionResult Cadastro(Guid? id)
        {
            ViewBag.IsSucesso = "";
            GlResposta<string> resultPermissao = VerificarPermissao();
            if (resultPermissao?.Succeeded != true)
            {
                ViewBag.IsSucesso = "false";
                ViewBag.LblMensagem = resultPermissao.Mensagem;

                return View();
            }
            CarregarMenuObsoleto();
            var idUsuario = HttpContext?.Session?.GetString("IdUsuario");
            var idEscola = HttpContext?.Session?.GetString("IdEscola");
            Guid idUsuarioGuid;
            if (!Guid.TryParse(idUsuario, out idUsuarioGuid))
            {
                TempData["PaginaAnterior"] = String.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, Request.Path, Request.QueryString);
                TempData["MensagemErro"] = "Sessão expirada";
                return RedirectToAction("Login", "Acesso");
            }
            GlResposta<Periodo> resultPeriodo = GetAux<Periodo>("Servico/ObterPeriodo", idEscola);
            if (resultPeriodo?.Dados != null)
            {
                ViewBag.IdPeriodo = new SelectList(resultPeriodo.Dados?.ToList(), "Id", "Nome");
            }
            GlResposta<Serie> resultSerie = GetAux<Serie>("Servico/ObterSerie", idEscola);
            if (resultSerie?.Dados != null)
            {
                ViewBag.IdSerie = new SelectList(resultSerie.Dados?.ToList(), "Id", "Nome");
            }
            GlResposta<Turno> resultTurno = GetAux<Turno>("Servico/ObterTurno", idEscola);
            if (resultTurno?.Dados != null)
            {
                ViewBag.IdTurno = new SelectList(resultTurno.Dados?.ToList(), "Id", "Nome");
            }
            if (id == null)
            {
                ViewBag.Id = Guid.Empty;
                return View();
            }

            GlResposta<CmTurma> result = Get<CmTurma>("Turma/Obter", id.ToString());
            ViewBag.Id = id;
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

            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cadastro([Bind("Id,Nome,IdSerie,IdPeriodo,IdTurno,NumeroAlunos,Excluido")] CmTurma mdlDados)
        {
            GlResposta<string> resultPermissao = VerificarPermissao();
            if (resultPermissao?.Succeeded != true)
            {
                ViewBag.IsSucesso = "false";
                ViewBag.LblMensagem = resultPermissao.Mensagem;

                return View();
            }
            CarregarMenuObsoleto();
            var idUsuario = HttpContext?.Session?.GetString("IdUsuario");
            Guid idUsuarioGuid;
            if (!Guid.TryParse(idUsuario, out idUsuarioGuid))
            {
                TempData["PaginaAnterior"] = String.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, Request.Path, Request.QueryString);
                TempData["MensagemErro"] = "Sessão expirada";
                return RedirectToAction("Login", "Acesso");
            }
            GlResposta<CmTurma> result;
            Guid id = !String.IsNullOrEmpty(Request.Form["HdnIdChave"]) ? Guid.Parse(Request.Form["HdnIdChave"]) : mdlDados.Id;
            if (id.Equals(Guid.Empty))
            {
                result = Post<CmTurma>("Turma/Criar", mdlDados);
            }
            else
            {
                mdlDados.Id = id;
                result = Put<CmTurma>("Turma/Modificar", mdlDados);
            }

            if (result == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }

            ViewBag.Id = result.Succeeded ? Guid.Parse(result.Id) : id;
            ViewBag.IsSucesso = result.Succeeded.ToString();
            ViewBag.LblMensagem = result.Mensagem;

            return PartialView("JanelaMensagem");
        }

        public IActionResult Excluir(Guid? id)
        {
            var idUsuario = HttpContext?.Session?.GetString("IdUsuario");
            Guid idUsuarioGuid;
            if (!Guid.TryParse(idUsuario, out idUsuarioGuid))
            {
                TempData["PaginaAnterior"] = String.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, Request.Path, Request.QueryString);
                TempData["MensagemErro"] = "Sessão expirada";
                return RedirectToAction("Login", "Acesso");
            }
            GlResposta<CmTurma> result = Get<CmTurma>("Turma/Obter", id.ToString());
            if (result == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }
            result = Put<CmTurma>("Turma/Apagar", result.Dados.FirstOrDefault());
            if (result == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }

            if (result.Id != null)
            {
                ViewBag.Id = result.Succeeded ? Guid.Parse(result.Id) : id;
            }
            ViewBag.IsSucesso = result.Succeeded.ToString();
            ViewBag.LblMensagem = result.Mensagem;

            return PartialView("JanelaMensagem");
        }

        public IActionResult ExcluirLinha(Guid? id)
        {
            var idUsuario = HttpContext?.Session?.GetString("IdUsuario");
            Guid idUsuarioGuid;
            if (!Guid.TryParse(idUsuario, out idUsuarioGuid))
            {
                TempData["PaginaAnterior"] = String.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, Request.Path, Request.QueryString);
                TempData["MensagemErro"] = "Sessão expirada";
                return RedirectToAction("Login", "Acesso");
            }
            GlResposta<AlunoTurma> result = Get<AlunoTurma>("Turma/ObterAlunoTurma", id.ToString());
            if (result == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }
            result = Put<AlunoTurma>("Turma/ApagarAlunoTurma", result.Dados.FirstOrDefault());
            if (result == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }

            if (result.Id != null)
            {
                ViewBag.Id = result.Succeeded ? Guid.Parse(result.Id) : id;
            }
            ViewBag.IsSucesso = result.Succeeded.ToString();
            ViewBag.LblMensagem = result.Mensagem;

            return PartialView("JanelaMensagem");
        }
    }
}
