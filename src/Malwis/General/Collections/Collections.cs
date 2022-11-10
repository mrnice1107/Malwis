using System.Collections;

namespace Malwis.General.Collections;
public static class Collections
{
    public static Type GetGenericCollectionType(IEnumerable collection) => collection.GetType().GetGenericArguments().Single();
}
