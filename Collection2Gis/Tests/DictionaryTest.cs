using System;
using System.Collections.Generic;
using System.Threading;
using Collection2Gis.Dictionary;
using NUnit.Framework;

namespace Collection2Gis.Tests
{
    [TestFixture]
    public class DictionaryTest
    {
        [Test]
        public void CountTest()
        {
            var complexKeyDictionary = new ComplexKeyDictionary<string, string, int>
            {
                { "102", "Konstantin", 1 },
                { "102", "Petya", 2 },
                { "105", "Konstantin", 3 },
                { "105", "Nikolay", 4 },
                { "100", "TestName", 5 }
            };
            Assert.That(complexKeyDictionary.Count, Is.EqualTo(5));
        }

        [Test]
        public void AddMethodsTest()
        {
            var complexKeyDictionary = new ComplexKeyDictionary<string, string, int>
            {
                { "102", "Konstantin", 1 }
            };
            var keyValuePair =
                new KeyValuePair<ComplexKey<string, string>, int>(new ComplexKey<string, string>("102", "Petya"), 2);
            var complexKey = new ComplexKey<string, string>("115", "Konstantin");
            var value = 3;

            complexKeyDictionary.Add(keyValuePair);
            complexKeyDictionary.Add(complexKey, value);


            Assert.That(complexKeyDictionary.Count, Is.EqualTo(3));
        }

        [Test]
        public void IndexerAddTest()
        {
            var complexKeyDictionary = new ComplexKeyDictionary<string, string, int>();
            complexKeyDictionary["102", "Konstantin"] = 102;

            Assert.That(complexKeyDictionary["102", "Konstantin"], Is.EqualTo(102));
        }

        [Test]
        public void IndexerSetTest()
        {
            var complexKeyDictionary = new ComplexKeyDictionary<string, string, int>
            {
                { "102", "Konstantin", 1 }
            };
            complexKeyDictionary["Id", "Name"] = 505;

            Assert.That(complexKeyDictionary["Id", "Name"], Is.EqualTo(505));
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
        public void RemoveItemTest()
        {
            var complexKeyDictionary = new ComplexKeyDictionary<string, string, int>
            {
                { "102", "Konstantin", 1 },
                { "102", "Petya", 2 },
                { "105", "Konstantin", 3 },
                { "105", "Nikolay", 4 },
                { "100", "TestName", 5 }
            };
            complexKeyDictionary.Remove(
                new KeyValuePair<ComplexKey<string, string>, int>(new ComplexKey<string, string>("102", "Petya"), 2));

            Assert.That(complexKeyDictionary.Count, Is.EqualTo(4));
        }

        [Test]
        public void RemoveByKeyTest()
        {
            var complexKeyDictionary = new ComplexKeyDictionary<string, string, int>
            {
                { "102", "Konstantin", 1 },
                { "102", "Petya", 2 },
                { "105", "Konstantin", 3 },
                { "105", "Nikolay", 4 },
                { "100", "TestName", 5 }
            };
            var complexKey = new ComplexKey<string, string>("102", "Petya");
            complexKeyDictionary.Remove("102", "Konstantin");
            complexKeyDictionary.Remove(complexKey);

            Assert.That(complexKeyDictionary.Count, Is.EqualTo(3));
        }

        [Test]
        public void RemoveByIdTest()
        {
            var complexKeyDictionary = new ComplexKeyDictionary<string, string, int>
            {
                { "102", "Konstantin", 1 },
                { "102", "Petya", 2 },
                { "105", "Konstantin", 3 },
                { "105", "Nikolay", 4 },
                { "100", "TestName", 5 }
            };
            complexKeyDictionary.RemoveById("102");

            Assert.That(complexKeyDictionary.Count, Is.EqualTo(3));
        }

        [Test]
        public void RemoveByNameTest()
        {
            var complexKeyDictionary = new ComplexKeyDictionary<string, string, int>
            {
                { "102", "Konstantin", 1 },
                { "102", "Petya", 2 },
                { "105", "Konstantin", 3 },
                { "105", "Nikolay", 4 },
                { "100", "TestName", 5 }
            };
            complexKeyDictionary.RemoveByName("Konstantin");

            Assert.That(complexKeyDictionary.Count, Is.EqualTo(3));
        }

        [Test]
        public void GetValuesByIdTest()
        {
            var complexKeyDictionary = new ComplexKeyDictionary<string, string, int>
            {
                { "102", "Konstantin", 1 },
                { "102", "Petya", 2 },
                { "105", "Konstantin", 3 },
                { "105", "Nikolay", 4 },
                { "100", "TestName", 5 }
            };
            var values = new List<int>
            {
                1,
                2
            };

            Assert.That(complexKeyDictionary.GetValuesById("102"), Is.EqualTo(values));
        }

        [Test]
        public void GetValuesByNameTest()
        {
            var complexKeyDictionary = new ComplexKeyDictionary<string, string, int>
            {
                { "102", "Konstantin", 1 },
                { "102", "Petya", 2 },
                { "105", "Konstantin", 3 },
                { "105", "Nikolay", 4 },
                { "100", "TestName", 5 }
            };
            var values = new List<int>
            {
                1,
                3
            };

            Assert.That(complexKeyDictionary.GetValuesByName("Konstantin"), Is.EqualTo(values));
        }

        [Test]
        public void GetValueByComplexKey()
        {
            var complexKeyDictionary = new ComplexKeyDictionary<string, string, int>
            {
                { "102", "Konstantin", 1 },
                { "102", "Petya", 2 },
                { "105", "Konstantin", 3 },
                { "105", "Nikolay", 4 },
                { "100", "TestName", 5 }
            };
            var complexKey = new ComplexKey<string, string>("100", "TestName");
            var value = 5;

            Assert.That(complexKeyDictionary.GetValueByKey(complexKey), Is.EqualTo(value));
        }

        [Test]
        public void GetValueByIdNameKey()
        {
            var complexKeyDictionary = new ComplexKeyDictionary<string, string, int>
            {
                { "102", "Konstantin", 1 },
                { "102", "Petya", 2 },
                { "105", "Konstantin", 3 },
                { "105", "Nikolay", 4 },
                { "100", "TestName", 5 }
            };
            var value = 3;

            Assert.That(complexKeyDictionary.GetValueByKey("105", "Konstantin"), Is.EqualTo(value));
        }


        [Test]
        public void EnumerableTest()
        {
            var complexKeyDictionary = new ComplexKeyDictionary<string, string, int>
            {
                { "102", "Konstantin", 1 },
                { "102", "Petya", 2 },
                { "105", "Konstantin", 3 },
                { "105", "Nikolay", 4 },
                { "100", "TestName", 5 }
            };

            var iterations = 0;

            foreach (var value in complexKeyDictionary) iterations++;

            Assert.That(iterations, Is.EqualTo(5));
        }

        [Test]
        public void DuplicatesIdTest()
        {
            var complexKeyDictionary = new ComplexKeyDictionary<string, string, int>
            {
                { "102", "Konstantin", 1 },
                { "102", "Petya", 2 }
            };

            var dictionaryCount = complexKeyDictionary.Count();

            var isExceptionThrown = false;
            try
            {
                complexKeyDictionary.Add("102", "Konstantin", 1);
            }
            catch (Exception)
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
                { "101", "Konstantin", 1 },
                { "102", "Petya", 2 },
                { "103", "Vladimir", 3 },
                { "104", "Nikolay", 4 },
                { "105", "TestName", 5 }
            };
            var initCount = complexKeyDictionary.Count();

            var firstThreadWithState = new ThreadWithState("115", "Ilay", 6, complexKeyDictionary);
            var secondThreadWithState = new ThreadWithState("116", "Bilain", 7, complexKeyDictionary);
            var thirdThreadWithState = new ThreadWithState("117", "Bezya", 8, complexKeyDictionary);

            var firstThread = new Thread(firstThreadWithState.ThreadProc);
            var secondThread = new Thread(secondThreadWithState.ThreadProc);
            var thirdThread = new Thread(thirdThreadWithState.ThreadProc);
            firstThread.Start();
            secondThread.Start();
            thirdThread.Start();

            Thread.Sleep(10);
            Assert.AreEqual(initCount + 3, complexKeyDictionary.Count());
        }
    }
}
