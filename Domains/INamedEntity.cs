using System;
using System.Collections.Generic;
using System.Text;

namespace Itec.Domains
{
    public interface INamedEntity:IEntity
    {
        string Name { get; set; }

        string Discription { get; set; }
    }
}
