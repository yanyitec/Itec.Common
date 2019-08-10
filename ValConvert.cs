using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Itec
{
    public static class ValConvert
    {
        
        
        public static Noneable<T> ConvertTo<T>(this string input) {
            var parser = GetParser<T>();
            return parser(input);
        }

        public static object ConvertTo(this string input,Type type)
        {
            var parser = GetParser(type);
            return parser(input);
        }



        public static Func<string,Noneable<T>> GetParser<T>()  {
            object valTypeInfo = null;
            Type t = typeof(T);
            if (!Parsers.TryGetValue(t.GetHashCode(), out valTypeInfo)) {
                if (!t.IsEnum) return null;
                valTypeInfo = DynamicParsers.GetOrAdd(t.GetHashCode(),(id)=> {
                     return MakeEnumParser<T>() as Func<string,Noneable<T>>;
                });
            }
            return valTypeInfo as Func<string, Noneable<T>>;
        }

        public static Func<string, object> GetParser(Type type) {
            object valTypeInfo = null;
            if (type == typeof(string)) return (t) => t;
            
            if (!ObjectParsers.TryGetValue(type.GetHashCode(), out valTypeInfo))
            {
                if (!type.IsEnum) return null;
                valTypeInfo = DynamicObjectParsers.GetOrAdd(type.GetHashCode(), (id) => {
                    return MakeEnumParser<object>() as Func<string,object>;
                });
            }
            return valTypeInfo as Func<string,object>;
        }

        

        static System.Text.RegularExpressions.Regex NumberRegex = new System.Text.RegularExpressions.Regex("\\s*\\d+\\s*");
        static MethodInfo IsNullOrWhiteSpaceMethod = typeof(string).GetMethod("IsNullOrWhiteSpace");
        static MethodInfo IsMatchMethod = typeof(Regex).GetMethod("IsMatch", new Type[] { typeof(string) });
        static MethodInfo IntParseMethod = typeof(int).GetMethod("Parse",new Type[] { typeof(string)});
        //static Noneable<T> ParseEnum
        static int GetEnumValue(Dictionary<string ,int> dic, string key) {
            int ret = 0;
            return dic.TryGetValue(key,out ret)?ret:int.MaxValue;
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="valueType"></param>
        /// <param name="noneable"></param>
        /// <returns>
        /// Func<string, Noneable<T>>
        /// Func<string,object>
        /// </returns>
        static object MakeEnumParser<T>() {
            Type valueType = typeof(T);
            bool noneable = valueType != typeof(object);
            var txtExpr = Expression.Parameter(typeof(string),"txt");
            //Expression.Invoke()
            var noneableType = typeof(Noneable<>).MakeGenericType(valueType);
            Expression noneExpr = Expression.New(noneableType);
            Expression nullExpr = Expression.Constant(null,typeof(object));
            var codes =new List<Expression>();
            var locals = new List<ParameterExpression>();
            var retTarget = Expression.Label(noneable?noneableType:typeof(object));
            //if (string.IsNullOrWhiteSpace(t)) return null;
            codes.Add(Expression.IfThen(
                //IF (string.IsNullOrWhiteSpace(t))
                Expression.Call(IsNullOrWhiteSpaceMethod, txtExpr)
                //THEN return null
                , Expression.Return(retTarget, noneable?noneExpr:nullExpr)
            ));
            //if (NumberRegex.IsMatch(t)) {return new Nullable<T>(int.Parse(txt)) }
            codes.Add(Expression.IfThen(
                //IF (NumberRegex.IsMatch(t))
                Expression.Call(Expression.Constant(NumberRegex), IsMatchMethod, txtExpr)
                //THEN return
                , Expression.Return(retTarget
                    //NEW
                    , noneable?
                        (Expression)Expression.New(
                            //Nullable<T>.ctor(T v)
                            typeof(Noneable<T>).GetConstructor(new Type[] { typeof(T) })
                            //(T) int.Parse(t)
                            , Expression.Convert(Expression.Call(IntParseMethod, txtExpr), typeof(T))
                        ) 
                        : (Expression)Expression.Convert(Expression.Call(IntParseMethod, txtExpr), typeof(object))
                )
            ));

            var names = Enum.GetNames(valueType);
            var values = Enum.GetValues(valueType);
            var dict = new Dictionary<string, int>();
            for (var i = 0; i < names.Length; i++) {
                dict.Add(names[i],(int)values.GetValue(i));
            }
            var mbs = new Func<Dictionary<string,int>,string,int>(GetEnumValue);
            var getEnumValueExpr = Expression.Invoke(Expression.Constant(mbs),Expression.Constant(dict), txtExpr);
            var nValueExpr = Expression.Parameter(typeof(int),"nval");
            locals.Add(nValueExpr);
            codes.Add(Expression.Assign(nValueExpr,getEnumValueExpr));
            var retValueExpr = Expression.Condition(
                Expression.Equal(nValueExpr, Expression.Constant(int.MaxValue))
                , noneable? Expression.New(noneableType):nullExpr
                , noneable?(Expression) Expression.New(noneableType.GetConstructor(new Type[] { valueType}),Expression.Convert(nValueExpr,noneableType)): Expression.Convert(nValueExpr, typeof(object))
            );
            codes.Add(Expression.Return(retTarget,retValueExpr));
            codes.Add(Expression.Label(retTarget,noneable?Expression.New(noneableType):nullExpr));
            var block = Expression.Block(locals,codes);//typeof(Nullable<T>),
            var lamda = noneable?(LambdaExpression)Expression.Lambda<Func<string, Noneable<T>>>(block,txtExpr): Expression.Lambda<Func<string, object>>(block, txtExpr);
            return lamda.Compile();

            
        }

        

        static bool IsNumber(string txt)
        {
            return NumberRegex.IsMatch(txt);
        }

        static ConcurrentDictionary<int, object> DynamicParsers = new ConcurrentDictionary<int, object>();

        static ConcurrentDictionary<int, object> DynamicObjectParsers = new ConcurrentDictionary<int, object>();

        static Dictionary<int, object> Parsers = new Dictionary<int, object>() {
            { typeof(byte).GetHashCode(),new Func<string, Noneable<byte>>((t)=>{
                byte rs = 0;return byte.TryParse(t,out rs)?new Noneable<byte>(rs):new Noneable<byte>();
            })}
            ,{ typeof(short).GetHashCode(),new Func<string, Noneable<short>>((t)=>{
                short rs = 0;return short.TryParse(t,out rs)?new Noneable<short>(rs):new Noneable<short>();
            })}
            ,{ typeof(ushort).GetHashCode(),new Func<string, Noneable<ushort>>((t)=>{
                ushort rs = 0;return ushort.TryParse(t,out rs)?new Noneable<ushort>(rs):new Noneable<ushort>();
            })}
            ,{ typeof(bool).GetHashCode(),new Func<string, Noneable<bool>>((t)=>{
                if(string.IsNullOrWhiteSpace(t)) return new Noneable<bool>(false);
                if(IsNumber(t)) {
                    var n = int.Parse(t);
                    if(n!=0) return new Noneable<bool>(true);
                    else return new Noneable<bool>(false);
                }
                if(t=="false"||t=="off") return new Noneable<bool>(false);
                return new Noneable<bool>(true);
            })}
            ,{ typeof(int).GetHashCode(),new Func<string, Noneable<int>>((t)=>{
                int v;
                return int.TryParse(t,out v)?new Noneable<int>(v):new Noneable<int>();
            })}
            ,{ typeof(uint).GetHashCode(),new Func<string, Noneable<uint>>((t)=>{
                uint v;
                return uint.TryParse(t,out v)?new Noneable<uint>(v):new Noneable<uint>();
            })}
            ,{ typeof(long).GetHashCode(),new Func<string, Noneable<long>>((t)=>{
                long v;
                return long.TryParse(t,out v)?new Noneable<long>(v):new Noneable<long>();
            })}
            ,{ typeof(ulong).GetHashCode(),new Func<string, Noneable<ulong>>((t)=>{
                ulong v;
                return ulong.TryParse(t,out v)?new Noneable<ulong>(v):new Noneable<ulong>();
            })}
            ,{ typeof(float).GetHashCode(),new Func<string, Noneable<float>>((t)=>{
                float v;
                return float.TryParse(t,out v)?new Noneable<float>(v):new Noneable<float>();
            })}
            ,{ typeof(double).GetHashCode(),new Func<string, Noneable<double>>((t)=>{
                double v;
                return double.TryParse(t,out v)?new Noneable<double>(v):new Noneable<double>();
            })}
            ,{ typeof(decimal).GetHashCode(),new Func<string, Noneable<decimal>>((t)=>{
                decimal v;
                return decimal.TryParse(t,out v)?new Noneable<decimal>(v):new Noneable<decimal>();
            })}
            ,{ typeof(DateTime).GetHashCode(),new Func<string, Noneable<DateTime>>((t)=>{
                if(string.IsNullOrWhiteSpace(t)) return new Noneable<DateTime>();
                t = t.Replace("T"," ");
                DateTime v;
                return DateTime.TryParse(t,out v)?new Noneable<DateTime>(v):new Noneable<DateTime>();
            })}
            ,{ typeof(Guid).GetHashCode(),new Func<string, Noneable<Guid>>((t)=>{
                if(string.IsNullOrWhiteSpace(t)) return new Noneable<Guid>();
                Guid v;
                return Guid.TryParse(t,out v)?new Noneable<Guid>(v):new Noneable<Guid>();
            })}
        };


        static Dictionary<int, object> ObjectParsers = new Dictionary<int, object>() {
            { typeof(byte).GetHashCode(),new Func<string, object>((t)=>{
                byte rs = 0;return byte.TryParse(t,out rs)?(object)rs:null;
            })}
            ,{ typeof(short).GetHashCode(),new Func<string, object>((t)=>{
                short rs = 0;return short.TryParse(t,out rs)?(object)rs:null;
            })}
            ,{ typeof(ushort).GetHashCode(),new Func<string, object>((t)=>{
                ushort rs = 0;return ushort.TryParse(t,out rs)?(object)rs:null;
            })}
            ,{ typeof(bool).GetHashCode(),new Func<string, object>((t)=>{
                if(string.IsNullOrWhiteSpace(t)) return null;
                if(IsNumber(t)) {
                    var n = int.Parse(t);
                    if(n!=0) return (object)(true);
                    else return (object)(false);
                }
                if(t=="false"||t=="off") return (object)(false);
                return (object)(true);
            })}
            ,{ typeof(int).GetHashCode(),new Func<string,object>((t)=>{
                int rs;
                return int.TryParse(t,out rs)?(object)rs:null;
            })}
            ,{ typeof(uint).GetHashCode(),new Func<string, object>((t)=>{
                uint rs;
                return uint.TryParse(t,out rs)?(object)rs:null;
            })}
            ,{ typeof(long).GetHashCode(),new Func<string, object>((t)=>{
                long rs;
                return long.TryParse(t,out rs)?(object)rs:null;
            })}
            ,{ typeof(ulong).GetHashCode(),new Func<string, object>((t)=>{
                ulong rs;
                return ulong.TryParse(t,out rs)?(object)rs:null;
            })}
            ,{ typeof(float).GetHashCode(),new Func<string, object>((t)=>{
                float rs;
                return float.TryParse(t,out rs)?(object)rs:null;
            })}
            ,{ typeof(double).GetHashCode(),new Func<string, object>((t)=>{
                double rs;
                return double.TryParse(t,out rs)?(object)rs:null;
            })}
            ,{ typeof(decimal).GetHashCode(),new Func<string, object>((t)=>{
                decimal rs;
                return decimal.TryParse(t,out rs)?(object)rs:null;
            })}
            ,{ typeof(DateTime).GetHashCode(),new Func<string, object>((t)=>{
                if(string.IsNullOrWhiteSpace(t)) return new Noneable<DateTime>();
                t = t.Replace("T"," ");
                DateTime rs;
                return DateTime.TryParse(t,out rs)?(object)rs:null;
            })}
            ,{ typeof(Guid).GetHashCode(),new Func<string,object>((t)=>{
                if(string.IsNullOrWhiteSpace(t)) return null;
                Guid rs;
                return Guid.TryParse(t,out rs)?(object)rs:null;
            })}
        };
    }
}
