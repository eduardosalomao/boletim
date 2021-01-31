using System;
using Negocio.Nucleo.Geral;
using Modelo.SchoolUp.Principal;
using Comum;
using Modelo.Nucleo.Geral;
using Modelo.SchoolUp.Custom;
using Modelo.Nucleo.Enumerador;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Modelo.SchoolUp.Recursos;
using Acesso.SchoolUp.Custom;
using Acesso.Nucleo.Geral;
using Negocio.SchoolUp.Auxiliar;
using Acesso.SchoolUp.Contexts;
using Acesso.SchoolUp.Comum;

namespace Negocio.SchoolUp.Custom
{
    public class BllDisciplina : BllAplicacao<Disciplina>
    {
        public string IdUsuarioAcao { get; set; }
        public string Ip { get; set; }
        public string Funcao { get; set; }
        public string CodigoToken { get; set; }

        public BllDisciplina() : base()
        {
        }

        public BllDisciplina(string ip, string nomeFuncao, string token) : base(ip, nomeFuncao, token)
        {
        }

        public BllDisciplina(string ip, string nomeFuncao) : base(ip, nomeFuncao)
        {
        }

        public GlResposta<CmDisciplinaMedias> ObterMediasPorPeriodo(Guid idPeriodo)
        {
            GlResposta<CmDisciplinaMedias> resposta = new GlResposta<CmDisciplinaMedias>();
            try
            {
                List<CmDisciplinaMedias> disciplinas = new DalDisciplina().ObterMediasPorPeriodo(idPeriodo);

                if (disciplinas == null || disciplinas.Count == 0)
                {
                    return new GlResposta<CmDisciplinaMedias>() { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado };
                }
                resposta.Dados = disciplinas;
                resposta.Succeeded = true;
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public GlResposta<CmDisciplinaMedias> ObterMediasPorTurma(Guid idTurma)
        {
            GlResposta<CmDisciplinaMedias> resposta = new GlResposta<CmDisciplinaMedias>();
            try
            {
                List<CmDisciplinaMedias> disciplinas = new DalDisciplina().ObterMediasPorTurma(idTurma);

                if (disciplinas == null || disciplinas.Count == 0)
                {
                    return new GlResposta<CmDisciplinaMedias>() { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado };
                }
                resposta.Dados = disciplinas;
                resposta.Succeeded = true;
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public GlResposta<CmDisciplinaMedias> ObterMediasPorAluno(Guid idAluno, Guid idPeriodo)
        {
            GlResposta<CmDisciplinaMedias> resposta = new GlResposta<CmDisciplinaMedias>();
            try
            {
                List<CmDisciplinaMedias> disciplinas = new DalDisciplina().ObterMediasPorAluno(idAluno, idPeriodo);

                if (disciplinas == null || disciplinas.Count == 0)
                {
                    return new GlResposta<CmDisciplinaMedias>() { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado };
                }
                resposta.Dados = disciplinas;
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
