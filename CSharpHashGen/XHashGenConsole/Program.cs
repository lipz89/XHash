using System;
using XHashGen;

namespace HashGen
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                Do();
                return;
            }

            if (args.Length != 2)
            {
                if (args.Length == 1 && args[0] == "-help")
                {
                    Help();
                }
                return;
            }

            var publicKey = args[0];
            var tick = args[1];
            var hash = XHash.CreateAuthorize(tick, publicKey);
            Console.WriteLine(hash);
        }

        private static void Help()
        {
            Console.WriteLine("HASH Help:");
            Console.WriteLine("请输入两个参数，分别为PublicKey和Tick。");
            Console.ReadLine();
        }

        private static void Do()
        {
            while (true)
            {
                Console.Write("请输入PunlicKey：");
                var publicKey = Console.ReadLine();
                if (publicKey == null) break;
                Console.Write("请输入Tick：");
                var tick = Console.ReadLine();
                if (tick == null) break;
                var hash = XHash.CreateAuthorize(tick, publicKey);
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
