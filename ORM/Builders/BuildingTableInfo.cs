using Itec.Metas;
using Itec.Queriables;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Itec.ORM.Builders
{
    public class BuildingTableInfo
    {
        public string Tablename { get; set; }

        public string Fieldnames { get; set; }
        

        

        

        
        Dictionary<string, BuildingReferenceInfo> _References;
        public IDictionary<string, BuildingReferenceInfo> References
        {
            get
            {
                if (_References == null) _References = new Dictionary<string, BuildingReferenceInfo>();
                return this._References;
            }
        }
    }
}
