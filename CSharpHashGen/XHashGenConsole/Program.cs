using System;
using XHashGen;

namespace HashGen
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length != 1)
            {
                Do();
                return;
            }

            if (args[0] == "-help")
            {
                Help();
            }

            var input = args[0];
            var hash = XHash.Create(input);
            Console.WriteLine(hash);
        }

        private static void Help()
        {
            Console.WriteLine("HASH Help:");
            Console.WriteLine("输入一个明文，输出一个密文。");
            Console.WriteLine("任意键继续。。。");
            Console.ReadKey();
        }

        private static void Do()
        {
            while (true)
            {
                Console.Write("请输入明文：");
                var input = Console.ReadLine();
                if (input == null) break;
                var hash = XHash.Create(input);
                Console.WriteLine("Hash：");
                Console.WriteLine(hash);
                Console.WriteLine();
                Console.Write("按y继续，否则退出：");
                var key = Console.ReadKey();
                if (key.KeyChar != 'y') break;
            }
        }
    }
}
