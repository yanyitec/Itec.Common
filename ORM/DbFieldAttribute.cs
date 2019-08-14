using System;
using System.Collections.Generic;
using System.Text;

namespace Itec.ORM
{
    [AttributeUsage( AttributeTargets.Field| AttributeTargets.Property)]
    public class DbFieldAttribute:Attribute
    {
        public DbFieldAttribute(string fieldname = null,bool nullable=true) {
            this.Fieldname = fieldname;
            this.Nullable = nullable;
        }

        public DbFieldAttribute(bool nullable)
        {
            this.Nullable = nullable;
        }

        public string Fieldname { get; private set; }

        public bool Nullable { get; private set; }
    }
}
