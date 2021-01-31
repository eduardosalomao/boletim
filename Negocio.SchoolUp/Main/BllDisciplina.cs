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

namespace Negocio.SchoolUp.Main
{
    public class BllDisciplina : BllAplicacao<Disciplina>
    {
        public string IdUsuarioAcao { get; set; }
        public string Ip { get; set; }
        public string Funcao { get; set; }
        public string CodigoToken { get; set; }

        public BllDisciplina(string ip, string nomeFuncao, string token) : base(ip, nomeFuncao, token)
        {
            Ip = ip;
            Funcao = nomeFuncao;
            CodigoToken = token;
            IdUsuarioAcao = base.IdUsuario;
        }

        public BllDisciplina(string ip, string nomeFuncao) : base(ip, nomeFuncao)
        {
            IdUsuarioAcao = base.IdUsuario;
            Ip = ip;
            Funcao = nomeFuncao;
        }

        public GlResposta<Disciplina> Obter(Guid idDisciplina)
        {
            GlResposta<Disciplina> resposta = new GlResposta<Disciplina>();
            DadosGravacao.IdAcao = BllAcao.IdAcaoDetalhar;
            try
            {
                resposta = ValidarAutenticacaoPermissao<Disciplina>(idDisciplina);
                if (!resposta.Succeeded)
                {
                    return resposta;
                }
                DadosGravacao = BrHistorico.MontarHistorico(DadosGravacao, "Obter()", DadosGravacao.IdAcao, idDisciplina.ToString());
                resposta = Read(idDisciplina);
                if (resposta.Dados == null || resposta.Dados.Count == 0)
                {
                    return new GlResposta<Disciplina>() { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado };
                }
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public GlResposta<CmDisciplina> ObterTodos(Guid idEscola)
        {
            GlResposta<CmDisciplina> resposta = new GlResposta<CmDisciplina>();
            DadosGravacao.IdAcao = BllAcao.IdAcaoAcessar;
            try
            {
                resposta = ValidarAutenticacaoPermissao<CmDisciplina>();
                if (!resposta.Succeeded)
                {
                    return resposta;
                }

                List<CmDisciplina> disciplinas = new DalDisciplina().ObterTodos(idEscola);

                if (disciplinas == null || disciplinas.Count == 0)
                {
                    return new GlResposta<CmDisciplina>() { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado };
                }
                resposta.Dados = disciplinas;
                resposta.Succeeded = true;

                if (!IsAnonimo)
                {
                    DadosGravacao.IdAcao = BllAcao.IdAcaoAcessar;
                    DadosGravacao = BrHistorico.MontarHistorico(DadosGravacao, "Obter()", DadosGravacao.IdAcao);
                    new DalGenerica<CmDisciplina>().InserirHistorico(DadosGravacao);
                }
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public GlResposta<CmDisciplinaHorario> ObterGrade(Guid idTurma)
        {
            GlResposta<CmDisciplinaHorario> resposta = new GlResposta<CmDisciplinaHorario>();
            new BllAcaoBoletim();
            DadosGravacao.IdAcao = BllAcaoBoletim.IdAcaoAcessarGrade;
            try
            {
                resposta = ValidarAutenticacaoPermissao<CmDisciplinaHorario>();
                if (!resposta.Succeeded)
                {
                    return resposta;
                }

                List<CmDisciplinaHorario> disciplinas = new DalDisciplinaHorario().ObterTodos(idTurma);

                if (disciplinas == null || disciplinas.Count == 0)
                {
                    return new GlResposta<CmDisciplinaHorario>() { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado };
                }
                resposta.Dados = disciplinas;
                resposta.Succeeded = true;

                if (!IsAnonimo)
                {
                    DadosGravacao.IdAcao = BllAcaoBoletim.IdAcaoAcessarGrade;
                    DadosGravacao = BrHistorico.MontarHistorico(DadosGravacao, "Obter()", DadosGravacao.IdAcao);
                    new DalGenerica<CmDisciplina>().InserirHistorico(DadosGravacao);
                }
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public GlResposta<CmProfessorDisciplina> ObterTodosProfessorComDisciplina(Guid idProfessor, Guid idEscola)
        {
            GlResposta<CmProfessorDisciplina> resposta = new GlResposta<CmProfessorDisciplina>();
            DadosGravacao.IdAcao = BllAcao.IdAcaoAcessar;
            try
            {
                resposta = ValidarAutenticacaoPermissao<CmProfessorDisciplina>();
                if (!resposta.Succeeded)
                {
                    return resposta;
                }

                List<CmProfessorDisciplina> disciplinas = new DalDisciplina().ObterTodosProfessorComDisciplina(idProfessor, idEscola);

                if (disciplinas == null || disciplinas.Count == 0)
                {
                    return new GlResposta<CmProfessorDisciplina>() { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado };
                }
                resposta.Dados = disciplinas;
                resposta.Succeeded = true;

                if (!IsAnonimo)
                {
                    DadosGravacao.IdAcao = BllAcao.IdAcaoAcessar;
                    DadosGravacao = BrHistorico.MontarHistorico(DadosGravacao, "Obter()", DadosGravacao.IdAcao);
                    new DalGenerica<CmProfessorDisciplina>().InserirHistorico(DadosGravacao);
                }
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public GlResposta<Disciplina> ObterTodosDisciplina(Guid idTurma)
        {
            GlResposta<Disciplina> resposta = new GlResposta<Disciplina>();
            new BllAcaoBoletim();
            DadosGravacao.IdAcao = BllAcaoBoletim.IdAcaoAcessarGrade;
            try
            {
                resposta = ValidarAutenticacaoPermissao<Disciplina>();
                if (!resposta.Succeeded)
                {
                    return resposta;
                }

                List<Disciplina> disciplinas = new DalDisciplina().ObterTodosDisciplinas(idTurma);

                if (disciplinas == null || disciplinas.Count == 0)
                {
                    return new GlResposta<Disciplina>() { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado };
                }
                resposta.Dados = disciplinas;
                resposta.Succeeded = true;

                if (!IsAnonimo)
                {
                    DadosGravacao.IdAcao = BllAcaoBoletim.IdAcaoAcessarGrade;
                    DadosGravacao = BrHistorico.MontarHistorico(DadosGravacao, "Obter()", DadosGravacao.IdAcao);
                    new DalGenerica<CmDisciplina>().InserirHistorico(DadosGravacao);
                }
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public GlResposta<CmDisciplinaHorario> GravarGrade(List<DisciplinaHorario> listDisciplinaHorario)
        {
            GlResposta<CmDisciplinaHorario> resposta = new GlResposta<CmDisciplinaHorario>();
            new BllAcaoBoletim();
            DadosGravacao.IdAcao = BllAcaoBoletim.IdAcaoEditarGrade;
            try
            {
                resposta = ValidarAutenticacaoPermissao<CmDisciplinaHorario>();
                if (!resposta.Succeeded)
                {
                    return resposta;
                }

                if (listDisciplinaHorario == null || listDisciplinaHorario.Count == 0)
                {
                    return new GlResposta<CmDisciplinaHorario>() { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado };
                }
                Guid idTurma = listDisciplinaHorario[0].IdTurma;
                NotValidate = true;

                using (SchoolContext context = new SchoolContext())
                {
                    DalSchool<DisciplinaHorario> daGenerica = new DalSchool<DisciplinaHorario>();
                    List<DisciplinaHorario> listDisciplinaHorarios = daGenerica.ObterTodos(i => i.IdTurma == idTurma && i.Excluido == false, o => o.Dia.ToString(), true);
                    foreach (var disciplinaHorario in listDisciplinaHorarios)
                    {
                        disciplinaHorario.Excluido = true;
                        Update<DisciplinaHorario>(context, disciplinaHorario.Id, disciplinaHorario);
                    }
                    foreach (var discipinaHorario in listDisciplinaHorario)
                    {
                        Create<DisciplinaHorario>(context, discipinaHorario.Id, discipinaHorario);
                    }
                    DadosGravacao.IdAcao = BllAcaoBoletim.IdAcaoEditarGrade;
                    DadosGravacao = BrHistorico.MontarHistoricoJson(DadosGravacao, listDisciplinaHorario, idTurma.ToString());
                    Salvar<DisciplinaHorario>(context, DadosGravacao);
                }
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public GlResposta<Disciplina> Filtrar(string filtro)
        {
            GlResposta<Disciplina> resposta = new GlResposta<Disciplina>();
            DadosGravacao.IdAcao = BllAcao.IdAcaoProcurar;
            try
            {
                resposta = ValidarAutenticacaoPermissao<Disciplina>();
                if (!resposta.Succeeded)
                {
                    return resposta;
                }
                DadosGravacao = BrHistorico.MontarHistorico(DadosGravacao, "Obter()", DadosGravacao.IdAcao);
                resposta = Search(i => i.Nome.Contains(filtro) && i.Excluido == false, o => o.Nome, true);
                if (resposta.Dados == null || resposta.Dados.Count == 0)
                {
                    return new GlResposta<Disciplina>() { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado };
                }
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public GlResposta<Disciplina> Inserir(Disciplina dados)
        {
            GlResposta<Disciplina> resposta = new GlResposta<Disciplina>() { Succeeded = false };
            DadosGravacao.IdAcao = BllAcao.IdAcaoInserir;
            try
            {
                dados.Id = Guid.NewGuid();
                dados.Excluido = false;
                resposta = Create(dados.Id, dados);
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public GlResposta<Disciplina> Alterar(Disciplina dados)
        {
            GlResposta<Disciplina> resposta = new GlResposta<Disciplina>() { Succeeded = false };
            DadosGravacao.IdAcao = BllAcao.IdAcaoAlterar;
            try
            {
                dados.Excluido = false;
                resposta = Update(dados.Id, dados);
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public GlResposta<Disciplina> Excluir(Disciplina dados)
        {
            GlResposta<Disciplina> resposta = new GlResposta<Disciplina>() { Succeeded = false };
            DadosGravacao.IdAcao = BllAcao.IdAcaoExcluir;
            try
            {
                dados.Excluido = true;
                resposta = DeleteLogical(dados.Id, dados);
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public override GlResposta<Disciplina> ValidaDados(Disciplina dados, EAcoes acao)
        {
            if (dados == null)
            {
                return new GlResposta<Disciplina>()
                {
                    Succeeded = false,
                    Mensagem = Mensagens.FormatoIncorreto
                };
            }
            GlResposta<Disciplina> resposta = new GlResposta<Disciplina>()
            {
                Succeeded = true,
                Mensagem = acao.Equals(EAcoes.DELETE) ? Mensagens.RegistroExcluidoSucesso : Mensagens.RegistroGravadoSucesso
            };

            if (acao.Equals(EAcoes.UPDATE) || acao.Equals(EAcoes.DELETE))
            {
                var objetoRetorno = Read(dados.Id);
                if (objetoRetorno.Dados == null)
                {
                    return new GlResposta<Disciplina>() { Succeeded = false, Mensagem = Mensagens.SemRegistroEncontrado };
                }
            }

            if (acao.Equals(EAcoes.ADD) || acao.Equals(EAcoes.UPDATE))
            {
                var resultadoValidacao = new List<ValidationResult>();
                var contexto = new ValidationContext(dados, null, null);
                Validator.TryValidateObject(dados, contexto, resultadoValidacao, true);
                resposta.Succeeded = !(resultadoValidacao.Count > 0);
                if (!resposta.Succeeded)
                {
                    resposta.Mensagem = String.Empty;
                }
                foreach (var item in resultadoValidacao)
                {
                    resposta.Mensagem += item.ErrorMessage + Mensagens.CaracterePulaLinha;
                }
            }

            return resposta;
        }
    }
}
