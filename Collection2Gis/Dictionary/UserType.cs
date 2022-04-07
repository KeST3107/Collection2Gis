using System;

namespace Collection2Gis.Dictionary
{
    public class UserType : IEquatable<UserType>
    {
        public long UserId;
        public string UserName;

        public bool Equals(UserType other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return UserId == other.UserId && string.Equals(UserName, other.UserName);
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            if (obj.GetType() != GetType())
                return false;
            return Equals((UserType)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + UserId.GetHashCode();
                hash = hash * 23 + UserName.GetHashCode();
                return hash;
            }
        }

        public static bool operator ==(UserType left, UserType right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(UserType left, UserType right)
        {
            return !Equals(left, right);
        }
    }
}
