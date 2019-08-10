using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Newtonsoft.Json.Schema;

namespace Itec.Queriables
{
    public static class IQueryExtensions
    {
        
        

        

        
        public static IQuery<T> Ascending<T>(this IQuery<T>  self,Expression<Func<T, object>> expr)
        {
            self.AscendingExpression = expr;
            return self;
        }

       
        public static IQuery<T> Descending<T>(this IQuery<T> self, Expression<Func<T, object>> expr)
        {
            self.DescendingExpression = expr;
            return self;
        }

        

        public static IQuery<T> Take<T>(this IQuery<T> self, int size) {
            self.TakeCount = size;
            return self;
        }
        
        public static IQuery<T> Skip<T>(this IQuery<T> self, int count)
        {
            self.SkipCount = count;
            return self ;
        }

        public static IQuery<T> Page<T>(this IQuery<T> self,  int index, int size = 10) {
            if (index <= 0) index = 1;
            if (size <= 2) size = 2;
            self.TakeCount = size;
            self.SkipCount = (index - 1) * size;
            return self;
        }



        


        public static Expression<Func<T,bool>> AndAlso<T>(this Expression<Func<T, bool>> self,Expression<Func<T, bool>> criteria)
        {
            if (criteria == null) return self;
            if (self == null)
            {
                return criteria;
            }
            else
            {
                //this._Expression = System.Linq.Expressions.Expression.AndAlso(this._Expression, criteria);
                return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(self.Body, ReplaceParameter<T>( criteria.Body, criteria.Parameters[0])), self.Parameters[0]);
            }
        }

        public static IQuery<T> AndAlso<T>(this IQuery<T> self, Expression<Func<T, bool>> criteria)
        {
            if (criteria == null) return self;
            if (self.FilterExpression == null) { self.FilterExpression = criteria; return self; }
            self.FilterExpression = Expression.Lambda<Func<T, bool>>(Expression.AndAlso(self.FilterExpression.Body, ReplaceParameter<T>(criteria.Body, criteria.Parameters[0])), self.FilterExpression.Parameters[0]);
            return self;
        }

        



        public static IQuery<T>  OrElse<T>(this IQuery<T> self, Expression<Func<T, bool>> criteria)
        {
            if (criteria == null) return self;
            if (self.FilterExpression == null) { self.FilterExpression = criteria; return self; }
            self.FilterExpression = Expression.Lambda<Func<T, bool>>(Expression.OrElse(self.FilterExpression.Body, ReplaceParameter<T>(criteria.Body, criteria.Parameters[0])), self.FilterExpression.Parameters[0]);
            return self;
        }

        

        public static Expression ReplaceParameter<T>( Expression expr, ParameterExpression param)
        {
            if (expr == param) return param;
            BinaryExpression bExpr = null;
            UnaryExpression uExpr = null;
            switch (expr.NodeType)
            {
                case ExpressionType.Lambda:
                    var lamda = (expr as LambdaExpression);
                    return ReplaceParameter<T>(lamda.Body,lamda.Parameters[0]);
                case ExpressionType.Constant:
                    return expr;
                case ExpressionType.And:
                    bExpr = expr as BinaryExpression;
                    return System.Linq.Expressions.Expression.And(ReplaceParameter<T>(bExpr.Left, param), ReplaceParameter<T>(bExpr.Right, param));
                case ExpressionType.Add:
                    bExpr = expr as BinaryExpression;
                    return System.Linq.Expressions.Expression.Add(ReplaceParameter<T>(bExpr.Left, param), ReplaceParameter<T>(bExpr.Right, param));
                case ExpressionType.AndAlso:
                    bExpr = expr as BinaryExpression;
                    return System.Linq.Expressions.Expression.OrElse( ReplaceParameter<T>(bExpr.Left, param), ReplaceParameter<T>(bExpr.Right, param));
                case ExpressionType.MemberAccess:
                    var member = expr as MemberExpression;
                    return System.Linq.Expressions.Expression.MakeMemberAccess( ReplaceParameter<T>(member.Expression,param),member.Member);
                case ExpressionType.Call:
                    var call = expr as MethodCallExpression;
                    var list = new List<Expression>();
                    foreach (var arg in call.Arguments)
                    {
                        list.Add( ReplaceParameter<T>(arg, param));
                    }
                    return System.Linq.Expressions.Expression.Call( ReplaceParameter<T>(call.Object, param), call.Method, list);
                case ExpressionType.Convert:
                    uExpr = expr as UnaryExpression;
                    return System.Linq.Expressions.Expression.Convert( ReplaceParameter<T>(uExpr.Operand, param), uExpr.Type);
                case ExpressionType.Divide:
                    bExpr = expr as BinaryExpression;
                    return System.Linq.Expressions.Expression.Divide( ReplaceParameter<T>(bExpr.Left, param), ReplaceParameter<T>(bExpr.Right, param));
                case ExpressionType.Equal:
                    bExpr = expr as BinaryExpression;
                    return System.Linq.Expressions.Expression.Equal( ReplaceParameter<T>(bExpr.Left, param), ReplaceParameter<T>(bExpr.Right, param));
                case ExpressionType.GreaterThan:
                    bExpr = expr as BinaryExpression;
                    return System.Linq.Expressions.Expression.GreaterThan( ReplaceParameter<T>(bExpr.Left, param), ReplaceParameter<T>(bExpr.Right, param));
                case ExpressionType.GreaterThanOrEqual:
                    bExpr = expr as BinaryExpression;
                    return System.Linq.Expressions.Expression.GreaterThanOrEqual( ReplaceParameter<T>(bExpr.Left, param), ReplaceParameter<T>(bExpr.Right, param));
                case ExpressionType.LessThan:
                    bExpr = expr as BinaryExpression;
                    return System.Linq.Expressions.Expression.LessThan( ReplaceParameter<T>(bExpr.Left, param), ReplaceParameter<T>(bExpr.Right, param));
                case ExpressionType.LessThanOrEqual:
                    bExpr = expr as BinaryExpression;
                    return System.Linq.Expressions.Expression.LessThan( ReplaceParameter<T>(bExpr.Left, param), ReplaceParameter<T>(bExpr.Right, param));
                case ExpressionType.LeftShift:
                    bExpr = expr as BinaryExpression;
                    return System.Linq.Expressions.Expression.LeftShift( ReplaceParameter<T>(bExpr.Left, param), ReplaceParameter<T>(bExpr.Right, param));
                case ExpressionType.Multiply:
                    bExpr = expr as BinaryExpression;
                    return System.Linq.Expressions.Expression.Multiply( ReplaceParameter<T>(bExpr.Left, param), ReplaceParameter<T>(bExpr.Right, param));
                case ExpressionType.Negate:
                    uExpr = expr as UnaryExpression;
                    return System.Linq.Expressions.Expression.Negate( ReplaceParameter<T>(uExpr.Operand, param));
                case ExpressionType.Not:
                    uExpr = expr as UnaryExpression;
                    return System.Linq.Expressions.Expression.Not( ReplaceParameter<T>(uExpr.Operand, param));
                case ExpressionType.NotEqual:
                    bExpr = expr as BinaryExpression;
                    return System.Linq.Expressions.Expression.NotEqual( ReplaceParameter<T>(bExpr.Left, param), ReplaceParameter<T>(bExpr.Right, param));
                case ExpressionType.Or:
                    bExpr = expr as BinaryExpression;
                    return System.Linq.Expressions.Expression.Or( ReplaceParameter<T>(bExpr.Left, param), ReplaceParameter<T>(bExpr.Right, param));
                case ExpressionType.OrElse:
                    bExpr = expr as BinaryExpression;
                    return System.Linq.Expressions.Expression.OrElse( ReplaceParameter<T>(bExpr.Left, param), ReplaceParameter<T>(bExpr.Right, param));
                case ExpressionType.Power:
                    bExpr = expr as BinaryExpression;
                    return System.Linq.Expressions.Expression.Power( ReplaceParameter<T>(bExpr.Left, param), ReplaceParameter<T>(bExpr.Right, param));
                case ExpressionType.RightShift:
                    bExpr = expr as BinaryExpression;
                    return System.Linq.Expressions.Expression.RightShift( ReplaceParameter<T>(bExpr.Left, param), ReplaceParameter<T>(bExpr.Right, param));
                case ExpressionType.Subtract:
                    bExpr = expr as BinaryExpression;
                    return System.Linq.Expressions.Expression.Subtract( ReplaceParameter<T>(bExpr.Left, param), ReplaceParameter<T>(bExpr.Right, param));

            }
            throw new NotSupportedException();
        }

        #region Equal
        public static IQuery<T> AndAlsoEqual<T>(this IQuery<T> self, Expression<Func<T, object>> propExpr,string text)
        {
            var expr = Equal<T>(propExpr, text);
            if (expr != null) AndAlso<T>(self, expr);
            return self;
        }

        public static IQuery<T> OrElseEqual<T>(this IQuery<T> self, Expression<Func<T, object>> propExpr, string text)
        {
            var expr = Equal<T>(propExpr, text);
            if (expr != null) OrElse<T>(self, expr);
            return self;
        }

        static Expression<Func<T,bool>> Equal<T>(Expression<Func<T, object>> propExpr, string value)
        {
            if (value == null || value == string.Empty) return null;
            var member = (propExpr.Body as MemberExpression);
            var objValue = ConvertValue(member,value);
            if (objValue == null) return null;
            return Expression.Lambda<Func<T,bool>>(Expression.Equal(propExpr.Body, Expression.Constant(objValue)), propExpr.Parameters[0]);

        }
        #endregion

        #region GreaterThan
        public static IQuery<T> AndAlsoGreaterThan<T>(this IQuery<T> self, Expression<Func<T, object>> propExpr, string text)
        {
            var expr = GreaterThan<T>(propExpr, text);
            if (expr != null) AndAlso<T>(self, expr);
            return self;
        }

        public static IQuery<T> OrElseGreaterThan<T>(this IQuery<T> self, Expression<Func<T, object>> propExpr, string text)
        {
            var expr = GreaterThan<T>(propExpr, text);
            if (expr != null) OrElse<T>(self, expr);
            return self;
        }

        static Expression<Func<T, bool>> GreaterThan<T>(Expression<Func<T, object>> propExpr, string value)
        {
            if (value == null || value == string.Empty) return null;
            var member = (propExpr.Body as MemberExpression);
            var objValue = ConvertValue(member, value);
            if (objValue == null) return null;
            return Expression.Lambda<Func<T, bool>>(Expression.GreaterThan(propExpr.Body, Expression.Constant(objValue)), propExpr.Parameters[0]);

        }
        #endregion

        #region GreaterThanOrEqual
        public static IQuery<T> AndAlsoGreaterThanOrEqual<T>(this IQuery<T> self, Expression<Func<T, object>> propExpr, string text)
        {
            var expr = GreaterThan<T>(propExpr, text);
            if (expr != null) AndAlso<T>(self, expr);
            return self;
        }

        public static IQuery<T> OrElseGreaterThanOrEqual<T>(this IQuery<T> self, Expression<Func<T, object>> propExpr, string text)
        {
            var expr = GreaterThanOrEqual<T>(propExpr, text);
            if (expr != null) OrElse<T>(self, expr);
            return self;
        }

        static Expression<Func<T, bool>> GreaterThanOrEqual<T>(Expression<Func<T, object>> propExpr, string value)
        {
            if (value == null || value == string.Empty) return null;
            var member = (propExpr.Body as MemberExpression);
            var objValue = ConvertValue(member, value);
            if (objValue == null) return null;
            return Expression.Lambda<Func<T, bool>>(Expression.GreaterThanOrEqual(propExpr.Body, Expression.Constant(objValue)), propExpr.Parameters[0]);

        }
        #endregion

        #region LessThan
        public static IQuery<T> AndAlsoLessThan<T>(this IQuery<T> self, Expression<Func<T, object>> propExpr, string text)
        {
            var expr = GreaterThan<T>(propExpr, text);
            if (expr != null) AndAlso<T>(self, expr);
            return self;
        }

        public static IQuery<T> OrElseLessThan<T>(this IQuery<T> self, Expression<Func<T, object>> propExpr, string text)
        {
            var expr = LessThan<T>(propExpr, text);
            if (expr != null) OrElse<T>(self, expr);
            return self;
        }

        static Expression<Func<T, bool>> LessThan<T>(Expression<Func<T, object>> propExpr, string value)
        {
            if (value == null || value == string.Empty) return null;
            var member = (propExpr.Body as MemberExpression);
            var objValue = ConvertValue(member, value);
            if (objValue == null) return null;
            return Expression.Lambda<Func<T, bool>>(Expression.LessThan(propExpr.Body, Expression.Constant(objValue)), propExpr.Parameters[0]);

        }
        #endregion

        #region LessThanOrEqual
        public static IQuery<T> AndAlsoLessThanOrEqual<T>(this IQuery<T> self, Expression<Func<T, object>> propExpr, string text)
        {
            var expr = GreaterThan<T>(propExpr, text);
            if (expr != null) AndAlso<T>(self, expr);
            return self;
        }

        public static IQuery<T> OrElseLessThanOrEqual<T>(this IQuery<T> self, Expression<Func<T, object>> propExpr, string text)
        {
            var expr = LessThanOrEqual<T>(propExpr, text);
            if (expr != null) OrElse<T>(self, expr);
            return self;
        }

        static Expression<Func<T, bool>> LessThanOrEqual<T>(Expression<Func<T, object>> propExpr, string value)
        {
            if (value == null || value == string.Empty) return null;
            var member = (propExpr.Body as MemberExpression);
            var objValue = ConvertValue(member, value);
            if (objValue == null) return null;
            return Expression.Lambda<Func<T, bool>>(Expression.LessThanOrEqual(propExpr.Body, Expression.Constant(objValue)), propExpr.Parameters[0]);

        }
        #endregion

        #region Like
        public static IQuery<T> AndAlsoLike<T>(this IQuery<T> self, Expression<Func<T, object>> propExpr, string text)
        {
            var expr = GreaterThan<T>(propExpr, text);
            if (expr != null) AndAlso<T>(self, expr);
            return self;
        }

        public static IQuery<T> OrElseLike<T>(this IQuery<T> self, Expression<Func<T, object>> propExpr, string text)
        {
            var expr = Like<T>(propExpr, text);
            if (expr != null) OrElse<T>(self, expr);
            return self;
        }

        readonly static MethodInfo StringContainsMethodInfo = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });

        static Expression<Func<T, bool>> Like<T>(Expression<Func<T, object>> propExpr, string value)
        {
            if (value == null || value == string.Empty) return null;


            var callExpr = Expression.Call(propExpr.Body, StringContainsMethodInfo, Expression.Constant(value));
            return Expression.Lambda<Func<T, bool>>(callExpr, propExpr.Parameters[0]);

        }
        #endregion


        #region Range
        public static IQuery<T> AndAlsoRange<T>(this IQuery<T> self, Expression<Func<T, object>> propExpr, string min,string max)
        {
            var expr = Range<T>(propExpr, min,max);
            if (expr != null) AndAlso<T>(self, expr);
            return self;
        }

        public static IQuery<T> OrElseRange<T>(this IQuery<T> self, Expression<Func<T, object>> propExpr, string min, string max)
        {
            var expr = Range<T>(propExpr, min,max);
            if (expr != null) OrElse<T>(self, expr);
            return self;
        }

       
        static Expression<Func<T, bool>> Range<T>(Expression<Func<T, object>> propExpr, string minValue,string maxValue)
        {
            var member = (propExpr.Body as MemberExpression);
            var objMinValue = ConvertValue(member, minValue);
            var objMaxValue = ConvertValue(member, maxValue);
            Expression expr = null;
            if (objMinValue != null)
            {
                expr = Expression.GreaterThanOrEqual(propExpr.Body, Expression.Constant(objMinValue));
                if (objMaxValue != null) {
                    var maxExpr = Expression.LessThanOrEqual(propExpr.Body,Expression.Constant(objMaxValue));
                    expr = Expression.AndAlso(expr, maxExpr);
                }
                
            }
            else if (objMaxValue != null) {
                expr = Expression.GreaterThanOrEqual(propExpr.Body, Expression.Constant(objMaxValue));
            }
            return Expression.Lambda<Func<T, bool>>(expr, propExpr.Parameters[0]); ;
        }
        #endregion

        static object ConvertValue(MemberExpression member, string value) {
            Type memberType = null;

            var propMember = member.Member as PropertyInfo;
            if (propMember != null) memberType = propMember.PropertyType;
            else
            {
                var fieldMember = member.Member as FieldInfo;
                if (fieldMember != null) memberType = fieldMember.FieldType;
            }
            if (memberType == null) return null;
            return value.ConvertTo(memberType);
        }
    }
}