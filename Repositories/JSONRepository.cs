using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WorkerInventory.Repositories.JSON
{
    public class JSONRepository //: IJSONRepository <- Causes error due to static coding
    {
        //public static class JsonHelper
        //{
            public static string FromClass<TClass>
                (TClass data, bool isEmptyToNull = false, JsonSerializerOptions? options = null)
                where TClass : class
            {
                //Console.WriteLine("Attempting Serialization");
                string response = string.Empty;
                if (!EqualityComparer<TClass>.Default.Equals(data, default))
                    response = JsonSerializer.Serialize(data, options: options);
                return isEmptyToNull ? response == "{}" ? "null" : response : response;
            }

            public static TClass? ToClass<TClass>(string data, JsonSerializerOptions? options = null)
                where TClass : class
            {
                options = new JsonSerializerOptions
                { PropertyNameCaseInsensitive = true };

                //Console.WriteLine("Attempting Deserialization");
                TClass? response = default(TClass);
                return string.IsNullOrEmpty(data) ? response : JsonSerializer.Deserialize<TClass>(data, options ?? null);
            }
        //}
    }
}
