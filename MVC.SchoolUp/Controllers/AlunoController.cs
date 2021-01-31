using Microsoft.AspNetCore.Mvc;
using Modelo.Nucleo.Geral;
using Modelo.SchoolUp.Principal;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using Modelo.SchoolUp.Custom;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Newtonsoft.Json;
using Modelo.SchoolUp.Recursos;

namespace MVC.SchoolUp.Controllers
{
    public class AlunoController : MasterController
    {
        public AlunoController() : base()
        {
            NomeFuncao = "SCHOOLUP_ALUNO";
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
            GlResposta<Aluno> result = Get<Aluno>("Aluno/ObterTodos", idEscola);
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

        [HttpGet("Aluno/Cadastro")]
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

            GlResposta<Periodo> resultPeriodo = GetAux<Periodo>("Servico/ObterPeriodo", idEscola);
            ViewBag.IdPeriodo = new SelectList(resultPeriodo.Dados?.ToList(), "Id", "Nome");
            Guid idPeriodo = resultPeriodo.Dados.FirstOrDefault().Id;

            GlResposta<SubPeriodo> resultSubPeriodo = GetAux<SubPeriodo>("Servico/ObterSubPeriodo", idPeriodo.ToString());
            if (resultSubPeriodo.Dados != null)
            {
                ViewBag.IdSubPeriodo = new SelectList(resultSubPeriodo.Dados?.ToList(), "Id", "Nome", resultSubPeriodo.Id);
            }

            if (id == null)
            {
                ViewBag.Id = Guid.Empty;
                return View();
            }

            GlResposta<CmAluno> result = Get<CmAluno>("Aluno/Obter", id.ToString());
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

        [HttpPost("Aluno/Cadastro")]
        [ValidateAntiForgeryToken]
        public IActionResult Cadastro([Bind("Id,IdUser,Nome,Email,Imagem,Cpf,DataNascimento,Matricula,Ativo,Excluido")] CmAluno mdlDados)
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
            GlResposta<CmAluno> result;
            Guid id = !String.IsNullOrEmpty(Request.Form["HdnIdChave"]) ? Guid.Parse(Request.Form["HdnIdChave"]) : mdlDados.Id;
            mdlDados.IdEscola = idEscolaGuid;
            if (id.Equals(Guid.Empty))
            {
                result = Post<CmAluno>("Aluno/Criar", mdlDados);
            }
            else
            {
                mdlDados.Id = id;
                result = Put<CmAluno>("Aluno/Modificar", mdlDados);
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

        [HttpGet("Aluno/Novo")]
        public IActionResult Novo()
        {
            ViewBag.LblMensagem = String.Empty;
            ViewBag.IsSucesso = String.Empty;

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
            ViewBag.IdUsuario = idUsuario;

            CarregarMenuObsoleto();

            GlResposta<Periodo> resultPeriodo = Get<Periodo>("Servico/ObterPeriodo", idEscola);
            if (resultPeriodo == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }

            Periodo periodo = resultPeriodo.Dados.OrderByDescending(o => o.Nome)?.FirstOrDefault();

            GlResposta<CmTurma> resultTurma = Get<CmTurma>("Servico/ObterTurma", periodo.Id.ToString());

            if (resultTurma == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }

            ViewBag.IdTurma = new SelectList(resultTurma.Dados?.ToList(), "Id", "Nome");

            if (!resultTurma.Succeeded)
            {
                ViewBag.IsSucesso = "false";
                ViewBag.LblMensagem = resultTurma.Mensagem;
            }

            return View();
        }

        [HttpPost("Aluno/Novo")]
        public IActionResult Novo([Bind()] CmNovoAcesso dados)
        {
            var idEscola = HttpContext?.Session?.GetString("IdEscola");
            Guid idEscolaGuid;
            if (!Guid.TryParse(idEscola, out idEscolaGuid))
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }

            GlResposta<CmNovoAcesso> result = Post<CmNovoAcesso>("Aluno/NovoAcesso", dados);

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

        [HttpGet("Aluno/Mudar")]
        public IActionResult Mudar()
        {
            ViewBag.LblMensagem = String.Empty;
            ViewBag.IsSucesso = String.Empty;

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
            ViewBag.IdUsuario = idUsuario;

            CarregarMenuObsoleto();

            GlResposta<Periodo> resultPeriodo = Get<Periodo>("Servico/ObterPeriodo", idEscola);
            if (resultPeriodo == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }

            GlResposta<CmUsuario> resultAlunos = Get<CmUsuario>("Servico/ObterEscola", idUsuario);
            if (resultAlunos == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }

            List<CmAluno> listaAlunos = resultAlunos.Dados?.FirstOrDefault().Alunos.ToList();

            HttpContext.Session.SetString("AlunoResponsavel", JsonConvert.SerializeObject(listaAlunos));

            if (listaAlunos == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }

            listaAlunos =
                (from a in resultAlunos.Dados?.FirstOrDefault().Alunos
                 group new { a } by new { a.Id, a.Nome }
                 into g
                 select new CmAluno()
                 {
                     Id = g.Select(s => s.a.Id).FirstOrDefault()
                    ,
                     Nome = g.Select(s => s.a.Nome).FirstOrDefault()
                 }).ToList();

            ViewBag.IdAluno = new SelectList(listaAlunos, "Id", "Nome");
            
            return View();
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

        public IActionResult GetNotas(string idAluno)
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
            Guid idAlunoGuid;
            if (!Guid.TryParse(idAluno, out idAlunoGuid))
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }

            GlResposta<CmNotas> result = Get<CmNotas>("Avaliacao/ObterNotasAluno", idAlunoGuid.ToString());
            if (result == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }
            if (!result.Succeeded)
            {
                ViewBag.IsSucesso = "false";
                ViewBag.LblMensagemGrid = result.Mensagem;
            }
            else
            {
                if (result.Dados == null || result.Dados.Count == 0)
                {
                    ViewBag.IsSucesso = "true";
                    ViewBag.LblMensagemGrid = result.Mensagem;
                }
            }

            return PartialView("PartialNotas", result);
        }

        public IActionResult GetNotasBimestre(string idAluno, string idPeriodo, string idBimestre)
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

            GlResposta<CmNotas> result = Get<CmNotas>("Avaliacao/ObterNotasAlunoBimestre", new string[] { idAluno, idPeriodo, idBimestre});
            if (result == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }
            if (!result.Succeeded)
            {
                ViewBag.IsSucesso = "false";
                ViewBag.LblMensagemGrid = result.Mensagem;
            }
            else
            {
                if (result.Dados == null || result.Dados.Count == 0)
                {
                    ViewBag.IsSucesso = "true";
                    ViewBag.LblMensagemGrid = result.Mensagem;
                }
            }

            return PartialView("PartialNotas", result);
        }

        [HttpPost("Aluno/GravarNota")]
        public IActionResult GravarNota(string idNota, string nota)
        {
            var idUsuario = HttpContext?.Session?.GetString("IdUsuario");
            Guid idUsuarioGuid;
            if (!Guid.TryParse(idUsuario, out idUsuarioGuid))
            {
                TempData["PaginaAnterior"] = String.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, Request.Path, Request.QueryString);
                TempData["MensagemErro"] = "Sessão expirada";
                return RedirectToAction("Login", "Acesso");
            }
            Guid idNotaGuid;
            GlResposta<Notas> resposta = new GlResposta<Notas>();
            if (!Guid.TryParse(idNota, out idNotaGuid))
            {
                return BadRequest(new GlResposta<Notas>() { Succeeded = false, Mensagem = Mensagens.FormatoIncorreto });
            }

            Notas mdlNotas = new Notas()
            {
                Id = idNotaGuid
            };

            decimal valorNota;
            if (String.IsNullOrEmpty(nota) == false)
            {
                if (!decimal.TryParse(nota, out valorNota))
                {
                    return BadRequest(new GlResposta<Notas>() { Succeeded = false, Mensagem = Mensagens.FormatoIncorreto });
                }
                mdlNotas.Nota = valorNota;
            }

            GlResposta<Notas> result = Put<Notas>("Avaliacao/GravarNota", mdlNotas);

            if (result == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }

            ViewBag.Id = result.Succeeded ? Guid.Parse(result.Id) : Guid.Empty;
            ViewBag.IsSucesso = result.Succeeded == false ? "false" : "";
            ViewBag.LblMensagem = result.Mensagem;

            return PartialView("JanelaMensagem");
        }
        
        [HttpPost("Aluno/GravarNotaRecuperacao")]
        public IActionResult GravarNotaRecuperacao(string idNota, string notaRecuperacao)
        {
            var idUsuario = HttpContext?.Session?.GetString("IdUsuario");
            Guid idUsuarioGuid;
            if (!Guid.TryParse(idUsuario, out idUsuarioGuid))
            {
                TempData["PaginaAnterior"] = String.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, Request.Path, Request.QueryString);
                TempData["MensagemErro"] = "Sessão expirada";
                return RedirectToAction("Login", "Acesso");
            }
            Guid idNotaGuid;
            GlResposta<Notas> resposta = new GlResposta<Notas>();
            if (!Guid.TryParse(idNota, out idNotaGuid))
            {
                return BadRequest(new GlResposta<Notas>() { Succeeded = false, Mensagem = Mensagens.FormatoIncorreto });
            }

            Notas mdlNotas = new Notas()
            {
                Id = idNotaGuid
            };

            decimal valorNota;
            if (String.IsNullOrEmpty(notaRecuperacao) == false)
            {
                if (!decimal.TryParse(notaRecuperacao, out valorNota))
                {
                    return BadRequest(new GlResposta<Notas>() { Succeeded = false, Mensagem = Mensagens.FormatoIncorreto });
                }
                mdlNotas.NotaRecuperacao = valorNota;
            }

            GlResposta<Notas> result = Put<Notas>("Avaliacao/GravarNotaRecuperacao", mdlNotas);

            if (result == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }

            ViewBag.Id = result.Succeeded ? Guid.Parse(result.Id) : Guid.Empty;
            ViewBag.IsSucesso = result.Succeeded == false ? "false" : "";
            ViewBag.LblMensagem = result.Mensagem;

            return PartialView("JanelaMensagem");
        }

        [HttpPost("Aluno/GravarFaltas")]
        public IActionResult GravarFaltas(string idNota, string faltas)
        {
            var idUsuario = HttpContext?.Session?.GetString("IdUsuario");
            Guid idUsuarioGuid;
            if (!Guid.TryParse(idUsuario, out idUsuarioGuid))
            {
                TempData["PaginaAnterior"] = String.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, Request.Path, Request.QueryString);
                TempData["MensagemErro"] = "Sessão expirada";
                return RedirectToAction("Login", "Acesso");
            }
            Guid idNotaGuid;
            GlResposta<Notas> resposta = new GlResposta<Notas>();
            if (!Guid.TryParse(idNota, out idNotaGuid))
            {
                return BadRequest(new GlResposta<Notas>() { Succeeded = false, Mensagem = Mensagens.FormatoIncorreto });
            }

            Notas mdlNotas = new Notas()
            {
                Id = idNotaGuid
            };

            int valorFaltas;
            if (String.IsNullOrEmpty(faltas) == false)
            {
                if (!int.TryParse(faltas, out valorFaltas))
                {
                    return BadRequest(new GlResposta<Notas>() { Succeeded = false, Mensagem = Mensagens.FormatoIncorreto });
                }
                mdlNotas.Faltas = valorFaltas;
            }

            GlResposta<Notas> result = Put<Notas>("Avaliacao/GravarFaltas", mdlNotas);

            if (result == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }

            ViewBag.Id = result.Succeeded ? Guid.Parse(result.Id) : Guid.Empty;
            ViewBag.IsSucesso = result.Succeeded == false ? "false" : "";
            ViewBag.LblMensagem = result.Mensagem;

            return PartialView("JanelaMensagem");
        }

        [HttpGet("Aluno/GetAlunos")]
        public IActionResult GetAlunos(string idTurma)
        {
            GlResposta<CmTurmaAluno> resultTurma = GetAux<CmTurmaAluno>("Servico/ObterAlunos", idTurma);

            return Ok(resultTurma?.Dados);
        }


        [HttpGet("Aluno/SetAluno")]
        public IActionResult SetAluno(string idAluno)
        {

            HttpContext.Session.SetString("IdAlunoSelecionado", idAluno);
            return Ok();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EnviarSenha(string email)
        {
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
            GlResposta<CmLogin> result;
            CmLogin aluno = new CmLogin() { Login = email, Senha = "123@Mudar" };
            result = Post<CmLogin>("Aluno/EnviarSenha", aluno);
            
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

            GlResposta<CmAluno> result = Get<CmAluno>("Aluno/Obter", id.ToString());
            if (result == null)
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }
            result = Put<CmAluno>("Aluno/Apagar", result.Dados.FirstOrDefault());
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
