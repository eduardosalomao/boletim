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
using Acesso.SchoolUp.Contexts;
using Modelo.SchoolUp.Custom;
using Acesso.SchoolUp.Custom;

namespace Negocio.SchoolUp.Main
{
    public class BllProfessor : BllAplicacao<Professor>
    {
        public string IdUsuarioAcao { get; set; }
        public string Ip { get; set; }
        public string Funcao { get; set; }
        public string CodigoToken { get; set; }

        public BllProfessor(string ip, string nomeFuncao, string token) : base(ip, nomeFuncao, token)
        {
            Ip = ip;
            Funcao = nomeFuncao;
            CodigoToken = token;
            IdUsuarioAcao = base.IdUsuario;
        }

        public BllProfessor(string ip, string nomeFuncao) : base(ip, nomeFuncao)
        {
            IdUsuarioAcao = base.IdUsuario;
            Ip = ip;
            Funcao = nomeFuncao;
        }

        public GlResposta<Professor> Obter(Guid idProfessor)
        {
            GlResposta<Professor> resposta = new GlResposta<Professor>();
            DadosGravacao.IdAcao = BllAcao.IdAcaoDetalhar;
            try
            {
                resposta = ValidarAutenticacaoPermissao<Professor>(idProfessor);
                if (!resposta.Succeeded)
                {
                    return resposta;
                }
                DadosGravacao = BrHistorico.MontarHistorico(DadosGravacao, "Obter()", DadosGravacao.IdAcao, idProfessor.ToString());
                resposta = Read(idProfessor);
                if (resposta.Dados == null || resposta.Dados.Count == 0)
                {
                    return new GlResposta<Professor>() { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado };
                }
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public GlResposta<EscolaProfessor> ObterEscolaProfessor(Guid idProfessor, Guid idEscola)
        {
            GlResposta<EscolaProfessor> resposta = new GlResposta<EscolaProfessor>();
            DadosGravacao.IdAcao = BllAcao.IdAcaoDetalhar;
            try
            {
                resposta = ValidarAutenticacaoPermissao<EscolaProfessor>(idProfessor);
                if (!resposta.Succeeded)
                {
                    return resposta;
                }
                DadosGravacao = BrHistorico.MontarHistorico(DadosGravacao, "Obter()", DadosGravacao.IdAcao, idProfessor.ToString());
                resposta = Search<EscolaProfessor>(i => i.IdProfessor == idProfessor && i.IdEscola == idEscola && i.Excluido == false, o => o.Matricula, true);
                if (resposta.Dados == null || resposta.Dados.Count == 0)
                {
                    return new GlResposta<EscolaProfessor>() { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado };
                }
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public GlResposta<Professor> ObterTodos(Guid idEscola)
        {
            GlResposta<Professor> resposta = new GlResposta<Professor>();
            DadosGravacao.IdAcao = BllAcao.IdAcaoAcessar;
            try
            {
                resposta = ValidarAutenticacaoPermissao<Professor>();
                if (!resposta.Succeeded)
                {
                    return resposta;
                }
                resposta = Search<Professor>(i => i.Ativo.HasValue && i.Ativo.Value && !i.Excluido && i.EscolaProfessor.Any(t => t.IdEscola.Equals(idEscola)), o => o.Nome, true);
                if (resposta.Dados == null || resposta.Dados.Count == 0)
                {
                    return new GlResposta<Professor>() { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado };
                }
                if (!IsAnonimo)
                {
                    DadosGravacao.IdAcao = BllAcao.IdAcaoAcessar;
                    DadosGravacao = BrHistorico.MontarHistorico(DadosGravacao, "Obter()", DadosGravacao.IdAcao);
                    new DalGenerica<Professor>().InserirHistorico(DadosGravacao);
                }
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public GlResposta<Professor> Filtrar(string filtro)
        {
            GlResposta<Professor> resposta = new GlResposta<Professor>();
            DadosGravacao.IdAcao = BllAcao.IdAcaoProcurar;
            try
            {
                resposta = ValidarAutenticacaoPermissao<Professor>();
                if (!resposta.Succeeded)
                {
                    return resposta;
                }
                DadosGravacao = BrHistorico.MontarHistorico(DadosGravacao, "Obter()", DadosGravacao.IdAcao);
                resposta = Search(i => i.Nome.Contains(filtro) && i.Excluido == false, o => o.Nome, true);
                if (resposta.Dados == null || resposta.Dados.Count == 0)
                {
                    return new GlResposta<Professor>() { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado };
                }
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }


        public GlResposta<ProfessorDisciplina> Inserir(ProfessorDisciplina dados)
        {
            GlResposta<ProfessorDisciplina> resposta = new GlResposta<ProfessorDisciplina>() { Succeeded = false };
            DadosGravacao.IdAcao = BllAcao.IdAcaoInserir;
            try
            {
                resposta = ValidaDados(dados, EAcoes.ADD);
                if (resposta.Succeeded == false)
                {
                    return resposta;
                }
                NotValidate = true;
                resposta = Create<ProfessorDisciplina>(dados.Id, dados);
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }


        public GlResposta<Professor> Inserir(CmProfessor dados)
        {
            GlResposta<Professor> resposta = new GlResposta<Professor>() { Succeeded = false };
            DadosGravacao.IdAcao = BllAcao.IdAcaoInserir;
            try
            {
                Professor mdlProfessor = new Professor()
                {
                    Id = Guid.NewGuid(),
                    Email = dados.Email,
                    Nome = dados.Nome,
                    Ativo = true,
                    Excluido = false
                };
                var escolaProfessor = dados.EscolaProfessor;
                escolaProfessor.ElementAt(0).Id = Guid.NewGuid();
                escolaProfessor.ElementAt(0).De = DateTime.Today;
                escolaProfessor.ElementAt(0).IdProfessor = dados.Id;
                escolaProfessor.ElementAt(0).Excluido = false;
                mdlProfessor.EscolaProfessor = escolaProfessor;
                resposta = Create(mdlProfessor.Id, mdlProfessor);
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public GlResposta<Professor> Alterar(CmProfessor dados)
        {
            GlResposta<Professor> resposta = new GlResposta<Professor>() { Succeeded = false };
            DadosGravacao.IdAcao = BllAcao.IdAcaoAlterar;
            try
            {
                Professor mdlProfessor = new Professor()
                {
                    Id = dados.Id,
                    Email = dados.Email,
                    Nome = dados.Nome,
                    Ativo = true,
                    Excluido = false
                };
                resposta = Update(dados.Id, mdlProfessor);
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public GlResposta<CmProfessor> Excluir(CmProfessor dados)
        {
            GlResposta<CmProfessor> resposta = new GlResposta<CmProfessor>() { Succeeded = false };
            DadosGravacao.IdAcao = BllAcao.IdAcaoExcluir;
            try
            {
                resposta = ValidarAutenticacaoPermissao<CmProfessor>();
                if (!resposta.Succeeded)
                {
                    return resposta;
                }
                resposta = ValidaDadosCustom(dados, EAcoes.DELETE);
                if (!resposta.Succeeded)
                {
                    return resposta;
                }
                SchoolContext contexto = new SchoolContext();
                Professor mdlProfessor = new Professor()
                {
                    Id = dados.Id,
                    Email = dados.Email,
                    Nome = dados.Nome,
                    Ativo = false,
                    Excluido = true
                };
                GlResposta<Professor> respostaProfessor = DeleteLogical(contexto, mdlProfessor);
                if (!respostaProfessor.Succeeded)
                {
                    resposta.Mensagem = respostaProfessor.Mensagem;
                    resposta.Succeeded = respostaProfessor.Succeeded;
                    return resposta;
                }
                var escolaProfessor = Read<EscolaProfessor>(i => i.IdEscola.Equals(dados.EscolaProfessor.FirstOrDefault().IdEscola) 
                && i.IdProfessor.Equals(mdlProfessor.Id) && i.Excluido == false).Dados.FirstOrDefault();
                escolaProfessor.Excluido = true;
                var respostaEscolaProfessor = DeleteLogical<EscolaProfessor>(contexto, escolaProfessor);
                if (!respostaEscolaProfessor.Succeeded)
                {
                    resposta.Mensagem = respostaEscolaProfessor.Mensagem;
                    resposta.Succeeded = respostaEscolaProfessor.Succeeded;
                    return resposta;
                }
                var professorDisciplina = Read<ProfessorDisciplina>(i => i.IdEscolaProfessor == escolaProfessor.Id && i.Excluido == false).Dados;

                if (professorDisciplina != null)
                {
                    foreach (var item in professorDisciplina)
                    {
                        item.Excluido = true;
                        var respostaProfessorDisciplina = DeleteLogical<ProfessorDisciplina>(contexto, item);
                        if (!respostaProfessorDisciplina.Succeeded)
                        {
                            resposta.Mensagem = respostaProfessorDisciplina.Mensagem;
                            resposta.Succeeded = respostaProfessorDisciplina.Succeeded;
                            return resposta;
                        }
                    }
                }
                BrHistorico.MontarHistoricoJson(DadosGravacao, dados, dados.Id.ToString());
                var respostaSalvar = Salvar<CmProfessor>(contexto, DadosGravacao);
                if (!respostaSalvar.Succeeded)
                {
                    return respostaSalvar;
                }
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public GlResposta<ProfessorDisciplina> Excluir(ProfessorDisciplina dados)
        {
            GlResposta<ProfessorDisciplina> resposta = new GlResposta<ProfessorDisciplina>() { Succeeded = false };
            DadosGravacao.IdAcao = BllAcao.IdAcaoExcluir;
            try
            {
                resposta = ValidarAutenticacaoPermissao<ProfessorDisciplina>();
                if (!resposta.Succeeded)
                {
                    return resposta;
                }
                resposta = ValidaDados(dados, EAcoes.DELETE);
                if (!resposta.Succeeded)
                {
                    return resposta;
                }
                NotValidate = true;

                resposta = Read<ProfessorDisciplina>(dados.Id);
                if (resposta == null || resposta.Dados == null || !resposta.Dados.Any())
                {
                    return resposta;
                }
                dados = resposta.Dados.FirstOrDefault();
                dados.Excluido = true;
                resposta = DeleteLogical<ProfessorDisciplina>(dados.Id, dados);
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public GlResposta<CmProfessor> ValidaDadosCustom(CmProfessor dados, EAcoes acao)
        {
            if (dados == null)
            {
                return new GlResposta<CmProfessor>()
                {
                    Succeeded = false,
                    Mensagem = Mensagens.FormatoIncorreto
                };
            }
            GlResposta<CmProfessor> resposta = new GlResposta<CmProfessor>()
            {
                Succeeded = true,
                Mensagem = acao.Equals(EAcoes.DELETE) ? Mensagens.RegistroExcluidoSucesso : Mensagens.RegistroGravadoSucesso
            };

            if (acao.Equals(EAcoes.UPDATE) || acao.Equals(EAcoes.DELETE))
            {
                var mdlProfessor = Read<Professor>(dados.Id);
                if (mdlProfessor.Dados == null)
                {
                    return new GlResposta<CmProfessor>() { Succeeded = false, Mensagem = Mensagens.SemRegistroEncontrado };
                }
                var mdlEscolaProfessor = Read<EscolaProfessor>(i => i.IdEscola.Equals(dados.EscolaProfessor.FirstOrDefault().IdEscola) 
                && i.IdProfessor.Equals(dados.Id) && i.Excluido == false);
                if (mdlEscolaProfessor.Dados == null)
                {
                    return new GlResposta<CmProfessor>() { Succeeded = false, Mensagem = Mensagens.SemRegistroEncontrado };
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

        public GlResposta<ProfessorDisciplina> ValidaDados(ProfessorDisciplina dados, EAcoes acao)
        {
            if (dados == null)
            {
                return new GlResposta<ProfessorDisciplina>()
                {
                    Succeeded = false,
                    Mensagem = Mensagens.FormatoIncorreto
                };
            }
            GlResposta<ProfessorDisciplina> resposta = new GlResposta<ProfessorDisciplina>()
            {
                Succeeded = true,
                Mensagem = acao.Equals(EAcoes.DELETE) ? Mensagens.RegistroExcluidoSucesso : Mensagens.RegistroGravadoSucesso
            };

            if (acao.Equals(EAcoes.UPDATE) || acao.Equals(EAcoes.DELETE))
            {
                var objetoRetorno = Read<ProfessorDisciplina>(dados.Id);
                if (objetoRetorno.Dados == null)
                {
                    return new GlResposta<ProfessorDisciplina>() { Succeeded = false, Mensagem = Mensagens.SemRegistroEncontrado };
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

        public override GlResposta<Professor> ValidaDados(Professor dados, EAcoes acao)
        {
            if (dados == null)
            {
                return new GlResposta<Professor>()
                {
                    Succeeded = false,
                    Mensagem = Mensagens.FormatoIncorreto
                };
            }
            GlResposta<Professor> resposta = new GlResposta<Professor>()
            {
                Succeeded = true,
                Mensagem = Mensagens.RegistroGravadoSucesso
            };

            if (acao.Equals(EAcoes.UPDATE) || acao.Equals(EAcoes.DELETE))
            {
                var objetoRetorno = Read(dados.Id);
                if (objetoRetorno.Dados == null)
                {
                    return new GlResposta<Professor>() { Succeeded = false, Mensagem = Mensagens.SemRegistroEncontrado };
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
