

using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using NModbus;
using NModbus.IO;

namespace serialapp
{
    class Program
    {

        static IEnumerable<string> GetNextChars(string str, int iterateCount)
        {
            var words = new List<string>();

            for (int i = 0; i < str.Length; i += iterateCount)
                if (str.Length - i >= iterateCount) words.Add(str.Substring(i, iterateCount));
                else words.Add(str.Substring(i, str.Length - i));

            return words;
        }



        static void Main(string[] args)
        {

            string address = "42-40-42-40-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-40-55-B1-B1-8D-81-9B-95-C9-4B-85-B1-81-83-1A-C9-A3-0A-06-00-00-00-00-00-00-00-00-00-00-00-00-00";



            Console.WriteLine($"Received BitConverter.ToSingle(arg2,0: {BitConverter.ToSingle(FromHex(address), 0)}");
            Console.WriteLine($"Received BitConverter.ToSingle(arg2,1: {BitConverter.ToSingle(FromHex(address), 1)}");
            //Console.WriteLine($"Received BitConverter.ToSingle(arg2,0: {BitConverter.ToSingle(FromHex("4A"), 0)}");
            //Console.WriteLine($"Received BitConverter.ToSingle(arg2,1: {BitConverter.ToSingle(FromHex("4A"), 1)}");



            foreach (var item in GetNextChars(address, 8))
            {
                var base10 = int.Parse(item.Replace("-", ""), System.Globalization.NumberStyles.HexNumber);
                Console.WriteLine($"base10 = {base10}");
            }

            foreach (var item in new string[] { "002A", "0049", "40A4", "4240", "1A" })
            {
                var base10 = Int64.Parse(item, System.Globalization.NumberStyles.HexNumber);
                Console.WriteLine($"base10 = {base10}");
            }
            Console.ReadLine();

            Console.WriteLine("Hello Serial port!");
            Console.WriteLine("Attach debugger and press any key to continue");
            Console.ReadLine();
            var ports = SerialDevice.GetPortNames();
            bool isTTY = false;
            foreach (var prt in ports)
            {
                Console.WriteLine($"Serial name: {prt}");
                if (prt.Contains("ttyS0"))
                {
                    isTTY = true;
                }
            }
            if (!isTTY)
            {
                Console.WriteLine("No ttyS0 serial port!");
                return;
            }
            Console.WriteLine("Yes, we have the embedded serial port available, opening it");
            SerialDevice mySer = new SerialDevice("/dev/ttyS0", BaudRate.B115200);
            mySer.DataReceived += MySer_DataReceived;
            mySer.Open();
            
            mySer.Write(System.Text.Encoding.UTF8.GetBytes("Hello Serial port!"));
            while (!Console.KeyAvailable) ;
            mySer.Close();
        }



        private static void MySer_DataReceived(object arg1, byte[] arg2)
        {
            //Console.WriteLine($"Received: {System.Text.Encoding.UTF8.GetString(arg2)}");
            //Console.WriteLine($"Received Encoding.ASCII.GetString: {Encoding.ASCII.GetString(arg2)}");
            //Console.WriteLine($"Received Unicode.ASCII.GetString: {Encoding.Unicode.GetString(arg2)}");
            //Console.WriteLine($"Received Unicode.UTF32.GetString: {Encoding.UTF32.GetString(arg2)}");
            //Console.WriteLine($"Received Unicode.UTF7.GetString: {Encoding.UTF7.GetString(arg2)}");
            //Console.WriteLine($"Received Unicode.BigEndianUnicode.GetString: {Encoding.BigEndianUnicode.GetString(arg2)}");
            //Console.WriteLine($"Received arg1: {arg1.ToString()}");
            Console.WriteLine($"Received BitConverter.ToString: {BitConverter.ToString(arg2)}");

            if (BitConverter.ToString(arg2).Length > 2)
            {
                Console.WriteLine($"Received BitConverter.ToSingle(arg2,0: {BitConverter.ToSingle(arg2, 0)}");
                Console.WriteLine($"Received BitConverter.ToSingle(arg2,1: {BitConverter.ToSingle(arg2, 1)}");
            }
            //Console.WriteLine($"Received HexadecimalEncoding.FromHexString(BitConverter.ToString(arg2)): {HexadecimalEncoding.FromHexString(BitConverter.ToString(arg2))}");





            var str = BitConverter.ToString(arg2);

            var base10 = int.Parse(str.Replace("-", "").ToString(), System.Globalization.NumberStyles.HexNumber);//Convert.ToString(r, 10);
            Console.WriteLine($"base10 is {base10}");




            //foreach (var item in str.Split("-"))
            //{
            //    var b = short.TryParse(item, out short r);
            //    if (b)
            //    {
            //        var base10 = int.Parse(item, System.Globalization.NumberStyles.HexNumber);//Convert.ToString(r, 10);
            //        Console.WriteLine($"base10 is {base10}");
            //    }
            //    else
            //    {
            //        Console.WriteLine($"not base 16 @ {b}");
            //    }
            //}
        }


        public static byte[] FromHex(string hex)
        {
            hex = hex.Replace("-", "");
            byte[] raw = new byte[hex.Length / 2];
            for (int i = 0; i < raw.Length; i++)
            {
                raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return raw;
        }

        public class HexadecimalEncoding
        {
            public static string ToHexString(string str)
            {
                var sb = new StringBuilder();

                var bytes = Encoding.Unicode.GetBytes(str);
                foreach (var t in bytes)
                {
                    sb.Append(t.ToString("X2"));
                }

                return sb.ToString(); // returns: "48656C6C6F20776F726C64" for "Hello world"
            }

            public static string FromHexString(string hexString)
            {
                var bytes = new byte[hexString.Length / 2];
                for (var i = 0; i < bytes.Length; i++)
                {
                    bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
                }

                return Encoding.Unicode.GetString(bytes); // returns: "Hello world" for "48656C6C6F20776F726C64"
            }
        }




    }


}
