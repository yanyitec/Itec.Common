using System;
using System.Collections.Generic;
using System.Text;
using Itec.Metas;
using Itec.ORM.DBs;

namespace Itec.ORM.Metas
{
    public class DbMetaFactory :MetaFactory
    {
        public Database Database { get; private set; }
    }
}
