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
    public class AvaliacaoController : Controller
    {
        public string NomeFuncao { get; } = "SCHOOLUP_WAVALIACAO";
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

        [HttpGet("ObterGrade")]
        [HttpGet("ObterGrade/{idTurma}")]
        [HttpGet("ObterGrade/{idTurma}/{idSubPeriodo}")]
        public IActionResult GetGrade(string idTurma, string idSubPeriodo)
        {
            BllAvaliacao brDisciplina = new BllAvaliacao(Ip, NomeFuncao, Token);
            Guid idTurmaGuid, idSubPeriodoGuid;
            Guid? idTurmaGuidNulo = null;
            Guid? idSubPeriodoGuidNulo = null;

            if (!String.IsNullOrEmpty(idTurma))
            {
                if (!Guid.TryParse(idTurma, out idTurmaGuid))
                {
                    return BadRequest(new GlResposta<CmAvaliacao>() { Succeeded = false, Mensagem = Mensagens.FormatoIncorreto });
                }
                idTurmaGuidNulo = idTurmaGuid;
            }
            if (!String.IsNullOrEmpty(idSubPeriodo))
            {
                if (!Guid.TryParse(idSubPeriodo, out idSubPeriodoGuid))
                {
                    return BadRequest(new GlResposta<CmAvaliacao>() { Succeeded = false, Mensagem = Mensagens.FormatoIncorreto });
                }
                idSubPeriodoGuidNulo = idSubPeriodoGuid;
            }
            GlResposta<CmAvaliacao> resposta = brDisciplina.ObterGrade(idTurmaGuidNulo, idSubPeriodoGuidNulo);
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
            return NotFound(new GlResposta<CmAvaliacao> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
        }

        [HttpGet("ObterGradeDisciplina/{idTurma}/{idSubPeriodo}")]
        public IActionResult GetGradeDisciplina(Guid idTurma, Guid idSubPeriodo)
        {
            BllAvaliacao brDisciplina = new BllAvaliacao(Ip, NomeFuncao, Token);
            GlResposta<CmAvaliacao> resposta = brDisciplina.ObterGradeDisciplina(idTurma, idSubPeriodo);
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

        //[HttpGet("ObterDisciplinaProfessor/{idProfessor}/{idEscola}")]
        //public IActionResult GetDisciplinaProfessor(Guid idProfessor, Guid idEscola)
        //{
        //    BllDisciplina brDisciplina = new BllDisciplina(Ip, "SCHOOLUP_PROFESSOR", Token);
        //    GlResposta<CmProfessorDisciplina> resposta = brDisciplina.ObterTodosProfessorComDisciplina(idProfessor, idEscola);
        //    if (resposta.Succeeded)
        //    {
        //        if (resposta.Dados?.Count > 0)
        //        {
        //            return Ok(resposta);
        //        }
        //    }
        //    else
        //    {
        //        return BadRequest(resposta);
        //    }
        //    return NotFound(new GlResposta<CmProfessorDisciplina> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
        //}

        [HttpPut("GravarNotaCompleta")]
        public IActionResult UpdateNotaCompleto([FromBody] Notas mdlNotas)
        {
            BllAvaliacao brAvaliacao = new BllAvaliacao(Ip, NomeFuncao, Token);
            GlResposta<Notas> resposta = new GlResposta<Notas>();
            resposta = brAvaliacao.AlterarNotaCompleto(mdlNotas);

            if (resposta.Succeeded)
            {
                return Ok(resposta);
            }

            return BadRequest(resposta);
        }


        [HttpPut("GravarNota")]
        public IActionResult UpdateNota([FromBody] Notas mdlNotas)
        {
            BllAvaliacao brAvaliacao = new BllAvaliacao(Ip, NomeFuncao, Token);
            GlResposta<Notas> resposta = new GlResposta<Notas>();
            resposta = brAvaliacao.AlterarNota(mdlNotas);

            if (resposta.Succeeded)
            {
                return Ok(resposta);
            }

            return BadRequest(resposta);
        }

        [HttpPut("GravarNotaRecuperacao")]
        public IActionResult UpdateNotaRecuperacao([FromBody] Notas mdlNotas)
        {
            BllAvaliacao brAvaliacao = new BllAvaliacao(Ip, NomeFuncao, Token);
            GlResposta<Notas> resposta = new GlResposta<Notas>();
            resposta = brAvaliacao.AlterarNotaRecuperacao(mdlNotas);

            if (resposta.Succeeded)
            {
                return Ok(resposta);
            }

            return BadRequest(resposta);
        }

        [HttpPut("GravarFaltas")]
        public IActionResult UpdateFaltas([FromBody] Notas mdlNotas)
        {
            BllAvaliacao brAvaliacao = new BllAvaliacao(Ip, NomeFuncao, Token);
            GlResposta<Notas> resposta = new GlResposta<Notas>();
            resposta = brAvaliacao.AlterarFaltas(mdlNotas);
            
            if (resposta.Succeeded)
            {
                return Ok(resposta);
            }

            return BadRequest(resposta);
        }

        [HttpGet("Obter/{id}")]
        public IActionResult Get(string id)
        {
            BllAvaliacao brAvaliacao = new BllAvaliacao(Ip, NomeFuncao, Token);
            Guid idAvaliacao;
            if (!Guid.TryParse(id, out idAvaliacao))
            {
                return BadRequest(new GlResposta<Avaliacao>() { Succeeded = false, Mensagem = Mensagens.FormatoIncorreto });
            }
            GlResposta<Avaliacao> resposta = brAvaliacao.Obter(idAvaliacao);
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

            return NotFound(new GlResposta<Avaliacao> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
        }

        [HttpGet("ObterNotas/{idAvaliacao}")]
        public IActionResult GetNotas(string idAvaliacao)
        {
            BllAvaliacao brAvaliacao = new BllAvaliacao(Ip, NomeFuncao, Token);
            Guid idAvaliacaoGuid;
            if (!Guid.TryParse(idAvaliacao, out idAvaliacaoGuid))
            {
                return BadRequest(new GlResposta<CmNotas>() { Succeeded = false, Mensagem = Mensagens.FormatoIncorreto });
            }
            GlResposta<CmNotas> resposta = brAvaliacao.ObterNotas(idAvaliacaoGuid);
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

            return NotFound(new GlResposta<CmAvaliacao> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
        }

        [HttpGet("ObterNotasAluno")]
        [HttpGet("ObterNotasAluno/{idAluno}")]
        public IActionResult GetNotasAluno(string idAluno)
        {
            BllAvaliacao brAvaliacao = new BllAvaliacao(Ip, NomeFuncao, Token);
            Guid idAvaliacaoGuid;
            if (!Guid.TryParse(idAluno, out idAvaliacaoGuid))
            {
                return BadRequest(new GlResposta<CmNotas>() { Succeeded = false, Mensagem = Mensagens.FormatoIncorreto });
            }
            GlResposta<CmNotas> resposta = brAvaliacao.ObterNotasAluno(idAvaliacaoGuid);
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

            return NotFound(new GlResposta<CmAvaliacao> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
        }

        [HttpGet("ObterNotasAlunoBimestre")]
        [HttpGet("ObterNotasAlunoBimestre/{idAluno}/{idPeriodo}")]
        [HttpGet("ObterNotasAlunoBimestre/{idAluno}/{idPeriodo}/{idBimestre}")]
        public IActionResult GetNotasAlunoBimestre(string idAluno, string idPeriodo, string idBimestre)
        {
            BllAvaliacao brAvaliacao = new BllAvaliacao(Ip, NomeFuncao, Token);
            Guid idAlunoGuid;
            if (!Guid.TryParse(idAluno, out idAlunoGuid))
            {
                return BadRequest(new GlResposta<CmNotas>() { Succeeded = false, Mensagem = Mensagens.FormatoIncorreto });
            }
            Guid idPeriodoGuid;
            if (!Guid.TryParse(idPeriodo, out idPeriodoGuid))
            {
                return BadRequest(new GlResposta<CmNotas>() { Succeeded = false, Mensagem = Mensagens.FormatoIncorreto });
            }

            GlResposta<CmNotas> resposta;
            Guid idBimestreGuid;

            if (String.IsNullOrEmpty(idBimestre))
            {
                resposta = brAvaliacao.ObterNotasAluno(idAlunoGuid, idPeriodoGuid, null);
            }
            else
            {
                if (!Guid.TryParse(idBimestre, out idBimestreGuid))
                {
                    return BadRequest(new GlResposta<CmNotas>() { Succeeded = false, Mensagem = Mensagens.FormatoIncorreto });
                }
                resposta = brAvaliacao.ObterNotasAluno(idAlunoGuid, idPeriodoGuid, idBimestreGuid);
            }

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

            return Ok(new GlResposta<Notas> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado, Dados = new List<Notas>() });
        }
        [HttpPost("Criar")]
        public IActionResult Create([FromBody] Avaliacao dadosAvaliacao)
        {
            BllAvaliacao brAvaliacao = new BllAvaliacao(Ip, NomeFuncao, Token);
            var resposta = brAvaliacao.Inserir(dadosAvaliacao);
            if (resposta.Succeeded)
            {
                return CreatedAtRoute("DefaultApi", new { id = resposta.Id }, resposta);
            }
            return BadRequest(resposta);
        }
        
        [HttpPost("CriarDisciplina")]
        public IActionResult CreateDisciplina([FromBody] Avaliacao dadosAvaliacao)
        {
            BllAvaliacao brAvaliacao = new BllAvaliacao(Ip, NomeFuncao, Token);
            var resposta = brAvaliacao.InserirDisciplina(dadosAvaliacao);
            if (resposta.Succeeded)
            {
                return CreatedAtRoute("DefaultApi", new { id = resposta.Id }, resposta);
            }
            return BadRequest(resposta);
        }

        [HttpPut("Modificar")]
        public IActionResult Update([FromBody] Avaliacao dadosAvaliacao)
        {
            BllAvaliacao brAvaliacao = new BllAvaliacao(Ip, NomeFuncao, Token);
            var resposta = brAvaliacao.Alterar(dadosAvaliacao);
            if (resposta.Succeeded)
            {
                return Ok(resposta);
            }
            return BadRequest(resposta);
        }

        [HttpPut("Apagar")]
        public IActionResult Delete([FromBody] Avaliacao dadosAvaliacao)
        {
            BllAvaliacao brAvaliacao = new BllAvaliacao(Ip, NomeFuncao, Token);
            var resposta = brAvaliacao.Excluir(dadosAvaliacao);
            if (resposta.Succeeded)
            {
                return Ok(resposta);
            }
            return BadRequest(resposta);
        }
    }
}
