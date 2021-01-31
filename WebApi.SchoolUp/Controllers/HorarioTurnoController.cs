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
    public class HorarioTurnoController : Controller
    {
        public string NomeFuncao { get; } = "SCHOOLUP_TEMPOTURMA";
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
            BllHorarioTurno brHorarioTurno = new BllHorarioTurno(Ip, NomeFuncao, Token);
            GlResposta<CmHorarioTurno> resposta = brHorarioTurno.ObterTodos(id);
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
            return NotFound(new GlResposta<CmHorarioTurno> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
        }

        [HttpGet("Obter/{id}")]
        public IActionResult Get(string id)
        {
            BllHorarioTurno brHorarioTurno = new BllHorarioTurno(Ip, NomeFuncao, Token);
            Guid idHorarioTurno;
            if (!Guid.TryParse(id, out idHorarioTurno))
            {
                return BadRequest(new GlResposta<HorarioTurno>() { Succeeded = false, Mensagem = Mensagens.FormatoIncorreto });
            }
            GlResposta<HorarioTurno> resposta = brHorarioTurno.Obter(idHorarioTurno);
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

            return NotFound(new GlResposta<HorarioTurno> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
        }

        [HttpGet("Filtrar/{filtro}")]
        public IActionResult Filtrar(string filtro)
        {
            BllHorarioTurno brHorarioTurno = new BllHorarioTurno(Ip, NomeFuncao, Token);
            GlResposta<HorarioTurno> resposta = brHorarioTurno.Filtrar(filtro);
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
            return NotFound(new GlResposta<CmHorarioTurno> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
        }

        [AllowAnonymous]
        [HttpPost("Criar")]
        public IActionResult Create([FromBody] HorarioTurno dadosHorarioTurno)
        {
            BllHorarioTurno brHorarioTurno = new BllHorarioTurno(Ip, NomeFuncao, Token);

            var resposta = brHorarioTurno.Inserir(dadosHorarioTurno);
            if (resposta.Succeeded)
            {                
                return CreatedAtRoute("DefaultApi", new { id = resposta.Id }, resposta);
            }
            return BadRequest(resposta);
        }

        [HttpPut("Modificar")]
        public IActionResult Update([FromBody] HorarioTurno dadosHorarioTurno)
        {
            BllHorarioTurno brHorarioTurno = new BllHorarioTurno(Ip, NomeFuncao, Token);
            var resposta = brHorarioTurno.Alterar(dadosHorarioTurno);
            if (resposta.Succeeded)
            {
                return Ok(resposta);
            }
            return BadRequest(resposta);
        }

        [HttpPut("Apagar")]
        public IActionResult Delete([FromBody] HorarioTurno dadosHorarioTurno)
        {
            BllHorarioTurno brHorarioTurno = new BllHorarioTurno(Ip, NomeFuncao, Token);
            var resposta = brHorarioTurno.Excluir(dadosHorarioTurno);
            if (resposta.Succeeded)
            {
                return Ok(resposta);
            }
            return BadRequest(resposta);
        }
    }
}
