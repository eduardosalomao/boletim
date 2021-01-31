using Microsoft.AspNetCore.Mvc;
using Modelo.SchoolUp.Custom;
using System;
using Modelo.Nucleo.Geral;
using Microsoft.AspNetCore.Authorization;
using Modelo.SchoolUp.Recursos;
using Negocio.SchoolUp.Main;
using Modelo.SchoolUp.Principal;
using System.Collections.Generic;

namespace WebApi.SchoolUp.Controllers
{
    [Authorize("Bearer")]
    [Route("apiUp/[controller]")]
    [Route("apiUp/[controller]/[action]")]
    public class DisciplinaController : Controller
    {
        public string NomeFuncao { get; } = "SCHOOLUP_DISCIPLINA";
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
            BllDisciplina brDisciplina = new BllDisciplina(Ip, NomeFuncao, Token);
            GlResposta<CmDisciplina> resposta = brDisciplina.ObterTodos(id);
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
            return NotFound(new GlResposta<CmDisciplina> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
        }

        [HttpGet("ObterGrade/{id}")]
        public IActionResult GetGrade(Guid id)
        {
            BllDisciplina brDisciplina = new BllDisciplina(Ip, NomeFuncao, Token);
            GlResposta<CmDisciplinaHorario> resposta = brDisciplina.ObterGrade(id);
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
            return NotFound(new GlResposta<CmDisciplinaHorario> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
        }

        [HttpGet("ObterDisciplinaProfessor/{idProfessor}/{idEscola}")]
        public IActionResult GetDisciplinaProfessor(Guid idProfessor, Guid idEscola)
        {
            BllDisciplina brDisciplina = new BllDisciplina(Ip, "SCHOOLUP_PROFESSOR", Token);
            GlResposta<CmProfessorDisciplina> resposta = brDisciplina.ObterTodosProfessorComDisciplina(idProfessor, idEscola);
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
            return NotFound(new GlResposta<CmProfessorDisciplina> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
        }

        [HttpPut("GravarGrade")]
        public IActionResult Update([FromBody] List<DisciplinaHorario> dadosDisciplina)
        {
            BllDisciplina brDisciplina = new BllDisciplina(Ip, NomeFuncao, Token);
            var resposta = brDisciplina.GravarGrade(dadosDisciplina);
            if (resposta.Succeeded)
            {
                return Ok(resposta);
            }
            return BadRequest(resposta);
        }

        [HttpGet("Obter/{id}")]
        public IActionResult Get(string id)
        {
            BllDisciplina brDisciplina = new BllDisciplina(Ip, NomeFuncao, Token);
            Guid idDisciplina;
            if (!Guid.TryParse(id, out idDisciplina))
            {
                return BadRequest(new GlResposta<Disciplina>() { Succeeded = false, Mensagem = Mensagens.FormatoIncorreto });
            }
            GlResposta<Disciplina> resposta = brDisciplina.Obter(idDisciplina);
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

            return NotFound(new GlResposta<CmDisciplina> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
        }

        [HttpGet("Filtrar/{filtro}")]
        public IActionResult Filtrar(string filtro)
        {
            BllDisciplina brDisciplina = new BllDisciplina(Ip, NomeFuncao, Token);
            GlResposta<Disciplina> resposta = brDisciplina.Filtrar(filtro);
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
            return NotFound(new GlResposta<CmDisciplina> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
        }

        [AllowAnonymous]
        [HttpPost("Criar")]
        public IActionResult Create([FromBody] Disciplina dadosDisciplina)
        {
            BllDisciplina brDisciplina = new BllDisciplina(Ip, NomeFuncao, Token);
            var resposta = brDisciplina.Inserir(dadosDisciplina);
            if (resposta.Succeeded)
            {                
                return CreatedAtRoute("DefaultApi", new { id = resposta.Id }, resposta);
            }
            return BadRequest(resposta);
        }

        [HttpPut("Modificar")]
        public IActionResult Update([FromBody] Disciplina dadosDisciplina)
        {
            BllDisciplina brDisciplina = new BllDisciplina(Ip, NomeFuncao, Token);
            var resposta = brDisciplina.Alterar(dadosDisciplina);
            if (resposta.Succeeded)
            {
                return Ok(resposta);
            }
            return BadRequest(resposta);
        }

        [HttpPut("Apagar")]
        public IActionResult Delete([FromBody] Disciplina dadosDisciplina)
        {
            BllDisciplina brDisciplina = new BllDisciplina(Ip, NomeFuncao, Token);
            var resposta = brDisciplina.Excluir(dadosDisciplina);
            if (resposta.Succeeded)
            {
                return Ok(resposta);
            }
            return BadRequest(resposta);
        }
    }
}
