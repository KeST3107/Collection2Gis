namespace Collection2Gis.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Collection2Gis.Dictionary;
    using NUnit.Framework;

    [TestFixture]
    public class DictionaryTest
    {
        [Test]
        public void CountTest()
        {
            var complexKeyDictionary = new ComplexKeyDictionary<string, string, int>
            {
                { "Id1", "Name1", 1 },
                { "Id1", "Name2", 2 },
                { "Id2", "Name1", 3 },
                { "Id2", "Name2", 4 },
                { "Id3", "Name1", 5 }
            };
            Assert.That(complexKeyDictionary.Count, Is.EqualTo(5));
        }

        [Test]
        public void IndexerAddTest()
        {
            var complexKeyDictionary = new ComplexKeyDictionary<string, string, int>();
            complexKeyDictionary["Id", "Name"] = 1;

            Assert.That(complexKeyDictionary["Id", "Name"], Is.EqualTo(1));
        }

        [Test]
        public void IndexerSetTest()
        {
            var complexKeyDictionary = new ComplexKeyDictionary<string, string, int>
            {
                { "Id", "Name", 1 }
            };
            complexKeyDictionary["Id", "Name"] = 2;

            Assert.That(complexKeyDictionary["Id", "Name"], Is.EqualTo(2));
        }
        [Test]
        public void GetValuesByIdTest()
        {
            var complexKeyDictionary = new ComplexKeyDictionary<string, string, int>
            {
                { "Id", "Name1", 1 },
                { "Id", "Name2", 2 },
                { "Id2", "Name3", 3 }
            };
            var values = new List<int>
            {
                1,
                2
            };

            Assert.That(complexKeyDictionary.GetValuesById("Id"), Is.EqualTo(values));
        }

        [Test]
        public void GetValuesByNameTest()
        {
            var complexKeyDictionary = new ComplexKeyDictionary<string, string, int>
            {
                { "Id1", "Name1", 1 },
                { "Id2", "Name1", 2 },
                { "Id3", "Name3", 3 }
            };
            var values = new List<int>
            {
                1,
                2
            };

            Assert.That(complexKeyDictionary.GetValuesByName("Name1"), Is.EqualTo(values));
        }

        [Test]
        public void EnumerableTest()
        {
            var complexKeyDictionary = new ComplexKeyDictionary<string, string, int>
            {
                { "Id1", "Name1", 1 },
                { "Id1", "Name2", 2 },
                { "Id2", "Name1", 3 },
                { "Id2", "Name2", 4 },
                { "Id3", "Name1", 5 }
            };

            var iterations = 0;

            foreach (var value in complexKeyDictionary)
            {
                iterations++;
            }

            Assert.That(iterations, Is.EqualTo(5));
        }

        [Test]
        public void RemoveByKeyTest()
        {
            var complexKeyDictionary = new ComplexKeyDictionary<string, string, int>
            {
                { "Id1", "Name1", 1 },
                { "Id1", "Name2", 2 },
                { "Id2", "Name1", 3 },
                { "Id2", "Name2", 4 },
                { "Id3", "Name1", 5 }
            };
            complexKeyDictionary.Remove("Id2", "Name2");

            Assert.That(complexKeyDictionary.Count, Is.EqualTo(4));
        }

        [Test]
        public void RemoveItemTest()
        {
            var complexKeyDictionary = new ComplexKeyDictionary<string, string, int>
            {
                { "Id1", "Name1", 1 },
                { "Id1", "Name2", 2 },
                { "Id2", "Name1", 3 },
                { "Id2", "Name2", 4 },
                { "Id3", "Name1", 5 }
            };
            complexKeyDictionary.Remove(new KeyValuePair<ComplexKey<string, string>, int>(new ComplexKey<string, string>("Id2", "Name2"), 4));

            Assert.That(complexKeyDictionary.Count, Is.EqualTo(4));
        }

        [Test]
        public void ClearTest()
        {
            var complexKeyDictionary = new ComplexKeyDictionary<string, string, int>
            {
                { "Id1", "Name1", 1 },
                { "Id1", "Name2", 2 },
                { "Id2", "Name1", 3 },
                { "Id2", "Name2", 4 },
                { "Id3", "Name1", 5 }
            };
            complexKeyDictionary.ClearAll();

            Assert.That(complexKeyDictionary.Count, Is.EqualTo(0));
        }

        [Test]
        public void DuplicatesIdTest()
        {
            var complexKeyDictionary = new ComplexKeyDictionary<string, string, int>
            {
                { "Id1", "Name1", 1 },
                { "Id1", "Name2", 2 },
            };

            var dictionaryCount = complexKeyDictionary.Count();

            var isExceptionThrown = false;
            try
            {
                complexKeyDictionary.Add("Id1", "Name1", 3);
            }
            catch (Exception e)
            {
                isExceptionThrown = true;
            }

            Assert.That(isExceptionThrown, Is.EqualTo(true), "ArgumentException didn't throw.");
            Assert.That(complexKeyDictionary.Count, Is.EqualTo(dictionaryCount));
        }

        [Test]
        public void ThreadsTest()
        {
            var complexKeyDictionary = new ComplexKeyDictionary<string, string, int>
            {
                {"Id1", "Name1", 1},
                {"Id1", "Name2", 2},
                {"Id2", "Name1", 3},
                {"Id2", "Name2", 4},
                {"Id3", "Name1", 5}
            };

            var initCount = complexKeyDictionary.Count();

            Task.Run(() => complexKeyDictionary.Add("TId1", "TName1", 6));
            Task.Run(() => complexKeyDictionary.Add("TId2", "TName2", 7));
            Task.Run(() => complexKeyDictionary.Add("TId3", "TName3", 8));

            Thread.Sleep(10);

            Assert.AreEqual(initCount + 3, complexKeyDictionary.Count());
        }
    }
}
