using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Itec.ORM.DBs
{
    /// <summary>
    /// 代表数据库中的表
    /// </summary>
    public class DbTable
    {
        /// <summary>
        /// 数据库实例
        /// </summary>
        public Database Database { get; private set; }

        /// <summary>
        /// 程序里写的表名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 数据库里的真实表名
        /// </summary>
        public string TableName { get; private set; }

        public DbType DbType { get; private set; }

        Dictionary<string, DbField> _Fields;

        /// <summary>
        /// 可以用索引方式获取到字段信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>

        public DbField this[string name]
        {
            get
            {
                DbField field = null;
                this._Fields.TryGetValue(name, out field);
                return field;
            }
            internal set
            {
                this._Fields[name] = value;
            }
        }

        public ReferenceTypes Type { get; private set; }
    }
}
