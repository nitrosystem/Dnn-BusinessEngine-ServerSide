using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Common.TypeCasting
{
    public static class TypeCastingUtil<T> where T : class
    {
        public static T TryJsonCasting(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) { return null; }
            json = json.Trim();
            if ((json.StartsWith("{") && json.EndsWith("}")) || //For object
                (json.StartsWith("[") && json.EndsWith("]")) || //For array
                json == "null")  //For null
            {
                try
                {
                    var result = JsonConvert.DeserializeObject<T>(json);
                    return result;
                }
                catch (JsonReaderException jex)
                {
                    return null;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            else
            {
                return (T)Convert.ChangeType(json, typeof(T));
            }
        }
    }
}
