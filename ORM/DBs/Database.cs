using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Itec.ORM.DBs
{
    /// <summary>
    /// 对应一个连接字符串，必然对应某个数据库实例
    /// </summary>
    public class Database
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString { get; private set; }

        /// <summary>
        /// 表前缀
        /// 当多个系统共用同一个数据库实例时，用前缀来区分
        /// </summary>
        public string TablePrefix { get; private set; }

        Dictionary<string, DbTable> _Tables;

        /// <summary>
        /// 可以用索引方式获取到字段信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>

        public DbTable this[string name]
        {
            get
            {
                DbTable tb = null;
                this._Tables.TryGetValue(name, out tb);
                return tb;
            }
            internal set
            {
                this._Tables[name] = value;
            }
        }
    }
}
