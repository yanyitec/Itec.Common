using System;
using System.Collections.Generic;
using System.Text;

namespace Itec
{
    public static class JSON
    {
        public static string Serialize(object obj) {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

        public static T Deserialize<T>(string json) {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }
    }
}
