namespace Collection2Gis
{
    using System;
    using Collection2Gis.Dictionary;

    internal class Program
    {
        public static void Main(string[] args)
        {
            var dict = new ComplexKeyDictionary<int, string, int>();
            var key = new ComplexKey<int, string>(5, "dddd");
            dict.Add(key, 5);
            Console.WriteLine(key.ToString());
        }
    }
}
