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
    public class BllTurno : BllAplicacao<Turno>
    {
        public BllTurno() : base()
        {
        }

        public BllTurno(string ip, string nomeFuncao) : base(ip, nomeFuncao)
        {
        }

        public BllTurno(string ip, string nomeFuncao, string token) : base(ip, nomeFuncao, token)
        {
        }

        public GlResposta<Turno> ObterTodos(Guid idEscola)
        {
            GlResposta<Turno> resposta = new GlResposta<Turno>();
            try
            {
                resposta = Search<Turno>(i => i.IdEscola == idEscola && i.Ativo == true, o => o.Nome, false);
                if (resposta.Dados == null || resposta.Dados.Count == 0)
                {
                    return new GlResposta<Turno>() { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado };
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