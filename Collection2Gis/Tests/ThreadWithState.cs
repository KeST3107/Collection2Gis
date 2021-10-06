namespace Collection2Gis.Tests
{
    using System;
    using Collection2Gis.Dictionary;

    public class ThreadWithState
    {
        // State information used in the task.
        private string Id;
        private string Name;
        private int Value;
        public ComplexKeyDictionary<string, string, int> Dictionary { get; set; }

        // The constructor obtains the state information.
        public ThreadWithState(string id, string name, int value, ComplexKeyDictionary<string, string, int> dictionary)
        {
            Id = id;
            Name = name;
            Value = value;
            Dictionary = dictionary;
        }

        // The thread procedure performs the task, such as formatting
        // and printing a document.
        public void ThreadProc()
        {
            Dictionary.Add(Id, Name, Value);
            Console.WriteLine("Поток с записью ID = {0}, Name = {1}, Value = {2}", Id, Name, Value);
        }
    }
}
