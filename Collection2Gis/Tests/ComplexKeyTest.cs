namespace Collection2Gis.Tests
{
    using Collection2Gis.Dictionary;
    using NUnit.Framework;

    [TestFixture]
    public class ComplexKeyTest
    {
        [Test]
        public void SameObjectEqualTest()
        {
            var firstObject = new ComplexKey<UserType, string>(new UserType { UserId = 1, UserName = "123456" }, "Konstantin");
            var secondObject = new ComplexKey<UserType, string>(new UserType { UserId = 1, UserName = "123456" }, "Konstantin");

            Assert.That(firstObject.Equals(secondObject), Is.True);
        }
        [Test]
        public void DifferentObjectEqualTest()
        {
            var firstObject = new ComplexKey<UserType, string>(new UserType { UserId = 1, UserName = "123456" }, "Konstantin");
            var secondObject = new ComplexKey<UserType, string>(new UserType { UserId = 2, UserName = "12345" }, "Petya");

            Assert.That(firstObject.Equals(secondObject), Is.False);
        }

        [Test]
        public void SameObjectGetHashCodeTest()
        {
            var firstObject = new ComplexKey<UserType, string>(new UserType { UserId = 1, UserName = "123456" }, "Konstantin");
            var secondObject = new ComplexKey<UserType, string>(new UserType { UserId = 1, UserName = "123456" }, "Konstantin");

            Assert.That(firstObject.GetHashCode() == secondObject.GetHashCode());
        }

        [Test]
        public void DifferentObjectGetHashCodeTest()
        {
            var firstObject = new ComplexKey<UserType, string>(new UserType { UserId = 1, UserName = "123456" }, "Konstantin");
            var secondObject = new ComplexKey<UserType, string>(new UserType { UserId = 2, UserName = "12345" }, "Petya");

            Assert.That(firstObject.GetHashCode() != secondObject.GetHashCode());
        }
    }
}
