using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Itec.Queriables
{
    public class PagingParameters<T>
    {
        public int PageIndex { get; set; }
        
        public int PageSize { get; set; }

        public string Asc { get; set; }

        public string Desc { get; set; }

        public IQuery<T> MakeQuery() {
            return null;
        }
    }
}
