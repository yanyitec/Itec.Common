using System;
using System.Collections.Generic;
using System.Text;

namespace Itec
{
    public interface IUser:IToJson
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// 用户名
        /// </summary>

        string Name { get; }

        /// <summary>
        /// 显示名
        /// </summary>
        string DisplayName { get; }

        //string ClaimJSON { get; }
    }
}
