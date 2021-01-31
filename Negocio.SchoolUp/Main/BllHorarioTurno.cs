using System;
using Negocio.Nucleo.Geral;
using Modelo.SchoolUp.Principal;
using Comum;
using Modelo.Nucleo.Geral;
using Modelo.Nucleo.Enumerador;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Modelo.SchoolUp.Recursos;
using Acesso.Nucleo.Geral;
using Modelo.SchoolUp.Custom;
using Acesso.SchoolUp.Custom;

namespace Negocio.SchoolUp.Main
{
    public class BllHorarioTurno : BllAplicacao<HorarioTurno>
    {
        public string IdUsuarioAcao { get; set; }
        public string Ip { get; set; }
        public string Funcao { get; set; }
        public string CodigoToken { get; set; }

        public BllHorarioTurno(string ip, string nomeFuncao, string token) : base(ip, nomeFuncao, token)
        {
            Ip = ip;
            Funcao = nomeFuncao;
            CodigoToken = token;
            IdUsuarioAcao = base.IdUsuario;
        }

        public BllHorarioTurno(string ip, string nomeFuncao) : base(ip, nomeFuncao)
        {
            IdUsuarioAcao = base.IdUsuario;
            Ip = ip;
            Funcao = nomeFuncao;
        }

        public GlResposta<HorarioTurno> Obter(Guid idHorarioTurno)
        {
            GlResposta<HorarioTurno> resposta = new GlResposta<HorarioTurno>();
            DadosGravacao.IdAcao = BllAcao.IdAcaoDetalhar;
            try
            {
                resposta = ValidarAutenticacaoPermissao<HorarioTurno>();
                if (!resposta.Succeeded)
                {
                    return resposta;
                }
                DadosGravacao = BrHistorico.MontarHistorico(DadosGravacao, "Obter()", DadosGravacao.IdAcao, idHorarioTurno.ToString());
                resposta = Read(idHorarioTurno);
                if (resposta.Dados == null || resposta.Dados.Count == 0)
                {
                    return new GlResposta<HorarioTurno>() { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado };
                }
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public GlResposta<CmHorarioTurno> ObterTodos(Guid idEscola)
        {
            GlResposta<CmHorarioTurno> resposta = new GlResposta<CmHorarioTurno>();
            DadosGravacao.IdAcao = BllAcao.IdAcaoAcessar;
            try
            {
                resposta = ValidarAutenticacaoPermissao<CmHorarioTurno>();
                if (!resposta.Succeeded)
                {
                    return resposta;
                }

                List<CmHorarioTurno> HorarioTurnos = new DalHorarioTurno().ObterTodos(idEscola);

                if (HorarioTurnos == null || HorarioTurnos.Count == 0)
                {
                    return new GlResposta<CmHorarioTurno>() { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado };
                }
                resposta.Dados = HorarioTurnos;
                resposta.Succeeded = true;

                if (!IsAnonimo)
                {
                    DadosGravacao.IdAcao = BllAcao.IdAcaoAcessar;
                    DadosGravacao = BrHistorico.MontarHistorico(DadosGravacao, "Obter()", DadosGravacao.IdAcao);
                    new DalGenerica<HorarioTurno>().InserirHistorico(DadosGravacao);
                }
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public GlResposta<HorarioTurno> Filtrar(string filtro)
        {
            GlResposta<HorarioTurno> resposta = new GlResposta<HorarioTurno>();
            DadosGravacao.IdAcao = BllAcao.IdAcaoProcurar;
            try
            {
                resposta = ValidarAutenticacaoPermissao<HorarioTurno>();
                if (!resposta.Succeeded)
                {
                    return resposta;
                }
                DadosGravacao = BrHistorico.MontarHistorico(DadosGravacao, "Obter()", DadosGravacao.IdAcao);
                resposta = Search(i => i.Nome.Contains(filtro) && i.Ativo == true, o => o.Nome, true);
                if (resposta.Dados == null || resposta.Dados.Count == 0)
                {
                    return new GlResposta<HorarioTurno>() { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado };
                }
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public GlResposta<HorarioTurno> Inserir(HorarioTurno dados)
        {
            GlResposta<HorarioTurno> resposta = new GlResposta<HorarioTurno>() { Succeeded = false };
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

        public GlResposta<HorarioTurno> Alterar(HorarioTurno dados)
        {
            GlResposta<HorarioTurno> resposta = new GlResposta<HorarioTurno>() { Succeeded = false };
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

        public GlResposta<HorarioTurno> Excluir(HorarioTurno dados)
        {
            GlResposta<HorarioTurno> resposta = new GlResposta<HorarioTurno>() { Succeeded = false };
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

        public override GlResposta<HorarioTurno> ValidaDados(HorarioTurno dados, EAcoes acao)
        {
            if (dados == null)
            {
                return new GlResposta<HorarioTurno>()
                {
                    Succeeded = false,
                    Mensagem = Mensagens.FormatoIncorreto
                };
            }
            GlResposta<HorarioTurno> resposta = new GlResposta<HorarioTurno>()
            {
                Succeeded = true,
                Mensagem = acao.Equals(EAcoes.DELETE) ? Mensagens.RegistroExcluidoSucesso : Mensagens.RegistroGravadoSucesso
            };

            if (acao.Equals(EAcoes.UPDATE) || acao.Equals(EAcoes.DELETE))
            {
                var objetoRetorno = Read(dados.Id);
                if (objetoRetorno.Dados == null)
                {
                    return new GlResposta<HorarioTurno>() { Succeeded = false, Mensagem = Mensagens.SemRegistroEncontrado };
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
