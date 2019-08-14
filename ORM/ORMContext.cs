using Itec.ORM.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Itec.ORM
{
    /// <summary>
    /// 关系映射上下文，
    /// 由这个对象关联Database/table/field/reference与 Entity/Property/Ref 的关系
    /// </summary>
    public class ORMContext : IORMContext
    {
        public string Prefix { get; private set; }

        public string ConnectionString { get; private set; }

        public ISelectBuilder<T> Select<T>(string fieldnames=null) {
            return new SelectBuilder<T>(this).Fieldnames(fieldnames);
        }
    }
}
