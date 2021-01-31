using Microsoft.AspNetCore.Mvc;
using Modelo.SchoolUp.Custom;
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
    public class SerieController : Controller
    {
        public string NomeFuncao { get; } = "SCHOOLUP_SERIE";
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
            BllSerie brSerie = new BllSerie(Ip, NomeFuncao, Token);
            GlResposta<CmSerie> resposta = brSerie.ObterTodos(id);
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
            return NotFound(new GlResposta<CmSerie> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
        }

        [HttpGet("Obter/{id}")]
        public IActionResult Get(string id)
        {
            BllSerie brSerie = new BllSerie(Ip, NomeFuncao, Token);
            Guid idSerie;
            if (!Guid.TryParse(id, out idSerie))
            {
                return BadRequest(new GlResposta<Serie>() { Succeeded = false, Mensagem = Mensagens.FormatoIncorreto });
            }
            GlResposta<Serie> resposta = brSerie.Obter(idSerie);
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

            return NotFound(new GlResposta<Serie> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
        }

        [HttpGet("Filtrar/{filtro}")]
        public IActionResult Filtrar(string filtro)
        {
            BllSerie brSerie = new BllSerie(Ip, NomeFuncao, Token);
            GlResposta<Serie> resposta = brSerie.Filtrar(filtro);
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
            return NotFound(new GlResposta<CmSerie> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
        }

        [AllowAnonymous]
        [HttpPost("Criar")]
        public IActionResult Create([FromBody] Serie dadosSerie)
        {
            BllSerie brSerie = new BllSerie(Ip, NomeFuncao, Token);

            var resposta = brSerie.Inserir(dadosSerie);
            if (resposta.Succeeded)
            {                
                return CreatedAtRoute("DefaultApi", new { id = resposta.Id }, resposta);
            }
            return BadRequest(resposta);
        }

        [HttpPut("Modificar")]
        public IActionResult Update([FromBody] Serie dadosSerie)
        {
            BllSerie brSerie = new BllSerie(Ip, NomeFuncao, Token);
            var resposta = brSerie.Alterar(dadosSerie);
            if (resposta.Succeeded)
            {
                return Ok(resposta);
            }
            return BadRequest(resposta);
        }

        [HttpPut("Apagar")]
        public IActionResult Delete([FromBody] Serie dadosSerie)
        {
            BllSerie brSerie = new BllSerie(Ip, NomeFuncao, Token);
            var resposta = brSerie.Excluir(dadosSerie);
            if (resposta.Succeeded)
            {
                return Ok(resposta);
            }
            return BadRequest(resposta);
        }
    }
}
