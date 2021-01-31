using Comum;
using Modelo.Nucleo.Geral;
using Modelo.SchoolUp.Principal;
using Modelo.SchoolUp.Recursos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Negocio.SchoolUp.Auxiliar
{
    public class BllTipoAvaliacao : BllAplicacao<TipoAvaliacao>
    {
        public BllTipoAvaliacao() : base()
        {
        }

        public BllTipoAvaliacao(string ip, string nomeFuncao) : base(ip, nomeFuncao)
        {
        }

        public BllTipoAvaliacao(string ip, string nomeFuncao, string token) : base(ip, nomeFuncao, token)
        {
        }

        public GlResposta<TipoAvaliacao> ObterTodos()
        {
            GlResposta<TipoAvaliacao> resposta = new GlResposta<TipoAvaliacao>();
            try
            {
                resposta = ValidarAutenticacaoPermissao();
                if (!resposta.Succeeded)
                {
                    return resposta;
                }
                resposta = Search(i => i.Excluido == false, o => o.Nome, true);
                if (resposta.Dados == null || resposta.Dados.Count == 0)
                {
                    return new GlResposta<TipoAvaliacao>() { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado };
                }
                resposta.Succeeded = true;
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public GlResposta<TipoAvaliacao> Obter(string codigo)
        {
            GlResposta<TipoAvaliacao> resposta = new GlResposta<TipoAvaliacao>();
            try
            {
                resposta = ValidarAutenticacaoPermissao();
                if (!resposta.Succeeded)
                {
                    return resposta;
                }
                resposta = Search(i => i.Codigo == codigo);
                if (resposta?.Dados == null || resposta?.Dados?.Count == 0)
                {
                    return new GlResposta<TipoAvaliacao>() { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado };
                }
                resposta.Succeeded = true;
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }
    }
}