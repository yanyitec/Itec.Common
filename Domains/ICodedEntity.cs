using System;
using System.Collections.Generic;
using System.Text;

namespace Itec.Domains
{
    public interface ICodedEntity: IEntity
    {
        /// <summary>
        /// 编号，Guid太大，而且不好记忆，有些实体需要一个短小的编号
        /// </summary>
        string Code { get; set; }
    }
}
