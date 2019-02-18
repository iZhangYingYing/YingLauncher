using System;
using System.Threading;
using System.Threading.Tasks;

namespace YingTest
{

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(DateTime.Now.ToString());
            Task.Run(() => {
                Thread.Sleep(6040);
                Console.WriteLine(DateTime.Now.ToString());
            });
            Console.ReadKey();


        }
    }
}
