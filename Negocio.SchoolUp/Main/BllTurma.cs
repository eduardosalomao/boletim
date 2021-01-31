using System;
using Negocio.Nucleo.Geral;
using Modelo.SchoolUp.Principal;
using Comum;
using Modelo.Nucleo.Geral;
using Modelo.Nucleo.Enumerador;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Modelo.SchoolUp.Recursos;
using System.Linq;
using Acesso.Nucleo.Geral;
using Modelo.SchoolUp.Custom;
using Acesso.SchoolUp.Custom;
using Negocio.SchoolUp.Auxiliar;
using Acesso.SchoolUp.Contexts;

namespace Negocio.SchoolUp.Main
{
    public class BllTurma : BllAplicacao<Turma>
    {
        public string IdUsuarioAcao { get; set; }
        public string Ip { get; set; }
        public string Funcao { get; set; }
        public string CodigoToken { get; set; }

        public BllTurma() : base()
        {
        }

        public BllTurma(string ip, string nomeFuncao, string token) : base(ip, nomeFuncao, token)
        {
            Ip = ip;
            Funcao = nomeFuncao;
            CodigoToken = token;
            IdUsuarioAcao = base.IdUsuario;
        }

        public BllTurma(string ip, string nomeFuncao) : base(ip, nomeFuncao)
        {
            IdUsuarioAcao = base.IdUsuario;
            Ip = ip;
            Funcao = nomeFuncao;
        }

        public GlResposta<CmTurmaAluno> ObterTurmaAluno(Guid idTurma)
        {
            GlResposta<CmTurmaAluno> resposta = new GlResposta<CmTurmaAluno>();
            new BllAcaoBoletim();
            DadosGravacao.IdAcao = BllAcaoBoletim.IdAcaoEditarGrade;
            try
            {
                resposta = ValidarAutenticacaoPermissao<CmTurmaAluno>(idTurma);
                if (!resposta.Succeeded)
                {
                    return resposta;
                }

                List<CmTurmaAluno> turmaAlunos = new DalTurma().ObterAlunos(idTurma);

                if (turmaAlunos == null || turmaAlunos.Count == 0)
                {
                    return new GlResposta<CmTurmaAluno>() { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado };
                }
                resposta.Dados = turmaAlunos;
                resposta.Succeeded = true;

                if (!IsAnonimo)
                {
                    DadosGravacao.IdAcao = BllAcaoBoletim.IdAcaoAcessarGrade;
                    DadosGravacao = BrHistorico.MontarHistorico(DadosGravacao, "Obter()", DadosGravacao.IdAcao);
                    new DalGenerica<CmTurmaAluno>().InserirHistorico(DadosGravacao);
                }
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public GlResposta<CmTurma> Obter(Guid idTurma)
        {
            GlResposta<CmTurma> resposta = new GlResposta<CmTurma>();
            DadosGravacao.IdAcao = BllAcao.IdAcaoDetalhar;
            try
            {
                resposta = ValidarAutenticacaoPermissao<CmTurma>(idTurma);
                if (!resposta.Succeeded)
                {
                    return resposta;
                }
                DadosGravacao = BrHistorico.MontarHistorico(DadosGravacao, "Obter()", DadosGravacao.IdAcao, idTurma.ToString());
                var respostaTurma = Read(idTurma);
                if (respostaTurma.Dados == null || respostaTurma.Dados.Count == 0)
                {
                    return new GlResposta<CmTurma>() { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado };
                }
                else
                {
                    if (respostaTurma.Dados != null && respostaTurma.Dados.Count > 0)
                    {
                        var mdlTurma = respostaTurma.Dados.FirstOrDefault();
                        CmTurma mdlCmTurma = new CmTurma()
                        {
                            Id = idTurma,
                            Nome = mdlTurma.Nome,
                            IdPeriodo = mdlTurma.IdPeriodo,
                            IdSerie = mdlTurma.IdSerie,
                            IdTurno = mdlTurma.IdTurno,
                            NumeroAlunos = mdlTurma.NumeroAlunos
                        };
                        resposta.Dados = new List<CmTurma>();
                        resposta.Dados.Add(mdlCmTurma);
                    }
                    resposta.Succeeded = respostaTurma.Succeeded;
                    resposta.Id = respostaTurma.Id;
                    resposta.Mensagem = respostaTurma.Mensagem;
                }
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public GlResposta<AlunoTurma> ObterAlunoTurma(Guid idAlunoTurma)
        {
            GlResposta<AlunoTurma> resposta = new GlResposta<AlunoTurma>();
            DadosGravacao.IdAcao = BllAcao.IdAcaoDetalhar;
            try
            {
                resposta = ValidarAutenticacaoPermissao<AlunoTurma>(idAlunoTurma);
                if (!resposta.Succeeded)
                {
                    return resposta;
                }
                DadosGravacao = BrHistorico.MontarHistorico(DadosGravacao, "Obter()", DadosGravacao.IdAcao, idAlunoTurma.ToString());
                var respostaTurma = Read<AlunoTurma>(idAlunoTurma);
                if (respostaTurma.Dados == null || respostaTurma.Dados.Count == 0)
                {
                    return new GlResposta<AlunoTurma>() { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado };
                }
                else
                {
                    if (respostaTurma.Dados != null && respostaTurma.Dados.Count > 0)
                    {
                        var mdlAlunoTurma = respostaTurma.Dados.FirstOrDefault();
                        resposta.Dados = new List<AlunoTurma>();
                        resposta.Dados.Add(mdlAlunoTurma);
                    }
                    resposta.Succeeded = respostaTurma.Succeeded;
                    resposta.Id = respostaTurma.Id;
                    resposta.Mensagem = respostaTurma.Mensagem;
                }
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public GlResposta<CmTurma> ObterTodos(Guid idEscola)
        {
            GlResposta<CmTurma> resposta = new GlResposta<CmTurma>();
            DadosGravacao.IdAcao = BllAcao.IdAcaoAcessar;
            try
            {
                resposta = ValidarAutenticacaoPermissao<CmTurma>();
                if (!resposta.Succeeded)
                {
                    return resposta;
                }

                List<CmTurma> turmas = new DalTurma().ObterTodos(idEscola);

                if (turmas == null || turmas.Count == 0)
                {
                    return new GlResposta<CmTurma>() { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado };
                }
                resposta.Dados = turmas;
                resposta.Succeeded = true;

                if (!IsAnonimo)
                {
                    DadosGravacao.IdAcao = BllAcao.IdAcaoAcessar;
                    DadosGravacao = BrHistorico.MontarHistorico(DadosGravacao, "Obter()", DadosGravacao.IdAcao);
                    new DalGenerica<CmTurma>().InserirHistorico(DadosGravacao);
                }
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public GlResposta<Turma> Filtrar(string filtro)
        {
            GlResposta<Turma> resposta = new GlResposta<Turma>();
            DadosGravacao.IdAcao = BllAcao.IdAcaoProcurar;
            try
            {
                resposta = ValidarAutenticacaoPermissao<Turma>();
                if (!resposta.Succeeded)
                {
                    return resposta;
                }
                DadosGravacao = BrHistorico.MontarHistorico(DadosGravacao, "Obter()", DadosGravacao.IdAcao);
                resposta = Search(i => i.Nome.Contains(filtro) && i.Excluido == false, o => o.Nome, true);
                if (resposta.Dados == null || resposta.Dados.Count == 0)
                {
                    return new GlResposta<Turma>() { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado };
                }
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public GlResposta<AlunoTurma> Inserir(AlunoTurma dados)
        {
            GlResposta<AlunoTurma> resposta = new GlResposta<AlunoTurma>() { Succeeded = false };
            DadosGravacao.IdAcao = BllAcao.IdAcaoInserir;
            try
            {
                resposta = ValidaDados(dados, EAcoes.ADD);
                if (resposta.Succeeded == false)
                {
                    return resposta;
                }
                NotValidate = true;
                AlunoTurma mdlAlunoTurma = new AlunoTurma()
                {
                    Id = Guid.NewGuid(),
                    IdTurma = dados.IdTurma,
                    IdAluno = dados.IdAluno
                };
                List<Disciplina> discplinas = new DalDisciplina().ObterTodosDisciplinas(dados.IdTurma);
                SchoolContext context = new SchoolContext();
                resposta = Create<AlunoTurma>(context, mdlAlunoTurma.Id, mdlAlunoTurma);

                GlResposta<Inscricao> respostaInscricao = Search<Inscricao>(i => i.IdTurma == dados.IdTurma && i.IdAluno == dados.IdAluno && i.Excluido == false, o => o.Excluido.ToString(), true);
                if (respostaInscricao != null && respostaInscricao.Dados != null)
                {
                    foreach (Inscricao inscricao in respostaInscricao.Dados)
                    {
                        inscricao.Excluido = true;
                        respostaInscricao = DeleteLogical<Inscricao>(context, inscricao);
                        if (respostaInscricao.Succeeded == false)
                        {
                            return new GlResposta<AlunoTurma>() { Mensagem = respostaInscricao.Mensagem, Succeeded = false };
                        }
                    }
                }

                if (discplinas.Any())
                {
                    foreach (Disciplina disciplina in discplinas)
                    {
                        Inscricao inscricao = new Inscricao()
                        {
                            Id = Guid.NewGuid(),
                            IdAluno = dados.IdAluno,
                            IdDisciplina = disciplina.Id,
                            IdTurma = dados.IdTurma,
                            DataInscricao = DateTime.Now,
                            IsDependencia = false,
                            Excluido = false
                        };
                        respostaInscricao = Create<Inscricao>(context, inscricao.Id, inscricao);
                        if (respostaInscricao.Succeeded == false)
                        {
                            return new GlResposta<AlunoTurma>() { Mensagem = respostaInscricao.Mensagem, Succeeded = false };
                        }
                    }
                }
                DadosGravacao.IdHistorico = Guid.NewGuid().ToString();
                resposta = Salvar<AlunoTurma>(context, DadosGravacao);
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public GlResposta<Turma> Inserir(CmTurma dados)
        {
            GlResposta<Turma> resposta = new GlResposta<Turma>() { Succeeded = false };
            DadosGravacao.IdAcao = BllAcao.IdAcaoInserir;
            try
            {
                Turma mdlTurma = new Turma()
                {
                    Id = Guid.NewGuid(),
                    IdPeriodo = dados.IdPeriodo,
                    IdSerie = dados.IdSerie,
                    IdTurno = dados.IdTurno,
                    Nome = dados.Nome,
                    NumeroAlunos = dados.NumeroAlunos,
                    Excluido = false
                };
                resposta = Create(mdlTurma.Id, mdlTurma);
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public GlResposta<Turma> Alterar(CmTurma dados)
        {
            GlResposta<Turma> resposta = new GlResposta<Turma>() { Succeeded = false };
            DadosGravacao.IdAcao = BllAcao.IdAcaoAlterar;
            try
            {
                Turma mdlTurma = new Turma()
                {
                    Id = dados.Id,
                    IdPeriodo = dados.IdPeriodo,
                    IdSerie = dados.IdSerie,
                    IdTurno = dados.IdTurno,
                    Nome = dados.Nome,
                    NumeroAlunos = dados.NumeroAlunos,
                    Excluido = false
                };
                resposta = Update(dados.Id, mdlTurma);
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public GlResposta<Turma> Excluir(CmTurma dados)
        {
            GlResposta<Turma> resposta = new GlResposta<Turma>() { Succeeded = false };
            DadosGravacao.IdAcao = BllAcao.IdAcaoExcluir;
            try
            {
                Turma mdlTurma = new Turma()
                {
                    Id = dados.Id,
                    IdPeriodo = dados.IdPeriodo,
                    IdSerie = dados.IdSerie,
                    IdTurno = dados.IdTurno,
                    Nome = dados.Nome,
                    NumeroAlunos = dados.NumeroAlunos,
                    Excluido = true
                };
                resposta = DeleteLogical(mdlTurma.Id, mdlTurma);
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public GlResposta<AlunoTurma> Excluir(AlunoTurma dados)
        {
            GlResposta<AlunoTurma> resposta = new GlResposta<AlunoTurma>() { Succeeded = false };
            DadosGravacao.IdAcao = BllAcao.IdAcaoExcluir;
            try
            {
                AlunoTurma mdlAlunoTurma = new AlunoTurma()
                {
                    Id = dados.Id,
                    IdAluno = dados.IdAluno,
                    IdTurma = dados.IdTurma,
                    Excluido = true
                };
                NotValidate = true;
                resposta = DeleteLogical(mdlAlunoTurma.Id, mdlAlunoTurma);
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public GlResposta<AlunoTurma> ValidaDados(AlunoTurma dados, EAcoes acao)
        {
            if (dados == null)
            {
                return new GlResposta<AlunoTurma>()
                {
                    Succeeded = false,
                    Mensagem = Mensagens.FormatoIncorreto
                };
            }
            GlResposta<AlunoTurma> resposta = new GlResposta<AlunoTurma>()
            {
                Succeeded = true,
                Mensagem = acao.Equals(EAcoes.DELETE) ? Mensagens.RegistroExcluidoSucesso : Mensagens.RegistroGravadoSucesso
            };

            if (acao.Equals(EAcoes.UPDATE) || acao.Equals(EAcoes.DELETE))
            {
                var objetoRetorno = Read(dados.Id);
                if (objetoRetorno.Dados == null)
                {
                    return new GlResposta<AlunoTurma>() { Succeeded = false, Mensagem = Mensagens.SemRegistroEncontrado };
                }
            }

            if (acao.Equals(EAcoes.ADD) || acao.Equals(EAcoes.UPDATE))
            {
                var resultadoValidacao = new List<ValidationResult>();
                var contexto = new ValidationContext(dados, null, null);
                Validator.TryValidateObject(dados, contexto, resultadoValidacao, true);
                resposta.Succeeded = !(resultadoValidacao.Count > 0);
                if (!resposta.Succeeded)
                {
                    resposta.Mensagem = String.Empty;
                }
                foreach (var item in resultadoValidacao)
                {
                    resposta.Mensagem += item.ErrorMessage + Mensagens.CaracterePulaLinha;
                }
            }

            return resposta;
        }

        public override GlResposta<Turma> ValidaDados(Turma dados, EAcoes acao)
        {
            if (dados == null)
            {
                return new GlResposta<Turma>()
                {
                    Succeeded = false,
                    Mensagem = Mensagens.FormatoIncorreto
                };
            }
            GlResposta<Turma> resposta = new GlResposta<Turma>()
            {
                Succeeded = true,
                Mensagem = acao.Equals(EAcoes.DELETE) ? Mensagens.RegistroExcluidoSucesso : Mensagens.RegistroGravadoSucesso
            };

            if (acao.Equals(EAcoes.UPDATE) || acao.Equals(EAcoes.DELETE))
            {
                var objetoRetorno = Read(dados.Id);
                if (objetoRetorno.Dados == null)
                {
                    return new GlResposta<Turma>() { Succeeded = false, Mensagem = Mensagens.SemRegistroEncontrado };
                }
            }

            if (acao.Equals(EAcoes.ADD) || acao.Equals(EAcoes.UPDATE))
            {
                var resultadoValidacao = new List<ValidationResult>();
                var contexto = new ValidationContext(dados, null, null);
                Validator.TryValidateObject(dados, contexto, resultadoValidacao, true);
                resposta.Succeeded = !(resultadoValidacao.Count > 0);
                if (!resposta.Succeeded)
                {
                    resposta.Mensagem = String.Empty;
                }
                foreach (var item in resultadoValidacao)
                {
                    resposta.Mensagem += item.ErrorMessage + Mensagens.CaracterePulaLinha;
                }
            }

            return resposta;
        }
    }
}
