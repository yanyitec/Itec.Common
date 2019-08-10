using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Itec.Domains
{
    public interface ITreeNodeEntity:ICodedEntity
    {
        ITreeNodeEntity Parent { get; set; }

        Guid ParentId { get; set; }

        string CodePath { get; set; }

        IList<ITreeNodeEntity> Children { get; set; }
    }
}
