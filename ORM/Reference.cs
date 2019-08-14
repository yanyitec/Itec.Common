using System;
using System.Collections.Generic;
using System.Text;

namespace Itec.ORM
{
    public class Reference<TPrimary,TForeign>
    {
        public string Fieldnames { get; set; }

        public string ForeignKey { get; set; }

        public string PrimaryKey { get; set; }

        public ReferenceTypes Type { get; set; }


    }
}
