using Microsoft.AspNetCore.Mvc;
using Modelo.Nucleo.Geral;
using Modelo.SchoolUp.Principal;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Modelo.SchoolUp.Custom;
using System.Collections.Generic;
using Modelo.Nucleo.Recursos;
using Modelo.SchoolUp.Enumeracao;

namespace MVC.SchoolUp.Controllers
{
    public class DisciplinaController : MasterController
    {
        public DisciplinaController() : base()
        {
            NomeFuncao = "SCHOOLUP_DISCIPLINA";
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

        public IActionResult GetProfessores(string idDisciplina)
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

            GlResposta<CmProfessorDisciplina> resultProfessor = GetAux<CmProfessorDisciplina>("Servico/ObterTodosProfessorPorDisciplina", idDisciplina);
            if (resultProfessor?.Succeeded != true)
            {
                return Ok(new List<CmProfessorDisciplina>());
            }

            return Ok(resultProfessor.Dados);
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

            return PartialView("PartialGrade", result);
        }

        public IActionResult Grade(Guid? idTurma)
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
            if (resultPeriodo.Dados != null)
            {
                ViewBag.IdPeriodo = new SelectList(resultPeriodo.Dados?.ToList(), "Id", "Nome");
                GlResposta<Turma> resultTurma = GetAux<Turma>("Servico/ObterTurma", resultPeriodo.Dados?.ToList().FirstOrDefault().Id.ToString());

                if (resultTurma.Dados != null)
                {
                    if (idTurma == null)
                    {
                        string idAlunoSelecionado = HttpContext?.Session?.GetString("IdAlunoSelecionado");
                        if (!String.IsNullOrWhiteSpace(idAlunoSelecionado))
                        {
                            CmAluno alunoSelecionado = ListaAlunos.Where(i => i.Id.ToString() == idAlunoSelecionado).FirstOrDefault();
                            ViewBag.IdTurma = new SelectList(resultTurma.Dados?.ToList(), "Id", "Nome", alunoSelecionado.IdTurma.ToString());
                            TempData["OcultarPanel"] = "true";
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
            }

            GlResposta<CmDisciplinaHorario> result = new GlResposta<CmDisciplinaHorario>();
            ViewBag.IsSucesso = "";
            
            return View(result);
        }

        public IActionResult Edicao(Guid? id)
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

            GlResposta<Disciplina> resultDisciplina = Get<Disciplina>("Disciplina/ObterTodos", idEscola);
            if (resultDisciplina.Dados != null)
            {
                ViewBag.IdDisciplina = new SelectList(resultDisciplina.Dados?.ToList(), "Id", "Nome");
            }
            
            GlResposta<CmProfessorDisciplina> resultProfessor = Get<CmProfessorDisciplina>("Servico/ObterTodosProfessorComDisciplina", idEscola);
            if (resultProfessor?.Dados != null)
            {
                ViewBag.ListaProfessor = resultProfessor.Dados;
            }
            ViewBag.IdTurma = id.Value.ToString();
            HttpContext.Session.SetString("IdTurma", id.Value.ToString());
            GlResposta<CmDisciplinaHorario> result = Get<CmDisciplinaHorario>("Disciplina/ObterGrade", id.Value.ToString());
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edicao(string idTurma)
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

            idTurma = Request.Form["HdnIdTurma"];
            idTurma = HttpContext?.Session?.GetString("IdTurma");
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

                return PartialView("JanelaMensagem");
            }
            else
            {
                List<DisciplinaHorario> listDisciplinaHorario = new List<DisciplinaHorario>();
                int linha = 0;
                List<Guid> listGuids = result.Dados.Select(i => i.IdHorarioTurno.Value).ToList();
                listGuids = listGuids.Distinct().ToList();
                foreach (var horarioTurno in listGuids)
                {
                    int coluna = 0;
                    foreach (var diaSemana in Modelo.SchoolUp.Enumeracao.DiasSemana.ObterDiasSemana().OrderBy(o => o.dia))
                    {
                        string strDisciplina = Request.Form[$"Disc_{linha}_{coluna}"];
                        string strProfessor = Request.Form[$"Prof_{linha}_{coluna}"];
                        strDisciplina = strDisciplina?.Replace(",", "");
                        strProfessor = strProfessor?.Replace(",", "");

                        if (!String.IsNullOrEmpty(strDisciplina) || !String.IsNullOrEmpty(strProfessor))
                        {
                            DisciplinaHorario disciplinaHorario = new DisciplinaHorario();
                            disciplinaHorario.Dia = diaSemana.dia;
                            disciplinaHorario.Excluido = false;
                            disciplinaHorario.IdHorarioTurno = horarioTurno;
                            disciplinaHorario.Id = Guid.NewGuid();
                            if (!String.IsNullOrEmpty(strDisciplina))
                            {
                                disciplinaHorario.IdDisciplina = Guid.Parse(strDisciplina);
                            }
                            if (!String.IsNullOrEmpty(strProfessor))
                            {
                                disciplinaHorario.IdProfessor = Guid.Parse(strProfessor);
                            }
                            disciplinaHorario.IdTurma = Guid.Parse(idTurma);
                            listDisciplinaHorario.Add(disciplinaHorario);
                        }
                        coluna++;
                    }
                    linha++;
                }
                GlResposta<DisciplinaHorario> resultGravar = Put<DisciplinaHorario>("Disciplina/GravarGrade", listDisciplinaHorario);
                if (!resultGravar.Succeeded)
                {
                    ViewBag.IsSucesso = "false";
                    ViewBag.LblMensagem = result.Mensagem;

                    return PartialView("JanelaMensagem");
                }
            }

            ViewBag.IsSucesso = "true";
            ViewBag.LblMensagem = "Grade alterada";
            return PartialView("JanelaMensagem");
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
            GlResposta<AreaConhecimento> resultAreaConhecimento = GetAux<AreaConhecimento>("Servico/ObterAreaConhecimento");
            if (resultAreaConhecimento.Dados != null)
            {
                ViewBag.IdArea = new SelectList(resultAreaConhecimento.Dados?.ToList(), "Id", "Nome");
            }
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

            GlResposta<Disciplina> result = Get<Disciplina>("Disciplina/Obter", id.ToString());
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
            GlResposta<CmDisciplina> result = Get<CmDisciplina>("Disciplina/ObterTodos", idEscola);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cadastro([Bind("Id,IdEnsino,IdEscola,Nome,Codigo,IdArea,Excluido")] Disciplina mdlDados)
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
            GlResposta<Disciplina> result;
            Guid id = !String.IsNullOrEmpty(Request.Form["HdnIdChave"]) ? Guid.Parse(Request.Form["HdnIdChave"]) : mdlDados.Id;
            if (id.Equals(Guid.Empty))
            {
                result = Post<Disciplina>("Disciplina/Criar", mdlDados);
            }
            else
            {
                mdlDados.Id = id;
                result = Put<Disciplina>("Disciplina/Modificar", mdlDados);
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
            GlResposta<Disciplina> result = Get<Disciplina>("Disciplina/Obter", id.ToString());
            if (result == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }
            result = Put<Disciplina>("Disciplina/Apagar", result.Dados.FirstOrDefault());
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
