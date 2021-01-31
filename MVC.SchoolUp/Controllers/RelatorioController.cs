using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Modelo.Nucleo.Geral;
using Modelo.SchoolUp.Custom;
using Modelo.SchoolUp.Principal;
using Modelo.SchoolUp.Recursos;
using Modelo.SchoolUp.Relatorio;

namespace MVC.SchoolUp.Controllers
{
    public class RelatorioController : MasterController
    {
        public RelatorioController() : base()
        {
        }

        [HttpGet("AcessoResponsavel")]
        [HttpGet("Relatorio/AcessoResponsavel")]
        public IActionResult AcessoResponsavel()
        {
            NomeFuncao = "SCHOOLUP_RELACESRESP";
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
            Guid idEscolaGuid;
            if (!Guid.TryParse(idEscola, out idEscolaGuid))
            {
                TempData["PaginaAnterior"] = String.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, Request.Path, Request.QueryString);
                TempData["MensagemErro"] = "Sessão expirada";
                return RedirectToAction("Login", "Acesso");
            }


            GlResposta<Periodo> resultPeriodo = new GlResposta<Periodo>();
            GlResposta<Turma> resultTurma = new GlResposta<Turma>();

            resultPeriodo = Get<Periodo>("Servico/ObterPeriodo", idEscola);
            if (resultPeriodo == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }

            Periodo periodo = resultPeriodo?.Dados?.OrderByDescending(o => o.Nome)?.FirstOrDefault();
            resultTurma = GetAux<Turma>("Servico/ObterTurma", periodo.Id.ToString());
            if (resultTurma == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }

            ViewBag.IdPeriodo = new SelectList(resultPeriodo.Dados?.ToList(), "Id", "Nome", periodo.Id);
            ViewBag.IdTurma = new SelectList(resultTurma.Dados?.ToList() ?? new List<Turma>(), "Id", "Nome");
            
            GlResposta<RptAcessoResponsavel> resposta = new GlResposta<RptAcessoResponsavel>() {
                Dados = new List<RptAcessoResponsavel>(),
                Succeeded = true
            };

            resposta.Dados.Add(new RptAcessoResponsavel() { IdEscola = idEscolaGuid });

            return View(resposta);
        }

        [HttpPost("ObterAcessoResponsavel")]
        [HttpPost("Relatorio/ObterAcessoResponsavel")]
        public IActionResult AcessoResponsavel(string idAluno, string idTurma, string inicio, string fim, bool isAgrupar)
        {
            NomeFuncao = "SCHOOLUP_RELACESRESP";
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
            var idEscola = HttpContext?.Session?.GetString("IdEscola");
            Guid idEscolaGuid;
            if (!Guid.TryParse(idEscola, out idEscolaGuid))
            {
                TempData["PaginaAnterior"] = String.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, Request.Path, Request.QueryString);
                TempData["MensagemErro"] = "Sessão expirada";
                return RedirectToAction("Login", "Acesso");
            }
            GlResposta<RptAcessoResponsavel> result;
            RptAcessoResponsavel mdlRptAcessoResponsavel = new RptAcessoResponsavel();
            mdlRptAcessoResponsavel.IdEscola = idEscolaGuid;
            mdlRptAcessoResponsavel.IdAluno = idAluno;
            mdlRptAcessoResponsavel.IdTurma = idTurma;
            mdlRptAcessoResponsavel.IsAgrupar = isAgrupar;

            try
            {
                if (!String.IsNullOrEmpty(inicio))
                {
                    mdlRptAcessoResponsavel.Inicio = DateTime.Parse(inicio).Date;
                }
                if (!String.IsNullOrEmpty(fim))
                {
                    mdlRptAcessoResponsavel.Fim = DateTime.Parse(fim).Date;
                }
            }
            catch (Exception excecao)
            {
                ViewBag.IsSucesso = "false";
                ViewBag.LblMensagem = Mensagens.DataInvalida;
            }

            result = Post<RptAcessoResponsavel>("Relatorio/ObterAcessoResponsavel", mdlRptAcessoResponsavel);
            if (result == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }

            if (result?.Succeeded == true)
            {
                ViewBag.IsSucesso = "";
            }
            else
            {
                ViewBag.IsSucesso = "false";
                ViewBag.LblMensagem = result?.Mensagem;
            }

            return PartialView("PartialAcessoResponsavel", result);
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



        public IActionResult GetAlunos(string idTurma)
        {
            GlResposta<CmTurmaAluno> resultTurma = GetAux<CmTurmaAluno>("Servico/ObterAlunos", idTurma);

            return Ok(resultTurma?.Dados);
        }
    }
}