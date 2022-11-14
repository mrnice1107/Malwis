using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace MalwisStandard2.Extensions
{
    public static class DictionaryExtensions
    {
        private const string EmptyDictionary = "{}";

        /// <param name="source">The source dictionary. This will not be modified.</param>
        /// <param name="keysToRemove">The keys to remove</param>
        /// <typeparam name="TKey">The key type of the dictionary</typeparam>
        /// <typeparam name="TValue">The value type of the dictionary</typeparam>
        /// <returns>A copy of the dictionary with the keys in <param name="keysToRemove"/> removed.</returns>
        [Pure]
        public static IDictionary<TKey, TValue> RemoveMany<TKey, TValue>(
            this IDictionary<TKey, TValue> source, params TKey[] keysToRemove)
        {
            IDictionary<TKey, TValue> clone = new Dictionary<TKey, TValue>(source);

            foreach (TKey key in keysToRemove)
            {
                clone.Remove(key);
            }

            return clone;
        }

        public static bool DictionaryIsNullOrEmpty<TKey, TValue>(IDictionary<TKey, TValue> source) =>
            source is null || source.Count == 0;


        public static string ToFancyString<TKey, TValue>(this IDictionary<TKey, TValue> source)
        {
            const char separator = ',';

            if (DictionaryIsNullOrEmpty(source))
            {
                return EmptyDictionary;
            }

            const string nullStr = "NULL";
            const string whiteSpace = " ";

            StringBuilder builder = new StringBuilder();
            builder.Append('{');
            builder.AppendLine();

            using (IEnumerator<KeyValuePair<TKey, TValue>> enumerator = source.GetEnumerator())
            {
                for (int i = 0; enumerator.MoveNext(); i++)
                {
                    KeyValuePair<TKey, TValue> pair = enumerator.Current;

                    builder.Append(
                        pair.Key is null ? nullStr :
                        pair.Key is string strKey ? $"\"{strKey}\"" :
                        pair.Key.ToString()
                    );

                    builder.Append(':');
                    builder.Append(whiteSpace);

                    builder.Append(
                        pair.Value is null ? nullStr :
                        pair.Value is string strValue ? $"\"{strValue}\"" :
                        pair.Value is IDictionary<object, object> valueDict ? valueDict.ToFancyString() :
                        pair.Value.ToString()
                    );

                    if (i < source.Count - 1)
                    {
                        builder.Append(separator);
                    }

                    builder.AppendLine();
                }

                builder.Append('}');

                return builder.ToString();
            }
        }

        /// <summary>
        /// Gets the value using the <paramref name="key"/> or adds the key value pair if the key does not exist.
        /// </summary>
        /// <typeparam name="TKey">The key type of the dictionary.</typeparam>
        /// <typeparam name="TValue">The value type of the dictionary.</typeparam>
        /// <param name="source">The dictionary to edit.</param>
        /// <param name="key">The key where the value should be.</param>
        /// <param name="value">The value to add when the key is missing.</param>
        /// <returns>A <typeparamref name="TValue"/> either by getting it from the dictionary or if nonexistent, adding it and returning the <paramref name="value"/></returns>
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue value)
        {
            if (source.TryGetValue(key, out TValue result))
            {
                return result;
            }

            source.Add(key, value);

            return value;
        }
        
        /// <summary>
        /// Gets the value using the Key of <paramref name="keyValue"/> or adds the key value pair if the key does not exist.
        /// </summary>
        /// <typeparam name="TKey">The key type of the dictionary.</typeparam>
        /// <typeparam name="TValue">The value type of the dictionary.</typeparam>
        /// <param name="source">The dictionary to edit.</param>
        /// <param name="keyValue">The key and value to look for and to add if key does not exist.</param>
        /// <returns>A <typeparamref name="TValue"/> either by getting it from the dictionary or if nonexistent, adding it and returning the value of <paramref name="keyValue"/></returns>
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> source, KeyValuePair<TKey, TValue> keyValue) => GetOrAdd(source, keyValue.Key, keyValue.Value);
        
        // TODO: summary, test
        [Pure]
        public static IDictionary<TKey, ICollection<TValue>> MergeKeys<TKey, TValue>(this IDictionary<TKey, ICollection<TValue>> source,
            TKey newKey, params TKey[] mergingKeys)
        {
            IDictionary<TKey, ICollection<TValue>> mergedDict = source.RemoveMany(mergingKeys);
            HashSet<TValue> mergedValues = new HashSet<TValue>();

            foreach (TKey mergingKey in mergingKeys)
            {
                ICollection<TValue> values = source[mergingKey];
                foreach (TValue value in values)
                {
                    mergedValues.Add(value);
                }
            }

            mergedDict[newKey] = mergedValues;
        
            return mergedDict;
        }
    }
}
