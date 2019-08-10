using System;
using System.Collections.Generic;
using System.Text;

namespace Itec.Domains
{
    public class User
    {
        public User(Guid id, string name, string displayName=null) {
            this.Id = id;
            this.Name = name;
            this.DisplayName = displayName ?? name;
        }
        protected User() { }

        public User(UserEntity entity) {
            this.Id = entity.Id;
            this.Name = entity.Name;
            this.DisplayName = entity.DisplayName ?? this.Name;
        }
        public Guid Id { get; protected set; }
        public string Name
        {
            get; protected set;

        }
        protected string _DisplayName;
        public string DisplayName
        {
            get {
                return this._DisplayName;
            }
            set {
                lock (this) this._DisplayName = value;
            }
        }



        protected string _JSON;

        public virtual string ToJSON()
        {
            if (this._JSON == null)
            {
                lock (this)
                {
                    if (this._JSON == null)
                        this._JSON = $"{{\"Id\":\"{this.Id}\",\"Name\":\"{this.Name.Replace("\"", "\\\"")}\",\"DisplayName\":\"{this.DisplayName.Replace("\"", "\\\"")}\"}}";
                }
            }
            return this._JSON;
        }
    }
}
