

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

         
           //Console.WriteLine($"Received BitConverter.ToSingle(arg2,0: {BitConverter.ToSingle(FromHex("4A"), 0)}");
            //Console.WriteLine($"Received BitConverter.ToSingle(arg2,1: {BitConverter.ToSingle(FromHex("4A"), 1)}");
            //var intValue = 30002;
            var intValue = 30002.ToString("X");
            Console.WriteLine($"to hex string {intValue}");

            //Console.WriteLine($"from hex {FromHex("01 03 75 31 00 00 0E 09")}");
            //Console.WriteLine($"from hex {HexadecimalEncoding.FromHexString("83 04")}");
        
          
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

            mySer.Write(new byte[] { 0x01, 0x03, 0x83, 0x04, 0x00, 0x01, 0xCF, 0xC9 });
            //mySer.Read(FromHex("01 03 75 31 00 00 0E 09"));
            //mySer.Write(System.Text.Encoding.UTF8.GetBytes("Hello Serial port!"));

            //mySer.
            Console.ReadLine();
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
            hex = hex.Replace(" ", "");
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
