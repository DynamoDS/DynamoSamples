using Autodesk.DesignScript.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleDataBridge.Functions.Packing
{
    [IsVisibleInDynamoLibrary(false)]
    public class UnPackFunctions
    {
        /// <summary>
        /// Returns the value of dictionary at a given key.
        /// </summary>
        /// <param name="dictionary">Dictionary exposing all available values</param>
        /// <param name="key">The key of the value to retrieve</param>
        /// <returns>The value retrieved from the dictionary at the given key</returns>
        public static object UnPackOutputByKey(DesignScript.Builtin.Dictionary dictionary, string key)
        {
            if (dictionary == null) return null;

            return dictionary.ValueAtKey(key);
        }
    }
}
