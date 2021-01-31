using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Modelo.Nucleo.Geral;
using Modelo.SchoolUp.Custom;
using Modelo.SchoolUp.Principal;
using MVC.SchoolUp.Models;
using Newtonsoft.Json;

namespace MVC.SchoolUp.Controllers
{
    public class HomeController : MasterController
    {
        public IActionResult Index()
        {
            string ambiente = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            ViewBag.Ambiente = ambiente;
            ViewBag.IsSucesso = "";
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
            List<string> codigoPerfis = JsonConvert.DeserializeObject<List<string>>(HttpContext?.Session?.GetString("PerfilCodigo"));

            string idAlunoSelecionado = HttpContext?.Session?.GetString("IdAlunoSelecionado");

            GlResposta<Periodo> resultPeriodo = new GlResposta<Periodo>();
            GlResposta<SubPeriodo> resultSubPeriodo = new GlResposta<SubPeriodo>();
            GlResposta<Turma> resultTurma = new GlResposta<Turma>();
            GlResposta<CmTurmaAluno> resultAluno = new GlResposta<CmTurmaAluno>();
            GlResposta<CmDisciplinaMedias> resultMedias = new GlResposta<CmDisciplinaMedias>();
            GlResposta<CmDisciplinaMedias> resultMediasRadar = new GlResposta<CmDisciplinaMedias>();
            Periodo periodo = new Periodo();

            resultPeriodo = Get<Periodo>("Servico/ObterPeriodo", idEscola);
            if (resultPeriodo == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }

            periodo = resultPeriodo?.Dados?.OrderByDescending(o => o.Nome)?.FirstOrDefault();

            resultSubPeriodo = GetAux<SubPeriodo>("Servico/ObterSubPeriodo", periodo.Id.ToString());
            if (resultSubPeriodo.Succeeded == false)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = resultSubPeriodo.Mensagem;
                return RedirectToAction("Erro", "Home");
            }

            if (codigoPerfis.Contains("SCHOOLUP_ADMIN") || codigoPerfis.Contains("SCHOOLUP_SECRETARIA"))
            {
                resultTurma = GetAux<Turma>("Servico/ObterTurma", periodo.Id.ToString());
                resultMedias = GetAux<CmDisciplinaMedias>("Servico/ObterDisciplinaMediasPeriodo", periodo.Id.ToString());

                ViewBag.IdPeriodo = new SelectList(resultPeriodo.Dados?.ToList(), "Id", "Nome", periodo.Id);
                ViewBag.IdTurma = new SelectList(resultTurma.Dados?.ToList() ?? new List<Turma>(), "Id", "Nome");
            }
            else
            {
                ViewBag.IdPeriodo = new SelectList(resultPeriodo.Dados?.ToList(), "Id", "Nome", periodo.Id);
                resultAluno = GetAux<CmTurmaAluno>("Servico/ObterAlunoPorResponsavel", new string[] { idUsuario });
                if (resultAluno?.Dados?.Any() != true)
                {
                    TempData["CodigoErro"] = "";
                    TempData["MensagemErro"] = resultAluno?.Mensagem;
                    return RedirectToAction("Erro", "Home");
                }
                CmTurmaAluno turmaAluno = resultAluno.Dados.Where(i => i.IdAluno.ToString() == idAlunoSelecionado && i.IdPeriodo == periodo.Id).FirstOrDefault();
                if (turmaAluno != null)
                {
                    resultTurma.Dados = new List<Turma>() { new Turma() { Id = turmaAluno.IdTurma } };
                    resultMedias = GetAux<CmDisciplinaMedias>("Servico/ObterDisciplinaMediasAluno", new string[] { turmaAluno.IdAluno.ToString(), periodo.Id.ToString() });
                    resultMediasRadar = GetAux<CmDisciplinaMedias>("Servico/ObterDisciplinaMediasTurma", turmaAluno.IdTurma.ToString());
                    if (resultMedias?.Dados?.Any() != true)
                    {
                        resultMedias = new GlResposta<CmDisciplinaMedias>();
                        resultMedias.Dados = new List<CmDisciplinaMedias>();
                    }
                    if (resultMediasRadar?.Dados?.Any() != true)
                    {
                        resultMediasRadar = new GlResposta<CmDisciplinaMedias>();
                        resultMediasRadar.Dados = new List<CmDisciplinaMedias>();
                    }
                    resultMediasRadar.Dados.RemoveAll(r => !resultMedias.Dados.Any(a => a.IdDisciplina == r.IdDisciplina));
                }
            }            

            if (resultTurma == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }

            if (resultMedias == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }

            ViewBag.IdAluno = idAlunoSelecionado;

            CmPrincipalAdmin cmPrincipalAdmin = new CmPrincipalAdmin()
            {
                PeriodoAtual = periodo,
                Bimestres = resultSubPeriodo.Dados,
                Turmas = resultTurma.Dados,
                Disciplinas = resultMedias.Dados,
                DisciplinasRadar = resultMediasRadar.Dados,
                Perfil = (codigoPerfis.Contains("SCHOOLUP_ADMIN") || codigoPerfis.Contains("SCHOOLUP_SECRETARIA")) ? "ADMIN" : "",
                IdAluno = idAlunoSelecionado
            };

            return View(cmPrincipalAdmin);
        }

        [HttpGet("Filtrar")]
        [HttpGet("Filtrar/{idPeriodo}/{idTurma}/{idAluno}")]
        public IActionResult Filtrar(string idPeriodo, string idTurma, string idAluno)
        {
            ViewBag.IsSucesso = "";
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
            List<string> codigoPerfis = JsonConvert.DeserializeObject<List<string>>(HttpContext?.Session?.GetString("PerfilCodigo"));

            string idAlunoSelecionado = HttpContext?.Session?.GetString("IdAlunoSelecionado");

            GlResposta<Periodo> resultPeriodo = new GlResposta<Periodo>();
            GlResposta<SubPeriodo> resultSubPeriodo = new GlResposta<SubPeriodo>();
            GlResposta<Turma> resultTurma = new GlResposta<Turma>();
            GlResposta<CmTurmaAluno> resultAluno = new GlResposta<CmTurmaAluno>();
            GlResposta<CmDisciplinaMedias> resultMedias = new GlResposta<CmDisciplinaMedias>();
            GlResposta<CmDisciplinaMedias> resultMediasRadar = new GlResposta<CmDisciplinaMedias>();
            Periodo periodo = new Periodo();

            if (codigoPerfis.Contains("SCHOOLUP_ADMIN") || codigoPerfis.Contains("SCHOOLUP_SECRETARIA"))
            {
                CmPrincipalAdmin dados = new CmPrincipalAdmin()
                {
                    IdDdlPeriodo = idPeriodo,
                    IdDdlTurma = idTurma,
                    IdDdlAluno = idAluno
                };

                resultPeriodo = Get<Periodo>("Servico/ObterPeriodo", idEscola);
                if (resultPeriodo == null)
                {
                    TempData["CodigoErro"] = "";
                    TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                    return RedirectToAction("Erro", "Home");
                }

                periodo = resultPeriodo?.Dados?.Where(i => i.Id.ToString() == idPeriodo)?.FirstOrDefault();

                resultSubPeriodo = GetAux<SubPeriodo>("Servico/ObterSubPeriodo", periodo.Id.ToString());
                if (resultSubPeriodo.Succeeded == false)
                {
                    TempData["CodigoErro"] = "";
                    TempData["MensagemErro"] = resultSubPeriodo.Mensagem;
                    return RedirectToAction("Erro", "Home");
                }

                if (!String.IsNullOrEmpty(dados.IdDdlAluno))
                {
                    resultAluno = GetAux<CmTurmaAluno>("Servico/ObterTurmaAluno", new string[] { dados.IdDdlAluno, dados.IdDdlPeriodo });
                    if (resultAluno?.Dados?.Any() != true)
                    {
                        TempData["CodigoErro"] = "";
                        TempData["MensagemErro"] = resultAluno?.Mensagem;
                        return RedirectToAction("Erro", "Home");
                    }
                    CmTurmaAluno turmaAluno = resultAluno.Dados.Where(i => i.IdAluno.ToString() == dados.IdDdlAluno).FirstOrDefault();
                    if (turmaAluno != null)
                    {
                        resultTurma.Dados = new List<Turma>() { new Turma() { Id = Guid.Parse(dados.IdDdlTurma) } };
                        resultMedias = GetAux<CmDisciplinaMedias>("Servico/ObterDisciplinaMediasAluno", new string[] { turmaAluno.IdAluno.ToString(), periodo.Id.ToString() });
                        resultMediasRadar = GetAux<CmDisciplinaMedias>("Servico/ObterDisciplinaMediasTurma", turmaAluno.IdTurma.ToString());
                        if (resultMedias?.Dados != null && resultMediasRadar?.Dados != null)
                        {
                            resultMediasRadar.Dados.RemoveAll(r => !resultMedias.Dados.Any(a => a.IdDisciplina == r.IdDisciplina));
                        }
                    }
                }
                else if (!String.IsNullOrEmpty(dados.IdDdlTurma))
                {
                    resultTurma.Dados = new List<Turma>() { new Turma() { Id = Guid.Parse(dados.IdDdlTurma) } };
                    resultMedias = GetAux<CmDisciplinaMedias>("Servico/ObterDisciplinaMediasPeriodo", dados.IdDdlPeriodo);
                    resultMedias.Dados = resultMedias.Dados.Where(i => i.IdTurma.ToString() == dados.IdDdlTurma)?.ToList();
                }
                else if (!String.IsNullOrEmpty(dados.IdDdlPeriodo))
                {
                    resultTurma = GetAux<Turma>("Servico/ObterTurma", dados.IdDdlPeriodo);
                    resultMedias = GetAux<CmDisciplinaMedias>("Servico/ObterDisciplinaMediasPeriodo", dados.IdDdlPeriodo);
                }
            }

            if (resultTurma == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }

            if (resultMedias == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }

            CmPrincipalAdmin cmPrincipalAdmin = new CmPrincipalAdmin()
            {
                PeriodoAtual = periodo,
                Bimestres = resultSubPeriodo.Dados,
                Turmas = resultTurma.Dados,
                Disciplinas = resultMedias.Dados,
                DisciplinasRadar = resultMediasRadar.Dados,
                Perfil = "ADMIN",
                IdAluno = idAluno
            };

            return PartialView("PartialIndex", cmPrincipalAdmin);
        }


        [HttpGet("FiltrarAluno")]
        [HttpGet("FiltrarAluno/{idPeriodo}/{idAluno}")]
        public IActionResult FiltrarAluno(string idPeriodo, string idAluno)
        {
            ViewBag.IsSucesso = "";
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
            List<string> codigoPerfis = JsonConvert.DeserializeObject<List<string>>(HttpContext?.Session?.GetString("PerfilCodigo"));

            string idAlunoSelecionado = HttpContext?.Session?.GetString("IdAlunoSelecionado");

            GlResposta<Periodo> resultPeriodo = new GlResposta<Periodo>();
            GlResposta<SubPeriodo> resultSubPeriodo = new GlResposta<SubPeriodo>();
            GlResposta<Turma> resultTurma = new GlResposta<Turma>();
            GlResposta<CmTurmaAluno> resultAluno = new GlResposta<CmTurmaAluno>();
            GlResposta<CmDisciplinaMedias> resultMedias = new GlResposta<CmDisciplinaMedias>();
            GlResposta<CmDisciplinaMedias> resultMediasRadar = new GlResposta<CmDisciplinaMedias>();
            Periodo periodo = new Periodo();

            CmPrincipalAdmin dados = new CmPrincipalAdmin()
            {
                IdDdlPeriodo = idPeriodo,
                IdDdlAluno = idAluno
            };

            resultPeriodo = Get<Periodo>("Servico/ObterPeriodo", idEscola);
            if (resultPeriodo == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }

            periodo = resultPeriodo?.Dados?.Where(i => i.Id.ToString() == idPeriodo)?.FirstOrDefault();

            resultSubPeriodo = GetAux<SubPeriodo>("Servico/ObterSubPeriodo", periodo.Id.ToString());
            if (resultSubPeriodo.Succeeded == false)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = resultSubPeriodo.Mensagem;
                return RedirectToAction("Erro", "Home");
            }

            resultAluno = GetAux<CmTurmaAluno>("Servico/ObterTurmaAluno", new string[] { dados.IdDdlAluno, dados.IdDdlPeriodo });
            if (resultAluno == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = resultAluno?.Mensagem;
                return RedirectToAction("Erro", "Home");
            }
            CmTurmaAluno turmaAluno = resultAluno.Dados.Where(i => i.IdAluno.ToString() == dados.IdDdlAluno).FirstOrDefault();
            if (turmaAluno != null)
            {
                resultTurma.Dados = new List<Turma>() { new Turma() { Id = turmaAluno.IdTurma, Nome = turmaAluno.TurmaNome } };
                resultMedias = GetAux<CmDisciplinaMedias>("Servico/ObterDisciplinaMediasAluno", new string[] { turmaAluno.IdAluno.ToString(), periodo.Id.ToString() });
                resultMediasRadar = GetAux<CmDisciplinaMedias>("Servico/ObterDisciplinaMediasTurma", turmaAluno.IdTurma.ToString());
                if (resultMedias != null && resultMediasRadar != null)
                {
                    resultMediasRadar.Dados.RemoveAll(r => !resultMedias.Dados.Any(a => a.IdDisciplina == r.IdDisciplina));
                }
            }            

            if (resultTurma == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }

            if (resultMedias == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }

            CmPrincipalAdmin cmPrincipalAdmin = new CmPrincipalAdmin()
            {
                PeriodoAtual = periodo,
                Bimestres = resultSubPeriodo.Dados,
                Turmas = resultTurma.Dados,
                Disciplinas = resultMedias.Dados,
                DisciplinasRadar = resultMediasRadar.Dados,
                Perfil = "ADMIN",
                IdAluno = idAluno
            };

            return PartialView("PartialIndex", cmPrincipalAdmin);
        }

        public IActionResult Erro()
        {
            ViewBag.LblCodigo = TempData["CodigoErro"]?.ToString();
            ViewBag.LblMensagem = TempData["MensagemErro"]?.ToString();
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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

            return Ok(resultSubPeriodo.Dados);
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
            
            return Ok(resultTurma?.Dados);
        }
        
        public IActionResult GetAlunos(string idTurma)
        {
            GlResposta<CmTurmaAluno> resultTurma = GetAux<CmTurmaAluno>("Servico/ObterAlunos", idTurma);

            return Ok(resultTurma?.Dados);
        }
    }
}
