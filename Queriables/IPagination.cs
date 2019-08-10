using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Itec.Queriables
{
    public interface IPagination<T> :IEnumerable<T>
    {
        IQuery<T> Query { get; }
        T this[int index] { get; }
        /// <summary>
        /// 当前记录数
        /// </summary>
        int Count { get; }
        /// <summary>
        /// 总记录数
        /// </summary>
        long TotalCount { get;  }
        /// <summary>
        /// 分页大小
        /// </summary>
        int PageSize { get; set; }
        /// <summary>
        /// 页码
        /// </summary>

        int PageIndex { get; set; }

        /// <summary>
        /// 页数
        /// </summary>

        int PageCount { get; }

        IList<T> ToList();
    }
}
