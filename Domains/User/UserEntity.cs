using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Itec.Domains
{
    public class UserEntity :Entity,IUser
    {
        

        public string Name
        {
            get; set;

        }

        public string DisplayName {
            get;
            set;
        }

        
        public virtual string ClaimJSON {
            get {
                return this.ToJSON();
            }
            set {
                this._JSON = value;
            }
        }

        protected string _JSON;

        public virtual string ToJSON()
        {
            if (this._JSON == null)
                this._JSON = $"{{\"Id\":\"{this.Id}\",\"Name\":\"{this.Name.Replace("\"", "\\\"")}\",\"DisplayName\":\"{this.DisplayName.Replace("\"", "\\\"")}\"}}";
            return this._JSON;
        }

    }
}
