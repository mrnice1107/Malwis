using System.Collections;
using Malwis.General.Collections;

namespace Malwis.Extentions.Enumerables;
public static class CollectionExtentions
{
    public static Type GetGenericCollectionType(this IEnumerable collection) => Collections.GetGenericCollectionType(collection);
}
