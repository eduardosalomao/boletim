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
    public class ProfessorController : Controller
    {
        public string NomeFuncao { get; } = "SCHOOLUP_PROFESSOR";

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
            BllProfessor brProfessor = new BllProfessor(Ip, NomeFuncao, Token);
            GlResposta<Professor> resposta = brProfessor.ObterTodos(id);
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
            return NotFound(new GlResposta<Professor> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
        }

        [HttpGet("Obter/{id}")]
        public IActionResult Get(string id)
        {
            BllProfessor brProfessor = new BllProfessor(Ip, NomeFuncao, Token);
            Guid idProfessor;
            if (!Guid.TryParse(id, out idProfessor))
            {
                return BadRequest(new GlResposta<Professor>() { Succeeded = false, Mensagem = Mensagens.FormatoIncorreto });
            }
            GlResposta<Professor> resposta = brProfessor.Obter(idProfessor);
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

            return NotFound(new GlResposta<Professor> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
        }

        [HttpGet("ObterProfessorEscola/{idProfessor}/{idEscola}")]
        public IActionResult ObterEscolaProfessor(string idProfessor, string idEscola)
        {
            BllProfessor brProfessor = new BllProfessor(Ip, NomeFuncao, Token);
            Guid idProfessorGuid;
            Guid idEscolaGuid;
            if (!Guid.TryParse(idProfessor, out idProfessorGuid) || !Guid.TryParse(idEscola, out idEscolaGuid))
            {
                return BadRequest(new GlResposta<EscolaProfessor>() { Succeeded = false, Mensagem = Mensagens.FormatoIncorreto });
            }
            GlResposta<EscolaProfessor> resposta = brProfessor.ObterEscolaProfessor(idProfessorGuid, idEscolaGuid);
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
            BllProfessor brProfessor = new BllProfessor(Ip, NomeFuncao, Token);
            GlResposta<Professor> resposta = brProfessor.Filtrar(filtro);
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
            return NotFound(new GlResposta<Professor> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
        }

        [HttpPost("RegistrarDisciplina")]
        public IActionResult Create([FromBody] ProfessorDisciplina dadosProfessorDisciplina)
        {
            BllProfessor brProfessor = new BllProfessor(Ip, NomeFuncao, Token);

            var resposta = brProfessor.Inserir(dadosProfessorDisciplina);
            if (resposta.Succeeded)
            {
                return CreatedAtRoute("DefaultApi", new { id = resposta.Id }, resposta);
            }
            return BadRequest(resposta);
        }

        [AllowAnonymous]
        [HttpPost("Criar")]
        public IActionResult Create([FromBody] CmProfessor dadosProfessor)
        {
            BllProfessor brProfessor;
            brProfessor = new BllProfessor(Ip, NomeFuncao, Token);

            var resposta = brProfessor.Inserir(dadosProfessor);
            if (resposta.Succeeded)
            {                
                return CreatedAtRoute("DefaultApi", new { id = resposta.Id }, resposta);
            }
            return BadRequest(resposta);
        }

        [HttpPut("Modificar")]
        public IActionResult Update([FromBody] CmProfessor dadosProfessor)
        {
            BllProfessor brProfessor = new BllProfessor(Ip, NomeFuncao, Token);
            var resposta = brProfessor.Alterar(dadosProfessor);
            if (resposta.Succeeded)
            {
                return Ok(resposta);
            }
            return BadRequest(resposta);
        }

        [HttpPut("Apagar")]
        public IActionResult Delete([FromBody] CmProfessor dadosProfessor)
        {
            BllProfessor brProfessor = new BllProfessor(Ip, NomeFuncao, Token);
            var resposta = brProfessor.Excluir(dadosProfessor);
            if (resposta.Succeeded)
            {
                return Ok(resposta);
            }
            return BadRequest(resposta);
        }

        [HttpPut("ApagarDisciplina")]
        public IActionResult DeleteDisciplina([FromBody] ProfessorDisciplina dadosProfessorDisciplina)
        {
            BllProfessor brProfessor = new BllProfessor(Ip, NomeFuncao, Token);
            var resposta = brProfessor.Excluir(dadosProfessorDisciplina);
            if (resposta.Succeeded)
            {
                return Ok(resposta);
            }
            return BadRequest(resposta);
        }
    }
}
