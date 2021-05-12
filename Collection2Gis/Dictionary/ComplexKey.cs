namespace Collection2Gis.Dictionary
{
    using System;

    public sealed class ComplexKey<TId, TName> : IEquatable<ComplexKey<TId, TName>>
    {
        public TId Id { get; }
        public TName Name { get; }

        public ComplexKey(TId id, TName name)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            Id = id;
            Name = name;
        }

        public bool Equals(ComplexKey<TId, TName> other)
        {
            if (other == null)
                return false;
            if (ReferenceEquals(this, other))
                return true;

            return Id.Equals(other.Id) && Name.Equals(other.Name);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            return obj is ComplexKey<TId, TName> key && Equals(key);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Id.GetHashCode();
                hash = hash * 23 + Name.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return $"Key: {Id.ToString()}, Name: {Name.ToString()}";
        }
    }
}
