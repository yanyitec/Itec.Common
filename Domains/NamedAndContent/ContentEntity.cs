using System;
using System.Collections.Generic;
using System.Text;

namespace Itec.Domains
{
    public class ContentEntity:RecordEntity,IContentEntity
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
    }
}
