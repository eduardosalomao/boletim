using Acesso.Nucleo.Geral;
using Acesso.SchoolUp.Contexts;
using Microsoft.EntityFrameworkCore;
using Modelo.Nucleo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Acesso.SchoolUp.Comum
{
    public class DalSchool<TEntity> where TEntity : class
    {
        #region "Comum"

        public SchoolContext contextoSchool { get; set; } = new SchoolContext();

        public void SalvarEInserirHistorico(Historico registro)
        {
            try
            {
                registro.IdHistorico = Guid.NewGuid().ToString();
                new DalGenerica<Historico>().InserirHistorico(registro);
                Save();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                LiberarMemoria();
            }
        }

        public void SalvarEInserirHistorico(SchoolContext contexto, Historico registro)
        {
            try
            {
                registro.IdHistorico = Guid.NewGuid().ToString();
                new DalGenerica<Historico>().InserirHistorico(registro);
                Save(contexto);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                LiberarMemoria(contexto);
            }
        }

        public void InserirHistorico(Historico registro)
        {
            new DalGenerica<Historico>().InserirHistorico(registro);
        }

        public void LiberarMemoria()
        {
            contextoSchool.Dispose();
            contextoSchool = null;
            GC.Collect(0, GCCollectionMode.Forced);
        }

        public void LiberarMemoria(SchoolContext SchoolContexto)
        {
            SchoolContexto.Dispose();
            SchoolContexto = null;
            //GC.Collect(0, GCCollectionMode.Forced);
        }

        public void Save(SchoolContext SchoolContexto)
        {
            SchoolContexto.SaveChanges();
        }

        public void Save()
        {
            contextoSchool.SaveChanges();
        }
        #endregion

        #region "Excluir"
        /// <summary>
        /// Exclui imediatamente um objeto
        /// </summary>
        /// <param name="registro"></param>
        /// <param name="objeto"></param>
        /// <returns></returns>
        public bool Excluir(Historico registro, TEntity objeto)
        {
            try
            {
                contextoSchool.Set<TEntity>().Remove(objeto);
                SalvarEInserirHistorico(registro);

                return true;
            }
            catch (Exception excecao)
            {
                throw new Exception("ErroBD", excecao);
            }
        }

        /// <summary>
        /// Exclui imediatamente um objeto pelo Id
        /// </summary>
        /// <param name="registro"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Excluir(Historico registro, Guid id)
        {
            try
            {
                contextoSchool.Set<TEntity>().Remove(contextoSchool.Set<TEntity>().Find(id));
                SalvarEInserirHistorico(registro);

                return true;
            }
            catch (Exception excecao)
            {
                throw new Exception("ErroBD", excecao);
            }
        }


        //public bool Excluir(List<Historico> listaHistorico, List<Guid> listaIds)
        //{
        //    try
        //    {
        //        foreach (Guid id in listaIds)
        //        {
        //            contextoPadrao.Set<TEntity>().Remove(contextoPadrao.Set<TEntity>().Find(id));
        //        }
        //        SalvarEInserirHistorico(listaHistorico);

        //        return true;
        //    }
        //    catch (Exception excecao)
        //    {
        //        throw new Exception("ErroBD", excecao);
        //    }
        //}

        /// <summary>
        /// Exclui imediatamente de acordo com a condição
        /// </summary>
        /// <param name="registro"></param>
        /// <param name="filtro"></param>
        /// <returns></returns>
        public bool Excluir(Historico registro, Expression<Func<TEntity, bool>> filtro)
        {
            try
            {
                contextoSchool.Set<TEntity>().RemoveRange(contextoSchool.Set<TEntity>().Where(filtro));
                SalvarEInserirHistorico(registro);
                return true;
            }
            catch (Exception excecao)
            {
                throw new Exception("ErroBD", excecao);
            }
        }

        /// <summary>
        /// Exclui um objeto em uma transação
        /// </summary>
        /// <param name="contexto"></param>
        /// <param name="objeto"></param>
        /// <returns></returns>
        public bool Excluir(SchoolContext contexto, TEntity objeto)
        {
            try
            {
                contexto.Set<TEntity>().Remove(objeto);
                return true;
            }
            catch (Exception excecao)
            {
                throw new Exception("ErroBD", excecao);
            }
        }

        /// <summary>
        /// Exclui um objeto em uma transação de acrodo com o filtro
        /// </summary>
        /// <param name="contexto"></param>
        /// <param name="registro"></param>
        /// <param name="filtro"></param>
        /// <returns></returns>
        public bool Excluir(SchoolContext contexto, Expression<Func<TEntity, bool>> filtro)
        {
            try
            {
                contexto.Set<TEntity>().RemoveRange(contexto.Set<TEntity>().Where(filtro));
                return true;
            }
            catch (Exception excecao)
            {
                throw new Exception("ErroBD", excecao);
            }
        }

        /// <summary>
        /// Excluir em massa em uma transação
        /// </summary>
        /// <param name="contexto"></param>
        /// <param name="listaIds"></param>
        /// <returns></returns>
        public bool Excluir(SchoolContext contexto, List<Guid> listaIds)
        {
            try
            {
                foreach (Guid id in listaIds)
                {
                    contexto.Set<TEntity>().Remove(contexto.Set<TEntity>().Find(id));
                }
                return true;
            }
            catch (Exception excecao)
            {
                throw new Exception("ErroBD", excecao);
            }
        }
        #endregion

        #region "Alterar"
        /// <summary>
        /// Usado para alteração imediata de dados
        /// </summary>
        /// <param name="registro"></param>
        /// <param name="objeto"></param>
        /// <returns></returns>
        public TEntity Alterar(Historico registro, TEntity objeto)
        {
            try
            {
                contextoSchool.Entry(objeto).State = EntityState.Modified;
                SalvarEInserirHistorico(registro);

                return objeto;
            }
            catch (Exception excecao)
            {
                throw new Exception("ErroBD", excecao);
            }
        }

        /// <summary>
        /// Utilizado para alteração de dados em uma transação
        /// </summary>
        /// <param name="contexto"></param>
        /// <param name="objeto"></param>
        /// <returns></returns>
        public TEntity Alterar(SchoolContext contexto, TEntity objeto)
        {
            try
            {
                contexto.Entry(objeto).State = EntityState.Modified;
                return objeto;
            }
            catch (Exception excecao)
            {
                throw new Exception("ErroBD", excecao);
            }
        }
        #endregion

        #region "Inserir"
        /// <summary>
        /// Usado para inserção imediata de dados
        /// </summary>
        /// <param name="registro"></param>
        /// <param name="objeto"></param>
        /// <returns></returns>
        public TEntity Inserir(Historico registro, TEntity objeto)
        {
            try
            {
                TEntity objetoRetorno = default(TEntity);
                objetoRetorno = contextoSchool.Set<TEntity>().Add(objeto).Entity;
                SalvarEInserirHistorico(registro);

                return objeto;
            }
            catch (Exception excecao)
            {
                throw new Exception("ErroBD", excecao);
            }
        }

        /// <summary>
        /// Utilizado para inserção de dados em uma transação
        /// </summary>
        /// <param name="contexto"></param>
        /// <param name="objeto"></param>
        /// <returns></returns>
        public TEntity Inserir(SchoolContext contexto, TEntity objeto)
        {
            try
            {
                TEntity objetoRetorno = default(TEntity);
                objetoRetorno = contexto.Set<TEntity>().Add(objeto).Entity;

                return objeto;
            }
            catch (Exception excecao)
            {
                throw new Exception("ErroBD", excecao);
            }
        }
        #endregion

        #region "Busca por lista"
        /// <summary>
        /// Obter todos os registros de um objeto sem restrições
        /// </summary>
        /// <returns></returns>
        public List<TEntity> ObterTodos()
        {
            try
            {
                List<TEntity> list = default(List<TEntity>);
                list = contextoSchool.Set<TEntity>().ToList<TEntity>();
                LiberarMemoria();

                return list;
            }
            catch (Exception excecao)
            {
                throw new Exception("ErroBD", excecao);
            }
        }

        /// <summary>
        /// Método para bind de combos, listas e grids sem paginação
        /// </summary>
        /// <param name="filtro"></param>
        /// <param name="ordem"></param>
        /// <param name="isAscendente"></param>
        /// <returns></returns>
        public List<TEntity> ObterTodos(Expression<Func<TEntity, bool>> filtro, Func<TEntity, string> ordem, bool isAscendente)
        {
            try
            {
                List<TEntity> list = default(List<TEntity>);
                IEnumerable<TEntity> listaGenerica = default(IEnumerable<TEntity>);
                if ((filtro == null))
                {
                    listaGenerica = contextoSchool.Set<TEntity>().ToList();
                }
                else
                {
                    listaGenerica = contextoSchool.Set<TEntity>().Where(filtro).ToList();
                }
                if (isAscendente)
                {
                    list = listaGenerica.OrderBy(ordem).ToList();
                }
                else
                {
                    list = listaGenerica.OrderByDescending(ordem).ToList();
                }
                LiberarMemoria();

                return list;
            }
            catch (Exception excecao)
            {
                throw new Exception("ErroBD", excecao);
            }
        }

        /// <summary>
        /// Método para bind de combos, listas e grids sem paginação
        /// </summary>
        /// <param name="filtro"></param>
        /// <returns></returns>
        public List<TEntity> ObterTodos(Expression<Func<TEntity, bool>> filtro)
        {
            try
            {
                List<TEntity> list = default(List<TEntity>);
                if ((filtro == null))
                {
                    list = contextoSchool.Set<TEntity>().ToList();
                }
                else
                {
                    list = contextoSchool.Set<TEntity>().Where(filtro).ToList();
                }
                LiberarMemoria();

                return list;
            }
            catch (Exception excecao)
            {
                throw new Exception("ErroBD", excecao);
            }
        }

        /// <summary>
        /// Método para a contrução de grids sem paginação a partir de uma consulta do usuário
        /// </summary>
        /// <param name="registro"></param>
        /// <param name="filtro"></param>
        /// <param name="ordem"></param>
        /// <param name="isAscendente"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public List<TEntity> ObterTodos(Historico registro, Expression<Func<TEntity, bool>> filtro, Func<TEntity, string> ordem, bool isAscendente)
        {
            try
            {
                List<TEntity> list = default(List<TEntity>);
                IEnumerable<TEntity> listaGenerica = contextoSchool.Set<TEntity>().Where(filtro).ToList();
                if (isAscendente)
                {
                    list = listaGenerica.OrderBy(ordem).ToList();
                }
                else
                {
                    list = listaGenerica.OrderByDescending(ordem).ToList();
                }
                SalvarEInserirHistorico(registro);

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para a contrução de grids com paginação a partir de uma consulta do usuário
        /// </summary>
        /// <param name="registro"></param>
        /// <param name="filtro"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRegistros"></param>
        /// <param name="ordem"></param>
        /// <param name="isAscendente"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public List<TEntity> ObterTodos(Historico registro, Expression<Func<TEntity, bool>> filtro, int currentPage, int pageSize, ref int totalRegistros, Func<TEntity, string> ordem, bool isAscendente)
        {
            try
            {
                List<TEntity> list = default(List<TEntity>);
                IEnumerable<TEntity> listaGenerica = contextoSchool.Set<TEntity>().Where(filtro);
                currentPage = (currentPage - 1) * pageSize;
                totalRegistros = listaGenerica.Count();
                if (isAscendente)
                {
                    list = listaGenerica.OrderBy(ordem).Skip(currentPage).Take(pageSize).ToList();
                }
                else
                {
                    list = listaGenerica.OrderByDescending(ordem).Skip(currentPage).Take(pageSize).ToList();
                }
                SalvarEInserirHistorico(registro);

                return list;
            }
            catch (Exception excecao)
            {
                throw new Exception("ErroBD", excecao);
            }
        }
        #endregion

        #region "Busca de um único registro"
        public TEntity Obter(Expression<Func<TEntity, bool>> filtro)
        {
            try
            {
                var objeto = contextoSchool.Set<TEntity>().Where(filtro).FirstOrDefault();
                LiberarMemoria();

                return objeto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para carregar dados de um único objeto. Deve ser usado quando há ação do usuário, como selecionar uma linha em um grid.
        /// </summary>
        /// <param name="registro"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        //public TEntity Obter(Modelo.Padrao.Historico registro, Guid id)
        //{
        //    try
        //    {
        //        TEntity objeto = default(TEntity);
        //        MoodleEntities contexto = new MoodleEntities();
        //        objeto = contexto.Set<TEntity>().Find(id);
        //        SalvarEInserirHistorico(contexto, registro);

        //        return objeto;
        //    }
        //    catch (Exception excecao)
        //    {
        //        throw new Exception("ErroBD", excecao);
        //    }
        //}

        /// <summary>
        /// Método para carregar dados de um único objeto. Deve ser usado quando não há ação do usuário, ou seja, quando é chamado internamente por um método.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public TEntity Obter(Guid id)
        {
            try
            {
                var objeto = contextoSchool.Set<TEntity>().Find(id);

                LiberarMemoria();

                return objeto;
            }
            catch (Exception excecao)
            {
                throw new Exception("ErroBD", excecao);
            }
        }
        #endregion
    }
}
