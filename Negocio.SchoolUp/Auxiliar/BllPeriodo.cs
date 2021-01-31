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
    public class BllPeriodo : BllAplicacao<Periodo>
    {
        public BllPeriodo() : base()
        {
        }

        public BllPeriodo(string ip, string nomeFuncao) : base(ip, nomeFuncao)
        {
        }

        public BllPeriodo(string ip, string nomeFuncao, string token) : base(ip, nomeFuncao, token)
        {
        }

        public GlResposta<Periodo> ObterTodos(Guid idEscola)
        {
            GlResposta<Periodo> resposta = new GlResposta<Periodo>();
            try
            {
                resposta = Search<Periodo>(i => i.IdEscola == idEscola && i.Excluido == false, o => o.Nome, false);
                if (resposta.Dados == null || resposta.Dados.Count == 0)
                {
                    return new GlResposta<Periodo>() { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado };
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

        public GlResposta<SubPeriodo> ObterTodosSubPeriodo(Guid idPeriodo)
        {
            GlResposta<SubPeriodo> resposta = new GlResposta<SubPeriodo>();
            try
            {
                resposta = Search<SubPeriodo>(i => i.IdPeriodo == idPeriodo && i.Excluido == false, o => o.Nome, false);
                if (resposta.Dados == null || resposta.Dados.Count == 0)
                {
                    return new GlResposta<SubPeriodo>() { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado };
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