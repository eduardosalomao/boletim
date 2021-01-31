using Acesso.SchoolUp.Comum;
using Acesso.SchoolUp.Contexts;
using Comum;
using Modelo.Nucleo.Enumerador;
using Modelo.Nucleo.Geral;
using Modelo.Nucleo.Models;
using Modelo.SchoolUp.Recursos;
using Negocio.Nucleo.Geral;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Negocio.SchoolUp
{
    public class BllAplicacao<T> where T : class
    {
        public Historico DadosGravacao { get; set; }
        public BllHistorico BrHistorico { get; set; }
        public string Token { get; set; }
        public string IdUsuario { get; set; }
        public string NomeFuncao { get; set; }
        public bool IsAutenticado { get; set; }
        public bool IsAnonimo { get; set; }
        public bool NotValidate { get; set; }


        public BllAplicacao()
        {
            IsAnonimo = true;
        }

        public BllAplicacao(string ip, string nomeFuncao, string token)
        {
            BrHistorico = new BllHistorico(ip, nomeFuncao, token);
            NomeFuncao = nomeFuncao;
            IsAnonimo = false;
            IdUsuario = BrHistorico.IdUsuario == null ? Configuracoes.IdUsuarioLogger : BrHistorico.IdUsuario;
            IsAutenticado = BrHistorico.IsAutenticado;
            DadosGravacao = BrHistorico.GetHistorico;
            DadosGravacao.IdUsuario = IdUsuario;
            BllAcao.ObterTodos();
        }

        public BllAplicacao(string ip, string nomeFuncao)
        {
            BrHistorico = new BllHistorico(ip, nomeFuncao, "");
            NomeFuncao = nomeFuncao;
            IsAnonimo = true;
            IdUsuario = BrHistorico.IdUsuario == null ? Configuracoes.IdUsuarioLogger : BrHistorico.IdUsuario;
            IsAutenticado = BrHistorico.IsAutenticado;
            DadosGravacao = BrHistorico.GetHistorico;
            DadosGravacao.IdUsuario = IdUsuario;
            BllAcao.ObterTodos();
        }

        public GlResposta<T> ValidarAutenticacaoPermissao(Guid? idUsuario)
        {
            GlResposta<T> resposta = new GlResposta<T>() { Succeeded = true };
            if (IsAnonimo)
            {
                return resposta;
            }
            if (!IsAutenticado)
            {
                return new GlResposta<T>() { Succeeded = false, Mensagem = Mensagens.UsuarioNaoAutenticado };
            }
            if (idUsuario == null)
            {
                return new GlResposta<T>() { Succeeded = false, Mensagem = Mensagens.ObjetoNulo };
            }
            if (idUsuario.ToString() != IdUsuario)
            {
                return new GlResposta<T>() { Succeeded = false, Mensagem = Mensagens.SomenteSuaConta };
            }
            if (!BllPermissao.VerificarPermissao(IdUsuario, DadosGravacao.IdAcao, NomeFuncao))
            {
                resposta.Succeeded = false;
                resposta.Mensagem = Mensagens.PerfilSemAcesso;
                return resposta;
            }
            return resposta;
        }

        public GlResposta<string> VerificarPermissao(string nomeFuncao)
        {
            GlResposta<string> resposta = new GlResposta<string>() { Succeeded = false };

            BllAcao.ObterTodos();
            
            if (!BllPermissao.VerificarPermissao(IdUsuario, BllAcao.IdAcaoAcessar, nomeFuncao))
            {
                resposta.Mensagem = Mensagens.PerfilSemAcesso;
                return resposta;
            }

            resposta.Succeeded = true;
            return resposta;
        }

        public GlResposta<T> ValidarAutenticacaoPermissao()
        {
            GlResposta<T> resposta = new GlResposta<T>() { Succeeded = true };
            if (IsAnonimo)
            {
                return resposta;
            }
            if (!IsAutenticado)
            {
                return new GlResposta<T>() { Succeeded = false, Mensagem = Mensagens.UsuarioNaoAutenticado };
            }
            if (!BllPermissao.VerificarPermissao(IdUsuario, DadosGravacao.IdAcao, NomeFuncao))
            {
                resposta.Succeeded = false;
                resposta.Mensagem = Mensagens.PerfilSemAcesso;
                return resposta;
            }
            return resposta;
        }

        public GlResposta<TC> ValidarAutenticacaoPermissao<TC>() where TC : class
        {
            GlResposta<TC> resposta = new GlResposta<TC>() { Succeeded = true };
            if (IsAnonimo)
            {
                return resposta;
            }
            if (!IsAutenticado)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = Mensagens.UsuarioNaoAutenticado;
                return resposta;
            }
            if (!BllPermissao.VerificarPermissao(IdUsuario, DadosGravacao.IdAcao, NomeFuncao))
            {
                resposta.Succeeded = false;
                resposta.Mensagem = Mensagens.PerfilSemAcesso;
                return resposta;
            }
            return resposta;
        }

        public GlResposta<TC> ValidarAutenticacaoPermissao<TC>(Guid? idUsuario) where TC : class
        {
            GlResposta<TC> resposta = new GlResposta<TC>() { Succeeded = true };
            if (IsAnonimo)
            {
                return resposta;
            }
            if (!IsAutenticado)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = Mensagens.UsuarioNaoAutenticado;
                return resposta;
            }
            if (idUsuario == null)
            {
                return new GlResposta<TC>() { Succeeded = false, Mensagem = Mensagens.ObjetoNulo };
            }
            //if (idUsuario.ToString() != IdUsuario)
            //{
            //    return new GlResposta<TC>() { Succeeded = false, Mensagem = Mensagens.SomenteSuaConta };
            //}
            if (!BllPermissao.VerificarPermissao(IdUsuario, DadosGravacao.IdAcao, NomeFuncao))
            {
                resposta.Succeeded = false;
                resposta.Mensagem = Mensagens.PerfilSemAcesso;
                return resposta;
            }
            return resposta;
        }

        public virtual GlResposta<T> ValidaDados(T dados, EAcoes acao)
        {
            return new GlResposta<T> { Succeeded = NotValidate };
        }

        public virtual GlResposta<TC> ValidaDadosAuxiliar<TC>(TC dados, EAcoes acao) where TC : class
        {
            return new GlResposta<TC> { Succeeded = NotValidate };
        }

        public GlResposta<TC> Salvar<TC>(SchoolContext contexto, Historico historico) where TC : class
        {
            GlResposta<TC> resposta = new GlResposta<TC>();
            try
            {
                new DalSchool<TC>().SalvarEInserirHistorico(contexto, historico);
                resposta.Succeeded = true;
                resposta.Mensagem = Mensagens.RegistroGravadoSucesso;
                return resposta;
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }
            return resposta;
        }

        public GlResposta<T> Create(Guid id, T dados)
        {
            GlResposta<T> resposta = new GlResposta<T>();
            DadosGravacao.IdAcao = BllAcao.IdAcaoInserir;
            try
            {
                resposta = ValidarAutenticacaoPermissao();
                if (!resposta.Succeeded)
                {
                    return resposta;
                }
                resposta = ValidaDados(dados, EAcoes.ADD);
                if (!resposta.Succeeded)
                {
                    return resposta;
                }
                DalSchool<T> daAcesso = new DalSchool<T>();
                BrHistorico.MontarHistoricoJson(DadosGravacao, dados, id.ToString());
                dados = daAcesso.Inserir(DadosGravacao, dados);
                resposta.Succeeded = true;
                resposta.Id = id.ToString();
                resposta.Mensagem = Mensagens.RegistroGravadoSucesso;
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }
            return resposta;
        }

        public GlResposta<TC> Create<TC>(Guid id, TC dados) where TC : class
        {
            GlResposta<TC> resposta = new GlResposta<TC>();
            DadosGravacao.IdAcao = BllAcao.IdAcaoInserir;
            try
            {
                resposta = ValidarAutenticacaoPermissao<TC>();
                if (!resposta.Succeeded)
                {
                    return resposta;
                }
                resposta = ValidaDadosAuxiliar<TC>(dados, EAcoes.ADD);
                if (!resposta.Succeeded)
                {
                    return resposta;
                }
                DalSchool<TC> daAcesso = new DalSchool<TC>();
                BrHistorico.MontarHistoricoJson(DadosGravacao, dados, id.ToString());
                dados = daAcesso.Inserir(DadosGravacao, dados);
                resposta.Succeeded = true;
                resposta.Id = id.ToString();
                resposta.Mensagem = Mensagens.RegistroGravadoSucesso;
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }
            return resposta;
        }

        public GlResposta<TC> Create<TC>(SchoolContext contexto, Guid id, TC dados) where TC : class
        {
            GlResposta<TC> resposta = new GlResposta<TC>();
            DadosGravacao.IdAcao = BllAcao.IdAcaoInserir;
            try
            {
                resposta = ValidarAutenticacaoPermissao<TC>();
                if (!resposta.Succeeded)
                {
                    return resposta;
                }
                resposta = ValidaDadosAuxiliar<TC>(dados, EAcoes.ADD);
                if (!resposta.Succeeded)
                {
                    return resposta;
                }
                DalSchool<TC> daAcesso = new DalSchool<TC>();
                dados = daAcesso.Inserir(contexto, dados);
                resposta.Succeeded = true;
                resposta.Id = id.ToString();
                resposta.Mensagem = Mensagens.RegistroGravadoSucesso;
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }
            return resposta;
        }

        public GlResposta<T> Update(Guid id, T dados)
        {
            GlResposta<T> resposta = new GlResposta<T>();
            DadosGravacao.IdAcao = BllAcao.IdAcaoAlterar;
            try
            {
                resposta = ValidarAutenticacaoPermissao();
                if (!resposta.Succeeded)
                {
                    return resposta;
                }
                resposta = ValidaDados(dados, EAcoes.UPDATE);
                if (!resposta.Succeeded)
                {
                    return resposta;
                }
                DalSchool<T> daAcesso = new DalSchool<T>();
                BrHistorico.MontarHistoricoJson(DadosGravacao, dados, id.ToString());
                dados = daAcesso.Alterar(DadosGravacao, dados);
                resposta.Succeeded = true;
                resposta.Id = id.ToString();
                resposta.Mensagem = Mensagens.RegistroGravadoSucesso;
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }
            return resposta;
        }

        public GlResposta<TC> Update<TC>(Guid id, TC dados) where TC : class
        {
            GlResposta<TC> resposta = new GlResposta<TC>();
            DadosGravacao.IdAcao = BllAcao.IdAcaoAlterar;
            try
            {
                resposta = ValidarAutenticacaoPermissao<TC>();
                if (!resposta.Succeeded)
                {
                    return resposta;
                }
                resposta = ValidaDadosAuxiliar<TC>(dados, EAcoes.UPDATE);
                if (!resposta.Succeeded)
                {
                    return resposta;
                }
                DalSchool<TC> daAcesso = new DalSchool<TC>();
                BrHistorico.MontarHistoricoJson(DadosGravacao, dados, id.ToString());
                dados = daAcesso.Alterar(DadosGravacao, dados);
                resposta.Succeeded = true;
                resposta.Id = id.ToString();
                resposta.Mensagem = Mensagens.RegistroGravadoSucesso;
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }
            return resposta;
        }

        public GlResposta<TC> Update<TC>(SchoolContext contexto, Guid id, TC dados) where TC : class
        {
            GlResposta<TC> resposta = new GlResposta<TC>();
            DadosGravacao.IdAcao = BllAcao.IdAcaoAlterar;
            try
            {
                resposta = ValidarAutenticacaoPermissao<TC>();
                if (!resposta.Succeeded)
                {
                    return resposta;
                }
                resposta = ValidaDadosAuxiliar(dados, EAcoes.UPDATE);
                if (!resposta.Succeeded)
                {
                    return resposta;
                }
                DalSchool<TC> daAcesso = new DalSchool<TC>();
                dados = daAcesso.Alterar(contexto, dados);
                resposta.Succeeded = true;
                resposta.Id = id.ToString();
                resposta.Mensagem = Mensagens.RegistroGravadoSucesso;
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }
            return resposta;
        }

        //public GlResposta<TC> Update<TC>(SchoolContext contexto, Expression<Func<TC, bool>> filtro, TC dados) where TC : class
        //{
        //    GlResposta<TC> resposta = new GlResposta<TC>();
        //    DadosGravacao.IdAcao = BllAcao.IdAcaoAlterar;
        //    try
        //    {
        //        resposta = ValidarAutenticacaoPermissao<TC>();
        //        if (!resposta.Succeeded)
        //        {
        //            return resposta;
        //        }
        //        resposta = ValidaDados(dados, EAcoes.UPDATE);
        //        if (!resposta.Succeeded)
        //        {
        //            return resposta;
        //        }
        //        DalSchool<TC> daAcesso = new DalSchool<TC>();
        //        dados = daAcesso.Alterar(contexto, dados);
        //        resposta.Succeeded = true;
        //        resposta.Id = id.ToString();
        //        resposta.Mensagem = Mensagens.RegistroGravadoSucesso;
        //    }
        //    catch (Exception excecao)
        //    {
        //        resposta.Succeeded = false;
        //        resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
        //    }
        //    return resposta;
        //}

        public GlResposta<T> Delete(Guid id, T dados)
        {
            GlResposta<T> resposta = new GlResposta<T>();
            DadosGravacao.IdAcao = BllAcao.IdAcaoExcluir;
            try
            {
                resposta = ValidarAutenticacaoPermissao();
                if (!resposta.Succeeded)
                {
                    return resposta;
                }
                resposta = ValidaDados(dados, EAcoes.DELETE);
                if (!resposta.Succeeded)
                {
                    return resposta;
                }
                DalSchool<T> daAcesso = new DalSchool<T>();
                BrHistorico.MontarHistoricoJson(DadosGravacao, dados, id.ToString());
                daAcesso.Excluir(DadosGravacao, id);
                resposta.Succeeded = true;
                resposta.Id = id.ToString();
                resposta.Mensagem = Mensagens.RegistroExcluidoSucesso;
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }
            return resposta;
        }

        //public GlResposta<TC> Delete<TC>(SchoolContext contexto, Guid id, TC dados) where TC : class
        //{
        //    GlResposta<TC> resposta = new GlResposta<TC>();
        //    DadosGravacao.IdAcao = BllAcao.IdAcaoExcluir;
        //    try
        //    {
        //        resposta = ValidarAutenticacaoPermissao<TC>();
        //        if (!resposta.Succeeded)
        //        {
        //            return resposta;
        //        }
        //        resposta = ValidaDados<TC>(dados, EAcoes.DELETE);
        //        if (!resposta.Succeeded)
        //        {
        //            return resposta;
        //        }
        //        DalSchool<TC> daAcesso = new DalSchool<TC>();
        //        BrHistorico.MontarHistoricoJson(DadosGravacao, dados, id.ToString());
        //        daAcesso.Excluir(DadosGravacao, id);
        //        resposta.Succeeded = true;
        //        resposta.Id = id.ToString();
        //        resposta.Mensagem = Mensagens.RegistroExcluidoSucesso;
        //    }
        //    catch (Exception excecao)
        //    {
        //        resposta.Succeeded = false;
        //        resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
        //    }
        //    return resposta;
        //}

        public GlResposta<TC> DeleteLogical<TC>(Guid id, TC dados) where TC : class
        {
            GlResposta<TC> resposta = new GlResposta<TC>();
            DadosGravacao.IdAcao = BllAcao.IdAcaoExcluir;
            try
            {
                resposta = ValidarAutenticacaoPermissao<TC>();
                if (!resposta.Succeeded)
                {
                    return resposta;
                }
                resposta = ValidaDadosAuxiliar<TC>(dados, EAcoes.DELETE);
                if (!resposta.Succeeded)
                {
                    return resposta;
                }
                DalSchool<TC> daAcesso = new DalSchool<TC>();
                BrHistorico.MontarHistoricoJson(DadosGravacao, dados, id.ToString());
                dados = daAcesso.Alterar(DadosGravacao, dados);
                resposta.Succeeded = true;
                resposta.Mensagem = Mensagens.RegistroExcluidoSucesso;
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }
            return resposta;
        }

        public GlResposta<T> DeleteLogical(Guid id, T dados) 
        {
            GlResposta<T> resposta = new GlResposta<T>();
            DadosGravacao.IdAcao = BllAcao.IdAcaoExcluir;
            try
            {
                resposta = ValidarAutenticacaoPermissao();
                if (!resposta.Succeeded)
                {
                    return resposta;
                }
                resposta = ValidaDados(dados, EAcoes.DELETE);
                if (!resposta.Succeeded)
                {
                    return resposta;
                }
                DalSchool<T> daAcesso = new DalSchool<T>();
                BrHistorico.MontarHistoricoJson(DadosGravacao, dados, id.ToString());
                dados = daAcesso.Alterar(DadosGravacao, dados);
                resposta.Succeeded = true;
                resposta.Mensagem = Mensagens.RegistroExcluidoSucesso;
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }
            return resposta;
        }
        
        public GlResposta<TC> DeleteLogical<TC>(SchoolContext contexto, TC dados) where TC : class
        {
            GlResposta<TC> resposta = new GlResposta<TC>();
            DadosGravacao.IdAcao = BllAcao.IdAcaoExcluir;
            try
            {
                DalSchool<TC> daAcesso = new DalSchool<TC>();
                daAcesso.Alterar(contexto, dados);
                resposta.Succeeded = true;
                resposta.Mensagem = Mensagens.RegistroExcluidoSucesso;
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }
            return resposta;
        }

        public GlResposta<T> Read(Guid id)
        {
            GlResposta<T> resposta = new GlResposta<T>();
            DalSchool<T> daAcesso = new DalSchool<T>();
            try
            {
                resposta = ValidarAutenticacaoPermissao();
                if (!resposta.Succeeded)
                {
                    return resposta;
                }
                var mdlObjeto = daAcesso.Obter(id);

                if (mdlObjeto != null)
                {
                    resposta.Dados = new List<T>();
                    resposta.Dados.Add(mdlObjeto);
                }
                if (!IsAnonimo)
                {
                    DadosGravacao.IdAcao = BllAcao.IdAcaoDetalhar;
                    BrHistorico.MontarHistorico(DadosGravacao, "Obter()", DadosGravacao.IdAcao, id.ToString());
                    daAcesso.InserirHistorico(DadosGravacao);
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

        public GlResposta<TC> Read<TC>(Expression<Func<TC, bool>> filtro) where TC : class
        {
            GlResposta<TC> resposta = new GlResposta<TC>();
            DalSchool<TC> daAcesso = new DalSchool<TC>();
            try
            {
                
                TC objeto = daAcesso.Obter(filtro);
                if (objeto != null)
                {
                    List<TC> lista = new List<TC>();
                    lista.Add(objeto);
                    resposta.Dados = lista;
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

        public GlResposta<TC> Read<TC>(Guid id) where TC : class
        {
            GlResposta<TC> resposta = new GlResposta<TC>();
            DalSchool<TC> daAcesso = new DalSchool<TC>();
            try
            {
                resposta = ValidarAutenticacaoPermissao<TC>();
                if (!resposta.Succeeded)
                {
                    return resposta;
                }
                var mdlObjeto = daAcesso.Obter(id);

                if (mdlObjeto != null)
                {
                    resposta.Dados = new List<TC>();
                    resposta.Dados.Add(mdlObjeto);
                }
                if (!IsAnonimo)
                {
                    DadosGravacao.IdAcao = BllAcao.IdAcaoDetalhar;
                    BrHistorico.MontarHistorico(DadosGravacao, "Obter()", DadosGravacao.IdAcao, id.ToString());
                    daAcesso.InserirHistorico(DadosGravacao);
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

        public GlResposta<T> ReadAll()
        {
            GlResposta<T> resposta = new GlResposta<T>();
            DalSchool<T> daAcesso = new DalSchool<T>();
            try
            {
                resposta = ValidarAutenticacaoPermissao();
                if (!resposta.Succeeded)
                {
                    return resposta;
                }
                List<T> lista = daAcesso.ObterTodos()?.ToList();
                if (!IsAnonimo)
                {
                    DadosGravacao.IdAcao = BllAcao.IdAcaoAcessar;
                    BrHistorico.MontarHistorico(DadosGravacao, "ObterTodos()", DadosGravacao.IdAcao);
                    daAcesso.InserirHistorico(DadosGravacao);
                }
                resposta.Dados = lista;
                resposta.Succeeded = true;
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        public GlResposta<TC> ReadAll<TC>() where TC : class
        {
            GlResposta<TC> resposta = new GlResposta<TC>();
            DalSchool<TC> daAcesso = new DalSchool<TC>();
            try
            {
                resposta = ValidarAutenticacaoPermissao<TC>();
                if (!resposta.Succeeded)
                {
                    return resposta;
                }
                List<TC> lista = daAcesso.ObterTodos()?.ToList();
                if (!IsAnonimo)
                {
                    DadosGravacao.IdAcao = BllAcao.IdAcaoAcessar;
                    BrHistorico.MontarHistorico(DadosGravacao, "ObterTodos()", DadosGravacao.IdAcao);
                    daAcesso.InserirHistorico(DadosGravacao);
                }
                resposta.Dados = lista;
                resposta.Succeeded = true;
            }
            catch (Exception excecao)
            {
                resposta.Succeeded = false;
                resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
            }

            return resposta;
        }

        //public GlResposta<TC> ReadAll<TC>(Expression<Func<TC, bool>> filtro) where TC : class
        //{
        //    GlResposta<TC> resposta = new GlResposta<TC>();
        //    DalSchool<TC> daAcesso = new DalSchool<TC>();
        //    try
        //    {
        //        List<TC> lista = daAcesso.ObterTodos(filtro)?.ToList();
        //        resposta.Dados = lista;
        //        resposta.Succeeded = true;
        //    }
        //    catch (Exception excecao)
        //    {
        //        resposta.Succeeded = false;
        //        resposta.Mensagem = new BllErro().Inserir(DadosGravacao, excecao);
        //    }

        //    return resposta;
        //}

        public GlResposta<T> Search(Expression<Func<T, bool>> filtro, Func<T, string> ordem, bool isAscendente)
        {
            GlResposta<T> resposta = new GlResposta<T>();
            DalSchool<T> daAcesso = new DalSchool<T>();
            try
            {
                resposta = ValidarAutenticacaoPermissao();
                if (!resposta.Succeeded)
                {
                    return resposta;
                }
                List<T> lista = daAcesso.ObterTodos(filtro, ordem, isAscendente);
                if (lista != null && lista.Count > 0)
                {
                    resposta.Dados = lista;
                }
                if (!IsAnonimo)
                {
                    DadosGravacao.IdAcao = BllAcao.IdAcaoProcurar;
                    BrHistorico.MontarHistorico(DadosGravacao, "Filtrar(" + filtro.ToString() + ")", DadosGravacao.IdAcao);
                    daAcesso.InserirHistorico(DadosGravacao);
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

        public GlResposta<T> Search(Expression<Func<T, bool>> filtro)
        {
            GlResposta<T> resposta = new GlResposta<T>();
            DalSchool<T> daAcesso = new DalSchool<T>();
            try
            {
                resposta = ValidarAutenticacaoPermissao();
                if (!resposta.Succeeded)
                {
                    return resposta;
                }
                List<T> lista = daAcesso.ObterTodos(filtro);
                if (lista != null && lista.Count > 0)
                {
                    resposta.Dados = lista;
                }
                if (!IsAnonimo)
                {
                    DadosGravacao.IdAcao = BllAcao.IdAcaoProcurar;
                    BrHistorico.MontarHistorico(DadosGravacao, "Filtrar(" + filtro.ToString() + ")", DadosGravacao.IdAcao);
                    daAcesso.InserirHistorico(DadosGravacao);
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
        
        public GlResposta<TC> Search<TC>(Expression<Func<TC, bool>> filtro) where TC : class
        {
            GlResposta<TC> resposta = new GlResposta<TC>();
            DalSchool<TC> daAcesso = new DalSchool<TC>();
            try
            {
                resposta = ValidarAutenticacaoPermissao<TC>();
                if (!resposta.Succeeded)
                {
                    return resposta;
                }
                List<TC> lista = daAcesso.ObterTodos(filtro);
                if (lista != null && lista.Count > 0)
                {
                    resposta.Dados = lista;
                }
                if (!IsAnonimo)
                {
                    DadosGravacao.IdAcao = BllAcao.IdAcaoProcurar;
                    BrHistorico.MontarHistorico(DadosGravacao, "Filtrar(" + filtro.ToString() + ")", DadosGravacao.IdAcao);
                    daAcesso.InserirHistorico(DadosGravacao);
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

        public GlResposta<TC> Search<TC>(Expression<Func<TC, bool>> filtro, Func<TC, string> ordem, bool isAscendente) where TC : class
        {
            GlResposta<TC> resposta = new GlResposta<TC>();
            DalSchool<TC> daAcesso = new DalSchool<TC>();
            try
            {
                resposta = ValidarAutenticacaoPermissao<TC>();
                if (!resposta.Succeeded)
                {
                    return resposta;
                }
                List<TC> lista = daAcesso.ObterTodos(filtro, ordem, isAscendente);
                if (lista != null && lista.Count > 0)
                {
                    resposta.Dados = lista;
                }
                if (!IsAnonimo)
                {
                    DadosGravacao.IdAcao = BllAcao.IdAcaoAcessar;
                    BrHistorico.MontarHistorico(DadosGravacao, "ObterTodos()", DadosGravacao.IdAcao);
                    daAcesso.InserirHistorico(DadosGravacao);
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
