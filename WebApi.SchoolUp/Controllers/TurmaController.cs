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
    public class TurmaController : Controller
    {
        public string NomeFuncao { get; } = "SCHOOLUP_TURMA";
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
            BllTurma brTurma = new BllTurma(Ip, NomeFuncao, Token);
            GlResposta<CmTurma> resposta = brTurma.ObterTodos(id);
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
            return NotFound(new GlResposta<Turma> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
        }

        [HttpGet("ObterGrade/{id}")]
        public IActionResult GetGrade(Guid id)
        {
            BllTurma brTurma = new BllTurma(Ip, NomeFuncao, Token);
            GlResposta<CmTurmaAluno> resposta = brTurma.ObterTurmaAluno(id);
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
            return NotFound(new GlResposta<CmTurmaAluno> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
        }

        [HttpGet("Obter/{id}")]
        public IActionResult Get(string id)
        {
            BllTurma brTurma = new BllTurma(Ip, NomeFuncao, Token);
            Guid idTurma;
            if (!Guid.TryParse(id, out idTurma))
            {
                return BadRequest(new GlResposta<CmTurma>() { Succeeded = false, Mensagem = Mensagens.FormatoIncorreto });
            }
            GlResposta<CmTurma> resposta = brTurma.Obter(idTurma);
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

            return NotFound(new GlResposta<CmTurma> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
        }

        [HttpGet("ObterAlunoTurma/{id}")]
        public IActionResult GetAlunoTurma(string id)
        {
            BllTurma brTurma = new BllTurma(Ip, NomeFuncao, Token);
            Guid idTurma;
            if (!Guid.TryParse(id, out idTurma))
            {
                return BadRequest(new GlResposta<AlunoTurma>() { Succeeded = false, Mensagem = Mensagens.FormatoIncorreto });
            }
            GlResposta<AlunoTurma> resposta = brTurma.ObterAlunoTurma(idTurma);
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

            return NotFound(new GlResposta<CmTurma> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
        }
                
        [HttpGet("Filtrar/{filtro}")]
        public IActionResult Filtrar(string filtro)
        {
            BllTurma brTurma = new BllTurma(Ip, NomeFuncao, Token);
            GlResposta<Turma> resposta = brTurma.Filtrar(filtro);
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
            return NotFound(new GlResposta<Turma> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
        }

        [HttpPost("MatricularAluno")]
        public IActionResult Create([FromBody] AlunoTurma dadosTurma)
        {
            BllTurma brTurma = new BllTurma(Ip, NomeFuncao, Token);

            var resposta = brTurma.Inserir(dadosTurma);
            if (resposta.Succeeded)
            {
                return CreatedAtRoute("DefaultApi", new { id = resposta.Id }, resposta);
            }
            return BadRequest(resposta);
        }

        [HttpPost("Criar")]
        public IActionResult Create([FromBody] CmTurma dadosTurma)
        {
            BllTurma brTurma = new BllTurma(Ip, NomeFuncao, Token);

            var resposta = brTurma.Inserir(dadosTurma);
            if (resposta.Succeeded)
            {                
                return CreatedAtRoute("DefaultApi", new { id = resposta.Id }, resposta);
            }
            return BadRequest(resposta);
        }

        [HttpPut("Modificar")]
        public IActionResult Update([FromBody] CmTurma dadosTurma)
        {
            BllTurma brTurma = new BllTurma(Ip, NomeFuncao, Token);
            var resposta = brTurma.Alterar(dadosTurma);
            if (resposta.Succeeded)
            {
                return Ok(resposta);
            }
            return BadRequest(resposta);
        }

        [HttpPut("Apagar")]
        public IActionResult Delete([FromBody] CmTurma dadosTurma)
        {
            BllTurma brTurma = new BllTurma(Ip, NomeFuncao, Token);
            var resposta = brTurma.Excluir(dadosTurma);
            if (resposta.Succeeded)
            {
                return Ok(resposta);
            }
            return BadRequest(resposta);
        }

        [HttpPut("ApagarAlunoTurma")]
        public IActionResult Delete([FromBody] AlunoTurma dadosAlunoTurma)
        {
            BllTurma brTurma = new BllTurma(Ip, NomeFuncao, Token);
            var resposta = brTurma.Excluir(dadosAlunoTurma);
            if (resposta.Succeeded)
            {
                return Ok(resposta);
            }
            return BadRequest(resposta);
        }
    }
}
