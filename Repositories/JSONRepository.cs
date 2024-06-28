using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WorkerInventory.Repositories.JSON
{
    public class JSONRepository
    {
        // Taken from https://www.codeproject.com/Articles/5339651/Working-with-System-Text-Json-in-Csharp

        /// <summary>
        /// Convert Class to Json object (string)
        /// </summary>
        /// <typeparam name="TClass">Class type to be serialized</typeparam>
        /// <param name="data">Class to serialize</param>
        /// <param name="isEmptyToNull">true = return null if empty; 
        /// false empty Json object</param>
        /// <param name="options">JsonSerializer options</param>
        /// <returns>Json encoded string</returns>

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


        /// <summary>
        /// Convert a Json object (string) to a class
        /// </summary>
        /// <typeparam name="TClass">Class type to be deserialized into</typeparam>
        /// <param name="data">Json string to be deserialized</param>
        /// <param name="options">JsonSerializer options</param>
        /// <returns>Deserialized class of TClass</returns>

        public static TClass? ToClass<TClass>(string data, JsonSerializerOptions? options = null)
                where TClass : class
            {
                options = new JsonSerializerOptions
                { PropertyNameCaseInsensitive = true };

                //Console.WriteLine("Attempting Deserialization");
                TClass? response = default(TClass);
                return string.IsNullOrEmpty(data) ? response : JsonSerializer.Deserialize<TClass>(data, options ?? null);
            }
        
    }
}
