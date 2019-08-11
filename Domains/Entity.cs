using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Itec.Domains
{
    public class Entity:IEntity
    {
        /// <summary>
        /// 主键，唯一Id
        /// </summary>
        public Guid Id { get; set; }
    }
}
