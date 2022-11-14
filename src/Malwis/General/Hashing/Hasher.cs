using Malwis.Extensions.Numbers;

namespace Malwis.General.Hashing;
public static class Hasher
{
    private static readonly int baseHash;

    static Hasher() => baseHash = Guid.NewGuid().GetHashCode();

    public static int GetHashCode(object? obj) => obj == null
            ? 0
            : obj.IsIntegerNumeric()
            ? baseHash ^ (int)obj
            : obj is ICollection<object> collection ? GetHashCode(collection) : obj.GetHashCode();
    public static int GetHashCode(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return 0;
        }

        int hash = baseHash;
        foreach (char c in text)
        {
            hash ^= c;
        }
        return hash;
    }
    public static int GetHashCode(ICollection<object> collection, bool considerReadonly = false)
    {
        if (collection.Count == 0)
        {
            return 0;
        }

        int hash = considerReadonly ? baseHash + Convert.ToInt16(collection.IsReadOnly) : baseHash;

        foreach (object item in collection)
        {
            hash ^= GetHashCode(item);
        }
        return hash;
    }

    public static int CombineHashes(params int[] hashes)
    {
        int hashResult = baseHash;

        foreach (int hash in hashes)
        {
            hashResult ^= hash;
        }
        return hashResult;
    }
}
