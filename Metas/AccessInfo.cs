using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Itec.Metas
{
    public class AccessInfo
    {
        public AccessInfo(string pathtext,MetaClass metaClass) {
            this.Pathtext = pathtext;
            var paramExpr = Expression.Parameter(typeof(object), "par");
            this.GetterExpression =Expression.Lambda<Func<object,object>>( CreateGetterExpression(pathtext,metaClass,paramExpr),paramExpr);
            this.GetValue = this.GetterExpression.Compile();
            this.SetValue = CreateSetter(pathtext, metaClass);
        }
        public string Pathtext {
            get;private set;
        }
        public MetaClass MetaClass { get; private set; }

        public Expression<Func<object, object>> GetterExpression { get; private set; }

        public Func<object, object> GetValue { get; private set; }

        public Action<object, object> SetValue { get; private set; }

        


        static Expression CreateGetterExpression(string pathtext,MetaClass cls,ParameterExpression paramExpr)
        {
            //var paramExpr = Expression.Parameter(typeof(object), "par");

            var rootExpr = Expression.Convert(paramExpr, cls.Type);

            Expression expr = rootExpr;
            if (string.IsNullOrWhiteSpace(pathtext)) return Expression.Lambda<Func<object, object>>(Expression.Convert(expr, typeof(object)), paramExpr);

            var paths = pathtext.Split(".");
            for (var i = 0; i < paths.Length; i++)
            {

                var path = paths[i].Trim();
                if (path == string.Empty) throw new InvalidOperationException(pathtext + " 不是正确的。");
                var metaProp = cls[path];
                if (metaProp == null) throw new InvalidOperationException(pathtext + " 无法找到.");
                expr = Expression.PropertyOrField(expr, metaProp.Name);
                if(i!=paths.Length-1)cls = cls.Factory.GetClass(metaProp.PropertyType);
            }
            return expr;
        }
        static MethodInfo MetaPropertySetValueMethodInfo = typeof(IMetaClass).GetMethod("SetValue", new Type[] { typeof(object), typeof(object) });
        static Action<object,object> CreateSetter(string pathtext, MetaClass cls)
        {
            var paramExpr = Expression.Parameter(typeof(object), "par");
            var valueExpr = Expression.Parameter(typeof(object),"value");
            var rootExpr = Expression.Convert(paramExpr, cls.Type);


            Expression expr = rootExpr;
            if (string.IsNullOrWhiteSpace(pathtext)) return Expression.Lambda<Action<object, object>>(expr, paramExpr,valueExpr).Compile();

            var paths = pathtext.Split(".");
            for (var i = 0; i < paths.Length; i++) {
                
                var path = paths[i].Trim();
                if (path == string.Empty) throw new InvalidOperationException(pathtext + " 不是正确的。");
                var metaProp = cls[path];
                if (metaProp == null) throw new InvalidOperationException(pathtext + " 无法找到.");
                if (i == paths.Length - 1)
                {
                    expr = Expression.Call(Expression.Constant(metaProp), MetaPropertySetValueMethodInfo, expr,valueExpr);
                }
                else {
                    expr = Expression.PropertyOrField(expr, metaProp.Name);
                    cls = cls.Factory.GetClass(metaProp.PropertyType);
                }
                
            }

            
            
            return Expression.Lambda<Action<object,object>>(expr,paramExpr,valueExpr).Compile();
        }

    }
}
