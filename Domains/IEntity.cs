using System;
using System.Collections.Generic;
using System.Text;

namespace Itec.Domains
{
    public interface IEntity
    {
        /// <summary>
        /// 主键，唯一Id
        /// </summary>
        Guid Id { get; set; }
    }

}
