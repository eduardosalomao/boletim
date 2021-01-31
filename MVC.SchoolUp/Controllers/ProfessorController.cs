using Microsoft.AspNetCore.Mvc;
using Modelo.Nucleo.Geral;
using Modelo.SchoolUp.Principal;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Collections.Generic;
using Modelo.SchoolUp.Custom;
using Microsoft.AspNetCore.Mvc.Rendering;
using Modelo.SchoolUp.Recursos;

namespace MVC.SchoolUp.Controllers
{
    public class ProfessorController : MasterController
    {

        public ProfessorController() : base()
        {
            NomeFuncao = "SCHOOLUP_PROFESSOR";
           
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
            GlResposta<Professor> result = Get<Professor>("Professor/ObterTodos", idEscola);
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
            Guid.TryParse(idEscola, out idEscolaGuid);
            ViewBag.IdEscola = idEscola;

            GlResposta<Disciplina> resultDisciplina = GetAux<Disciplina>("Disciplina/ObterTodos", idEscola);
            if (resultDisciplina.Dados != null)
            {
                ViewBag.IdDisciplina = new SelectList(resultDisciplina.Dados?.ToList(), "Id", "Nome");
            }

            if (id == null)
            {
                ViewBag.Id = Guid.Empty;
                return View();
            }

            GlResposta<CmProfessor> result = new GlResposta<CmProfessor>();
            ViewBag.Id = id;

            GlResposta<Professor> resultProfessor = Get<Professor>("Professor/Obter", id.ToString());
            if (resultProfessor == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }
            if (!resultProfessor.Succeeded)
            {
                ViewBag.IsSucesso = "false";
                ViewBag.LblMensagem = resultProfessor.Mensagem;
                result.Dados = new List<CmProfessor>();
            }
            else
            {
                Professor mdlProfessor = resultProfessor.Dados.FirstOrDefault();
                CmProfessor mdlCmProfessor = new CmProfessor()
                {
                    Id = mdlProfessor.Id,
                    Email = mdlProfessor.Email,
                    Nome = mdlProfessor.Nome
                };
                EscolaProfessor mdlEscolaProfessor = new EscolaProfessor() { IdEscola = idEscolaGuid };
                mdlCmProfessor.EscolaProfessor = new List<EscolaProfessor>();
                mdlCmProfessor.EscolaProfessor.Add(mdlEscolaProfessor);
                result.Dados = new List<CmProfessor>();
                result.Dados.Add(mdlCmProfessor);
            }

            result.Id = resultProfessor.Id;

            return View(result);
        }

        public IActionResult GetGrade(string idProfessor)
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

            GlResposta<CmProfessorDisciplina> result = GetAux<CmProfessorDisciplina>("Disciplina/ObterDisciplinaProfessor", new string[] { idProfessor, idEscola });
            if (result == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }
            if (!result.Succeeded)
            {
                ViewBag.IsSucesso = "false";
                ViewBag.LblMensagemDisciplina = result.Mensagem;
            }
            else
            {
                if (result.Dados == null || result.Dados.Count == 0)
                {
                    ViewBag.IsSucesso = "true";
                    ViewBag.LblMensagemDisciplina = result.Mensagem;
                }
            }

            return PartialView("PartialGrade", result);
        }

        [HttpPost("Inscrever")]
        public IActionResult Inscrever(string idDisciplina, string idProfessor)
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
            
            GlResposta<EscolaProfessor> resultProfessor = GetAux<EscolaProfessor>("Professor/ObterProfessorEscola", new string[] { idProfessor, idEscola });
            if (resultProfessor == null || !resultProfessor.Dados.Any())
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }

            Guid idProfessorEscola = resultProfessor.Dados.FirstOrDefault().Id;

            ProfessorDisciplina professorDisciplina = new ProfessorDisciplina()
            {
                Id = Guid.NewGuid(),
                IdEscolaProfessor = idProfessorEscola,
                IdDisciplina = String.IsNullOrEmpty(idDisciplina) ? Guid.Empty : Guid.Parse(idDisciplina),
                Excluido = false
            };

            GlResposta<ProfessorDisciplina> result = Post<ProfessorDisciplina>("Professor/RegistrarDisciplina", professorDisciplina);

            if (result == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }

            ViewBag.Id = result.Succeeded ? Guid.Parse(result.Id) : Guid.Empty;
            ViewBag.IsSucesso = result.Succeeded.ToString();
            ViewBag.LblMensagem = result.Mensagem;

            return PartialView("JanelaMensagem");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cadastro([Bind("Id,IdUser,Nome,Email,Imagem,Ativo,Excluido")] CmProfessor mdlDados)
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
            GlResposta<CmProfessor> result;
            Guid id = !String.IsNullOrEmpty(Request.Form["HdnIdChave"]) ? Guid.Parse(Request.Form["HdnIdChave"]) : mdlDados.Id;
            if (id.Equals(Guid.Empty))
            {
                mdlDados.EscolaProfessor = new List<EscolaProfessor>();
                mdlDados.EscolaProfessor.Add(new EscolaProfessor() { IdEscola = idEscolaGuid });
                result = Post<CmProfessor>("Professor/Criar", mdlDados);
            }
            else
            {
                mdlDados.Id = id;
                mdlDados.EscolaProfessor = null;
                result = Put<CmProfessor>("Professor/Modificar", mdlDados);
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
            var idEscola = HttpContext?.Session?.GetString("IdEscola");
            Guid idUsuarioGuid;
            if (!Guid.TryParse(idUsuario, out idUsuarioGuid))
            {
                TempData["PaginaAnterior"] = String.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, Request.Path, Request.QueryString);
                TempData["MensagemErro"] = "Sessão expirada";
                return RedirectToAction("Login", "Acesso");
            }
            Guid idEscolaGuid;
            Guid.TryParse(idEscola, out idEscolaGuid);

            GlResposta<Professor> resultProfessor = Get<Professor>("Professor/Obter", id.ToString());
            GlResposta<CmProfessor> result = new GlResposta<CmProfessor>();
            if (resultProfessor == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }
            else
            {
                Professor mdlProfessor = resultProfessor.Dados.FirstOrDefault();
                CmProfessor mdlCmProfessor = new CmProfessor()
                {
                    Id = mdlProfessor.Id,
                    Email = mdlProfessor.Email,
                    Nome = mdlProfessor.Nome
                };
                EscolaProfessor mdlEscolaProfessor = new EscolaProfessor() { IdEscola = idEscolaGuid };
                mdlCmProfessor.EscolaProfessor = new List<EscolaProfessor>();
                mdlCmProfessor.EscolaProfessor.Add(mdlEscolaProfessor);
                result.Dados = new List<CmProfessor>();
                result.Dados.Add(mdlCmProfessor);
            }
            result = Put<CmProfessor>("Professor/Apagar", result.Dados.FirstOrDefault());
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
            var idEscola = HttpContext?.Session?.GetString("IdEscola");
            Guid idUsuarioGuid;
            if (!Guid.TryParse(idUsuario, out idUsuarioGuid))
            {
                TempData["PaginaAnterior"] = String.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, Request.Path, Request.QueryString);
                TempData["MensagemErro"] = "Sessão expirada";
                return RedirectToAction("Login", "Acesso");
            }

            ProfessorDisciplina professorDisciplina = new ProfessorDisciplina()
            {
                Id = id.Value,
                Excluido = true
            };
            var result = Put<ProfessorDisciplina>("Professor/ApagarDisciplina", professorDisciplina);
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
