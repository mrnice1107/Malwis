using System.Collections;
using System.Text;

namespace Malwis.Extentions.Dictionary;
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

    public static bool IsNullOrEmpty(this IDictionary? source) => source is null || source.Count == 0;
    public static bool IsNullOrEmpty<TKey, TValue>(this IDictionary<TKey, TValue>? source) => source is null || source.Count == 0;
}
