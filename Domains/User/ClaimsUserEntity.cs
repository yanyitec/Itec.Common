using System;
using System.Collections.Generic;
using System.Text;

namespace Itec.Domains
{
    public class ClaimsUserEntity:UserEntity
    {
        Dictionary<string, string> _Claims;
        /// <summary>
        /// 不推荐使用这个属性，只是用于序列化
        /// </summary>
        public Dictionary<string, string> Claims {
            get {
                if (_Claims == null) {
                    if (!string.IsNullOrWhiteSpace(this._JSON)) {
                        this._Claims = JSON.Deserialize<Dictionary<string, string>>(this._JSON);
                    }
                }
                return this._Claims;
            }
            set {
                this._Claims = value;
                this._JSON = null;
            }
        }

        
        public override string ClaimJSON {
            get {
                if (this._JSON == null) {
                    if (this._Claims != null) this._JSON = JSON.Serialize(this._Claims);
                }
                return _JSON;
            }
            set {
                this._JSON = value;
                this._Claims = null;
            }
        }

       
    }
}
