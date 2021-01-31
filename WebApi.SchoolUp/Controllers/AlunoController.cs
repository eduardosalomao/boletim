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
    public class AlunoController : Controller
    {
        public string NomeFuncao { get; } = "SCHOOLUP_ALUNO";
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
            BllAluno brAluno = new BllAluno(Ip, NomeFuncao, Token);
            GlResposta<Aluno> resposta = brAluno.ObterTodos(id);
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
            return NotFound(new GlResposta<Aluno> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
        }

        [HttpGet("Obter/{id}")]
        public IActionResult Get(string id)
        {
            BllAluno brAluno = new BllAluno(Ip, NomeFuncao, Token);
            Guid idAluno;
            if (!Guid.TryParse(id, out idAluno))
            {
                return BadRequest(new GlResposta<CmAluno>() { Succeeded = false, Mensagem = Mensagens.FormatoIncorreto });
            }
            GlResposta<CmAluno> resposta = brAluno.Obter(idAluno);
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

            return NotFound(new GlResposta<CmAluno> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
        }

        [HttpGet("Filtrar/{filtro}/{idEscola}")]
        public IActionResult Filtrar(string filtro, Guid idEscola)
        {
            BllAluno brAluno = new BllAluno(Ip, NomeFuncao, Token);
            GlResposta<Aluno> resposta = brAluno.Filtrar(filtro, idEscola);
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
            return NotFound(new GlResposta<Aluno> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
        }

        
        [HttpPost("Criar")]
        public IActionResult Create([FromBody] CmAluno dadosAluno)
        {
            BllAluno brAluno = new BllAluno(Ip, NomeFuncao, Token);

            var resposta = brAluno.Inserir(dadosAluno);
            if (resposta.Succeeded)
            {                
                return CreatedAtRoute("DefaultApi", new { id = resposta.Id }, resposta);
            }
            return BadRequest(resposta);
        }

        [AllowAnonymous]
        [HttpPost("CriarAcesso")]
        public IActionResult CreateAcesso([FromBody] CmPrimeiroAcesso dadosAluno)
        {
            BllAluno brAluno = new BllAluno(Ip, NomeFuncao, Token);

            var resposta = brAluno.CriarAcesso(dadosAluno);
            if (resposta.Succeeded)
            {
                return CreatedAtRoute("DefaultApi", new { id = resposta.Id }, resposta);
            }
            return BadRequest(resposta);
        }

        [HttpPost("NovoAcesso")]
        public IActionResult NewAcesso([FromBody] CmNovoAcesso dadosAluno)
        {
            BllAluno brAluno = new BllAluno(Ip, NomeFuncao, Token);

            var resposta = brAluno.CriarAcesso(dadosAluno);
            if (resposta.Succeeded)
            {
                return CreatedAtRoute("DefaultApi", new { id = resposta.Id }, resposta);
            }
            return BadRequest(resposta);
        }

        //[HttpPost("EnviarSenha")]
        //public IActionResult SendPassword([FromBody] CmLogin dadosLogin)
        //{
        //    BllAluno brAluno = new BllAluno(Ip, NomeFuncao, Token);

        //    var resposta = brAluno.EnviarEmail(dadosLogin);
        //    if (resposta.Succeeded)
        //    {
        //        return CreatedAtRoute("DefaultApi", new { id = resposta.Id }, resposta);
        //    }
        //    return BadRequest(resposta);
        //}


        [HttpPut("Modificar")]
        public IActionResult Update([FromBody] CmAluno dadosAluno)
        {
            BllAluno brAluno = new BllAluno(Ip, NomeFuncao, Token);
            var resposta = brAluno.Alterar(dadosAluno);
            if (resposta.Succeeded)
            {
                return Ok(resposta);
            }
            return BadRequest(resposta);
        }

        [HttpPut("Apagar")]
        public IActionResult Delete([FromBody] CmAluno dadosAluno)
        {
            BllAluno brAluno = new BllAluno(Ip, NomeFuncao, Token);
            var resposta = brAluno.Excluir(dadosAluno);
            if (resposta.Succeeded)
            {
                return Ok(resposta);
            }
            return BadRequest(resposta);
        }
    }
}
