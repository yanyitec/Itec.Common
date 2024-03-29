﻿
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Itec.Metas
{
    public class MetaProperty : IMetaProperty
    {
        public MetaProperty(MemberInfo memberInfo, IMetaClass cls =null) {
            this.MemberInfo = memberInfo;
            this.Class = cls;
        }

        public IMetaClass Class { get; private set; }
        
        public MemberInfo MemberInfo { get;private set; }
        public string Name { get { return this.MemberInfo.Name; } }
        Type _PropertyType;
        public Type PropertyType {
            get {
                if (_PropertyType == null) {
                    lock (this) {
                        if (_PropertyType == null) {
                            MakePropertyType();
                        }
                            
                    }
                }
                return _PropertyType;
            }
        }
        void MakePropertyType() {

            if (MemberInfo.MemberType == MemberTypes.Property)
            {
                _PropertyType = (this.MemberInfo as PropertyInfo).PropertyType;
            }
            else if (MemberInfo.MemberType == MemberTypes.Field)
            {
                _PropertyType = (this.MemberInfo as FieldInfo).FieldType;
            }

            _nullable = this.PropertyType.FullName.StartsWith("System.Nullable`1");
            if (_nullable == false) _NonullableType = _PropertyType;
            else _NonullableType = _PropertyType.GetGenericArguments()[0];
            _nullChecker = CheckNull.GetChecker(_PropertyType);

            if (this._PropertyType.IsAssignableFrom(typeof(IEnumerable))) {
                foreach (var ifc in this._PropertyType.GetInterfaces()) {
                    if (ifc.Name.StartsWith("System.IEnumerable"))
                    {
                        this._IsEnumerable = true;
                    }
                    else if (ifc.Name.StartsWith("System.IEnumerable<"))
                    {
                        this._EnumerableItemType = ifc.GetGenericArguments()[0];
                    }
                    else if (ifc.Name.StartsWith("System.IDictionary<")) {
                        this._IsDictionary = true;
                        var kvts = ifc.GetGenericArguments()[0].GetGenericArguments();
                        this._DictionaryKeyType = kvts[0];
                        this._DictionaryValueType = kvts[1];
                    }
                }
            }
        }
        bool? _nullable;
        public bool Nullable {
            get {
                if (_nullable == null) {
                    lock (this) {
                        if (_nullable == null) {
                            MakePropertyType();
                        }
                    }
                }
                return _nullable.Value;
            }
        }
        Type _NonullableType;
        public Type NonullableType {
            get
            {
                if (_NonullableType == null)
                {
                    lock (this)
                    {
                        if (_NonullableType == null) {
                            MakePropertyType();
                        }
                        
                    }
                }
                return _NonullableType;
            }
        }
        bool? _IsEnumerable;
        public bool IsEnumerable
        {
            get
            {
                if (_IsEnumerable == null)
                {
                    lock (this)
                    {
                        if (_IsEnumerable == null)
                        {
                            MakePropertyType();
                        }

                    }
                }
                return _IsEnumerable.Value;
            }
        }

        Type _EnumerableItemType;
        
        public Type EnumerableItemType
        {
            get
            {
                if (_EnumerableItemType == null)
                {
                    lock (this)
                    {
                        if (_EnumerableItemType == null) {
                            MakePropertyType();
                        }
                        
                    }
                }
                return _EnumerableItemType == typeof(void)?null:this._EnumerableItemType;
            }
        }
        bool? _IsDictionary;
        public bool IsDictionary
        {
            get
            {
                if (_IsDictionary == null)
                {
                    lock (this)
                    {
                        if (_IsDictionary == null)
                        {
                            MakePropertyType();
                        }

                    }
                }
                return _IsDictionary.Value;
            }
        }

        Type _DictionaryKeyType;

        public Type DictionaryKeyType
        {
            get
            {
                if (_DictionaryKeyType == null)
                {
                    lock (this)
                    {
                        if (_DictionaryKeyType == null)
                        {
                            MakePropertyType();
                        }

                    }
                }
                return _DictionaryKeyType == typeof(void) ? null : this._DictionaryKeyType;
            }
        }

        Type _DictionaryValueType;

        public Type DictionaryValueType
        {
            get
            {
                if (_DictionaryValueType == null)
                {
                    lock (this)
                    {
                        if (_DictionaryValueType == null)
                        {
                            MakePropertyType();
                        }

                    }
                }
                return _DictionaryValueType == typeof(void) ? null : this._DictionaryValueType;
            }
        }

        object _defaultValue;
        public object DefaultValue {
            get {
                if (_defaultValue == null)
                {
                    lock (this)
                    {
                        if (_defaultValue == null)
                        {
                            var t = typeof(Noneable<>).MakeGenericType(this.PropertyType);
                            var none = Activator.CreateInstance(t);
                            _defaultValue = t.GetProperty("DefaultValue").GetValue(none);
                            if (_defaultValue == null) _defaultValue = Nothing.Value;
                        }
                    }
                }
                return _defaultValue == Nothing.Value?null:_defaultValue;
            }
        }

        Func<object, bool> _nullChecker;

        public bool HasValue(object instance) {
            
            return !_nullChecker(this.GetValue(instance));

        }

        Func<object, object> _EnsureValue;

        public object EnsureValue(object instance) {
            if (_EnsureValue == null)
            {
                lock (this)
                {
                    if (_EnsureValue == null)
                    {
                        if (this.Nullable)
                        {

                            var valExpr = Expression.Parameter(typeof(object), "val");
                            var expr = Expression.Call(Expression.Convert(valExpr, this.PropertyType), this.PropertyType.GetMethod("GetValueOrDefault"));
                            var lamda = Expression.Lambda<Func<object, object>>(Expression.Convert(expr,typeof(object)), valExpr);
                            _EnsureValue = lamda.Compile();
                        }
                        else if (this.PropertyType == typeof(string)) {
                            _EnsureValue = (val) => val==null? string.Empty:val.ToString();
                        }
                        else
                        {
                            _EnsureValue = (val) => val;
                        }
                    }
                }
            }
            return _EnsureValue(this.GetValue(instance));
        }

        List<Attribute> _Attributes;
        public IReadOnlyList<Attribute> Attributes {
            get {
                if (_Attributes == null) {
                    lock (this) {
                        if (_Attributes == null) {
                            _Attributes = new List<Attribute>(this.MemberInfo.GetCustomAttributes());
                        }
                    }
                }
                return _Attributes;
            }
        }

        public T GetAttribute<T>() where T:Attribute {
            return this.Attributes.FirstOrDefault(p=>p.GetType()==typeof(T)) as T;
        }

        //Dictionary<string, IValidation> _Validations;

        //protected IDictionary<string, IValidation> Validations {
        //    get {
        //        if (_Validations == null) {
        //            lock (this) {
        //                if (_Validations == null) {
        //                    _Validations = new Dictionary<string, IValidation>();
        //                    var attrs = this.MemberInfo.GetCustomAttributes();
        //                    foreach (Attribute attr in attrs) {
        //                        var valid = attr as IValidation;
        //                        if (valid != null) _Validations.TryAdd(valid.Name, valid);
        //                    }
        //                }
        //            }
        //        }
        //        return _Validations;
        //    }
        //}

        //public ValidationResult Validate(object target,ValidateOptions opts = ValidateOptions.Undefined) {
        //    var value = this.GetValue(target);
        //    foreach (var valid in this.Validations.Values) {
        //        if (valid.Name == "Required")
        //        {
        //            if (opts.HasFlag(ValidateOptions.IgnoreRequire)) continue;
        //            if (value == null || (opts.HasFlag(ValidateOptions.DefaultAsNull) && value.Equals(this.DefaultValue)))
        //                return new ValidationResult(valid,"Required");
        //        }
        //        var errorCode = valid.Check(value);
        //        if (!string.IsNullOrEmpty(errorCode)) return new ValidationResult(valid,errorCode);
        //    }
        //    return null;
        //}

        //public ValidationResult ValidateValue(object value, ValidateOptions opts = ValidateOptions.Undefined)
        //{ 
        //    foreach (var valid in this.Validations.Values)
        //    {
        //        if (valid.Name == "Required")
        //        {
        //            if (opts.HasFlag(ValidateOptions.IgnoreRequire)) continue;
        //            if (value == null || (opts.HasFlag(ValidateOptions.DefaultAsNull) && value.Equals(this.DefaultValue)))
        //                return new ValidationResult(valid, "Required");
        //        }
        //        var errorCode = valid.Check(value);
        //        if (!string.IsNullOrEmpty(errorCode)) return new ValidationResult(valid, errorCode);
        //    }
        //    return null;
        //}





        #region GetValue
        Func<object, object> _GetValue;
        public object GetValue(object instance)
        {
            if (_GetValue == null)
            {
                lock (this)
                {
                    if (_GetValue == null) _GetValue = MakeGetter(this.MemberInfo);
                }
            }
            return _GetValue(instance);
        }

        

        static Func<object, object> MakeGetter(MemberInfo memberInfo)
        {
            var instanceParameterExpr = Expression.Parameter(typeof(object), "instanceParameter");
            var bodyExpr = MakeGetterExpression(memberInfo, instanceParameterExpr);
            var lamda = Expression.Lambda<Func<object, object>>(bodyExpr, instanceParameterExpr);
            return lamda.Compile();
        }

        protected static Expression MakeGetterExpression(MemberInfo memberInfo, ParameterExpression instanceParameterExpr)
        {
            var memberType = memberInfo.MemberType == MemberTypes.Field ? ((FieldInfo)memberInfo).FieldType : ((PropertyInfo)memberInfo).PropertyType;
            var isInstanceTyped = instanceParameterExpr.Type == memberType;
            Expression instanceExpr = null;


            if (isInstanceTyped) instanceExpr = instanceParameterExpr;
            else instanceExpr = Expression.Convert(instanceParameterExpr, memberInfo.DeclaringType);

            var getterExpr = Expression.PropertyOrField(instanceExpr, memberInfo.Name);

            return Expression.Convert(getterExpr, typeof(object));
        }

        #endregion

        #region setValue


        Action<object, object> _SetValue;
        public void SetValue(object instance, object value)
        {
            if (_SetValue == null)
            {
                lock (this)
                {
                    if (_SetValue == null) _SetValue = MakeSetter(this.MemberInfo);
                }
            }
            _SetValue(instance, value);
        }
        static Action<object, object> MakeSetter(MemberInfo memberInfo)
        {
            var instanceParameterExpr = Expression.Parameter(typeof(object), "instanceParameter");
            var valueParameterExpr = Expression.Parameter(typeof(object), "valueParameter");
            var bodyExpr = MakeSetterExpression(memberInfo, instanceParameterExpr, valueParameterExpr);
            var lamda = Expression.Lambda<Action<object, object>>(bodyExpr, instanceParameterExpr, valueParameterExpr);
            return lamda.Compile();
        }

        protected static Expression MakeSetterExpression(MemberInfo memberInfo, ParameterExpression instanceParameterExpr, ParameterExpression valueParameterExpr)
        {
            var memberType = memberInfo.MemberType == MemberTypes.Field ? ((FieldInfo)memberInfo).FieldType : ((PropertyInfo)memberInfo).PropertyType;
            var isInstanceTyped = instanceParameterExpr.Type == memberType;
            Expression instanceExpr = null;
            if (isInstanceTyped) instanceExpr = instanceParameterExpr;
            else instanceExpr = Expression.Convert(instanceParameterExpr, memberInfo.DeclaringType);
            Expression setterExpr = null;
            if (memberType.IsClass)
            {
                if (memberType == typeof(string))
                {
                    setterExpr = MakeStringSetterExpression(memberInfo, memberType, instanceExpr, valueParameterExpr);
                }
                else {
                    setterExpr = MakeRefValueSetterExpression(memberInfo, memberType, instanceExpr, valueParameterExpr);
                }
                
            }
            else {
                var isNullable = memberType.FullName.StartsWith("System.Nullable`");
                var innerType = memberType;
                if (isNullable)
                {
                    innerType = memberType.GetGenericArguments()[0];
                }
                setterExpr = MakeValValueSetterExpression(memberInfo, memberType, innerType,isNullable, instanceExpr, valueParameterExpr);
            }
            return setterExpr;
            

        }

        #region RefValueSetter
        static MethodInfo ToStringMethod = typeof(object).GetMethod("ToString", Type.EmptyTypes);
        static MethodInfo ObjectGetTypeMethod = typeof(object).GetMethod("GetType", Type.EmptyTypes);
        static MethodInfo IsAssginableFromMethod = typeof(Type).GetMethod("IsAssignableFrom",new Type[] { typeof(Type)});
        static MethodInfo DeserializeObjectMethod = typeof(Newtonsoft.Json.JsonConvert).GetMethod("DeserializeObject",1, new Type[] { typeof(string) });
        static MethodInfo ToObjectMethod = typeof(JContainer).GetMethod("ToObject",Type.EmptyTypes);
        static Type NullableType = typeof(Nullable<>);
        static MethodInfo ConvertToMethod = typeof(ValConvert).GetMethod("ConvertTo");
        //static MethodInfo Deser
        static Expression MakeStringSetterExpression(MemberInfo memberInfo, Type memberType, Expression instanceExpr, ParameterExpression valueParameterExpr)
        {
            /*
             (instance,value):
             if(value==null) return instance.Profile = null;
             var t = value.GetType();
             if(JContainer.IsAssingableFrom(t)) return instance.Profile = (JCOntainer).ToObject(Profile);
             else if(memberType.IsAssignableFrom(t)) return instance.Profile = (Profile)value;
             else if(t==typeof(string)) return instance.Profile = Desieraize<Profile>(value.ToString());
             else throw new InvalidOperation("无法设置值");
             */
            var codes = new List<Expression>();
            var locals = new List<ParameterExpression>();
            var retTarget = Expression.Label();
            var instMemberExpr = Expression.PropertyOrField(instanceExpr, memberInfo.Name);
            //if(value==null) return instance.Profile = null;
            codes.Add(Expression.IfThen(
                Expression.Equal(valueParameterExpr, Expression.Constant(null))
                , Expression.Block(
                    Expression.Assign(instMemberExpr, Expression.Constant(null, memberType))
                    , Expression.Return(retTarget)
                )
            ));


            codes.Add(Expression.Assign(instMemberExpr, Expression.Call(valueParameterExpr, ToStringMethod)));


            codes.Add(Expression.Label(retTarget));

            return Expression.Block(locals, codes);
            //memberType.IsAssignableFrom()
        }

        static Expression MakeRefValueSetterExpression(MemberInfo memberInfo,Type memberType, Expression instanceExpr, ParameterExpression valueParameterExpr) {
            /*
             (instance,value):
             if(value==null) return instance.Profile = null;
             var t = value.GetType();
             if(JContainer.IsAssingableFrom(t)) return instance.Profile = (JCOntainer).ToObject(Profile);
             else if(memberType.IsAssignableFrom(t)) return instance.Profile = (Profile)value;
             else if(t==typeof(string)) return instance.Profile = Desieraize<Profile>(value.ToString());
             else throw new InvalidOperation("无法设置值");
             */
            var codes = new List<Expression>();
            var locals = new List<ParameterExpression>();
            var retTarget = Expression.Label();
            var instMemberExpr = Expression.PropertyOrField(instanceExpr,memberInfo.Name);
            //if(value==null) return instance.Profile = null;
            codes.Add(Expression.IfThen(
                Expression.Equal(valueParameterExpr, Expression.Constant(null))
                , Expression.Block(
                    Expression.Assign(instMemberExpr, Expression.Constant(null,memberType))
                    , Expression.Return(retTarget)
                )
            ));

            //var t = value.GetType();
            var tExpr = Expression.Parameter(typeof(Type), "t");
            locals.Add(tExpr);
            codes.Add(Expression.Assign(tExpr, Expression.Call(valueParameterExpr, ObjectGetTypeMethod)));

            // if (t is MemberType) inst.Profile = t as Profile;
            codes.Add(Expression.IfThen(
                Expression.Call(Expression.Constant(memberType),IsAssginableFromMethod,tExpr)
                ,Expression.Block(
                    Expression.Assign(instMemberExpr, Expression.Convert(valueParameterExpr,memberType))
                    ,Expression.Return(retTarget)
                )
            ));

            //if(JContainer.IsAssingableFrom(t)) return instance.Profile = (JCOntainer).ToObject(Profile);
            var toObjectMethod = ToObjectMethod.MakeGenericMethod(memberType);
            codes.Add(Expression.IfThen(
                Expression.Call(Expression.Constant(typeof(JContainer)), IsAssginableFromMethod, tExpr)
                , Expression.Block(
                    Expression.Assign(instMemberExpr, Expression.Call(Expression.Convert(valueParameterExpr, typeof(JContainer)), toObjectMethod))
                    , Expression.Return(retTarget)
                )
            ));
            // try{inst.Profile = Des<Profile>(value.ToString());}catch{return}
            //var exExpr = Expression.Parameter(typeof(Exception));
            //locals.Add(exExpr);
            codes.Add(
                Expression.IfThenElse(
                    Expression.Equal(tExpr, Expression.Constant(typeof(string)))
                    , Expression.TryCatch(
                        Expression.Assign(
                            instMemberExpr,
                            Expression.Call(DeserializeObjectMethod.MakeGenericMethod(memberType), Expression.Call(valueParameterExpr, ToStringMethod))
                        ),
                        Expression.Catch(typeof(Exception), Expression.Assign(instMemberExpr, Expression.Constant(null, memberType)))
                    )
                    , Expression.Assign(instMemberExpr, Expression.Constant(null, memberType))
                )
            );

            codes.Add(Expression.Label(retTarget));

            return Expression.Block(locals, codes);
            //memberType.IsAssignableFrom()
        }
        #endregion

        #region ValValueSetter
        static Expression MakeValValueSetterExpression(MemberInfo memberInfo,Type memberType, Type actualType,bool isNullable, Expression instanceExpr, ParameterExpression valueParameterExpr)
        {
            /*
             var v
             if(value==null) inst.Gender = v or new Nullable<T>;return;
             var t = value.gettype();
             if(t==memberType) return inst.Gender = (Gender)value;
             if(t==atype) return inst.Gender= new Nullable<Gender>((Gender)value);
             if(t==JToken) v = ((JToken)value).ToObject<Gender>();
             else if(t==string) v = parse(value);
             return v or new Nullable(v);
             */
            var codes = new List<Expression>();
            var locals = new List<ParameterExpression>();
            var vExpr = Expression.Parameter(memberType,"v");
            var instMemberExpr = Expression.PropertyOrField(instanceExpr, memberInfo.Name);
            locals.Add(vExpr);
            var retLabel = Expression.Label();
            
            //if(value==null) inst.Gender = v or new Nullable<T>;return;
            codes.Add(Expression.IfThen(
                //IF (value==null)
                Expression.Equal(valueParameterExpr, Expression.Constant(null))
                //THEN
                , Expression.Block(
                    //inst.Gender = v; or T?()
                    Expression.Assign(instMemberExpr, (isNullable)?(Expression)Expression.New(memberType): vExpr)
                    //return
                    ,Expression.Return(retLabel)
                )
            ));
            //var t = value.gettype();
            var tExpr = Expression.Parameter(typeof(Type), "t");
            locals.Add(tExpr);
            var getTypeMethod = ObjectGetTypeMethod;
            codes.Add(Expression.Assign(tExpr,Expression.Call(valueParameterExpr, getTypeMethod)));

            //if(t==memberType) return inst.Gender = (Gender)value;
            codes.Add(Expression.IfThen(
                Expression.Equal(tExpr, Expression.Constant(memberType))
                , Expression.Block(
                    Expression.Assign(instMemberExpr, Expression.Convert(valueParameterExpr,memberType))
                    , Expression.Return(retLabel)
                )
            ));

            //if (t == atype) return inst.Gender = new Nullable<Gender>((Gender)value);
            if (isNullable) {
                codes.Add(Expression.IfThen(
                Expression.Equal(tExpr, Expression.Constant(actualType))
                , Expression.Block(
                    Expression.Assign(instMemberExpr, Expression.New(memberType.GetConstructor(new Type[] { actualType }), Expression.Convert(valueParameterExpr, actualType)))
                    , Expression.Return(retLabel)
                )
            ));
            }

            var convertToMethod = ConvertToMethod.MakeGenericMethod(actualType);
            //noneable<T> parsedValue = parser(value.toString());
            var parsedValueExpr = Expression.Call(convertToMethod, Expression.Call(valueParameterExpr,ToStringMethod));
            
            if (isNullable) {
                //inst.Gender = parsedValue.HasValue?new Nullable<Gender>(parsedValue.Value):new Nullable<Gender>();
                var nullableExpr = Expression.Condition(
                    Expression.PropertyOrField(parsedValueExpr,"HasValue")
                    ,Expression.New(NullableType.MakeGenericType(actualType).GetConstructor(new Type[] { actualType}),Expression.PropertyOrField(parsedValueExpr,"Value"))
                    ,Expression.New(NullableType.MakeGenericType(actualType))
                    );
                codes.Add(Expression.Assign(instMemberExpr, nullableExpr));
            } else {
                //inst.Gender = parsedValue.GetValue();
                codes.Add(Expression.Assign(instMemberExpr, Expression.Call(parsedValueExpr,typeof(Noneable<>).MakeGenericType(actualType).GetMethod("GetValue"))));

            }
            codes.Add(Expression.Label(retLabel));
            return Expression.Block(locals,codes);
        }

        #endregion

        #endregion setvalue

    }
}
