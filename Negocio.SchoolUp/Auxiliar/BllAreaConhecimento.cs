using Comum;
using Modelo.Nucleo.Geral;
using Modelo.SchoolUp.Principal;
using Modelo.SchoolUp.Recursos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Negocio.SchoolUp.Auxiliar
{
    public class BllAreaConhecimento : BllAplicacao<AreaConhecimento>
    {
        public BllAreaConhecimento() : base()
        {
        }

        public BllAreaConhecimento(string ip, string nomeFuncao) : base(ip, nomeFuncao)
        {
        }

        public BllAreaConhecimento(string ip, string nomeFuncao, string token) : base(ip, nomeFuncao, token)
        {
        }

        public GlResposta<AreaConhecimento> Obter()
        {
            GlResposta<AreaConhecimento> resposta = new GlResposta<AreaConhecimento>();
            try
            {
                resposta = ValidarAutenticacaoPermissao();
                if (!resposta.Succeeded)
                {
                    return resposta;
                }
                resposta = ReadAll();
                if (resposta.Dados == null || resposta.Dados.Count == 0)
                {
                    return new GlResposta<AreaConhecimento>() { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado };
                }
                resposta.Dados = resposta.Dados.OrderBy(o => o.Nome).ToList();
                resposta.Succeeded = true;
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }
        
        public GlResposta<AreaConhecimento> Obter(Guid id)
        {
            GlResposta<AreaConhecimento> resposta = new GlResposta<AreaConhecimento>();
            try
            {
                resposta = ValidarAutenticacaoPermissao<AreaConhecimento>();
                if (!resposta.Succeeded)
                {
                    return resposta;
                }
                resposta = Search<AreaConhecimento>(i => i.Id == id && i.Excluido == false, o => o.Nome, true);
                if (resposta.Dados == null || resposta.Dados.Count == 0)
                {
                    return new GlResposta<AreaConhecimento>() { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado };
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
