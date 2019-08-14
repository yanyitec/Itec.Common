namespace Itec.Metas
{
    public interface IMetaProperty<T>:IMetaProperty
    {
        new IMetaClass<T> Class { get; }

        object GetValue(T instance);
        void SetValue(T instance, object value);
    }
}