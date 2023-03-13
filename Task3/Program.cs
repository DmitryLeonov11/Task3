using SHA3.Net;
using System;
using System.Collections.Generic;
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
            WinTableGenerator winTableGenerator = new WinTableGenerator();
            Rules rules = new Rules();
            HelpTableGenerator helpTableGenerator = new HelpTableGenerator();
            string key = generator.Generate().Replace("-", "");
            string HMAC = generator.GenerateHMAC(key,"hello");
            Console.WriteLine("HMAC: " + HMAC);
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
