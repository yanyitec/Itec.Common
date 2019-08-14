using Itec.Metas;
using Itec.Queriables;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Itec.ORM.Builders
{
    public class BuildingRootTableInfo: BuildingJoinableTableInfo
    {
        public BuildingRootTableInfo(IMetaClass cls) :base(cls) {

        }
        

        public Expression FilterExpression { get; set; }

        public Expression AscendingExpression { get; set; }

        public Expression DescendingExpression { get; set; }

        public int TakeCount{get;set;}

        public int SkipCount { get; set; }

        

    }
}
