using Microsoft.AspNetCore.Mvc;
using System;
using Modelo.Nucleo.Geral;
using Microsoft.AspNetCore.Authorization;
using Modelo.SchoolUp.Recursos;
using Negocio.SchoolUp.Main;
using Modelo.SchoolUp.Principal;

namespace WebApi.SchoolUp.Controllers
{
    [Authorize("Bearer")]
    [Route("apiUp/[controller]")]
    [Route("apiUp/[controller]/[action]")]
    public class PeriodoController : Controller
    {
        public string NomeFuncao { get; } = "SCHOOLUP_PERIODO";
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

        [HttpGet("ObterTodos/{id}")]
        public IActionResult GetAll(Guid id)
        {
            BllPeriodo brPeriodo = new BllPeriodo(Ip, NomeFuncao, Token);
            GlResposta<Periodo> resposta = brPeriodo.ObterTodos(id);
            if (resposta.Succeeded)
            {
                if (resposta.Dados?.Count > 0)
                {
                    return Ok(resposta);
                }
            }
            else
            {
                return BadRequest(resposta);
            }
            return NotFound(new GlResposta<Periodo> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
        }

        [HttpGet("Obter/{id}")]
        public IActionResult Get(string id)
        {
            BllPeriodo brPeriodo = new BllPeriodo(Ip, NomeFuncao, Token);
            Guid idPeriodo;
            if (!Guid.TryParse(id, out idPeriodo))
            {
                return BadRequest(new GlResposta<Periodo>() { Succeeded = false, Mensagem = Mensagens.FormatoIncorreto });
            }
            GlResposta<Periodo> resposta = brPeriodo.Obter(idPeriodo);
            if (resposta.Succeeded)
            {
                if (resposta.Dados != null)
                {
                    return Ok(resposta);
                }
            }
            else
            {
                return BadRequest(resposta);
            }

            return NotFound(new GlResposta<Periodo> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
        }

        [HttpGet("Filtrar/{filtro}")]
        public IActionResult Filtrar(string filtro)
        {
            BllPeriodo brPeriodo = new BllPeriodo(Ip, NomeFuncao, Token);
            GlResposta<Periodo> resposta = brPeriodo.Filtrar(filtro);
            if (resposta.Succeeded)
            {
                if (resposta.Dados?.Count > 0)
                {
                    return Ok(resposta);
                }
            }
            else
            {
                return BadRequest(resposta);
            }
            return NotFound(new GlResposta<Periodo> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
        }

        [AllowAnonymous]
        [HttpPost("Criar")]
        public IActionResult Create([FromBody] Periodo dadosPeriodo)
        {
            BllPeriodo brPeriodo = new BllPeriodo(Ip, NomeFuncao, Token);
            var resposta = brPeriodo.Inserir(dadosPeriodo);
            if (resposta.Succeeded)
            {                
                return CreatedAtRoute("DefaultApi", new { id = resposta.Id }, resposta);
            }
            return BadRequest(resposta);
        }

        [HttpPut("Modificar")]
        public IActionResult Update([FromBody] Periodo dadosPeriodo)
        {
            BllPeriodo brPeriodo = new BllPeriodo(Ip, NomeFuncao, Token);
            var resposta = brPeriodo.Alterar(dadosPeriodo);
            if (resposta.Succeeded)
            {
                return Ok(resposta);
            }
            return BadRequest(resposta);
        }

        [HttpPut("Apagar")]
        public IActionResult Delete([FromBody] Periodo dadosPeriodo)
        {
            BllPeriodo brPeriodo = new BllPeriodo(Ip, NomeFuncao, Token);
            var resposta = brPeriodo.Excluir(dadosPeriodo);
            if (resposta.Succeeded)
            {
                return Ok(resposta);
            }
            return BadRequest(resposta);
        }
    }
}
