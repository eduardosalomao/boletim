using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Modelo.Nucleo.Geral;
using Modelo.SchoolUp.Custom;
using Modelo.SchoolUp.Principal;
using Modelo.SchoolUp.Recursos;
using Negocio.SchoolUp;
using Negocio.SchoolUp.Auxiliar;
using Negocio.SchoolUp.Custom;

namespace WebApi.SchoolUp.Controllers
{
    [Produces("application/json")]
    [Route("apiUp/[controller]")]
    [Route("apiUp/[controller]/[action]")]
    public class ServicoController : Controller
    {
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
        [HttpGet("ObterAreaConhecimento")]
        public IActionResult GetAreaConhecimento()
        {
            BllAreaConhecimento brAreaConhecimento = new BllAreaConhecimento();
            GlResposta<AreaConhecimento> resposta = brAreaConhecimento.Obter();
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
            return NotFound(new GlResposta<AreaConhecimento> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
        }

        [HttpGet("ObterAreaConhecimento/{id}")]
        public IActionResult GetAreaConhecimento(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return NotFound(new GlResposta<AreaConhecimento> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
            }
            Guid idCategoria;
            if (!Guid.TryParse(id, out idCategoria))
            {
                return BadRequest(new GlResposta<AreaConhecimento>() { Succeeded = false, Mensagem = Mensagens.FormatoIncorreto });
            }
            BllAreaConhecimento brAreaConhecimento = new BllAreaConhecimento();
            GlResposta<AreaConhecimento> resposta = brAreaConhecimento.Obter(idCategoria);
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
            return NotFound(new GlResposta<AreaConhecimento> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
        }

        [HttpGet("ObterEscola/{id}")]
        public IActionResult GetEscola(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return NotFound(new GlResposta<CmUsuario> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
            }
            Guid idUsuario;
            if (!Guid.TryParse(id, out idUsuario))
            {
                return BadRequest(new GlResposta<CmUsuario>() { Succeeded = false, Mensagem = Mensagens.FormatoIncorreto });
            }
            BllEscola brEscola = new BllEscola();
            GlResposta<CmUsuario> resposta = brEscola.Obter(idUsuario);
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
            return NotFound(new GlResposta<CmUsuario> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
        }

        [HttpGet("ObterAlunos/{idTurma}")]
        public IActionResult GetAlunos(string idTurma)
        {
            BllTurma brTurma = new BllTurma();
            Guid idTurmaGuid;
            if (!Guid.TryParse(idTurma, out idTurmaGuid))
            {
                return BadRequest(new GlResposta<CmTurmaAluno>() { Succeeded = false, Mensagem = Mensagens.FormatoIncorreto });
            }
            GlResposta<CmTurmaAluno> resposta = brTurma.ObterAlunos(idTurmaGuid);
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

            return NotFound(new GlResposta<CmTurmaAluno> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
        }

        [HttpGet("ObterAlunoPorResponsavel/{idUsuario}")]
        public IActionResult GetAlunoPorResponsavel(string idUsuario)
        {
            BllAluno brAluno = new BllAluno();
            Guid idUsuarioGuid;
            if (!Guid.TryParse(idUsuario, out idUsuarioGuid))
            {
                return BadRequest(new GlResposta<CmTurmaAluno>() { Succeeded = false, Mensagem = Mensagens.FormatoIncorreto });
            }
            GlResposta<CmTurmaAluno> resposta = brAluno.ObterAlunoPorResponsavel(idUsuarioGuid);
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

            return NotFound(new GlResposta<Aluno> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
        }

        [HttpGet("ObterTurmaAluno/{idAluno}/{idPeriodo}")]
        public IActionResult GetTurmaAluno(string idAluno, string idPeriodo)
        {
            BllAluno brAluno = new BllAluno();
            Guid idAlunoGuid;
            Guid idPeriodoGuid;
            if (!Guid.TryParse(idAluno, out idAlunoGuid) || !Guid.TryParse(idPeriodo, out idPeriodoGuid))
            {
                return BadRequest(new GlResposta<CmTurmaAluno>() { Succeeded = false, Mensagem = Mensagens.FormatoIncorreto });
            }
            GlResposta<CmTurmaAluno> resposta = brAluno.ObterTurmaAluno(idAlunoGuid, idPeriodoGuid);
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

            return NotFound(new GlResposta<Aluno> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
        }

        [HttpGet("ObterTurmas/{idAluno}")]
        public IActionResult GetTurmas(string idAluno)
        {
            BllAluno brAluno = new BllAluno();
            Guid idAlunoGuid;
            GlResposta<CmTurmaAluno> resposta = brAluno.ObterTurmas(idAlunoGuid);
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

            return NotFound(new GlResposta<Aluno> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
        }

        [HttpGet("ObterEscolaPorCodigo/{codigo}")]
        public IActionResult GetEscolaPorCodigo(string codigo)
        {
            if (String.IsNullOrEmpty(codigo))
            {
                return NotFound(new GlResposta<Escola> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
            }

            BllEscola brEscola = new BllEscola();
            brEscola.IsAnonimo = true;
            GlResposta<Escola> resposta = brEscola.ObterPorCodigo(codigo);
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
            return NotFound(new GlResposta<Escola> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
        }

        [HttpGet("ObterEnsino/{id}")]
        public IActionResult GetEnsino(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return NotFound(new GlResposta<Ensino> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
            }
            Guid idEscola;
            if (!Guid.TryParse(id, out idEscola))
            {
                return BadRequest(new GlResposta<Ensino>() { Succeeded = false, Mensagem = Mensagens.FormatoIncorreto });
            }
            BllEnsino brEnsino = new BllEnsino();
            GlResposta<Ensino> resposta = brEnsino.ObterTodos(idEscola);
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
            return NotFound(new GlResposta<Ensino> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
        }

        [HttpGet("ObterTipoAvaliacao")]
        public IActionResult GetTipoAvaliacao()
        {
            BllTipoAvaliacao brTipoAvaliacao = new BllTipoAvaliacao();
            GlResposta<TipoAvaliacao> resposta = brTipoAvaliacao.ObterTodos();
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
            return NotFound(new GlResposta<Ensino> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
        }

        [HttpGet("ObterTodosProfessorComDisciplina/{idEscola}")]
        public IActionResult GetProfessorComDisiciplina(Guid idEscola)
        {
            BllProfessor brProfessor = new BllProfessor();
            GlResposta<CmProfessorDisciplina> resposta = brProfessor.ObterTodosProfessorComDisciplina(idEscola);
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

        [HttpGet("ObterTodosProfessorPorTurma/{idTurma}")]
        public IActionResult GetProfessorPorTurma(Guid idTurma)
        {
            BllProfessor brProfessor = new BllProfessor();
            GlResposta<CmProfessorDisciplina> resposta = brProfessor.ObterTodosProfessorPorTurma(idTurma);
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

        [HttpGet("ObterTodosProfessorPorDisciplina/{idDisciplina}")]
        public IActionResult GetProfessorPorDisiciplina(Guid idDisciplina)
        {
            BllProfessor brProfessor = new BllProfessor();
            GlResposta<CmProfessorDisciplina> resposta = brProfessor.ObterTodosProfessorPorDisciplina(idDisciplina);
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

        [HttpGet("ObterPeriodo/{id}")]
        public IActionResult GetPeriodo(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return NotFound(new GlResposta<Periodo> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
            }
            Guid idEscola;
            if (!Guid.TryParse(id, out idEscola))
            {
                return BadRequest(new GlResposta<Periodo>() { Succeeded = false, Mensagem = Mensagens.FormatoIncorreto });
            }
            BllPeriodo brPeriodo = new BllPeriodo();
            GlResposta<Periodo> resposta = brPeriodo.ObterTodos(idEscola);
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


        [HttpGet("ObterSubPeriodo/{idPeriodo}")]
        public IActionResult GetSubPeriodo(string idPeriodo)
        {
            if (String.IsNullOrEmpty(idPeriodo))
            {
                return NotFound(new GlResposta<SubPeriodo> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
            }
            Guid idPeriodoGuid;
            if (!Guid.TryParse(idPeriodo, out idPeriodoGuid))
            {
                return BadRequest(new GlResposta<SubPeriodo>() { Succeeded = false, Mensagem = Mensagens.FormatoIncorreto });
            }
            BllPeriodo brPeriodo = new BllPeriodo();
            GlResposta<SubPeriodo> resposta = brPeriodo.ObterTodosSubPeriodo(idPeriodoGuid);
            if (resposta.Succeeded)
            {
                if (resposta.Dados?.Count > 0)
                {
                    DateTime dataAtual = DateTime.Today;
                    Guid? idSubPeriodoAtual = resposta.Dados.Where(i => i.De <= dataAtual && i.Ate >= dataAtual).FirstOrDefault()?.Id;

                    if (idSubPeriodoAtual != null)
                    {
                        resposta.Id = idSubPeriodoAtual.ToString();
                    }
                    
                    return Ok(resposta);
                }
            }
            else
            {
                return BadRequest(resposta);
            }
            return NotFound(new GlResposta<SubPeriodo> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
        }

        [HttpGet("ObterDisciplinaMediasPeriodo/{idPeriodo}")]
        public IActionResult GetDisciplinaMedias(string idPeriodo)
        {
            if (String.IsNullOrEmpty(idPeriodo))
            {
                return NotFound(new GlResposta<CmDisciplinaMedias> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
            }
            Guid idPeriodoGuid;
            if (!Guid.TryParse(idPeriodo, out idPeriodoGuid))
            {
                return BadRequest(new GlResposta<CmDisciplinaMedias>() { Succeeded = false, Mensagem = Mensagens.FormatoIncorreto });
            }
            BllDisciplina brDisciplina = new BllDisciplina();
            GlResposta<CmDisciplinaMedias> resposta = brDisciplina.ObterMediasPorPeriodo(idPeriodoGuid);
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
            return NotFound(new GlResposta<CmDisciplinaMedias> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
        }

        [HttpGet("VerificarPermissao/{nomeFuncao}")]
        public IActionResult VerificarPermissao(string nomeFuncao)
        {
            BllAplicacao<string> brPermissao = new BllAplicacao<string>(Ip, nomeFuncao, Token);
            Guid idUsuarioGuid;
            if (!Guid.TryParse(brPermissao.IdUsuario, out idUsuarioGuid))
            {
                return BadRequest(new GlResposta<string>() { Succeeded = false, Mensagem = Mensagens.FormatoIncorreto });
            }
            GlResposta<string> resposta = brPermissao.VerificarPermissao(nomeFuncao);
            if (resposta.Succeeded)
            {
                return Ok(resposta);
            }

            return BadRequest(resposta);
        }

        [HttpGet("ObterDisciplinaMediasTurma/{idTurma}")]
        public IActionResult GetDisciplinaMediasTurma(string idTurma)
        {
            if (String.IsNullOrEmpty(idTurma))
            {
                return NotFound(new GlResposta<CmDisciplinaMedias> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
            }
            Guid idTurmaGuid;
            if (!Guid.TryParse(idTurma, out idTurmaGuid))
            {
                return BadRequest(new GlResposta<CmDisciplinaMedias>() { Succeeded = false, Mensagem = Mensagens.FormatoIncorreto });
            }
            BllDisciplina brDisciplina = new BllDisciplina();
            GlResposta<CmDisciplinaMedias> resposta = brDisciplina.ObterMediasPorTurma(idTurmaGuid);
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
            return NotFound(new GlResposta<CmDisciplinaMedias> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
        }

        [HttpGet("ObterDisciplinaMediasAluno/{idAluno}/{idPeriodo}")]
        public IActionResult GetDisciplinaMediasAluno(string idAluno, string idPeriodo)
        {
            if (String.IsNullOrEmpty(idAluno))
            {
                return NotFound(new GlResposta<CmDisciplinaMedias> { Succeeded = true, Mensagem = String.Format(Mensagens.CampoObrigatorio, "idAluno") });
            }
            Guid idAlunoGuid;
            if (!Guid.TryParse(idAluno, out idAlunoGuid))
            {
                return BadRequest(new GlResposta<CmDisciplinaMedias>() { Succeeded = false, Mensagem = Mensagens.FormatoIncorreto });
            }
            if (String.IsNullOrEmpty(idPeriodo))
            {
                return NotFound(new GlResposta<CmDisciplinaMedias> { Succeeded = true, Mensagem = String.Format(Mensagens.CampoObrigatorio, "idPeriodo") });
            }
            Guid idPeriodoGuid;
            if (!Guid.TryParse(idPeriodo, out idPeriodoGuid))
            {
                return BadRequest(new GlResposta<CmDisciplinaMedias>() { Succeeded = false, Mensagem = Mensagens.FormatoIncorreto });
            }
            BllDisciplina brDisciplina = new BllDisciplina();
            GlResposta<CmDisciplinaMedias> resposta = brDisciplina.ObterMediasPorAluno(idAlunoGuid, idPeriodoGuid);
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
            return NotFound(new GlResposta<CmDisciplinaMedias> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
        }

        [HttpGet("ObterSerie/{id}")]
        public IActionResult GetSerie(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return NotFound(new GlResposta<Serie> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
            }
            Guid idEscola;
            if (!Guid.TryParse(id, out idEscola))
            {
                return BadRequest(new GlResposta<Serie>() { Succeeded = false, Mensagem = Mensagens.FormatoIncorreto });
            }
            BllSerie brSerie = new BllSerie();
            GlResposta<Serie> resposta = brSerie.ObterTodos(idEscola);
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
            return NotFound(new GlResposta<Serie> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
        }

        [HttpGet("ObterTurma/{id}")]
        public IActionResult GetTurma(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return NotFound(new GlResposta<Turma> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
            }
            Guid idPeriodo;
            if (!Guid.TryParse(id, out idPeriodo))
            {
                return BadRequest(new GlResposta<Turma>() { Succeeded = false, Mensagem = Mensagens.FormatoIncorreto });
            }
            BllTurma brTurma = new BllTurma();
            GlResposta<Turma> resposta = brTurma.ObterPorPeriodo(idPeriodo);
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
            return NotFound(new GlResposta<Ensino> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
        }
        
        [HttpGet("ObterTurno/{id}")]
        public IActionResult GetTurno(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return NotFound(new GlResposta<Turno> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
            }
            Guid idEscola;
            if (!Guid.TryParse(id, out idEscola))
            {
                return BadRequest(new GlResposta<Turno>() { Succeeded = false, Mensagem = Mensagens.FormatoIncorreto });
            }
            BllTurno brTurno = new BllTurno();
            GlResposta<Turno> resposta = brTurno.ObterTodos(idEscola);
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
            return NotFound(new GlResposta<Ensino> { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado });
        }
    }
}