using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Itec.Domains
{
    public interface ITreeNodeEntity:ICodedEntity
    {
        /// <summary>
        /// 路径，包含了所有的Parent信息
        /// 可以用like('%a/b/c/')查询出所有的子节点
        /// 也可以用 StartWith('a/b/c/',path)查询出所有的父节点
        /// </summary>
        string Path { get; set; }
        /// <summary>
        /// 父节点Id
        /// </summary>

        Guid ParentId { get; set; }
        /// <summary>
        /// 父节点
        /// </summary>

        ITreeNodeEntity Parent { get; set; }
        /// <summary>
        /// 子节点
        /// </summary>

        IList<ITreeNodeEntity> Children { get; set; }

        
    }
}
