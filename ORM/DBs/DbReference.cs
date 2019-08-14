using System;
using System.Collections.Generic;
using System.Text;

namespace Itec.ORM.DBs
{
    /// <summary>
    /// 数据库表间关系
    /// </summary>
    public class DbReference
    {
        public DbReference(DbField primary, DbField foreign, ReferenceTypes type) {
            this.Primary = primary;
            this.Foreign = foreign;
            this.Type = type;
        }

        public DbReference(DbField primary, DbField foreign, string interName)
        {
            this.Primary = primary;
            this.Foreign = foreign;
            this.Type = ReferenceTypes.ManyToMany;
            this.InterName = interName;
        }
        public DbField Primary { get; private set; }

        public DbField Foreign { get; private set; }

        public string InterName { get; private set; }

        public ReferenceTypes Type { get; private set; }
    }
}
