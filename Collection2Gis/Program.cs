namespace Collection2Gis
{
    using System;

    internal class Program
    {
        public static void Main(string[] args)
        {
            var key = new ComplexKey<int, string>(5, "dddd");
            Console.WriteLine(key.ToString());
        }
    }
}
