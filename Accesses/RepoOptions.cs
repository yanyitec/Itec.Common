using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Itec.Accesses
{
    public class RepoOptions
    {
        /// <summary>
        /// 允许访问的字段
        /// </summary>
        public string AllowedFields { get; set; }

        
        public ITransaction DbTrans { get; set; }
    }
}
