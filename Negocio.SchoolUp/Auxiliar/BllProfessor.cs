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
using Acesso.SchoolUp.Contexts;
using Modelo.SchoolUp.Custom;
using Acesso.SchoolUp.Custom;

namespace Negocio.SchoolUp.Auxiliar
{
    public class BllProfessor : BllAplicacao<Professor>
    {
        public string IdUsuarioAcao { get; set; }
        public string Ip { get; set; }
        public string Funcao { get; set; }
        public string CodigoToken { get; set; }

        public BllProfessor() : base()
        {
        }

        public BllProfessor(string ip, string nomeFuncao, string token) : base(ip, nomeFuncao, token)
        {
        }

        public BllProfessor(string ip, string nomeFuncao) : base(ip, nomeFuncao)
        {
        }

        public GlResposta<CmProfessorDisciplina> ObterTodosProfessorComDisciplina(Guid idEscola)
        {
            GlResposta<CmProfessorDisciplina> resposta = new GlResposta<CmProfessorDisciplina>();
            try
            {
                List<CmProfessorDisciplina> professorDisciplinas = new DalProfessor().ObterTodosProfessorComDisciplina(idEscola);

                if (professorDisciplinas == null || professorDisciplinas.Count == 0)
                {
                    return new GlResposta<CmProfessorDisciplina>() { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado };
                }
                resposta.Dados = professorDisciplinas;
                resposta.Succeeded = true;
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public GlResposta<CmProfessorDisciplina> ObterTodosProfessorPorTurma(Guid idTurma)
        {
            GlResposta<CmProfessorDisciplina> resposta = new GlResposta<CmProfessorDisciplina>();
            try
            {
                List<CmProfessorDisciplina> professorDisciplinas = new DalProfessor().ObterTodosProfessorPorTurma(idTurma);

                if (professorDisciplinas == null || professorDisciplinas.Count == 0)
                {
                    return new GlResposta<CmProfessorDisciplina>() { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado };
                }
                resposta.Dados = professorDisciplinas;
                resposta.Succeeded = true;
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public GlResposta<CmProfessorDisciplina> ObterTodosProfessorPorDisciplina(Guid idDisciplina)
        {
            GlResposta<CmProfessorDisciplina> resposta = new GlResposta<CmProfessorDisciplina>();
            try
            {
                List<CmProfessorDisciplina> professorDisciplinas = new DalProfessor().ObterTodosProfessorPorDisciplina(idDisciplina);

                if (professorDisciplinas == null || professorDisciplinas.Count == 0)
                {
                    return new GlResposta<CmProfessorDisciplina>() { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado };
                }
                resposta.Dados = professorDisciplinas;
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
