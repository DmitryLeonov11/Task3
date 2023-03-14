using SHA3.Net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.IO;
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
            Generator generator = new Generator();
            Rules rules = new Rules(args);
            HelpTableGenerator helpTableGenerator = new HelpTableGenerator();
            string key = generator.Generate().Replace("-", "");
            string hmac;
            int computerChoice = new Random().Next(0, args.Length);
            int choice = -1;
            hmac = generator.GenerateHMAC(key, args[computerChoice]);
            Console.WriteLine("HMAC: " + hmac);
            while (choice != 0)
            {
                Console.WriteLine("Available moves:");
                for (int i = 0; i < args.Length; i++)
                {
                    Console.WriteLine(i + 1 + " - " + args[i]);
                }
                Console.WriteLine(0 + " - exit");
                Console.WriteLine("? - help");;
                Console.Write("Enter your move: ");
                string buff = Console.ReadLine();
                bool result = int.TryParse(buff, out choice);
                if (result)
                {
                    if (choice < 0 || choice > args.Length)
                        Console.WriteLine("Wrong move. Select one of the items from the menu");
                    else if(choice != 0)
                    {
                        Console.WriteLine("Your move: " + args[choice - 1]);
                        Console.WriteLine("Computer move: " + args[computerChoice]);
                        break;
                    }
                }
                else if(buff.Equals("?"))
                {
                    choice = -1;
                    helpTableGenerator.GetHelpTable(args, rules);
                }
                else
                {
                    choice = -1;
                    Console.WriteLine("Wrong move. Select one of the items from the menu");
                }
            }

            if (choice != 0)
            {
                WinResolver.WhoWillWin(choice - 1, computerChoice, rules);
                Console.WriteLine("Your choice: exit");
            }
            Console.WriteLine("HMAC key: " + key);
        }
    }

    public class Generator
    {
        private Sha3 sha3 = Sha3.Sha3256();
        private RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
        public string Generate()
        {
            byte[] randomBytes = new byte[32];
            randomNumberGenerator.GetBytes(randomBytes);
            string key = BitConverter.ToString(randomBytes);
            return key;
        }

        public string GenerateHMAC(string key, string variant)
        {
            Stream stream = GenerateStreamFromString(key + variant);
            string hash = BitConverter.ToString(sha3.ComputeHash(stream)).Replace("-", "");
            return hash;
        }

        private Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }

    public static class WinResolver
    {
        public static void WhoWillWin(int you, int computer, Rules rules)
        {
            int[,] table = rules.GetTable();
            if (table[you, computer] == 0)
            {
                Console.WriteLine("Draw");
            }
            if (table[you, computer] == 1)
            {
                Console.WriteLine("You win!");
            }
            if (table[you, computer] == -1)
            {
                Console.WriteLine("You lose!");
            }
        }
    }

    public class Rules
    {
        private static int[,] winTable;

        public Rules(string[] args)
        {
            int[] res = new int[args.Length];
            res[0] = 0;
            winTable = new int[args.Length,args.Length];
            for (int i = 1; i < args.Length; i++)
            {
                if( i < (args.Length - ((args.Length - 1)/2)))
                {
                    res[i] = -1;
                } 
                else
                {
                    res[i] = 1;
                }
            }
            Console.WriteLine();
            for (int i = 0; i < args.Length; i++)
            {
                for (int j = 0; j < args.Length; j++)
                {
                    winTable[i,j] = res[j];
                }
                int temp = res[args.Length - 1];
                for (int k = args.Length - 2; k >= 0; k--)
                {
                    res[k + 1] = res[k];
                }
                res[0] = temp;
            }
        }

        public int[,] GetTable()
        {
            return winTable;
        }
    }

    public class HelpTableGenerator
    {
        private string[,] helpTable;
        public void GetHelpTable(string[] args, Rules rules)
        {
            int[,] table = rules.GetTable();
            helpTable = new string[table.GetLength(1) + 1, table.GetLength(1) + 1];
            helpTable[0, 0] = "  ";
            for (int i = 1; i < helpTable.GetLength(1); i++)
            {
                helpTable[0, i] = args[i - 1];
                helpTable[i, 0] = args[i - 1];
            }
            for (int i = 1; i < helpTable.GetLength(1); i++)
            {
                for (int j = 1; j < helpTable.GetLength(1); j++)
                {
                    if (table[i - 1, j - 1] == 0)
                    {
                        helpTable[i, j] = "Draw";
                    }
                    if (table[i - 1, j - 1] == -1)
                    {
                        helpTable[i, j] = "Lose";
                    }
                    if (table[i - 1, j - 1] == 1)
                    {
                        helpTable[i, j] = "Win";
                    }
                }
            }
            for (int i = 0; i < helpTable.GetLength(1); i++)
            {
                for (int j = 0; j < helpTable.GetLength(1); j++)
                {
                    Console.Write("{0,10} ", helpTable[i, j]);
                }
                Console.WriteLine();
            }
        }
    }
}
