using Itec.Declaratives;
using Itec.ORM.DBs;
using Itec.ORM.Metas;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Itec.ORM.Utils
{
    public static class DbFieldBuilder
    {
        public static DbField Build(DbMetaProperty prop, JObject obj, DbField field=null)
        {
            
            if (prop.GetAttribute<NotDbFieldAttribute>() != null) return null;
            
            if (field == null) field = new DbField((prop.Class as DbMetaClass).DbTable, null);

            var fieldAttr = prop.GetAttribute<DbFieldAttribute>();
            JToken jValue = null;
            obj?.TryGetValue("FieldName", out jValue);
            if (jValue != null && jValue.Type != JTokenType.Undefined && jValue.Type != JTokenType.Null)
            {
                field.Name = jValue.ToString().Trim();
            }
            if (string.IsNullOrEmpty(field.Name))
            {
                //if (prop.GetAttribute<NotDbFieldAttribute>() != null) return null;
                field.Name = (fieldAttr?.Fieldname ?? prop.Name).Trim();
            }
            #region type
            jValue = null;
            var propType = prop.NonullableType;
            var dbType = GetDbTypeFromType(propType);

            field.DbType = dbType;
            if (dbType == DbType.String)
            {
                obj?.TryGetValue("DbType", out jValue);
                if (jValue != null && jValue.Type != JTokenType.Undefined && jValue.Type != JTokenType.Null)
                {
                    System.Data.DbType dbt = DbType.Binary;
                    if (Enum.TryParse<System.Data.DbType>(jValue.ToString(), out dbt))
                    {
                        field.DbType = dbt;
                    }
                }
            }
            if (propType.IsEnum)
            {
                if (propType.GetCustomAttributes(false).Any(p => p.GetType() == typeof(FlagsAttribute)))
                {
                    field.DbType = System.Data.DbType.Int32;
                }
                else
                {

                    field.DbType = System.Data.DbType.AnsiStringFixedLength;
                }
            }
            #endregion
            #region length

            if (propType.IsEnum && field.DbType != DbType.Int32)
            {
                var names = Enum.GetNames(propType);
                var max = 0;
                foreach (var n in names)
                {
                    if (n.Length > max) max = n.Length;
                }
                field.Length = ((max / 8) + 1) * 8;
            }
            else if (propType == typeof(Guid))
            {
                field.Length = 64;
            }

            var lengthAttr = prop.GetAttribute<LengthAttribute>();
            if (lengthAttr != null)
            {
                field.Length = lengthAttr.Max;
            }
            else
            {

                obj?.TryGetValue("Length", out jValue);
                if (jValue != null && jValue.Type != JTokenType.Null && jValue.Type != JTokenType.Undefined)
                {
                    var maxMinJVal = jValue as JObject;
                    if (maxMinJVal != null)
                    {
                        if (maxMinJVal.TryGetValue("Max", out jValue))
                        {
                            int len = 0;
                            if (jValue != null)
                            {
                                int.TryParse(jValue.ToString(), out len);
                            }
                            if (len > field.Length) field.Length = len;
                        }
                    }
                    else
                    {
                        int len = 0;
                        if (jValue != null)
                        {
                            int.TryParse(jValue.ToString(), out len);
                        }
                        if (len > field.Length) field.Length = len;
                    }
                }
            }

            #endregion

            #region nullable
            field.Nullable = (prop.PropertyType.IsByRef);
            if (prop.Nullable) field.Nullable = true;
            else if (prop.PropertyType == typeof(string))
            {
                if (fieldAttr != null) field.Nullable = fieldAttr.Nullable;
                else field.Nullable = true;
            }
            #endregion

            #region Precision
            if (prop.NonullableType == typeof(Decimal))
            {
                var preAttr = prop.GetAttribute<PrecisionAttribute>();
                if (preAttr != null)
                {
                    var len = preAttr.Integer + preAttr.Scale;
                    field.Length = len;
                    field.Precision = preAttr.Scale;
                }
            }
            #endregion

            return field;
        }

        public static DbType GetDbTypeFromType(Type type)
        {
            if (type == typeof(byte))
            {
                return System.Data.DbType.SByte;
            }


            if (type == typeof(short))
            {
                return System.Data.DbType.Int16;
            }

            if (type == typeof(ushort))
            {
                return System.Data.DbType.UInt16;
            }

            if (type == typeof(bool))
            {
                return System.Data.DbType.Boolean;
            }

            if (type == typeof(int))
            {
                return System.Data.DbType.Int32;
            }

            if (type == typeof(uint))
            {
                return System.Data.DbType.UInt32;
            }

            if (type == typeof(long))
            {
                return System.Data.DbType.Int64;
            }

            if (type == typeof(ulong))
            {
                return System.Data.DbType.UInt64;
            }

            if (type == typeof(float))
            {
                return System.Data.DbType.Single;
            }
            if (type == typeof(double))
            {
                return System.Data.DbType.Double;
            }

            if (type == typeof(decimal))
            {
                return System.Data.DbType.Decimal;
            }

            if (type == typeof(DateTime))
            {
                return System.Data.DbType.DateTime;
            }

            if (type == typeof(Guid))
            {
                return System.Data.DbType.Guid;
            }
            if (type == typeof(char[]))
            {
                return System.Data.DbType.StringFixedLength;
            }
            if (type == typeof(byte[]))
            {
                return System.Data.DbType.AnsiStringFixedLength;
            }

            return System.Data.DbType.String;
        }
    }
}
