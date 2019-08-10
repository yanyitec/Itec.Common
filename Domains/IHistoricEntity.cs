using System;
using System.Collections.Generic;
using System.Text;

namespace Itec.Domains
{
    /// <summary>
    /// 可以追溯历史版本的记录
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface HistoricEntity:IRecordEntity
    {
        /// <summary>
        /// 历史唯一Id,多个Id会对应一个UniqueId
        /// 一般是Comfirmed后，就会生成历史唯一
        /// </summary>
        Guid UniqueId { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        uint Version { get; set; }

        /// <summary>
        /// 变更原因
        /// </summary>
        string ChangeReason { get; set; }

        /// <summary>
        /// 该字段用于记录变更详情
        /// </summary>
        string ChangeDetails { get; set; }
    }
}
