using System.Collections;
using System.Text;
using System.Collections.Concurrent;
using System.Net.Http.Headers;

namespace Malwis.Extentions.Dictionary;

// TODO: Write tests for dictionaries
public static class DictionaryExtentions
{
    private static readonly string emptyDictionary = "{}";

    public static IDictionary<TValue, TKey> Reverse<TKey, TValue>(this IDictionary<TKey, TValue> source) where TKey : notnull where TValue : notnull
    {
        Dictionary<TValue, TKey> result = new();

        foreach (KeyValuePair<TKey, TValue> entry in source)
        {
            result[entry.Value] = entry.Key;
        }

        return result;
    }

    public static IDictionary<TValue, ICollection<TKey>> ReverseListDictionary<TKey, TValue>(this IDictionary<TKey, IEnumerable<TValue>> source) where TKey : notnull where TValue : notnull
    {
        Dictionary<TValue, ICollection<TKey>> result = new();

        foreach (KeyValuePair<TKey, IEnumerable<TValue>> entry in source)
        {
            TKey value = entry.Key;

            foreach (TValue key in entry.Value)
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

    public static string ToFancyString<TKey, TValue>(this IDictionary<TKey, TValue>? source, char separator = ',', bool doWhiteSpaces = false, bool doNewLines = false)
    {
        if (source.IsNullOrEmpty())
        {
            return emptyDictionary;
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

        IEnumerator<KeyValuePair<TKey, TValue>> enumerator = source!.GetEnumerator();
        for (int i = 0; enumerator.MoveNext(); i++)
        {
            KeyValuePair<TKey, TValue> pair = enumerator.Current;

            builder.Append(
                pair.Key is null ? nullStr :
                pair.Key is string str ? $"\"{pair.Value}\"" :
                pair.Key is IDictionary<object, object> keyDict ? keyDict.ToFancyString() :
                pair.Key.ToString()
                );

            builder.Append(':');

            builder.Append(whiteSpace);

            builder.Append(
                pair.Value is null ? nullStr :
                pair.Value is string ? $"\"{pair.Value}\"" :
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

    public static bool IsNullOrEmpty(this IDictionary source) => source is null || source.Count == 0;
    public static bool IsNullOrEmpty<TKey, TValue>(this IDictionary<TKey, TValue> source) => source is null || source.Count == 0;

    /// <summary>
    /// Gets the value using the <paramref name="key"/> or adds the key value pair if the key does not exist.
    /// </summary>
    /// <typeparam name="TKey">The key type of the dictionary.</typeparam>
    /// <typeparam name="TValue">The value type of the dictionary.</typeparam>
    /// <param name="source">The dictionary to edit.</param>
    /// <param name="key">The key where the value shoule be.</param>
    /// <param name="value">The value to add when the key is missing.</param>
    /// <returns>A <typeparamref name="TValue"/> either by getting it from the dictionary or if nonexistent, adding it and returing the <paramref name="value"/></returns>
    public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue value)
    {
        if (source.TryGetValue(key, out TValue? result))
        {
            return result;
        }

        source.Add(key, value);

        return value;
    }

    /// <summary>
    /// Gets the value using the <paramref name="key"/> or adds the key value pair if the key does not exist.
    /// </summary>
    /// <typeparam name="TKey">The key type of the dictionary.</typeparam>
    /// <typeparam name="TValue">The value type of the dictionary.</typeparam>
    /// <param name="source">The dictionary to edit.</param>
    /// <param name="keyValue">The key and value to look for and to add if key does not exist.</param>
    /// <returns>A <typeparamref name="TValue"/> either by getting it from the dictionary or if nonexistent, adding it and returing the <paramref name="value"/></returns>
    public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> source, KeyValuePair<TKey, TValue> keyValue) => GetOrAdd(source, keyValue.Key, keyValue.Value);
}
