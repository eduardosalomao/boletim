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
using Negocio.Nucleo.Permissao;
using Modelo.Nucleo.Custom;
using Modelo.Nucleo.Models;
using Acesso.SchoolUp.Contexts;
using Acesso.Nucleo.Contexts;
using Acesso.SchoolUp.Comum;
using Negocio.SchoolUp.Utilidades;

namespace Negocio.SchoolUp.Main
{
    public class BllAluno : BllAplicacao<Aluno>
    {
        public string IdUsuarioAcao { get; set; }
        public string Ip { get; set; }
        public string Funcao { get; set; }
        public string CodigoToken { get; set; }

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

        public GlResposta<CmAluno> Obter(Guid idAluno)
        {
            GlResposta<CmAluno> resposta = new GlResposta<CmAluno>();
            DadosGravacao.IdAcao = BllAcao.IdAcaoDetalhar;
            try
            {
                resposta = ValidarAutenticacaoPermissao<CmAluno>(idAluno);
                if (!resposta.Succeeded)
                {
                    return resposta;
                }
                DadosGravacao = BrHistorico.MontarHistorico(DadosGravacao, "Obter()", DadosGravacao.IdAcao, idAluno.ToString());
                var respostaAluno = Read(idAluno);
                if (respostaAluno.Dados == null || respostaAluno.Dados.Count == 0)
                {
                    return new GlResposta<CmAluno>() { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado };
                }
                else
                {
                    if (respostaAluno.Dados != null && respostaAluno.Dados.Count > 0)
                    {
                        var mdlAluno = respostaAluno.Dados.FirstOrDefault();
                        CmAluno mdlCmAluno = new CmAluno()
                        {
                            Id = idAluno,
                            IdEscola = mdlAluno.IdEscola,
                            Nome = mdlAluno.Nome,
                            Email = mdlAluno.Email,
                            Cpf = mdlAluno.Cpf,
                            DataNascimento = mdlAluno.DataNascimento,
                            Matricula = mdlAluno.Matricula,
                            Ativo = mdlAluno.Ativo,
                            Excluido = mdlAluno.Excluido
                        };
                        resposta.Dados = new List<CmAluno>();
                        resposta.Dados.Add(mdlCmAluno);
                    }
                    resposta.Succeeded = respostaAluno.Succeeded;
                    resposta.Id = respostaAluno.Id;
                    resposta.Mensagem = respostaAluno.Mensagem;
                }
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public GlResposta<Aluno> ObterTodos(Guid idEscola)
        {
            GlResposta<Aluno> resposta = new GlResposta<Aluno>();
            DadosGravacao.IdAcao = BllAcao.IdAcaoAcessar;
            try
            {
                resposta = ValidarAutenticacaoPermissao<Aluno>();
                if (!resposta.Succeeded)
                {
                    return resposta;
                }
                resposta = Search<Aluno>(i => i.Ativo.HasValue && i.Ativo.Value && !i.Excluido && i.IdEscola.Equals(idEscola), o => o.Nome, true);
                if (resposta.Dados == null || resposta.Dados.Count == 0)
                {
                    return new GlResposta<Aluno>() { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado };
                }
                if (!IsAnonimo)
                {
                    DadosGravacao.IdAcao = BllAcao.IdAcaoAcessar;
                    DadosGravacao = BrHistorico.MontarHistorico(DadosGravacao, "Obter()", DadosGravacao.IdAcao);
                    new DalGenerica<Aluno>().InserirHistorico(DadosGravacao);
                }
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }
        
        public GlResposta<Aluno> Filtrar(string filtro, Guid idEscola)
        {
            GlResposta<Aluno> resposta = new GlResposta<Aluno>();
            DadosGravacao.IdAcao = BllAcao.IdAcaoProcurar;
            try
            {
                resposta = ValidarAutenticacaoPermissao<Aluno>();
                if (!resposta.Succeeded)
                {
                    return resposta;
                }
                DadosGravacao = BrHistorico.MontarHistorico(DadosGravacao, "Obter()", DadosGravacao.IdAcao);
                resposta = Search(i => i.Ativo.Value && !i.Excluido && i.IdEscola.Equals(idEscola) && (i.Nome.Contains(filtro) || i.Matricula.Contains(filtro)), o => o.Nome, true);
                if (resposta.Dados == null || resposta.Dados.Count == 0)
                {
                    return new GlResposta<Aluno>() { Succeeded = true, Mensagem = Mensagens.SemRegistroEncontrado };
                }
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public GlResposta<CmNovoAcesso> CriarAcesso(CmNovoAcesso dados)
        {
            IsAnonimo = true;   
            GlResposta<CmNovoAcesso> glNovoAcesso = ValidaPrimeiroAcesso(dados);
            if (!glNovoAcesso.Succeeded)
            {
                return glNovoAcesso;
            }

            Historico historico = DadosGravacao;
            historico.IdHistorico = Guid.NewGuid().ToString();

            try
            {
                using (SchoolContext context = new SchoolContext())
                {
                    GlResposta<ResponsavelAluno> glResponsavel = Search<ResponsavelAluno>(i => i.IdResponsavelNavigation.IdUser.HasValue 
                                                            && i.IdResponsavelNavigation.IdUser.Value.ToString() == dados.IdUsuario);
                    ResponsavelAluno mdlResponsavel = glResponsavel.Dados.FirstOrDefault();

                    ResponsavelAluno mdlResponsavelAluno = new ResponsavelAluno()
                    {
                        Id = Guid.NewGuid(),
                        IdAluno = dados.IdAluno,
                        IdRelacao = mdlResponsavel.IdRelacao,
                        IdResponsavel = mdlResponsavel.IdResponsavel,
                        IsResponsavelFinanceiro = mdlResponsavel.IsResponsavelFinanceiro,
                        Excluido = false
                    };

                    DalSchool<ResponsavelAluno> daGenericaResponsavelAluno = new DalSchool<ResponsavelAluno>();

                    daGenericaResponsavelAluno.Inserir(context, mdlResponsavelAluno);

                    daGenericaResponsavelAluno.SalvarEInserirHistorico(context, historico);
                }
            }
            catch (Exception excecao)
            {
                return new GlResposta<CmNovoAcesso>() { Succeeded = false, Mensagem = new BllErro().Inserir(DadosGravacao, excecao) };
            }

            return glNovoAcesso;
        }


        //public GlResposta<CmNovoAcesso> EnviarEmail(CmLogin dados)
        //{
            
        //    try
        //    {
        //        IsAnonimo = true;
        //        if (!new BllAcesso().ExistLogin(dados.Login))
        //        {
        //            return new GlResposta<CmNovoAcesso>() { Succeeded = false, Mensagem = Mensagens.EmailInexistente };
        //        }
        //        Historico historico = DadosGravacao;
        //        historico.IdHistorico = Guid.NewGuid().ToString();
        //        historico.DadosGravados = $"Recuperar senha do email: {dados.Login}";
        //        historico.TabelaObjeto = "Recuperar Senha";
                
        //        DalSchool<ResponsavelAluno> daGenericaResponsavelAluno = new DalSchool<ResponsavelAluno>();

        //        daGenericaResponsavelAluno.SalvarEInserirHistorico(historico);
        //    }
        //    catch (Exception excecao)
        //    {
        //        return new GlResposta<CmNovoAcesso>() { Succeeded = false, Mensagem = new BllErro().Inserir(DadosGravacao, excecao) };
        //    }

        //    return new GlResposta<CmNovoAcesso>() { Succeeded = true };
        //}
        
        public GlResposta<CmPrimeiroAcesso> CriarAcesso(CmPrimeiroAcesso dados)
        {

            GlResposta<CmPrimeiroAcesso> glPrimeiroAcesso = ValidaPrimeiroAcesso(dados);
            if (!glPrimeiroAcesso.Succeeded)
            {
                return glPrimeiroAcesso;
            }

            Historico historico = DadosGravacao;
            historico.IdHistorico = Guid.NewGuid().ToString();

            CmRegistro cmRegistro = new CmRegistro()
            {
                Login = dados.Email,
                Senha = dados.Senha
            };
                       
            BllAutenticacao bllAutenticacao = new BllAutenticacao();
            
            var glAutenticacao = bllAutenticacao.CriarUsuario(cmRegistro).Result;

            if (glAutenticacao?.Succeeded != true)
            {
                return new GlResposta<CmPrimeiroAcesso>() { Succeeded = false, Mensagem = glAutenticacao?.Mensagem };
            }

            BllPerfil brPerfis = new BllPerfil();
            GlResposta<PerfilUsuario> respostaPerfil = new GlResposta<PerfilUsuario>() { Succeeded = false };
            try
            {
                var perfilAluno = BllPerfil.ListaPerfis.Where(i => i.Codigo == "SCHOOLUP_RESPONSAVEL").FirstOrDefault();


                using (PadraoContext context = new PadraoContext())
                {
                    Usuario mdlUsuario = new Usuario()
                    {
                        IdUsuario = glAutenticacao.Id,
                        UserId = glAutenticacao.Id,
                        Nome = "NÃO INFORMADO",
                        Ativo = true,
                        Excluido = false,
                        Reservado = false
                    };

                    PerfilUsuario mdlPerfilUsuario = new PerfilUsuario() { IdPerfilUsuario = Guid.NewGuid().ToString(), IdPerfil = perfilAluno.IdPerfil, IdUsuario = glAutenticacao.Id };

                    DalGenerica<Usuario> daGenericaUsuario = new DalGenerica<Usuario>();
                    DalGenerica<PerfilUsuario> daGenericaPerfilUsuario = new DalGenerica<PerfilUsuario>();

                    daGenericaUsuario.Inserir(context, mdlUsuario);
                    daGenericaPerfilUsuario.Inserir(context, mdlPerfilUsuario);

                    daGenericaUsuario.SalvarEInserirHistorico(context, historico);
                }
            }
            catch (Exception excecao)
            {
                var respostaExclusaoUsuario = bllAutenticacao.ExcluirUsuario(new CmRegistro() { IdUsuario = glAutenticacao.Id, Login = dados.Matricula });
                return new GlResposta<CmPrimeiroAcesso>() { Succeeded = false, Mensagem = new BllErro().Inserir(DadosGravacao, excecao) };
            }

            try
            {
                GlResposta<TipoRelacao> glTipoRelacao = ReadAll<TipoRelacao>();

                using (SchoolContext context = new SchoolContext())
                {
                    Responsavel mdlResponsavel = new Responsavel()
                    {
                        Id = Guid.NewGuid(),
                        IdUser = Guid.Parse(glAutenticacao.Id),
                        Email = dados.Email,
                        Nome = "NÃO INFORMADO",
                        Excluido = false
                    };

                    ResponsavelAluno mdlResponsavelAluno = new ResponsavelAluno()
                    {
                        Id = Guid.NewGuid(),
                        IdAluno = dados.IdAluno,
                        IdRelacao = glTipoRelacao.Dados.Where(i => !i.Excluido && i.Codigo == "NAOINFORMADA").FirstOrDefault().Id,
                        IdResponsavel = mdlResponsavel.Id,
                        IsResponsavelFinanceiro = false,
                        Excluido = false
                    };

                    DalSchool<Responsavel> daGenericaResponsavel = new DalSchool<Responsavel>();
                    DalSchool<ResponsavelAluno> daGenericaResponsavelAluno = new DalSchool<ResponsavelAluno>();

                    daGenericaResponsavel.Inserir(context, mdlResponsavel);
                    daGenericaResponsavelAluno.Inserir(context, mdlResponsavelAluno);

                    daGenericaResponsavel.SalvarEInserirHistorico(context, historico);
                }
            }
            catch (Exception excecao)
            {
                bllAutenticacao.ExcluirUsuario(new CmRegistro() { IdUsuario = glAutenticacao.Id, Login = dados.Matricula });
                return new GlResposta<CmPrimeiroAcesso>() { Succeeded = false, Mensagem = new BllErro().Inserir(DadosGravacao, excecao) };
            }

            return new GlResposta<CmPrimeiroAcesso>()
            {
                Succeeded = true,
                Mensagem = Mensagens.AcessoCriado
            };
        }

        public GlResposta<CmPrimeiroAcesso> ValidaPrimeiroAcesso(CmPrimeiroAcesso dados)
        {
            GlResposta<CmPrimeiroAcesso> validaPrimeiroAcesso = ValidaDadosPrimeiroAcesso(dados, EAcoes.ADD);

            if (!validaPrimeiroAcesso.Succeeded)
            {
                return validaPrimeiroAcesso;
            }

            BllAutenticacao brAutenticacao = new BllAutenticacao();
            var glAutenticacao = brAutenticacao.ValidarLogin(dados.Email, "").Result;

            if (glAutenticacao?.Succeeded != true)
            {
                return new GlResposta<CmPrimeiroAcesso>()
                {
                    Succeeded = false,
                    Mensagem = glAutenticacao.Mensagem
                };
            }

            IsAnonimo = true;
            GlResposta<CmAluno> glAluno = Obter(dados.IdAluno);

            if (glAluno?.Dados?.Any() != true)
            {
                return new GlResposta<CmPrimeiroAcesso>()
                {
                    Succeeded = false,
                    Mensagem = String.Format(Mensagens.ObjetoNaoEncontrado, "Aluno")
                };
            }

            CmAluno mdlAluno = glAluno.Dados.FirstOrDefault();
            if (mdlAluno.Excluido || !mdlAluno.Ativo.HasValue || !mdlAluno.Ativo.Value)
            {
                return new GlResposta<CmPrimeiroAcesso>()
                {
                    Succeeded = false,
                    Mensagem = String.Format(Mensagens.ObjetoExcluidoInativo, "Aluno")
                };
            }

            if (String.Compare(mdlAluno.Matricula, dados.Matricula, StringComparison.InvariantCultureIgnoreCase) != 0
                || DateTime.Compare(mdlAluno.DataNascimento.Date, dados.DataNascimento.Date) != 0)
            {
                return new GlResposta<CmPrimeiroAcesso>()
                {
                    Succeeded = false,
                    Mensagem = String.Format(Mensagens.MatriculaNascimentoNaoCorrespondem, "Aluno")
                };
            }

            return new GlResposta<CmPrimeiroAcesso>()
            {
                Succeeded = true
            };
        }
        
        public GlResposta<CmNovoAcesso> ValidaPrimeiroAcesso(CmNovoAcesso dados)
        {
            GlResposta<CmNovoAcesso> validaPrimeiroAcesso = ValidaDadosPrimeiroAcesso(dados, EAcoes.ADD);

            if (!validaPrimeiroAcesso.Succeeded)
            {
                return validaPrimeiroAcesso;
            }

            GlResposta<CmAluno> glAluno = Obter(dados.IdAluno);

            if (glAluno?.Dados?.Any() != true)
            {
                return new GlResposta<CmNovoAcesso>()
                {
                    Succeeded = false,
                    Mensagem = String.Format(Mensagens.ObjetoNaoEncontrado, "Aluno")
                };
            }

            GlResposta<ResponsavelAluno> glResponsavel = Search<ResponsavelAluno>(i => i.IdResponsavelNavigation.IdUser.HasValue
                                                            && i.IdResponsavelNavigation.IdUser.Value.ToString() == dados.IdUsuario);
            if (glResponsavel?.Succeeded != true || glResponsavel?.Dados?.Any() != true)
            {
                return new GlResposta<CmNovoAcesso>()
                {
                    Succeeded = false,
                    Mensagem = String.Format(Mensagens.ObjetoNaoEncontrado, "Responsável")
                };
            }

            CmAluno mdlAluno = glAluno.Dados.FirstOrDefault();
            if (mdlAluno.Excluido || !mdlAluno.Ativo.HasValue || !mdlAluno.Ativo.Value)
            {
                return new GlResposta<CmNovoAcesso>()
                {
                    Succeeded = false,
                    Mensagem = String.Format(Mensagens.ObjetoExcluidoInativo, "Aluno")
                };
            }

            if (String.Compare(mdlAluno.Matricula, dados.Matricula, StringComparison.InvariantCultureIgnoreCase) != 0 
                || DateTime.Compare(mdlAluno.DataNascimento.Date, dados.DataNascimento.Date) != 0)
            {
                return new GlResposta<CmNovoAcesso>()
                {
                    Succeeded = false,
                    Mensagem = String.Format(Mensagens.MatriculaNascimentoNaoCorrespondem, "Aluno")
                };
            }

            return validaPrimeiroAcesso;
        }

        public GlResposta<CmPrimeiroAcesso> ValidaDadosPrimeiroAcesso(CmPrimeiroAcesso dados, EAcoes acao)
        {
            if (dados == null)
            {
                return new GlResposta<CmPrimeiroAcesso>()
                {
                    Succeeded = false,
                    Mensagem = Mensagens.FormatoIncorreto
                };
            }
            GlResposta<CmPrimeiroAcesso> resposta = new GlResposta<CmPrimeiroAcesso>()
            {
                Succeeded = true,
                Mensagem = acao.Equals(EAcoes.DELETE) ? Mensagens.RegistroExcluidoSucesso : Mensagens.RegistroGravadoSucesso
            };

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

        public GlResposta<CmNovoAcesso> ValidaDadosPrimeiroAcesso(CmNovoAcesso dados, EAcoes acao)
        {
            if (dados == null)
            {
                return new GlResposta<CmNovoAcesso>()
                {
                    Succeeded = false,
                    Mensagem = Mensagens.FormatoIncorreto
                };
            }
            GlResposta<CmNovoAcesso> resposta = new GlResposta<CmNovoAcesso>()
            {
                Succeeded = true,
                Mensagem = acao.Equals(EAcoes.DELETE) ? Mensagens.RegistroExcluidoSucesso : Mensagens.RegistroGravadoSucesso
            };

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

        public GlResposta<Aluno> Inserir(CmAluno dados)
        {
            GlResposta<Aluno> resposta = new GlResposta<Aluno>() { Succeeded = false };
            DadosGravacao.IdAcao = BllAcao.IdAcaoInserir;
            try
            {
                Aluno mdlAluno = new Aluno()
                {
                    Id = Guid.NewGuid(),
                    IdEscola = dados.IdEscola,
                    Email = dados.Email,
                    Nome = dados.Nome,
                    DataNascimento = dados.DataNascimento,
                    Matricula = dados.Matricula,
                    Ativo = true,
                    Excluido = false
                };
                resposta = Create(mdlAluno.Id, mdlAluno);
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public GlResposta<Aluno> Alterar(CmAluno dados)
        {
            GlResposta<Aluno> resposta = new GlResposta<Aluno>() { Succeeded = false };
            DadosGravacao.IdAcao = BllAcao.IdAcaoAlterar;
            try
            {
                Aluno mdlAluno = new Aluno()
                {
                    Id = dados.Id,
                    IdEscola = dados.IdEscola,
                    Email = dados.Email,
                    Cpf = dados.Cpf,
                    Nome = dados.Nome,
                    DataNascimento = dados.DataNascimento,
                    Matricula = dados.Matricula,
                    Ativo = true,
                    Excluido = false
                };
                resposta = Update(dados.Id, mdlAluno);
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public GlResposta<Aluno> Excluir(CmAluno dados)
        {
            GlResposta<Aluno> resposta = new GlResposta<Aluno>() { Succeeded = false };
            DadosGravacao.IdAcao = BllAcao.IdAcaoExcluir;
            try
            {
                Aluno mdlAluno = new Aluno()
                {
                    Id = dados.Id,
                    IdEscola = dados.IdEscola,
                    Email = dados.Email,
                    Nome = dados.Nome,
                    DataNascimento = dados.DataNascimento,
                    Matricula = dados.Matricula,
                    Ativo = false,
                    Excluido = true
                };
                resposta = DeleteLogical(mdlAluno.Id, mdlAluno);
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public override GlResposta<Aluno> ValidaDados(Aluno dados, EAcoes acao)
        {
            if (dados == null)
            {
                return new GlResposta<Aluno>()
                {
                    Succeeded = false,
                    Mensagem = Mensagens.FormatoIncorreto
                };
            }
            GlResposta<Aluno> resposta = new GlResposta<Aluno>()
            {
                Succeeded = true,
                Mensagem = acao.Equals(EAcoes.DELETE) ? Mensagens.RegistroExcluidoSucesso : Mensagens.RegistroGravadoSucesso
            };

            if (acao.Equals(EAcoes.UPDATE) || acao.Equals(EAcoes.DELETE))
            {
                var objetoRetorno = Read(dados.Id);
                if (objetoRetorno.Dados == null)
                {
                    return new GlResposta<Aluno>() { Succeeded = false, Mensagem = Mensagens.SemRegistroEncontrado };
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
