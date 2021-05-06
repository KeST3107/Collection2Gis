namespace Collection2Gis
{
    using System.Collections;
    using System.Collections.Generic;

    public sealed class ComplexKeyDictionary<TId, TName, TValue> : IEnumerable<TValue>
    {
        private readonly Dictionary<ComplexKey<TId, TName>, TValue> _dictionary =
            new Dictionary<ComplexKey<TId, TName>, TValue>();


        IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
        {
            return _dictionary.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            var e = (_dictionary as IEnumerable).GetEnumerator();
            return e;
        }
    }
}
