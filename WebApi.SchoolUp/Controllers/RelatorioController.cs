using Microsoft.AspNetCore.Mvc;
using Modelo.SchoolUp.Custom;
using System;
using Modelo.Nucleo.Geral;
using Microsoft.AspNetCore.Authorization;
using Modelo.SchoolUp.Recursos;
using Negocio.SchoolUp.Main;
using Modelo.SchoolUp.Principal;
using Negocio.SchoolUp.Relatorio;
using Modelo.SchoolUp.Relatorio;

namespace WebApi.SchoolUp.Controllers
{
    [Authorize("Bearer")]
    [Route("apiUp/[controller]")]
    [Route("apiUp/[controller]/[action]")]
    public class RelatorioController : Controller
    {
        public string NomeFuncao { get; } = "SCHOOLUP_";
        public string Ip
        {
            get
            {
                var ipRemoto = Request.HttpContext.Connection.RemoteIpAddress;
                if (ipRemoto == null)
                {
                    return "indefinido";
                }
                return ipRemoto.ToString();
            }
        }
        public string Token
        {
            get
            {
                try
                {
                    var headerAutorization = Request.Headers["Authorization"];
                    if (headerAutorization.Count == 0)
                    {
                        return "indefinido";
                    }
                    return headerAutorization.ToString().Replace("Bearer ", "");
                }
                catch (Exception)
                {
                    return "indefinido";
                }
            }
        }

        [HttpPost("ObterAcessoResponsavel")]
        public IActionResult ObterAcessoResponsavel([FromBody] RptAcessoResponsavel mdlRptAcessoResponsavel)
        {
            BllRelatorio brRelatorio = new BllRelatorio(Ip, "SCHOOLUP_RELACESRESP", Token);
            GlResposta<RptAcessoResponsavel> resposta = brRelatorio.ObterAcessoResponsavel(mdlRptAcessoResponsavel);
            if (resposta.Succeeded)
            {
                if (resposta.Dados?.Count > 0)
                {
                    return Ok(resposta);
                }
            }
            else
            {
                return Ok(resposta);
            }
            return Ok(new GlResposta<RptAcessoResponsavel> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
        }
    }
}
