using System;
using System.Collections.Generic;
using System.Text;

namespace Itec.Domains
{
    public interface ICodedEntity: IEntity
    {
        string Code { get; set; }
    }
}
