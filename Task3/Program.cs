using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Task3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("The number of arguments should be greater than 2");
                return;
            }
            else if(args.Length % 2 == 0)
            {
                Console.WriteLine("The number of arguments should be odd");
                return;
            }

            for (int i = 0; i < args.Length - 1; i++)
            {
                var str = args[i];
                for (int j = i + 1; j < args.Length; j++)
                {
                    if (str.Equals(args[j]))
                    {
                        Console.WriteLine("Arguments should not be repeated.");
                        return;
                    }
                }
            }
            
            Console.WriteLine("he he");
        }
    }

    public class Generator
    {

    }

    public class WinTableGenerator
    {

    }

    public class Rules
    {

    }

    public class HelpTableGenerator
    {

    }
}
