using System;
using Collection2Gis.Dictionary;

namespace Collection2Gis.Tests
{
    public class ThreadWithState
    {
        private readonly string Id;
        private readonly string Name;
        private readonly int Value;

        public ThreadWithState(string id, string name, int value, ComplexKeyDictionary<string, string, int> dictionary)
        {
            Id = id;
            Name = name;
            Value = value;
            Dictionary = dictionary;
        }

        public ComplexKeyDictionary<string, string, int> Dictionary { get; set; }

        public void ThreadProc()
        {
            Dictionary.Add(Id, Name, Value);
            Console.WriteLine("Поток с записью ID = {0}, Name = {1}, Value = {2}", Id, Name, Value);
        }
    }
}
