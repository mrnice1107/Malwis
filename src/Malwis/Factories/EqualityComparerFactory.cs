using Malwis.General.Hashing;

namespace Malwis.Factories;
public static class EqualityComparerFactory
{
    public static IEqualityComparer<T> Create<T>(Func<T?, T?, bool> equals) => Create(DefaultGetHashCode<T>(), equals);
    public static IEqualityComparer<T> Create<T>(Func<T?, int> getHashCode) => Create(getHashCode, DefaultEquals(getHashCode));
    public static IEqualityComparer<T> Create<T>(Func<T?, int> getHashCode, Func<T?, T?, bool> equals) => getHashCode == null
            ? throw new ArgumentNullException(nameof(getHashCode))
            : equals == null ? throw new ArgumentNullException(nameof(equals)) : (IEqualityComparer<T>)new Comparer<T>(getHashCode, equals);

    private class Comparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T?, int> _getHashCode;
        private readonly Func<T?, T?, bool> _equals;

        public Comparer(Func<T?, int> getHashCode, Func<T?, T?, bool> equals)
        {
            _getHashCode = getHashCode;
            _equals = equals;
        }

        public bool Equals(T? x, T? y) => _equals(x, y);

        public int GetHashCode(T obj) => _getHashCode(obj);
    }

    private static Func<T?, T?, bool> DefaultEquals<T>(Func<T?, int> getHashCode) => (t, t2) => getHashCode(t) == getHashCode(t2);
    private static Func<T?, int> DefaultGetHashCode<T>() => t => Hasher.GetHashCode(t);

}
