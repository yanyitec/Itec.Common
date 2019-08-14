using Itec.Metas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text;

namespace Itec.ORM.Builders
{
    public class SelectBuilder<T>: Builder<T>,ISelectBuilder<T>
    {
        public SelectBuilder(IORMContext context) : base(context) {
        }

        #region Queriable
        public Expression<Func<T, object>> AscendingExpression
        {
            get { return this.RootTable.AscendingExpression as Expression<Func<T,object>>; }
            set { this.RootTable.AscendingExpression = value; }
        }
        public Expression<Func<T, object>> DescendingExpression
        {
            get { return this.RootTable.DescendingExpression as Expression<Func<T, object>>; }
            set { this.RootTable.DescendingExpression = value; }
        }

        public Expression<Func<T, bool>> FilterExpression
        {
            get { return this.RootTable.FilterExpression as Expression<Func<T, bool>>; }
            set { this.RootTable.FilterExpression = value; }
        }
        public int SkipCount
        {
            get { return this.RootTable.SkipCount; }
            set { this.RootTable.SkipCount = value; }
        }
        public int TakeCount
        {
            get { return this.RootTable.TakeCount ; }
            set { this.RootTable.TakeCount = value; }
        }

        #endregion
        /// <summary>
        /// 指定要查询的表名，默认为Class名称
        /// </summary>
        /// <param name="tablename">指定的表名</param>
        /// <param name="usePrefix">是否使用前缀</param>
        /// <returns></returns>

        public ISelectBuilder<T> From(string tablename, bool usePrefix = true)
        {
            return this.Tablename(tablename,usePrefix);
        }
        /// <summary>
        /// 指定要查询的表名，默认为Class名称
        /// </summary>
        /// <param name="tablename">指定的表名</param>
        /// <param name="usePrefix">是否使用前缀</param>
        /// <returns></returns>
        public ISelectBuilder<T> Tablename(string tablename, bool usePrefix = true) {
            this.RootTable.Tablename = tablename;
            return this;
        }

        /// <summary>
        /// 指定要查询的字段
        /// 默认为全部字段
        /// </summary>
        /// <param name="fieldnames">字段名字符串</param>
        /// <returns></returns>
        public ISelectBuilder<T> Fieldnames(string fieldnames) {
            this.RootTable.Fieldnames = fieldnames;
            return this;
        }

        /// <summary>
        /// 指定查询条件
        /// </summary>
        /// <param name="criteria">查询条件</param>
        /// <returns></returns>
        public ISelectBuilder<T> Where(Expression<Func<T, bool>> criteria)
        {
            this.RootTable.FilterExpression = criteria;
            return this;
        }
        /// <summary>
        /// 指定升序排列字段
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>

        public ISelectBuilder<T> Asc(Expression<Func<T, object>> prop)
        {
            this.RootTable.AscendingExpression = prop;
            return this;
        }

        /// <summary>
        /// 指定降序排列字段
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        public ISelectBuilder<T> Desc(Expression<Func<T, object>> prop)
        {
            this.RootTable.DescendingExpression = prop;
            return this;
        }

        public ISelectBuilder<T> Take(int count)
        {
            this.RootTable.TakeCount = count;
            return this;
        }

        public ISelectBuilder<T> Skip(int count)
        {
            
            this.RootTable.SkipCount = count;
            return this;
        }

        public ISelectBuilder<T> WithOne<TForeign>(Expression<Func<T, TForeign>> propExpr, WithOptions<T, TForeign> withOpts = null)
        {
            this.RootTable.WithOne(propExpr,withOpts);
            return this;
        }

        public ISelectBuilder<T> WithMany<TForeign>(Expression<Func<T, IEnumerable<TForeign>>> propExpr, WithOptions<T, TForeign> withOpts = null)
        {
            this.RootTable.WithMany(propExpr, withOpts);
            return this;
        }
    }
}
