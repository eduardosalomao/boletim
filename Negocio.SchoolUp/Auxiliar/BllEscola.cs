using Acesso.SchoolUp.Custom;
using Comum;
using Modelo.Nucleo.Geral;
using Modelo.SchoolUp.Custom;
using Modelo.SchoolUp.Principal;
using Modelo.SchoolUp.Recursos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Negocio.SchoolUp.Auxiliar
{
    public class BllEscola : BllAplicacao<EscolaUsuario>
    {
        public BllEscola() : base()
        {
        }

        public BllEscola(string ip, string nomeFuncao) : base(ip, nomeFuncao)
        {
        }

        public BllEscola(string ip, string nomeFuncao, string token) : base(ip, nomeFuncao, token)
        {
        }

        public GlResposta<Escola> ObterPorCodigo(string codigo)
        {
            GlResposta<Escola> resposta = new GlResposta<Escola>();
            try
            {
                resposta = ValidarAutenticacaoPermissao<Escola>();
                if (!resposta.Succeeded)
                {
                    return resposta;
                }
                Escola escola = Search<Escola>(i => i.Codigo == codigo
                    && !i.Excluido && i.Ativo.HasValue && i.Ativo.Value == true)?.Dados?.FirstOrDefault();

                if (escola == null || String.IsNullOrEmpty(escola.Nome) == true)
                {
                    return new GlResposta<Escola>() { Succeeded = false, Mensagem = Mensagens.SemRegistroEncontrado };
                }
                resposta.Dados = new List<Escola>() { escola };
                resposta.Succeeded = true;
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public GlResposta<CmUsuario> Obter(Guid idUsuario)
        {
            GlResposta<CmUsuario> resposta = new GlResposta<CmUsuario>();
            try
            {
                resposta = ValidarAutenticacaoPermissao<CmUsuario>();
                if (!resposta.Succeeded)
                {
                    return resposta;
                }
                CmUsuario usuario = new DalUsuario().Obter(idUsuario);
                if (usuario == null || String.IsNullOrEmpty(usuario.Nome) == true)
                {
                    return new GlResposta<CmUsuario>() { Succeeded = false, Mensagem = Mensagens.SemRegistroEncontrado };
                }
                resposta.Dados = new List<CmUsuario>() { usuario };
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