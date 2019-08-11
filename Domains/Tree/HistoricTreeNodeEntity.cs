using System;
using System.Collections.Generic;
using System.Text;

namespace Itec.Domains.Tree
{
    public class HistoricTreeNodeEntity:HistoricEntity,ITreeNodeEntity,INamedEntity
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 描述
        /// </summary>

        public string Discription { get; set; }
        /// <summary>
        /// 一个简短的编号
        /// </summary>

        public string Code { get; set; }
        /// <summary>
        /// 路径，包含了所有的Parent信息
        /// 可以用like('%a/b/c/')查询出所有的子节点
        /// 也可以用 StartWith('a/b/c/',path)查询出所有的父节点
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 父节点Id
        /// </summary>

        public Guid ParentId { get; set; }
        /// <summary>
        /// 父节点
        /// </summary>

        public ITreeNodeEntity Parent { get; set; }
        /// <summary>
        /// 子节点
        /// </summary>

        public IList<ITreeNodeEntity> Children { get; set; }
    }
}
