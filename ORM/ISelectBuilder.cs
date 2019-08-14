using Itec.Queriables;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Itec.ORM
{
    public interface ISelectBuilder<T>:IQuery<T>
    {
        /// <summary>
        /// 指定要查询的表名，默认为Class名称
        /// </summary>
        /// <param name="tablename">指定的表名</param>
        /// <param name="usePrefix">是否使用前缀</param>
        /// <returns></returns>
        ISelectBuilder<T> Tablename(string tablename, bool usePrefix = true);

        // <summary>
        /// 指定要查询的表名，默认为Class名称
        /// </summary>
        /// <param name="tablename">指定的表名</param>
        /// <param name="usePrefix">是否使用前缀</param>
        /// <returns></returns>
        ISelectBuilder<T> From(string tablename, bool usePrefix = true);

        /// <summary>
        /// 指定要查询的字段
        /// 默认为全部字段
        /// </summary>
        /// <param name="fieldnames">字段名字符串</param>
        /// <returns></returns>
        ISelectBuilder<T> Fieldnames(string fieldnames);
        /// <summary>
        /// 指定查询条件
        /// </summary>
        /// <param name="criteria">查询条件</param>
        /// <returns></returns>
        ISelectBuilder<T> Where(Expression<Func<T, bool>> criteria);

        /// <summary>
        /// 指定升序排列字段
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>

        ISelectBuilder<T> Asc(Expression<Func<T, object>> prop);

        /// <summary>
        /// 指定降序排列字段
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>

        ISelectBuilder<T> Desc(Expression<Func<T, object>> prop);

        ISelectBuilder<T> Take(int count);

        ISelectBuilder<T> Skip(int count);



        ISelectBuilder<T> WithOne<TForeign>(Expression<Func<T, TForeign>> propExpr, WithOptions<T, TForeign> withOpts = null);

        ISelectBuilder<T> WithMany<TForeign>(Expression<Func<T, IEnumerable<TForeign>>> propExpr, WithOptions<T, TForeign> withOpts = null);

        //IExecuteContextBuilder<T> WithOne(Expression<Func<T, object>> propExpr, string foreignKey, string primaryKey = null);


    }
}
