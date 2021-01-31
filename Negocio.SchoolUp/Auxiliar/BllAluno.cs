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
    public class BllAluno : BllAplicacao<Aluno>
    {
        public string IdUsuarioAcao { get; set; }
        public string Ip { get; set; }
        public string Funcao { get; set; }
        public string CodigoToken { get; set; }

        public BllAluno() : base()
        {
        }

        public BllAluno(string ip, string nomeFuncao, string token) : base(ip, nomeFuncao, token)
        {
            Ip = ip;
            Funcao = nomeFuncao;
            CodigoToken = token;
            IdUsuarioAcao = base.IdUsuario;
        }

        public BllAluno(string ip, string nomeFuncao) : base(ip, nomeFuncao)
        {
            IdUsuarioAcao = base.IdUsuario;
            Ip = ip;
            Funcao = nomeFuncao;
        }

        public GlResposta<CmTurmaAluno> ObterAlunoPorResponsavel(Guid idUsuario)
        {
            GlResposta<CmTurmaAluno> resposta = new GlResposta<CmTurmaAluno>();
            try
            {
                List<CmTurmaAluno> listAlunos = new DalAluno().ObterPorResponsavel(idUsuario);
                if (listAlunos == null)
                {
                    return new GlResposta<CmTurmaAluno>() { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado };
                }
                resposta.Dados = listAlunos;
                resposta.Succeeded = true;
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public GlResposta<CmTurmaAluno> ObterTurmaAluno(Guid idAluno, Guid idPeriodo)
        {
            GlResposta<CmTurmaAluno> resposta = new GlResposta<CmTurmaAluno>();
            try
            {
                List<CmTurmaAluno> listAlunos = new DalAluno().ObterTurma(idAluno, idPeriodo);
                if (listAlunos == null)
                {
                    return new GlResposta<CmTurmaAluno>() { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado };
                }
                resposta.Dados = listAlunos;
                resposta.Succeeded = true;
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public GlResposta<CmTurmaAluno> ObterTurmas(Guid idAluno)
        {
            GlResposta<CmTurmaAluno> resposta = new GlResposta<CmTurmaAluno>();
            try
            {
                List<CmTurmaAluno> listAlunos = new DalAluno().ObterTurmas(idAluno);
                if (listAlunos == null)
                {
                    return new GlResposta<CmTurmaAluno>() { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado };
                }
                resposta.Dados = listAlunos;
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

