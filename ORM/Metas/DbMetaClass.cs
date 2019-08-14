using Itec.Metas;
using Itec.ORM.DBs;
using Itec.ORM.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Itec.ORM.Metas
{
    public class DbMetaClass:MetaClass
    {
        public DbMetaClass(Type type,DbTable mappedTable, DbMetaFactory factory) : base(type, factory) {
            this.DbTable = mappedTable;
        }

        public DbTable DbTable { get; private set; }

        protected override IMetaProperty CreateProperty(MemberInfo memberInfo)
        {
            var prop = new DbMetaProperty(memberInfo,this);
            this.SetField(prop);
            return prop;
        }

        void SetField(DbMetaProperty prop) {

            var field = this.DbTable[prop.Name];
            prop.DbField = DbFieldBuilder.Build(prop, null, field);
            if (prop.DbField != null) this._FieldedProps.Add(prop.Name,prop);
        }

        Dictionary<string, DbMetaProperty> _FieldedProps;

        /// <summary>
        /// 有数据库映射的属性集合
        /// Key = 属性名
        /// Value = 属性元数据实例
        /// </summary>
        public IReadOnlyDictionary<string, DbMetaProperty> FieldedProps
        {
            get
            {

                return _FieldedProps;
            }
        }
    }
}
