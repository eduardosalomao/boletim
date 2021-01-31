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
    public class BllEnsino : BllAplicacao<Ensino>
    {
        public BllEnsino() : base()
        {
        }

        public BllEnsino(string ip, string nomeFuncao) : base(ip, nomeFuncao)
        {
        }

        public BllEnsino(string ip, string nomeFuncao, string token) : base(ip, nomeFuncao, token)
        {
        }

        public GlResposta<Ensino> ObterTodos(Guid idEscola)
        {
            GlResposta<Ensino> resposta = new GlResposta<Ensino>();
            try
            {
                resposta = ValidarAutenticacaoPermissao<Ensino>();
                if (!resposta.Succeeded)
                {
                    return resposta;
                }
                resposta = Search<Ensino>(i => i.IdEscola == idEscola && i.Ativo == true, o => o.Nome, true);
                if (resposta.Dados == null || resposta.Dados.Count == 0)
                {
                    return new GlResposta<Ensino>() { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado };
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