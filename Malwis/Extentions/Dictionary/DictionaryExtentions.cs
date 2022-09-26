using System.Collections;

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

    public static string ToStringFancy<TKey, TValue>(this IDictionary<TKey, TValue> source)
    {
        if (source.IsNullOrEmpty())
        {
            return emptyDictionary;
        }

        //TODO -> {"str1": 0, "str2": 1, "str3": 2, "str4": 3} smth like that

        return source.ToString() ?? emptyDictionary;
    }

    public static bool IsNullOrEmpty(this IDictionary source) => source is null || source.Count == 0;
    public static bool IsNullOrEmpty<TKey, TValue>(this IDictionary<TKey, TValue> source) => source is null || source.Count == 0;
}

/*
v1:

{"str1": 0, "str2": 1, "str3": 2, "str4": 3}
-> 
{0: "str1", 1: "str2", 2: "str3", 3: "str4"}

v2:

{"str1": [0,1,2], "str2":[1,2,3]} 
{0:["str1"],1:["str1","str2"],2:["str1","str2"],3:["str2"]}
 */
