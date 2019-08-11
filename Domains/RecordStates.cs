using System;
using System.Collections.Generic;
using System.Text;

namespace Itec.Domains
{
    public enum RecordStates
    {
        /// <summary>
        /// 草稿状态，不是正式版本，
        /// </summary>
        Draft,
        /// <summary>
        /// 新建状态
        /// </summary>
        New,
        /// <summary>
        /// 修订状态
        /// </summary>
        Modifying,
        /// <summary>
        /// 冻结状态，当该记录处于审批中时，使用该值
        /// </summary>
        Frozen,
        /// <summary>
        /// 已经确认，一般情况下不可再修改
        /// </summary>
        Comfirmed,
        /// <summary>
        /// 失效状态，还可以被查询出
        /// </summary>
        Invalid,
        /// <summary>
        /// 删除状态
        /// </summary>
        Deleted
    }
}
