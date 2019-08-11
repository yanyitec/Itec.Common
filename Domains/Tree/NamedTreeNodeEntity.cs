using System;
using System.Collections.Generic;
using System.Text;

namespace Itec.Domains.Tree
{
    public class NamedTreeNodeEntity:TreeNodeEntity,INamedEntity
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 描述
        /// </summary>

        public string Discription { get; set; }
    }
}
