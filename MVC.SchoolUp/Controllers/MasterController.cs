using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Modelo.Nucleo.Geral;
using Modelo.Nucleo.Models;
using Modelo.SchoolUp.Custom;
using Newtonsoft.Json;

namespace MVC.SchoolUp.Controllers
{
    public class MasterController : Controller
    {
        public HttpClient client = new HttpClient();

        public string UrlBase
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
        public string IdUsuario { get; set; }
        public string Token { get; set; }
        public bool IsAnonimo { get; set; }
        public string NomeFuncao { get; set; }

        public List<CmAluno> ListaAlunos { get; set; }
        public List<string> ListaPerfis { get; set; }

        private static readonly string UrlRenovacao = "Acesso/RenovarToken";

        public MasterController()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public GlResposta<string> VerificarPermissao()
        {
            return GetAux<string>("Servico/VerificarPermissao", NomeFuncao);
        }

        public void CarregarMenuObsoleto()
        {            
        }


        public void CarregarMenu()
        {
            ListaPerfis = JsonConvert.DeserializeObject<List<string>>(HttpContext?.Session?.GetString("PerfilCodigo") ?? "");
            ViewBag.Perfil = ListaPerfis;
            ListaAlunos = JsonConvert.DeserializeObject<List<CmAluno>>(HttpContext?.Session?.GetString("AlunoResponsavel") ?? "");

            string idAlunoSelecionado = HttpContext?.Session?.GetString("IdAlunoSelecionado");
            if (!String.IsNullOrWhiteSpace(idAlunoSelecionado) && ListaAlunos?.Any() == true)
            {
                TempData["NomeAluno"] = ListaAlunos.Where(i => i.Id.ToString() == idAlunoSelecionado).FirstOrDefault().Nome;
            }

            if (ListaPerfis?.Any() == true && !ListaPerfis.Contains("SCHOOLUP_ADMIN") && !ListaPerfis.Contains("SCHOOLUP_SECRETARIA") && String.IsNullOrWhiteSpace(idAlunoSelecionado)
                && HttpContext.Request.Path.HasValue && HttpContext.Request.Path.Value != "/Aluno/Mudar")
            {
                var redirect = RedirectToAction("Mudar", "Aluno");
                ActionContext action = new ActionContext();
                action.HttpContext = HttpContext;
                redirect.ExecuteResult(action);
            }
        }

        public void PrepararAutenticacao()
        {
            if (!IsAnonimo)
            {
                Token = HttpContext?.Session?.GetString("TkAutenticacao");
                if (Token == null)
                {
                    RedirectToAction("Login", "Acesso");
                }
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            }
            IdUsuario = HttpContext?.Session?.GetString("IdUsuario");
            ViewBag.NomeUsuario = HttpContext?.Session?.GetString("NomeUsuario");
            CarregarMenu();
        }

        private void RenovarAutorizacao()
        {
            try
            {
                HttpResponseMessage response = client.GetAsync(UrlBase + UrlRenovacao + "/" + HttpContext?.Session?.GetString("IdUsuario")).Result;
                GlResposta<UsuarioAutenticacao> respostaAutenticacao = FormatarResposta<UsuarioAutenticacao>(response);
                HttpContext.Session.SetString("TkAutenticacao", respostaAutenticacao.Dados?.FirstOrDefault()?.Token);
                HttpContext.Session.SetString("IdUsuario", respostaAutenticacao?.Id);
            }
            catch 
            {
            }
        }

        private GlResposta<T> FormatarResposta<T>(HttpResponseMessage response) where T : class
        {
            if (response == null)
            {
                TempData["CodigoErro"] = response.StatusCode.GetHashCode().ToString();
                TempData["MensagemErro"] = "Ocorreu um erro inesperado";
                return null;
            }
            var content = response.Content.ReadAsStringAsync().Result;
            if (content == null)
            {
                TempData["CodigoErro"] = response.StatusCode.GetHashCode().ToString();
                TempData["MensagemErro"] = response.RequestMessage.Content == null ? "Erro não identificado" : response.RequestMessage.Content.ToString();
                return null;
            }
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                TempData["CodigoErro"] = response.StatusCode.GetHashCode().ToString();
                TempData["MensagemErro"] = "Não autorizado";
                return null;
            }
            GlResposta<T> resultCliente = JsonConvert.DeserializeObject<GlResposta<T>>(content);
            if (resultCliente == null)
            {
                TempData["CodigoErro"] = response.StatusCode.GetHashCode().ToString();
                TempData["MensagemErro"] = response.RequestMessage.Content == null ? "Erro não identificado" : response.RequestMessage.Content.ToString();
                return null;
            }

            return resultCliente;
        }

        public PartialViewResult JanelaMensagem()
        {
            return PartialView();
        }

        public GlResposta<T> Get<T>(string url, string id) where T:class
        {
            PrepararAutenticacao();
            HttpResponseMessage response = client.GetAsync(UrlBase + url + "/" + id).Result;
            GlResposta<T> resposta = FormatarResposta<T>(response);
            if (resposta != null)
            {
                RenovarAutorizacao();
            }
            return resposta;
        }

        public GlResposta<T> Get<T>(string url, string[] ids) where T : class
        {
            PrepararAutenticacao();
            string id = "";
            foreach (var item in ids)
            {
                id += "/" + (item ?? "");
            }
            HttpResponseMessage response = client.GetAsync(UrlBase + url + id).Result;
            GlResposta<T> resposta = FormatarResposta<T>(response);
            if (resposta != null)
            {
                RenovarAutorizacao();
            }
            return resposta;
        }

        public GlResposta<T> GetAux<T>(string url, string[] ids) where T : class
        {
            PrepararAutenticacao();
            string id = "";
            foreach (var item in ids)
            {
                if (!String.IsNullOrEmpty(item))
                {
                    id += "/" + item;
                }
            }
            HttpResponseMessage response = client.GetAsync(UrlBase + url + id).Result;
            GlResposta<T> resposta = FormatarResposta<T>(response);

            return resposta;
        }

        public GlResposta<T> GetAux<T>(string url, string id) where T : class
        {
            PrepararAutenticacao();
            HttpResponseMessage response = client.GetAsync(UrlBase + url + "/" + id).Result;
            GlResposta<T> resposta = FormatarResposta<T>(response);

            return resposta;
        }

        public GlResposta<T> Search<T>(string url, string filtro) where T : class
        {
            PrepararAutenticacao();
            HttpResponseMessage response = client.GetAsync(UrlBase + url + "/" + filtro).Result;
            GlResposta<T> resposta = FormatarResposta<T>(response);

            return resposta;
        }

        public GlResposta<T> Get<T>(string url) where T : class
        {
            PrepararAutenticacao();
            HttpResponseMessage response = client.GetAsync(UrlBase + url).Result;
            GlResposta<T> resposta = FormatarResposta<T>(response);
            if (resposta != null)
            {
                RenovarAutorizacao();
            }
            return resposta;
        }

        public GlResposta<T> GetAux<T>(string url) where T : class
        {
            PrepararAutenticacao();
            HttpResponseMessage response = client.GetAsync(UrlBase + url).Result;
            GlResposta<T> resposta = FormatarResposta<T>(response);
            
            return resposta;
        }

        public GlResposta<T> Put<T>(string url, T mdlDados) where T : class
        {
            PrepararAutenticacao();
            var content = new StringContent(JsonConvert.SerializeObject(mdlDados).ToString(), Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PutAsync(UrlBase + url, content).Result;
            GlResposta<T> resposta = FormatarResposta<T>(response);
            if (resposta != null)
            {
                RenovarAutorizacao();
            }
            return resposta;
        }

        public GlResposta<T> Put<T>(string url, List<T> listDados) where T : class
        {
            PrepararAutenticacao();
            var content = new StringContent(JsonConvert.SerializeObject(listDados).ToString(), Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PutAsync(UrlBase + url, content).Result;
            GlResposta<T> resposta = FormatarResposta<T>(response);
            if (resposta != null)
            {
                RenovarAutorizacao();
            }
            return resposta;
        }

        public GlResposta<T> Post<T>(string url, T mdlDados) where T : class
        {
            PrepararAutenticacao();
            var content = new StringContent(JsonConvert.SerializeObject(mdlDados).ToString(), Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(UrlBase + url, content).Result;
            GlResposta<T> resposta = FormatarResposta<T>(response);
            if (resposta != null)
            {
                RenovarAutorizacao();
            }
            return resposta;
        }

        public GlResposta<T> Delete<T>(string url, string id) where T : class
        {
            PrepararAutenticacao();
            HttpResponseMessage response = client.DeleteAsync(UrlBase + url + "/" + id).Result;
            GlResposta<T> resposta = FormatarResposta<T>(response);
            if (resposta != null)
            {
                RenovarAutorizacao();
            }
            return resposta;
        }
    }
}