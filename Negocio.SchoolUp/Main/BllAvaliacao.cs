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
using System.Linq;

namespace Negocio.SchoolUp.Main
{
    public class BllAvaliacao : BllAplicacao<Avaliacao>
    {
        public string IdUsuarioAcao { get; set; }
        public string Ip { get; set; }
        public string Funcao { get; set; }
        public string CodigoToken { get; set; }
        public Guid IdAcao { get; set; }

        public BllAvaliacao(string ip, string nomeFuncao, string token) : base(ip, nomeFuncao, token)
        {
            Ip = ip;
            Funcao = nomeFuncao;
            CodigoToken = token;
            IdUsuarioAcao = base.IdUsuario;
        }

        public BllAvaliacao(string ip, string nomeFuncao) : base(ip, nomeFuncao)
        {
            IdUsuarioAcao = base.IdUsuario;
            Ip = ip;
            Funcao = nomeFuncao;
        }

        public GlResposta<Avaliacao> Obter(Guid idAvaliacao)
        {
            GlResposta<Avaliacao> resposta = new GlResposta<Avaliacao>();
            DadosGravacao.IdAcao = BllAcao.IdAcaoDetalhar;
            try
            {
                resposta = ValidarAutenticacaoPermissao<Avaliacao>(idAvaliacao);
                if (!resposta.Succeeded)
                {
                    return resposta;
                }
                DadosGravacao = BrHistorico.MontarHistorico(DadosGravacao, "Obter()", DadosGravacao.IdAcao, idAvaliacao.ToString());
                resposta = Read(idAvaliacao);
                if (resposta.Dados == null || resposta.Dados.Count == 0)
                {
                    return new GlResposta<Avaliacao>() { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado };
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

        public GlResposta<CmNotas> ObterNotas(Guid idAvaliacao)
        {
            GlResposta<CmNotas> resposta = new GlResposta<CmNotas>();
            DadosGravacao.IdAcao = BllAcao.IdAcaoAcessar;
            try
            {
                resposta = ValidarAutenticacaoPermissao<CmNotas>();
                if (!resposta.Succeeded)
                {
                    return resposta;
                }

                CarregarNotas(idAvaliacao);

                List<CmNotas> disciplinas = new DalAvaliacao().ObterTodos(idAvaliacao);

                if (disciplinas == null || disciplinas.Count == 0)
                {
                    return new GlResposta<CmNotas>() { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado };
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

        public GlResposta<CmNotas> ObterNotasAluno(Guid idAluno)
        {
            GlResposta<CmNotas> resposta = new GlResposta<CmNotas>();
            DadosGravacao.IdAcao = BllAcao.IdAcaoAcessar;
            try
            {
                resposta = ValidarAutenticacaoPermissao<CmNotas>();
                if (!resposta.Succeeded)
                {
                    return resposta;
                }

                CarregarNotas(idAluno);

                List<CmNotas> disciplinas = new DalAvaliacao().ObterTodosAluno(idAluno);

                if (disciplinas == null || disciplinas.Count == 0)
                {
                    return new GlResposta<CmNotas>() { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado };
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
        
        public GlResposta<CmNotas> ObterNotasAluno(Guid idAluno, Guid? idPeriodo, Guid? idBimestre)
        {
            GlResposta<CmNotas> resposta = new GlResposta<CmNotas>();
            DadosGravacao.IdAcao = BllAcao.IdAcaoAcessar;
            try
            {
                resposta = ValidarAutenticacaoPermissao<CmNotas>();
                if (!resposta.Succeeded)
                {
                    return resposta;
                }

                CarregarNotas(idAluno);

                List<CmNotas> disciplinas = new DalAvaliacao().ObterTodosAluno(idAluno, idPeriodo, idBimestre);

                if (disciplinas == null || disciplinas.Count == 0)
                {
                    return new GlResposta<CmNotas>() { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado };
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

        public GlResposta<CmAvaliacao> ObterGrade(Guid? idTurma, Guid? idSubPeriodo)
        {
            GlResposta<CmAvaliacao> resposta = new GlResposta<CmAvaliacao>();
            DadosGravacao.IdAcao = BllAcao.IdAcaoAcessar;
            try
            {
                resposta = ValidarAutenticacaoPermissao<CmAvaliacao>();
                if (!resposta.Succeeded)
                {
                    return resposta;
                }

                List<CmAvaliacao> avaliacoes = new DalAvaliacao().ObterTodos(idTurma, idSubPeriodo);

                if (avaliacoes == null || avaliacoes.Count == 0)
                {
                    return new GlResposta<CmAvaliacao>() { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado };
                }
                resposta.Dados = avaliacoes;
                resposta.Succeeded = true;

                if (!IsAnonimo)
                {
                    DadosGravacao = BrHistorico.MontarHistorico(DadosGravacao, "Obter()", DadosGravacao.IdAcao);
                    new DalGenerica<CmAvaliacao>().InserirHistorico(DadosGravacao);
                }
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public GlResposta<CmAvaliacao> ObterGradeDisciplina(Guid idTurma, Guid idSubPeriodo)
        {
            GlResposta<CmAvaliacao> resposta = new GlResposta<CmAvaliacao>();
            DadosGravacao.IdAcao = BllAcao.IdAcaoAcessar;
            try
            {
                resposta = ValidarAutenticacaoPermissao<CmAvaliacao>();
                if (!resposta.Succeeded)
                {
                    return resposta;
                }

                List<CmAvaliacao> avaliacoes = new DalAvaliacao().ObterTodosDisciplina(idTurma, idSubPeriodo);

                if (avaliacoes == null || avaliacoes.Count == 0)
                {
                    return new GlResposta<CmAvaliacao>() { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado };
                }
                resposta.Dados = avaliacoes;
                resposta.Succeeded = true;

                if (!IsAnonimo)
                {
                    DadosGravacao = BrHistorico.MontarHistorico(DadosGravacao, "Obter()", DadosGravacao.IdAcao);
                    new DalGenerica<CmAvaliacao>().InserirHistorico(DadosGravacao);
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

        public GlResposta<CmAvaliacao> Filtrar(string filtro)
        {
            GlResposta<CmAvaliacao> resposta = new GlResposta<CmAvaliacao>();
            DadosGravacao.IdAcao = BllAcao.IdAcaoProcurar;
            try
            {
                resposta = ValidarAutenticacaoPermissao<CmAvaliacao>();
                if (!resposta.Succeeded)
                {
                    return resposta;
                }
                DadosGravacao = BrHistorico.MontarHistorico(DadosGravacao, "Obter()", DadosGravacao.IdAcao);
                resposta = Search<CmAvaliacao>(i => i.IdSubPeriodo.ToString().Contains(filtro) && i.Excluido == false, o => o.NomeSubPeriodo, true);
                if (resposta.Dados == null || resposta.Dados.Count == 0)
                {
                    return new GlResposta<CmAvaliacao>() { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado };
                }
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public GlResposta<Avaliacao> InserirDisciplina(Avaliacao dados)
        {
            GlResposta<Avaliacao> resposta = new GlResposta<Avaliacao>() { Succeeded = false };
            DadosGravacao.IdAcao = BllAcao.IdAcaoInserir;
            try
            {
                BllTipoAvaliacao brTipoAvaliacao = new BllTipoAvaliacao();
                GlResposta<TipoAvaliacao> respostaTipoAvaliacao = brTipoAvaliacao.Obter("MEDIA");
                if (respostaTipoAvaliacao?.Dados?.Any() == false)
                {
                    return new GlResposta<Avaliacao>() { Succeeded = false, Mensagem = respostaTipoAvaliacao.Mensagem };
                }

                dados.Id = Guid.NewGuid();
                dados.Peso = 1;
                dados.Excluido = false;
                dados.Sigla = "MEDIA";
                dados.De = DateTime.Now;
                dados.Ate = DateTime.Now;
                dados.IdTipoAvaliacao = respostaTipoAvaliacao.Dados.FirstOrDefault().Id;

                resposta = Create(dados.Id, dados);
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public GlResposta<Avaliacao> Inserir(Avaliacao dados)
        {
            GlResposta<Avaliacao> resposta = new GlResposta<Avaliacao>() { Succeeded = false };
            DadosGravacao.IdAcao = BllAcao.IdAcaoInserir;
            try
            {
                dados.Id = Guid.NewGuid();
                dados.Peso = 1;
                dados.Excluido = false;
                resposta = Create(dados.Id, dados);
                //using (SchoolContext contexto = new SchoolContext())
                //{
                //    NotValidate = true;
                //    resposta = Create(contexto, dados.Id, dados);
                //    if (resposta?.Succeeded == false)
                //    {
                //        return resposta;
                //    }
                //    resposta = Search(i => i.IdProfessorDisciplina == dados.IdProfessorDisciplina && i.IdSubPeriodo == dados.IdSubPeriodo
                //    && i.IdTurma == dados.IdTurma && i.IdTipoAvaliacaoNavigation.Codigo == "MEDIA");
                //    if (resposta?.Dados?.Any() != true)
                //    {
                //        BllTipoAvaliacao brTipoAvaliacao = new BllTipoAvaliacao();
                //        GlResposta<TipoAvaliacao> respostaTipoAvaliacao = brTipoAvaliacao.Obter("MEDIA");
                //        if (respostaTipoAvaliacao?.Dados.Any() == false)
                //        {
                //            return new GlResposta<Avaliacao>() { Succeeded = false, Mensagem = respostaTipoAvaliacao.Mensagem };
                //        }

                //        DadosGravacao = BrHistorico.MontarHistoricoJson(DadosGravacao, dados, dados.Id.ToString());
                //        Avaliacao avaliacao = new Avaliacao()
                //        {
                //            Id = Guid.NewGuid(),
                //            Sigla = "MEDIA",
                //            De = dados.De,
                //            Ate = dados.De,
                //            IdTipoAvaliacao = respostaTipoAvaliacao.Dados.FirstOrDefault().Id,
                //            IdProfessorDisciplina = dados.IdProfessorDisciplina,
                //            IdSubPeriodo = dados.IdSubPeriodo,
                //            IdTurma = dados.IdTurma,
                //            Peso = 1,
                //            Excluido = false
                //        };
                        

                //        resposta = Create(contexto, avaliacao.Id, avaliacao);
                //        if (resposta?.Succeeded == false)
                //        {
                //            return resposta;
                //        }

                //    }
                //    resposta = Salvar<Avaliacao>(contexto, DadosGravacao);
                //}                                  
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public GlResposta<Avaliacao> Alterar(Avaliacao dados)
        {
            GlResposta<Avaliacao> resposta = new GlResposta<Avaliacao>() { Succeeded = false };
            DadosGravacao.IdAcao = BllAcao.IdAcaoAlterar;
            try
            {
                dados.Excluido = false;
                dados.Peso = 1;
                resposta = Update(dados.Id, dados);
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public GlResposta<Notas> AlterarNotaCompleto(Notas mdlNotas)
        {
            GlResposta<Notas> resposta = new GlResposta<Notas>() { Succeeded = false };
            DadosGravacao.IdAcao = BllAcao.IdAcaoAlterar;
            try
            {
                resposta = Read<Notas>(mdlNotas.Id);
                if (resposta?.Dados?.Count == null || resposta?.Dados?.Count == 0)
                {
                    return resposta;
                }
                Notas dados = resposta.Dados.FirstOrDefault();
                dados.Nota = mdlNotas.Nota;
                dados.NotaRecuperacao = mdlNotas.NotaRecuperacao;
                dados.Faltas = mdlNotas.Faltas;
                dados.Excluido = false;
                NotValidate = true;
                resposta = Update<Notas>(dados.Id, dados);
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public GlResposta<Notas> AlterarNota(Notas mdlNotas)
        {
            GlResposta<Notas> resposta = new GlResposta<Notas>() { Succeeded = false };
            DadosGravacao.IdAcao = BllAcao.IdAcaoAlterar;
            try
            {
                resposta = Read<Notas>(mdlNotas.Id);
                if (resposta?.Dados?.Count == null || resposta?.Dados?.Count == 0)
                {
                    return resposta;
                }
                Notas dados = resposta.Dados.FirstOrDefault();
                dados.Nota = mdlNotas.Nota;
                dados.Excluido = false;
                NotValidate = true;
                resposta = Update<Notas>(dados.Id, dados);
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public GlResposta<Notas> AlterarNotaRecuperacao(Notas mdlNotas)
        {
            GlResposta<Notas> resposta = new GlResposta<Notas>() { Succeeded = false };
            DadosGravacao.IdAcao = BllAcao.IdAcaoAlterar;
            try
            {
                resposta = Read<Notas>(mdlNotas.Id);
                if (resposta?.Dados?.Count == null || resposta?.Dados?.Count == 0)
                {
                    return resposta;
                }
                Notas dados = resposta.Dados.FirstOrDefault();
                dados.NotaRecuperacao = mdlNotas.NotaRecuperacao;
                dados.Excluido = false;
                NotValidate = true;
                resposta = Update<Notas>(dados.Id, dados);
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }


        public GlResposta<Notas> AlterarFaltas(Notas mdlNotas)
        {
            GlResposta<Notas> resposta = new GlResposta<Notas>() { Succeeded = false };
            DadosGravacao.IdAcao = BllAcao.IdAcaoAlterar;
            try
            {
                resposta = Read<Notas>(mdlNotas.Id);
                if (resposta?.Dados?.Count == null || resposta?.Dados?.Count == 0)
                {
                    return resposta;
                }
                Notas dados = resposta.Dados.FirstOrDefault();
                dados.Faltas = mdlNotas.Faltas;
                dados.Excluido = false;
                NotValidate = true;
                resposta = Update<Notas>(dados.Id, dados);
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public GlResposta<Notas> CarregarNotas(Guid? idAvaliacao)
        {
            GlResposta<Notas> resposta = new GlResposta<Notas>();
            DadosGravacao.IdAcao = BllAcao.IdAcaoAlterar;
            try
            {
                resposta = ValidarAutenticacaoPermissao<Notas>();
                if (!resposta.Succeeded)
                {
                    return resposta;
                }

                GlResposta<Avaliacao> respostaAvaliacao = Read<Avaliacao>(idAvaliacao.Value);
                if (respostaAvaliacao.Dados == null || respostaAvaliacao.Dados.Count == 0)
                {
                    return new GlResposta<Notas>() { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado };
                }
                else if(respostaAvaliacao.Succeeded == false)
                {
                    return new GlResposta<Notas>() { Succeeded = false, Mensagem = respostaAvaliacao.Mensagem };
                }

                Avaliacao mdlAvaliacao = respostaAvaliacao.Dados?.FirstOrDefault();

                GlResposta<ProfessorDisciplina> respostaProfessorDisciplina = Read<ProfessorDisciplina>(mdlAvaliacao.IdProfessorDisciplina);
                if (respostaProfessorDisciplina.Dados == null || respostaProfessorDisciplina.Dados.Count == 0)
                {
                    return new GlResposta<Notas>() { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado };
                }
                else if (respostaProfessorDisciplina.Succeeded == false)
                {
                    return new GlResposta<Notas>() { Succeeded = false, Mensagem = respostaProfessorDisciplina.Mensagem };
                }

                ProfessorDisciplina mdlProfessorDisciplina = respostaProfessorDisciplina.Dados?.FirstOrDefault();

                GlResposta<Inscricao> respostaInscricao = Search<Inscricao>(i => i.IdTurma == mdlAvaliacao.IdTurma && i.IdDisciplina == mdlProfessorDisciplina.IdDisciplina
                    && !i.Excluido && !i.IdAlunoNavigation.Excluido && !i.IdDisciplinaNavigation.Excluido && !i.IdTurmaNavigation.Excluido);

                if (respostaInscricao.Dados == null || respostaInscricao.Dados.Count == 0)
                {
                    return new GlResposta<Notas>() { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado };
                }
                else if (respostaInscricao.Succeeded == false)
                {
                    return new GlResposta<Notas>() { Succeeded = false, Mensagem = respostaInscricao.Mensagem };
                }

                resposta = Search<Notas>(i => i.IdAvaliacao == mdlAvaliacao.Id && !i.Excluido, o => o.Excluido.ToString(), true);
                if (resposta.Succeeded != true)
                {
                    return resposta;
                }
                NotValidate = true;

                using (SchoolContext context = new SchoolContext())
                {
                    DalSchool<Notas> daGenerica = new DalSchool<Notas>();
                    List<Inscricao> listInscritos = respostaInscricao.Dados;
                    List<Inscricao> listInscritosHistorico = new List<Inscricao>();

                    if (resposta != null && resposta.Dados != null && resposta?.Dados?.Any() == true)
                    {
                        List<Notas> listNotas = resposta.Dados;
                        foreach (var inscricao in listInscritos)
                        {
                            if (listNotas.Any(i => i.IdInscricao == inscricao.Id && i.Excluido == false) == false)
                            {
                                Notas mdlNotas = new Notas()
                                {
                                    Id = Guid.NewGuid(),
                                    IdAvaliacao = mdlAvaliacao.Id,
                                    IdInscricao = inscricao.Id,
                                    Excluido = false
                                };
                                resposta = Create<Notas>(context, mdlNotas.Id, mdlNotas);
                                if (resposta.Succeeded != true)
                                {
                                    return new GlResposta<Notas>() { Succeeded = false, Mensagem = respostaAvaliacao.Mensagem };
                                }
                                listInscritosHistorico.Add(inscricao);
                            }
                        }
                    }
                    else
                    {
                        foreach (var inscricao in listInscritos)
                        {
                            Notas mdlNotas = new Notas()
                            {
                                Id = Guid.NewGuid(),
                                IdAvaliacao = mdlAvaliacao.Id,
                                IdInscricao = inscricao.Id,
                                Excluido = false
                            };
                            resposta = Create<Notas>(context, mdlNotas.Id, mdlNotas);
                            if (resposta.Succeeded != true)
                            {
                                return new GlResposta<Notas>() { Succeeded = false, Mensagem = respostaAvaliacao.Mensagem };
                            }
                            listInscritosHistorico.Add(inscricao);
                        }
                    }
                    if (listInscritosHistorico.Any() == true)
                    {
                        DadosGravacao = BrHistorico.MontarHistoricoJson(DadosGravacao, listInscritosHistorico, mdlAvaliacao.ToString());
                        resposta = Salvar<Notas>(context, DadosGravacao);
                    }
                }
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public GlResposta<Avaliacao> Excluir(Avaliacao dados)
        {
            GlResposta<Avaliacao> resposta = new GlResposta<Avaliacao>() { Succeeded = false };
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

        public override GlResposta<Avaliacao> ValidaDados(Avaliacao dados, EAcoes acao)
        {
            if (dados == null)
            {
                return new GlResposta<Avaliacao>()
                {
                    Succeeded = false,
                    Mensagem = Mensagens.FormatoIncorreto
                };
            }
            GlResposta<Avaliacao> resposta = new GlResposta<Avaliacao>()
            {
                Succeeded = true,
                Mensagem = acao.Equals(EAcoes.DELETE) ? Mensagens.RegistroExcluidoSucesso : Mensagens.RegistroGravadoSucesso
            };

            if (acao.Equals(EAcoes.UPDATE) || acao.Equals(EAcoes.DELETE))
            {
                var objetoRetorno = Read(dados.Id);
                if (objetoRetorno.Dados == null)
                {
                    return new GlResposta<Avaliacao>() { Succeeded = false, Mensagem = Mensagens.SemRegistroEncontrado };
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
