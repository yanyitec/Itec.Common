using Itec.Metas;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Itec.ORM.Builders
{
    public class Builder
    {
        public Builder(IORMContext context)
        {
            this.Context = context;
            
        }
        public IORMContext Context { get; set; }

        public BuildingRootTableInfo RootTable { get; protected set; }

        public static string GetTablename(MetaClass cls) {
            var clsname = cls.Name;
            if (clsname.EndsWith("Entity")) clsname = clsname.Substring(0, clsname.Length - "Entity".Length);
            return clsname;
        }

       
    }
}
