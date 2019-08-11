using System;
using System.Collections.Generic;
using System.Text;

namespace Itec.Domains
{
    public interface INamedEntity:IEntity
    {
        /// <summary>
        /// 名称
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// 描述
        /// </summary>

        string Discription { get; set; }
    }

}
