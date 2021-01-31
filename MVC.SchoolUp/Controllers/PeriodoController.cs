using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Modelo.SchoolUp.Principal;
using Modelo.Nucleo.Geral;
using Microsoft.AspNetCore.Http;

namespace MVC.SchoolUp.Controllers
{
    public class PeriodoController : MasterController
    {
        public PeriodoController() : base()
        {
            NomeFuncao = "SCHOOLUP_PERIODO";
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
            GlResposta<Periodo> result = Get<Periodo>("Periodo/ObterTodos", idEscola);
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
            Guid idEscolaGuid;
            if (!Guid.TryParse(idEscola, out idEscolaGuid))
            {
                TempData["PaginaAnterior"] = String.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, Request.Path, Request.QueryString);
                TempData["MensagemErro"] = "Sessão expirada";
                return RedirectToAction("Login", "Acesso");
            }
            ViewBag.IdEscola = idEscola;
            GlResposta<Ensino> resultEnsino = GetAux<Ensino>("Servico/ObterEnsino", idEscola);
            if (resultEnsino.Dados != null)
            {
                ViewBag.IdEnsino = new SelectList(resultEnsino.Dados?.ToList(), "Id", "Nome");
            }

            if (id == null)
            {
                ViewBag.Id = Guid.Empty;
                return View();
            }

            GlResposta<Periodo> result = Get<Periodo>("Periodo/Obter", id.ToString());
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
        public IActionResult Cadastro([Bind("Id,IdEscola,Nome,Codigo,MediaAprovacao,MediaAprovacaoFinal,MediaReprovacao,De,Ate,Excluido")] Periodo mdlDados)
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
            GlResposta<Periodo> result;
            Guid id = !String.IsNullOrEmpty(Request.Form["HdnIdChave"]) ? Guid.Parse(Request.Form["HdnIdChave"]) : mdlDados.Id;
            mdlDados.IdEscola = idEscolaGuid;
            if (id.Equals(Guid.Empty))
            {
                result = Post<Periodo>("Periodo/Criar", mdlDados);
            }
            else
            {
                mdlDados.Id = id;
                result = Put<Periodo>("Periodo/Modificar", mdlDados);
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
            GlResposta<Periodo> result = Get<Periodo>("Periodo/Obter", id.ToString());
            if (result == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }
            result = Put<Periodo>("Periodo/Apagar", result.Dados.FirstOrDefault());
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
    }
}
