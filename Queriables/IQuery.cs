using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Itec.Queriables
{
    public interface IQuery<T>
    {
        /// <summary>
        /// 正向排序表达式
        /// </summary>
        Expression<Func<T, object>> AscendingExpression { get; set; }
        /// <summary>
        /// 反向排序表达式
        /// </summary>
        Expression<Func<T, object>> DescendingExpression { get; set; }

        /// <summary>
        /// 过滤表达式
        /// </summary>
        Expression<Func<T, bool>> FilterExpression { get; set; }

        //ParameterExpression QueryParameter { get; }

        /// <summary>
        /// 跳过若干个记录
        /// </summary>
        int SkipCount { get; set; }
        /// <summary>
        /// 只取若干条记录
        /// </summary>
        int TakeCount { get; set; }

        

        //IList<T> ToList();
    }
}