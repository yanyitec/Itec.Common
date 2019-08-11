using System;
using System.Collections.Generic;
using System.Text;

namespace Itec.Domains
{
    public class HistoricEntity:RecordEntity,IHistoricEntity
    {
        /// <summary>
        /// 历史唯一Id,多个Id会对应一个UniqueId
        /// 一般是Comfirmed后，就会生成历史唯一
        /// </summary>
        public Guid UniqueId { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public uint Version { get; set; }

        /// <summary>
        /// 变更原因
        /// </summary>
        public string ChangeReason { get; set; }

        /// <summary>
        /// 该字段用于记录变更详情
        /// </summary>
        public string ChangeDetails { get; set; }
    }
}
