using System;
using System.Text;
using System.IO;

namespace DigitalSignature
{
    class Program
    {
        static void Main(string[] args)
        {
            BigInteger p = new BigInteger("6277101735386680763835789423207666416083908700390324961279", 10);
            BigInteger a = new BigInteger("-3", 10);
            BigInteger b = new BigInteger("64210519e59c80e70fa7e9ab72243049feb8deecc146b9b1", 16);
            byte[] xG = fromHexStringToByte("03188da80eb03090f67cbf20eb43a18800f4ff0afd82ff1012");
            BigInteger n = new BigInteger("ffffffffffffffffffffffff99def836146bc9b1b4d22831", 16);

            DS ds = new DS(p, a, b, n, xG);
            BigInteger d = ds.privateKeyGen(192);
            CECPoint Q = ds.publicKeyGen(d);
            Stribog256 hash = new Stribog256();


            Console.WriteLine("Выберите файл содержащий сообщение: ");
            String path = Console.ReadLine();
            String message = File.ReadAllText(path);
            byte[] H = hash.GetHash(message);
            string sign = ds.DSGen(H, d);
            Console.WriteLine("Сообщение "+ message + " имеет следующую ЭЦП: "+ sign);
            Console.WriteLine("Выберите файл для сохранение ЭЦП: ");
            path = Console.ReadLine();
            File.WriteAllText(path, sign);

            Console.WriteLine("Выберите файл для верификации сообщения: ");
            path = Console.ReadLine();
            String message2 = File.ReadAllText(path);
            byte[] H2 = hash.GetHash(message2);
            Console.WriteLine("Выберите файл содержащий цифровую подпись: ");
            path = Console.ReadLine();
            string signVer = File.ReadAllText(path);

            bool result = ds.verifyDS(H2, signVer, Q);

            if (result)
                Console.WriteLine("Верификация прошла успешно. Цифровая подпись верна.");
            else
                Console.WriteLine("Верификация не прошла! Цифровая подпись не верна.");

            Console.ReadLine();
        }

        private static byte[] fromHexStringToByte(string input)
        {
            byte[] data = new byte[input.Length / 2];
            string HexByte = "";
            for (int i = 0; i < data.Length; i++)
            {
                HexByte = input.Substring(i * 2, 2);
                data[i] = Convert.ToByte(HexByte, 16);
            }
            return data;
        }

    }
}
