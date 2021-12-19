using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace PSD_Lab_3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int hashLength;

            while (true)
            {
                Console.Write("Enter amount of bits: ");
                hashLength = int.Parse(Console.ReadLine());

                if (hashLength == 2 || hashLength == 4 || hashLength == 8)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Input is not valid\n");
                }
            }

            Console.WriteLine($"Hash len: {hashLength}");

            //var firstFilePath = "first.jpg";
            //var secondFilePath = "second.jpg";
            //var thirdFilePath = "third.jpg";

            var firstFilePath = "aaa.txt";
            var secondFilePath = "bbb.txt";
            var thirdFilePath = "ccc.txt";

            var firstHash = HashOfFile(firstFilePath, hashLength);
            var secondHash = HashOfFile(secondFilePath, hashLength);

            Console.WriteLine($"First file: {firstHash}");
            Console.WriteLine($"Second file: {secondHash}");

            if (firstHash != secondHash)
            {
                ChangeFile(secondFilePath, thirdFilePath, firstHash);
            }

            var thirdHash = HashOfFile(thirdFilePath, hashLength);

            Console.WriteLine($"Third file: {thirdHash}");
        }

        public static void ChangeFile(string originFilePath, string targetFilePath, string targetHash)
        {
            var fileBytes = File.ReadAllBytes(originFilePath);
            var hash = HashOfFile(originFilePath, targetHash.Length);

            int i = 0;
            var random = new Random();

            while (hash != targetHash)
            {
                fileBytes[fileBytes.Length / 2 + i] = (byte)random.Next(0, 255);
                i++;

                hash = HashOfBytes(fileBytes, targetHash.Length);
            }

            Console.WriteLine($"Iterations: {i}");

            File.WriteAllBytes(targetFilePath, fileBytes);
        }

        public static string HashOfFile(string path, int bitsLength)
        {
            var fileBytes = File.ReadAllBytes(path);

            return HashOfBytes(fileBytes, bitsLength);
        }

        public static string HashOfBytes(byte[] bytes, int bitsLength)
        {
            using var sha1 = SHA1.Create();
            var hash = sha1.ComputeHash(bytes);
            var lastByte = hash[^1];

            var bits = ToBitArray(lastByte);

            //Console.WriteLine($"{path}:\n   {string.Join("", bits)}");

            return string.Join("", bits.Skip(8 - bitsLength).Take(bitsLength));
        }

        private static int[] ToBitArray(int byteValue)
        {
            var result = new int[8];
            var bits = new BitArray(new byte[] { (byte)byteValue });

            for (int i = 0; i < 8; i++)
            {
                result[7 - i] = bits[i] ? 1 : 0;
            }

            return result;
        }
    }
}
