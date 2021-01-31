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
    public class SubPeriodoController : Controller
    {
        public string NomeFuncao { get; } = "SCHOOLUP_SUBPERIODO";
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
            BllSubPeriodo brSubPeriodo = new BllSubPeriodo(Ip, NomeFuncao, Token);
            GlResposta<CmSubPeriodo> resposta = brSubPeriodo.ObterTodos(id);
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
            return NotFound(new GlResposta<CmSubPeriodo> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
        }

        [HttpGet("Obter/{id}")]
        public IActionResult Get(string id)
        {
            BllSubPeriodo brSubPeriodo = new BllSubPeriodo(Ip, NomeFuncao, Token);
            Guid idSubPeriodo;
            if (!Guid.TryParse(id, out idSubPeriodo))
            {
                return BadRequest(new GlResposta<SubPeriodo>() { Succeeded = false, Mensagem = Mensagens.FormatoIncorreto });
            }
            GlResposta<SubPeriodo> resposta = brSubPeriodo.Obter(idSubPeriodo);
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

            return NotFound(new GlResposta<SubPeriodo> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
        }

        [HttpGet("Filtrar/{filtro}")]
        public IActionResult Filtrar(string filtro)
        {
            BllSubPeriodo brSubPeriodo = new BllSubPeriodo(Ip, NomeFuncao, Token);
            GlResposta<SubPeriodo> resposta = brSubPeriodo.Filtrar(filtro);
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
            return NotFound(new GlResposta<CmSubPeriodo> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
        }

        [AllowAnonymous]
        [HttpPost("Criar")]
        public IActionResult Create([FromBody] SubPeriodo dadosSubPeriodo)
        {
            BllSubPeriodo brSubPeriodo = new BllSubPeriodo(Ip, NomeFuncao, Token);

            var resposta = brSubPeriodo.Inserir(dadosSubPeriodo);
            if (resposta.Succeeded)
            {                
                return CreatedAtRoute("DefaultApi", new { id = resposta.Id }, resposta);
            }
            return BadRequest(resposta);
        }

        [HttpPut("Modificar")]
        public IActionResult Update([FromBody] SubPeriodo dadosSubPeriodo)
        {
            BllSubPeriodo brSubPeriodo = new BllSubPeriodo(Ip, NomeFuncao, Token);
            var resposta = brSubPeriodo.Alterar(dadosSubPeriodo);
            if (resposta.Succeeded)
            {
                return Ok(resposta);
            }
            return BadRequest(resposta);
        }

        [HttpPut("Apagar")]
        public IActionResult Delete([FromBody] SubPeriodo dadosSubPeriodo)
        {
            BllSubPeriodo brSubPeriodo = new BllSubPeriodo(Ip, NomeFuncao, Token);
            var resposta = brSubPeriodo.Excluir(dadosSubPeriodo);
            if (resposta.Succeeded)
            {
                return Ok(resposta);
            }
            return BadRequest(resposta);
        }
    }
}
