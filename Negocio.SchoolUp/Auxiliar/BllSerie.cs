using Acesso.SchoolUp.Custom;
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
    public class BllSerie : BllAplicacao<Serie>
    {
        public BllSerie() : base()
        {
        }

        public BllSerie(string ip, string nomeFuncao) : base(ip, nomeFuncao)
        {
        }

        public BllSerie(string ip, string nomeFuncao, string token) : base(ip, nomeFuncao, token)
        {
        }

        public GlResposta<Serie> ObterTodos(Guid idEscola)
        {
            GlResposta<Serie> resposta = new GlResposta<Serie>();
            try
            {
                List<Serie> series = new DalSerie().ObterTodosCombo(idEscola);

                if (series == null || series.Count == 0)
                {
                    return new GlResposta<Serie>() { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado };
                }
                resposta.Dados = series;
                resposta.Succeeded = true;

                if (resposta.Dados == null || resposta.Dados.Count == 0)
                {
                    return new GlResposta<Serie>() { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado };
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