using Itec.Metas;
using Itec.ORM.DBs;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Itec.ORM.Metas
{
    public class DbMetaProperty:MetaProperty
    {
        public DbMetaProperty(MemberInfo memberInfo, IMetaClass cls = null):base(memberInfo,cls)
        {
            
        }

        
        public DbField DbField {
            get;
            internal set;
        }

        public DbReference DbReference {
            get;private set;
        }


    }
}
