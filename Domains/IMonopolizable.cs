using System;
using System.Collections.Generic;
using System.Text;

namespace Itec.Domains
{
    /// <summary>
    /// 可独占的实体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMonopolizable:IEntity
    {
        
        /// <summary>
        /// 独占者Id
        /// </summary>
        Guid? MonopolizerId { get; set; }
        /// <summary>
        /// 独占者名称
        /// </summary>

        string MonopolizerName { get; set; }

        /// <summary>
        /// 独占者序列化成JSON后的字符串
        /// </summary>

        string MonopolizerJSON { get; set; }

        void Monopolize(IUser user);

        void Unmonopolize(IUser user, string reason);

        /// <summary>
        /// 独占开始时间
        /// </summary>

        DateTime? MonopolizeTime { get; set; }
        /// <summary>
        /// 独占过期时间，过了这个时间，独占失效
        /// </summary>

        DateTime? MonopolizeExpireTime { get; set; }

        /// <summary>
        /// 释放独占的理由
        /// </summary>
        string MonopolizeReleaseReason { get; set; }
    }
}
