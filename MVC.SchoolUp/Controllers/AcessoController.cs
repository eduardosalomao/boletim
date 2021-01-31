using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Modelo.Nucleo.Custom;
using Modelo.Nucleo.Geral;
using Modelo.SchoolUp.Custom;
using Modelo.SchoolUp.Principal;
using Newtonsoft.Json;

namespace MVC.SchoolUp.Controllers
{
    public class AcessoController : MasterController
    {
        HttpClient clientLogin = new HttpClient();
        public string UrlBaseLogin
        {
            get
            {
                var builder = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile($"appsettings.json");
                var config = builder.Build();

                return config.GetSection("API_Access:UrlBase").Value;
            }
        }

        public AcessoController()
        {
            clientLogin.DefaultRequestHeaders.Accept.Clear();
            clientLogin.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        [HttpGet("Acesso/PrimeiroAcesso")]
        public IActionResult PrimeiroAcesso()
        {
            ViewBag.LblMensagem = String.Empty;
            ViewBag.IsSucesso = String.Empty;

            string url = "alternativa";// HttpContext.Request.GetDisplayUrl();

            GlResposta<Escola> resultEscola = Get<Escola>("Servico/ObterEscolaPorCodigo", url.Split(".")[0]);
            if (resultEscola?.Dados == null || !resultEscola.Dados.Any())
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }
            HttpContext?.Session?.SetString("IdEscola", resultEscola.Dados.FirstOrDefault().Id.ToString());

            CarregarMenuObsoleto();

            GlResposta<Periodo> resultPeriodo = Get<Periodo>("Servico/ObterPeriodo", resultEscola.Dados.FirstOrDefault().Id.ToString());
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

        [HttpPost("Acesso/PrimeiroAcesso")]
        public IActionResult PrimeiroAcesso([Bind()] CmPrimeiroAcesso dados)
        {
            var idEscola = HttpContext?.Session?.GetString("IdEscola");
            Guid idEscolaGuid;
            if (!Guid.TryParse(idEscola, out idEscolaGuid))
            {
                TempData["CodigoErro"] = "";
                TempData["MensagemErro"] = "Ocorreu um erro desconhecido";
                return RedirectToAction("Erro", "Home");
            }

            CarregarMenuObsoleto();

            GlResposta<CmPrimeiroAcesso> result = Post<CmPrimeiroAcesso>("Aluno/CriarAcesso", dados);

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

        [HttpGet("Acesso/RecuperarSenha")]
        public IActionResult RecuperarSenha()
        {
            ViewBag.LblMensagem = String.Empty;
            ViewBag.IsSucesso = String.Empty;

            return View();
        }

        [HttpPost("Acesso/RecuperarSenha")]
        public IActionResult RecuperarSenha([Bind()] CmRecuperarSenha dadosRecuperarSenha)
        {
            IsAnonimo = true;
            CmLogin dadosLogin = new CmLogin() { Login = dadosRecuperarSenha.Email };
            var content = new StringContent(JsonConvert.SerializeObject(dadosLogin).ToString(), Encoding.UTF8, "application/json");
            clientLogin.Timeout = new TimeSpan(0, 2, 0);
            HttpResponseMessage response = clientLogin.PutAsync(UrlBaseLogin + "Acesso/RecuperarSenha/", content).Result;
            GlAutenticacao glResposta = JsonConvert.DeserializeObject<GlAutenticacao>(response.Content.ReadAsStringAsync().Result);

            if (glResposta.Succeeded)
            {
                ViewBag.IsSucesso = "true";
                ViewBag.LblMensagem = $"Foi enviado uma nova senha para o email {dadosLogin.Login}";
            }
            else
            {
                ViewBag.IsSucesso = "false";
                ViewBag.LblMensagem = glResposta.Mensagem;
            }

            return PartialView("JanelaMensagem");
        }

        [HttpGet("Acesso/Login")]
        public IActionResult Login()
        {
            TempData["StyleDisplayMenu"] = " style=display:none!important; ";
            TempData["StyleDisplay"] = " style=display:none!important; ";
            TempData["StyleDisplayTrocarAluno"] = " style=display:none!important; ";
            TempData["StyleDisplayNovoAluno"] = " style=display:none!important; ";

            HttpContext.Session.Clear();

            ViewBag.IsSucesso = String.Empty;

            ViewBag.LblMensagem = TempData["MensagemErro"] == null ? String.Empty : TempData["MensagemErro"].ToString();
            ViewData.Add("PaginaAnterior", TempData["PaginaAnterior"]?.ToString());
            return View();
        }

        [HttpGet("Acesso/GetAlunos")]
        public IActionResult GetAlunos(string idTurma)
        {
            GlResposta<CmTurmaAluno> resultTurma = GetAux<CmTurmaAluno>("Servico/ObterAlunos", idTurma);

            return Ok(resultTurma?.Dados);
        }

        [HttpPost("Acesso/Login")]
        public IActionResult Login([Bind("Login", "Senha")] CmLogin dadosLogin)
        {
            TempData["StyleDisplayMenu"] = " style=display:none!important; ";
            TempData["StyleDisplay"] = " style=display:none!important; ";
            TempData["StyleDisplayTrocarAluno"] = " style=display:none!important; ";
            TempData["StyleDisplayNovoAluno"] = " style=display:none!important; ";

            HttpContext.Session.Clear();

            CmRegistro dadosRegistro = new CmRegistro() { Login = dadosLogin.Login, Senha = dadosLogin.Senha };
            var content = new StringContent(JsonConvert.SerializeObject(dadosRegistro).ToString(), Encoding.UTF8, "application/json");

            HttpResponseMessage response = clientLogin.PostAsync(UrlBaseLogin + "Acesso/Login/", content).Result;
            GlAutenticacao glResposta = JsonConvert.DeserializeObject<GlAutenticacao>(response.Content.ReadAsStringAsync().Result);

            if (glResposta.Succeeded)
            {
                HttpContext.Session.SetString("TkAutenticacao", glResposta.Token);
                HttpContext.Session.SetString("IdUsuario", glResposta.Id);
                GlResposta<CmUsuario> glRespostaEscola = GetAux<CmUsuario>("Servico/ObterEscola", glResposta.Id);
                if (glRespostaEscola?.Dados?.Any() == true)
                {
                    HttpContext.Session.SetString("IdEscola", glRespostaEscola?.Dados?.FirstOrDefault()?.IdEscola.ToString());
                    HttpContext.Session.SetString("NomeUsuario", glRespostaEscola?.Dados?.FirstOrDefault()?.Nome.ToString());
                    HttpContext.Session.SetString("PerfilCodigo", JsonConvert.SerializeObject(glRespostaEscola?.Dados?.FirstOrDefault()?.PerfilCodigo));
                    List<CmAluno> listaAlunos = glRespostaEscola?.Dados?.FirstOrDefault()?.Alunos;
                    List<CmAluno> listaAlunosGroupBy = new List<CmAluno>();
                    if (listaAlunos?.Any() == true)
                    {
                        listaAlunosGroupBy =
                        (from a in listaAlunos
                         group new { a } by new { a.Id, a.Nome }
                         into g
                         select new CmAluno()
                         {
                             Id = g.Select(s => s.a.Id).FirstOrDefault()
                            , Nome = g.Select(s => s.a.Nome).FirstOrDefault()
                         }).ToList();
                    }
                    if (listaAlunosGroupBy?.Count == 1)
                    {
                        HttpContext.Session.SetString("IdAlunoSelecionado", listaAlunosGroupBy.FirstOrDefault().Id.ToString());
                    }
                    else
                    {
                        HttpContext.Session.Remove("IdAlunoSelecionado");
                    }
                    HttpContext.Session.SetString("AlunoResponsavel", JsonConvert.SerializeObject(listaAlunos));
                }
                if (!String.IsNullOrEmpty(Request.Form["HdnPaginaAnterior"]))
                {
                    return Redirect(Request.Form["HdnPaginaAnterior"]);
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("MensagemErro", glResposta.Mensagem);
            }
            return View();
        }

        [HttpGet("Acesso/Logout")]
        public IActionResult Logout()
        {
            string idUsuario = HttpContext.Session.GetString("IdUsuario");

            GlAutenticacao glResposta = new GlAutenticacao() { Succeeded = true };

            if (!String.IsNullOrEmpty(idUsuario))
            {
                CmRegistro dadosRegistro = new CmRegistro() { IdUsuario = idUsuario };
                var content = new StringContent(JsonConvert.SerializeObject(dadosRegistro).ToString(), Encoding.UTF8, "application/json");

                HttpResponseMessage response = clientLogin.PostAsync(UrlBaseLogin + "Acesso/Logout/", content).Result;
                glResposta = JsonConvert.DeserializeObject<GlAutenticacao>(response.Content.ReadAsStringAsync().Result);

            }

            if (glResposta.Succeeded)
            {
                TempData["StyleDisplayMenu"] = " style=display:none!important; ";
                TempData["StyleDisplay"] = " style=display:none!important; ";
                TempData["StyleDisplayTrocarAluno"] = " style=display:none!important; ";
                TempData["StyleDisplayNovoAluno"] = " style=display:none!important; ";

                HttpContext.Session.Clear();

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("MensagemErro", glResposta.Mensagem);
            }

            return View();
        }


        [HttpGet("Acesso/MudarSenha")]
        public IActionResult MudarSenha()
        {
            ViewBag.IsSucesso = "";
            PrepararAutenticacao();
            CarregarMenuObsoleto();
            return View();
        }

        [HttpPost("Acesso/MudarSenha")]
        public IActionResult MudarSenha(CmSenha dadosSenha)
        {
            string idUsuario = HttpContext.Session.GetString("IdUsuario");

            CmRegistro dadosRegistro = new CmRegistro() { IdUsuario = idUsuario, SenhaAntiga = dadosSenha.SenhaAntiga, Senha = dadosSenha.Senha, ConfirmaSenha = dadosSenha.ConfirmaSenha };
            var content = new StringContent(JsonConvert.SerializeObject(dadosRegistro).ToString(), Encoding.UTF8, "application/json");

            string token = HttpContext?.Session?.GetString("TkAutenticacao");
            if (token == null)
            {
                RedirectToAction("Login", "Acesso");
            }
            clientLogin.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = clientLogin.PutAsync(UrlBaseLogin + "Acesso/AlterarSenha/", content).Result;
            GlAutenticacao glResposta = JsonConvert.DeserializeObject<GlAutenticacao>(response.Content.ReadAsStringAsync().Result);

            if (glResposta?.Succeeded == true)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (String.IsNullOrEmpty(glResposta?.Mensagem))
                {
                    ModelState.AddModelError("MensagemErro", glResposta.Mensagem);
                }
                else
                {
                    ModelState.AddModelError("MensagemErro", response.StatusCode.ToString());
                }
            }
            return View();
        }
    }
}