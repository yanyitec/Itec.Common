using Itec.Domains;
using Itec.Queriables;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Itec.Accesses
{
    public interface IRepository<T>
        where T:IEntity
    {
        #region add
        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="opts"></param>
        /// <returns></returns>
        bool Add(T entity, RepoOptions opts);

        /// <summary>
        /// 异步新添实体
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="opts"></param>
        /// <returns></returns>
        Task<bool> AddAsync(T entity,RepoOptions opts);

        /// <summary>
        /// 添加多个实体
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="opts"></param>
        /// <returns>新添的实体个数</returns>
        int AddRange(IEnumerable<T> entities, RepoOptions opts = null);

        /// <summary>
        /// 异步添加多个实体
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="opts"></param>
        /// <returns></returns>
        Task<int> AddRangeAsync(IEnumerable<T> entities, RepoOptions opts = null);

        #endregion

        #region get 
        T GetById(Guid id,RepoOptions opts=null);

        Task<T> GetByIdAsync(Guid id, RepoOptions opts = null);

        T Get(IQuery<T> query, RepoOptions opts = null);

        Task<T> GetAsync(IQuery<T> query, RepoOptions opts = null);

        #endregion

        #region remove
        bool RemoveById(Guid id,RepoOptions opts=null);
        Task<bool> RemoveByIdAsync(Guid id, RepoOptions opts = null);

        bool Remove(T entity, RepoOptions opts = null);
        Task<bool> RemoveAsync(T entity, RepoOptions opts = null);

        int RemoveRange(IEnumerable<Guid> id, RepoOptions opts = null);
        Task<int> RemoveRangeAsync(IEnumerable<Guid> id, RepoOptions opts = null);

        int RemoveRange(IEnumerable<T> entities, RepoOptions opts = null);
        Task<int> RemoveRangeAsync(IEnumerable<T> entities, RepoOptions opts = null);


        int Remove(IQuery<T> query, RepoOptions opts = null);

        Task<int> RemoveAsync(IQuery<T> query, RepoOptions opts = null);

        #endregion

        #region update & Modify
        bool Save(T entity, RepoOptions opts = null);
        Task<bool> SaveAsync(T entity ,RepoOptions opts = null);

        int SaveRange(IEnumerable<T> entites , RepoOptions opts=null);

        Task<int> SaveRangeAsync(IEnumerable<T> entites, RepoOptions opts = null);

        int Update(IQuery<T> query, T data, RepoOptions opts = null);

        Task<int> UpdateAsync(IQuery<T> query, T data, RepoOptions opts = null);
        #endregion

        #region List

        IPagination<T> List(IQuery<T> query,RepoOptions opts = null);

        IPagination<T> ListAsync(IQuery<T> query, RepoOptions opts = null);
        #endregion

    }
}
