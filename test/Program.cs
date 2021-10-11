using System;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            var p1 = new TestPerson("Vlad");
            var p2 = p1;

            p2.Name = "Vasile";

            Console.WriteLine(p1.Name);

            Console.ReadLine();

        }
    }
}
