using System.Collections;
using System.Diagnostics.Contracts;
using System.Net.Http.Headers;
using System.Security;
using System.Text;
using Malwis.Extensions.Numbers;

namespace Malwis.Extensions.Dictionary;

// TODO: Write tests for dictionaries
public static class DictionaryExtensions
{
    private const string EmptyDictionary = "{}";

    // TODO: write test
    /// <param name="source">The source dictionary. This will not be modified.</param>
    /// <param name="keysToRemove">The keys to remove</param>
    /// <typeparam name="TKey">The key type of the dictionary</typeparam>
    /// <typeparam name="TValue">The value type of the dictionary</typeparam>
    /// <returns>A copy of the dictionary with the keys in <param name="keysToRemove"/> removed.</returns>
    [Pure] public static IDictionary<TKey, TValue> RemoveMany<TKey, TValue>(
        this IDictionary<TKey, TValue> source, params TKey[] keysToRemove) where TKey : notnull
    {
        IDictionary<TKey, TValue> clone = new Dictionary<TKey, TValue>(source);

        foreach (TKey key in keysToRemove)
        {
            clone.Remove(key);
        }

        return clone;
    }

    // TODO: summary, test
    public static IDictionary<TValue, TKey> Reverse<TKey, TValue>(this IDictionary<TKey, TValue> source) where TKey : notnull where TValue : notnull
    {
        Dictionary<TValue, TKey> result = new();

        foreach (KeyValuePair<TKey, TValue> entry in source)
        {
            result[entry.Value] = entry.Key;
        }

        return result;
    }

    // TODO: summary, test
    public static IDictionary<TValue, ICollection<TKey>> ReverseListDictionary<TKey, TValue>(this IDictionary<TKey, IEnumerable<TValue>> source) where TKey : notnull where TValue : notnull
    {
        Dictionary<TValue, ICollection<TKey>> result = new();

        foreach ((TKey? value, IEnumerable<TValue>? enumerable) in source)
        {
            foreach (TValue key in enumerable)
            {
                if (!result.ContainsKey(key))
                {
                    result[key] = new HashSet<TKey>();
                }

                result[key].Add(value);
            }
        }

        return result;
    }

    // TODO: summary
    public static string ToFancyString<TKey, TValue>(this IDictionary<TKey, TValue>? source, char separator = ',', bool doWhiteSpaces = false, bool doNewLines = false)
    {
        if (source.IsNullOrEmpty())
        {
            return EmptyDictionary;
        }

        const string nullStr = "NULL";
        string whiteSpace = doWhiteSpaces ? " " : string.Empty;

        StringBuilder builder = new();
        builder.Append('{');
        if (doNewLines)
        {
            builder.AppendLine();
        }
        else
        {
            builder.Append(whiteSpace);
        }

        using IEnumerator<KeyValuePair<TKey, TValue>> enumerator = source!.GetEnumerator();
        for (int i = 0; enumerator.MoveNext(); i++)
        {
            KeyValuePair<TKey, TValue> pair = enumerator.Current;

            builder.Append(
                pair.Key is null ? nullStr :
                pair.Key is string || pair.Key.IsFloatingPointNumeric() ? $"\"{pair.Key}\"" :
                pair.Key.ToString()
                );

            builder.Append(':');

            builder.Append(whiteSpace);

            builder.Append(
                pair.Value is null ? nullStr :
                pair.Value is string || pair.Value.IsFloatingPointNumeric() ? $"\"{pair.Value}\"" :
                pair.Value is IDictionary<object, object> valueDict ? valueDict.ToFancyString() :
                pair.Value.ToString()
                );

            if (i < source.Count -1)
            {
                builder.Append(separator);
            }
            if (doNewLines)
            {
                builder.AppendLine();
            }
            else
            {
                builder.Append(whiteSpace);
            }
        }

        builder.Append('}');

        return builder.ToString();
    }

    // TODO: summary, test
    public static bool IsNullOrEmpty(this IDictionary? source) => DictionaryIsNullOrEmpty(source);

    // TODO: summary
    public static bool IsNullOrEmpty<TKey, TValue>(this IDictionary<TKey, TValue>? source) => DictionaryIsNullOrEmpty(source);
    
    // TODO: summary
    public static bool DictionaryIsNullOrEmpty(IDictionary? source) => source is null || source.Count == 0;
    // TODO: summary
    public static bool DictionaryIsNullOrEmpty<TKey, TValue>(IDictionary<TKey, TValue>? source) => source is null || source.Count == 0;

    // TODO: test
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
        if (source.TryGetValue(key, out TValue? result))
        {
            return result;
        }

        source.Add(key, value);

        return value;
    }

    // TODO: test
    /// <summary>
    /// Gets the value using the Key of <paramref name="keyValue"/> or adds the key value pair if the key does not exist.
    /// </summary>
    /// <typeparam name="TKey">The key type of the dictionary.</typeparam>
    /// <typeparam name="TValue">The value type of the dictionary.</typeparam>
    /// <param name="source">The dictionary to edit.</param>
    /// <param name="keyValue">The key and value to look for and to add if key does not exist.</param>
    /// <returns>A <typeparamref name="TValue"/> either by getting it from the dictionary or if nonexistent, adding it and returning the value of <paramref name="keyValue"/></returns>
    public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> source, KeyValuePair<TKey, TValue> keyValue) => GetOrAdd(source, keyValue.Key, keyValue.Value);

    // TODO: test
    /// <summary>
    /// Gets the value using the <paramref name="key"/> or adds the key value pair if the key does not exist.
    /// If the <param name="valueCreator"/> will be Invoked to create the value if it does not exist.
    /// </summary>
    /// <typeparam name="TKey">The key type of the dictionary.</typeparam>
    /// <typeparam name="TValue">The value type of the dictionary.</typeparam>
    /// <param name="source">The dictionary to edit.</param>
    /// <param name="key">The key where the value should be.</param>
    /// <param name="valueCreator">The a function that creates a value if not already existing.</param>
    /// <returns>A <typeparamref name="TValue"/> either by getting it from the dictionary or if nonexistent, adding it and returning the value from <paramref name="valueCreator"/></returns>
    public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key,
        Func<TValue> valueCreator)
    {
        if (source.TryGetValue(key, out TValue? result))
        {
            return result;
        }
        
        TValue value = valueCreator.Invoke();
        source.Add(key, value);
        return value;
    }
    
    // TODO: summary, test
    [Pure]
    public static IDictionary<TKey, ICollection<TValue>> MergeKeys<TKey, TValue>(this IDictionary<TKey, ICollection<TValue>> source,
        TKey newKey, params TKey[] mergingKeys) where TKey : notnull
    {
        IDictionary<TKey, ICollection<TValue>> mergedDict = source.RemoveMany(mergingKeys);
        HashSet<TValue> mergedValues = new();

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
