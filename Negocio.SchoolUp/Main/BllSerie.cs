using System;
using Negocio.Nucleo.Geral;
using Modelo.SchoolUp.Principal;
using Comum;
using Modelo.Nucleo.Geral;
using Modelo.SchoolUp.Custom;
using Modelo.Nucleo.Enumerador;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Modelo.SchoolUp.Recursos;
using Acesso.SchoolUp.Custom;
using Acesso.Nucleo.Geral;

namespace Negocio.SchoolUp.Main
{
    public class BllSerie : BllAplicacao<Serie>
    {
        public string IdUsuarioAcao { get; set; }
        public string Ip { get; set; }
        public string Funcao { get; set; }
        public string CodigoToken { get; set; }

        public BllSerie(string ip, string nomeFuncao, string token) : base(ip, nomeFuncao, token)
        {
            Ip = ip;
            Funcao = nomeFuncao;
            CodigoToken = token;
            IdUsuarioAcao = base.IdUsuario;
        }

        public BllSerie(string ip, string nomeFuncao) : base(ip, nomeFuncao)
        {
            IdUsuarioAcao = base.IdUsuario;
            Ip = ip;
            Funcao = nomeFuncao;
        }

        public GlResposta<Serie> Obter(Guid idSerie)
        {
            GlResposta<Serie> resposta = new GlResposta<Serie>();
            DadosGravacao.IdAcao = BllAcao.IdAcaoDetalhar;
            try
            {
                resposta = ValidarAutenticacaoPermissao<Serie>();
                if (!resposta.Succeeded)
                {
                    return resposta;
                }
                DadosGravacao = BrHistorico.MontarHistorico(DadosGravacao, "Obter()", DadosGravacao.IdAcao, idSerie.ToString());
                resposta = Read(idSerie);
                if (resposta.Dados == null || resposta.Dados.Count == 0)
                {
                    return new GlResposta<Serie>() { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado };
                }
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public GlResposta<CmSerie> ObterTodos(Guid idEscola)
        {
            GlResposta<CmSerie> resposta = new GlResposta<CmSerie>();
            DadosGravacao.IdAcao = BllAcao.IdAcaoAcessar;
            try
            {
                resposta = ValidarAutenticacaoPermissao<CmSerie>();
                if (!resposta.Succeeded)
                {
                    return resposta;
                }

                List<CmSerie> series = new DalSerie().ObterTodos(idEscola);

                if (series == null || series.Count == 0)
                {
                    return new GlResposta<CmSerie>() { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado };
                }
                resposta.Dados = series;
                resposta.Succeeded = true;

                if (!IsAnonimo)
                {
                    DadosGravacao.IdAcao = BllAcao.IdAcaoAcessar;
                    DadosGravacao = BrHistorico.MontarHistorico(DadosGravacao, "Obter()", DadosGravacao.IdAcao);
                    new DalGenerica<CmSerie>().InserirHistorico(DadosGravacao);
                }
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public GlResposta<Serie> Filtrar(string filtro)
        {
            GlResposta<Serie> resposta = new GlResposta<Serie>();
            DadosGravacao.IdAcao = BllAcao.IdAcaoProcurar;
            try
            {
                resposta = ValidarAutenticacaoPermissao<Serie>();
                if (!resposta.Succeeded)
                {
                    return resposta;
                }
                DadosGravacao = BrHistorico.MontarHistorico(DadosGravacao, "Obter()", DadosGravacao.IdAcao);
                resposta = Search(i => i.Nome.Contains(filtro) && i.Ativo == true, o => o.Nome, true);
                if (resposta.Dados == null || resposta.Dados.Count == 0)
                {
                    return new GlResposta<Serie>() { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado };
                }
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public GlResposta<Serie> Inserir(Serie dados)
        {
            GlResposta<Serie> resposta = new GlResposta<Serie>() { Succeeded = false };
            DadosGravacao.IdAcao = BllAcao.IdAcaoInserir;
            try
            {
                dados.Id = Guid.NewGuid();
                dados.Ativo = true;
                resposta = Create(dados.Id, dados);
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public GlResposta<Serie> Alterar(Serie dados)
        {
            GlResposta<Serie> resposta = new GlResposta<Serie>() { Succeeded = false };
            DadosGravacao.IdAcao = BllAcao.IdAcaoAlterar;
            try
            {
                dados.Ativo = true;
                resposta = Update(dados.Id, dados);
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public GlResposta<Serie> Excluir(Serie dados)
        {
            GlResposta<Serie> resposta = new GlResposta<Serie>() { Succeeded = false };
            DadosGravacao.IdAcao = BllAcao.IdAcaoExcluir;
            try
            {
                dados.Ativo = false;
                resposta = Update(dados.Id, dados);
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public override GlResposta<Serie> ValidaDados(Serie dados, EAcoes acao)
        {
            if (dados == null)
            {
                return new GlResposta<Serie>()
                {
                    Succeeded = false,
                    Mensagem = Mensagens.FormatoIncorreto
                };
            }
            GlResposta<Serie> resposta = new GlResposta<Serie>()
            {
                Succeeded = true,
                Mensagem = acao.Equals(EAcoes.DELETE) ? Mensagens.RegistroExcluidoSucesso : Mensagens.RegistroGravadoSucesso
            };

            if (acao.Equals(EAcoes.UPDATE) || acao.Equals(EAcoes.DELETE))
            {
                var objetoRetorno = Read(dados.Id);
                if (objetoRetorno.Dados == null)
                {
                    return new GlResposta<Serie>() { Succeeded = false, Mensagem = Mensagens.SemRegistroEncontrado };
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
