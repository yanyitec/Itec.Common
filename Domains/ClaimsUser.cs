using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Itec.Domains
{
    public class ClaimsUser : User, IClaimsUser
    {
        public ClaimsUser(ClaimsUserEntity entity) : base(entity.Id, entity.Name, entity.DisplayName)
        {
            this._Claims = new Dictionary<string, string>();
            if (entity.Claims != null)
            {
                foreach (var pair in entity.Claims)
                {
                    this._Claims.Add(pair.Key, pair.Value);
                }
            }
            this._Claims["Id"] = entity.Id.ToString();
            this._Claims["Name"] = entity.Name;
            this._Claims["DisplayName"] = entity.DisplayName;
        }

        public ClaimsUser(string json) : this(JSON.Deserialize<Dictionary<string, string>>(json))
        {

        }

        public ClaimsUser(IDictionary<string, string> claims)
        {


            this._Claims = new Dictionary<string, string>();
            foreach (var pair in claims)
            {
                if (pair.Key == "Id") this.Id = new Guid(pair.Value);
                else if (pair.Key == "Name") this.Name = pair.Value;
                else if (pair.Key == "DisplayName") this.DisplayName = pair.Value;
                this._Claims.Add(pair.Key, pair.Value);
            }
            if (this._DisplayName == null) this._DisplayName = this.Name;
        }


        Dictionary<string, string> _Claims;

        public string this[string key]
        {
            get
            {
                lock (this)
                {
                    string value = null;
                    this._Claims.TryGetValue(key, out value);
                    return value;
                }
            }
            set
            {
                if (key == "Id" || key == "Name") throw new InvalidOperationException("不能设置Id 与 Name");
                lock (this)
                {
                    if (key == "DisplayName") this._DisplayName = value;
                    this._Claims[key] = value;
                }
            }
        }

        public ClaimsUserEntity ToEntity(ClaimsUserEntity entity = null)
        {
            if (entity == null) entity = new ClaimsUserEntity();
            entity.Id = this.Id;
            entity.Name = this.Name;
            lock (this)
            {
                entity.DisplayName = this.DisplayName;
                entity.ClaimJSON = JSON.Serialize(this._Claims);
            }

            return entity;

        }

        public override string ToJSON()
        {
            lock (this)
            {
                return JSON.Serialize(this._Claims);
            }
        }

        public IDictionary<string, string> ToDictionary(IDictionary<string,string> dic = null)
        {
            if(dic==null) dic = new Dictionary<string, string>();
            lock (this)
            {
                foreach (var pair in this._Claims)
                {
                    dic.Add(pair.Key, pair.Value);
                }
            }
            return dic;
        }



    }
}
