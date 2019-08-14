using Itec.ORM.DBs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Itec.ORM
{
    public class ExecuteContext
    {
        public Database Database { get; private set; }

        public string Fieldnames { get; private set; }

        public string Tablename { get; private set; }

        public ExecuteContext From(string table, bool usePrefix = true)
        {

        }
    }
}
