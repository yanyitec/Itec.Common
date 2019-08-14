using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Itec.ORM.DBs
{
    public class DbField
    {
        public DbField(DbTable tb, string name, DbType type, bool nullable,int length, int precision) {
            this.DbTable = tb;
            this.Name = name;
            this.DbType = type;
            this.Length = length;
            this.Precision = precision;

        }

        internal DbField(DbTable tb, string name) {
            this.DbTable = tb;
            this.Name = name;
        }

        public string Name { get; internal set; }

        public DbType DbType { get; internal set; }

        public int? Length { get; internal set; }

        public int? Precision { get; internal set; }

        public bool Nullable { get; internal set; }

        /// <summary>
        /// 数据库里的真实字段
        /// 和Name完全一摸一样，只是Name的别名。
        /// 字段名称是没有前缀的，所以Name跟FieldName是一样的
        /// 设置FieldName是为了跟DbTable保持一致
        /// </summary>
        public string FieldName { get { return this.Name; } internal set { this.Name = value; } }

        public DbTable DbTable { get; internal set; }

        List<DbReference> _References;

    }
}
