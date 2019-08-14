using Itec.Metas;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Itec.ORM.Builders
{
    public class BuildingReferenceInfo
    {
        
        public ReferenceTypes ReferenceType { get; set; }

        public IMetaProperty ReferenceProperty { get; set; }

        //public MetaProperty PrimaryProperty { get; set; }

        public string PrimaryKey { get; set; }

        //public MetaProperty ForeignProperty { get; set; }

        public string ForeignKey { get; set; }

        public BuildingTableInfo ReferenceTableInfo { get; set; }

        public string InterTable { get; set; }
    }
}
