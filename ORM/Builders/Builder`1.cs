using Itec.Metas;
using System;
using System.Collections.Generic;
using System.Text;

namespace Itec.ORM.Builders
{
    public class Builder<T>:Builder
    {
        public Builder(IORMContext context):base(context)
        {
            this.MetaClass = MetaFactory.Default.GetClass<T>();
            this.RootTable = new BuildingRootTableInfo(this.MetaClass);
            
        }
        public IMetaClass<T> MetaClass { get; private set; }

        
    }
}
