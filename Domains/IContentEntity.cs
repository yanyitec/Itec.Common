using System;
using System.Collections.Generic;
using System.Text;

namespace Itec.Domains
{
    public interface IContentEntity:IRecordEntity
    {
        /// <summary>
        /// 标题
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        string Content { get; set; }
    }
}
