using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Modelo.SchoolUp.Principal;
using Modelo.Nucleo.Geral;
using Microsoft.AspNetCore.Http;

namespace MVC.SchoolUp.Controllers
{
    public class TurnoController : MasterController
    {
        public TurnoController() : base()
        {
            NomeFuncao = "SCHOOLUP_TURNO";
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
            GlResposta<Turno> result = Get<Turno>("Turno/ObterTodos", idEscola);
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
            ViewBag.IdEscola = idEscola;
            if (id == null)
            {
                ViewBag.Id = Guid.Empty;
                return View();
            }

            GlResposta<Turno> result = Get<Turno>("Turno/Obter", id.ToString());
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
        public IActionResult Cadastro([Bind("Id,IdTurno,Nome,Codigo,Inicio,Termino,Ativo")] Turno mdlDados)
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
            var idEscola = HttpContext?.Session?.GetString("IdEscola");
            Guid idEscolaGuid;
            if (!Guid.TryParse(idEscola, out idEscolaGuid))
            {
                TempData["PaginaAnterior"] = String.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, Request.Path, Request.QueryString);
                TempData["MensagemErro"] = "Sessão expirada";
                return RedirectToAction("Login", "Acesso");
            }
            mdlDados.IdEscola = idEscolaGuid;
            GlResposta<Turno> result;
            Guid id = !String.IsNullOrEmpty(Request.Form["HdnIdChave"]) ? Guid.Parse(Request.Form["HdnIdChave"]) : mdlDados.Id;
            if (id.Equals(Guid.Empty))
            {
                result = Post<Turno>("Turno/Criar", mdlDados);
            }
            else
            {
                mdlDados.Id = id;
                result = Put<Turno>("Turno/Modificar", mdlDados);
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
            GlResposta<Turno> result = Get<Turno>("Turno/Obter", id.ToString());
            if (result == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }
            result = Put<Turno>("Turno/Apagar", result.Dados.FirstOrDefault());
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
