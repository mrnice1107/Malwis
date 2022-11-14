using System.Collections;
using Malwis.General.Collections;

namespace Malwis.Extensions.Enumerables;
public static class CollectionExtentions
{
    public static Type GetGenericCollectionType(this IEnumerable collection) => Collections.GetGenericCollectionType(collection);
}
