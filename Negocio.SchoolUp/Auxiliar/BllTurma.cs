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
using Modelo.SchoolUp.Custom;
using Acesso.SchoolUp.Custom;

namespace Negocio.SchoolUp.Auxiliar
{
    public class BllTurma : BllAplicacao<Turma>
    {
        public string IdUsuarioAcao { get; set; }
        public string Ip { get; set; }
        public string Funcao { get; set; }
        public string CodigoToken { get; set; }

        public BllTurma() : base()
        {
        }

        public BllTurma(string ip, string nomeFuncao, string token) : base(ip, nomeFuncao, token)
        {
            Ip = ip;
            Funcao = nomeFuncao;
            CodigoToken = token;
            IdUsuarioAcao = base.IdUsuario;
        }

        public BllTurma(string ip, string nomeFuncao) : base(ip, nomeFuncao)
        {
            IdUsuarioAcao = base.IdUsuario;
            Ip = ip;
            Funcao = nomeFuncao;
        }

        public GlResposta<CmTurmaAluno> ObterAlunos(Guid idTurma)
        {
            GlResposta<CmTurmaAluno> resposta = new GlResposta<CmTurmaAluno>();
            try
            {

                List<CmTurmaAluno> alunos = new DalTurma().ObterAlunos(idTurma);

                if (alunos == null || alunos.Count == 0)
                {
                    return new GlResposta<CmTurmaAluno>() { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado };
                }
                resposta.Dados = alunos;
                resposta.Succeeded = true;
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public GlResposta<Turma> ObterPorPeriodo(Guid idPeriodo)
        {
            GlResposta<Turma> resposta = new GlResposta<Turma>();
            try
            {
                resposta = Search<Turma>(i => i.IdPeriodo == idPeriodo && i.Excluido == false, o => o.Nome, false);
                if (resposta.Dados == null || resposta.Dados.Count == 0)
                {
                    return new GlResposta<Turma>() { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado };
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
