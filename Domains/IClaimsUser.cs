using System.Collections;
using System.Collections.Generic;

namespace Itec.Domains
{
    public interface IClaimsUser:IUser
    {
        string this[string key] { get; set; }

        IDictionary<string, string> ToDictionary(IDictionary<string,string> dic=null);
        ClaimsUserEntity ToEntity(ClaimsUserEntity entity = null);
    }
}