using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Itec.ORM
{
    public interface IUpdateContext<T>
    {
        IUpdateContext<T> Where(Expression<Func<T, bool>> criteria);

        IUpdateContext<T> Set(T data,string fieldnames=null);

        
    }
}
