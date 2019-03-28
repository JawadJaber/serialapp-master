

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
            SerialDevice mySer = new SerialDevice("/dev/tty.usbserial-AL00YYL3", BaudRate.B57600);
            mySer.DataReceived += MySer_DataReceived;
            mySer.Open();

            mySer.Write(new byte[] { 0x01, 0x03, 0x83, 0x04, 0x00, 0x01, 0xCF, 0xC9 });

            ModbusRTU mrtu = new ModbusRTU();

            for (ushort i = 30000; i < 30050; i++)
            {
                var v = new short[13];
                var message = new byte[8];
                mrtu.BuildMessage(Convert.ToByte(1), Convert.ToByte(3), i,1, ref message);
                mySer.Write(message);

                mySer.Read(message);
                
            }

            

          
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

            Console.ReadLine();

            //if (BitConverter.ToString(arg2).Length > 2)
            //{
            //    Console.WriteLine($"Received BitConverter.ToSingle(arg2,0: {BitConverter.ToSingle(arg2, 0)}");
            //    Console.WriteLine($"Received BitConverter.ToSingle(arg2,1: {BitConverter.ToSingle(arg2, 1)}");
            //}
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


    //---------------------------------------------------------------
    // ModbusRTU
    //
    // This is used in serial communication & makes use of a compact,
    // binary representation of the data for protocol communication.
    //
    // 2013 Rectius Informática Ltda -  http://www.rectius.com.br
    //---------------------------------------------------------------


    public class ModbusRTU
    {
       
        public string modbusStatus;

        #region Constructor / Deconstructor
        public ModbusRTU()
        {
        }
        ~ModbusRTU()
        {
        }
        #endregion

     

        #region CRC Computation
        public void GetCRC(byte[] message, ref byte[] CRC)
        {
            //Function expects a modbus message of any length as well as a 2 byte CRC array in which to 
            //return the CRC values:

            ushort CRCFull = 0xFFFF;
            byte CRCHigh = 0xFF, CRCLow = 0xFF;
            char CRCLSB;

            for (int i = 0; i < (message.Length) - 2; i++)
            {
                CRCFull = (ushort)(CRCFull ^ message[i]);

                for (int j = 0; j < 8; j++)
                {
                    CRCLSB = (char)(CRCFull & 0x0001);
                    CRCFull = (ushort)((CRCFull >> 1) & 0x7FFF);

                    if (CRCLSB == 1)
                        CRCFull = (ushort)(CRCFull ^ 0xA001);
                }
            }
            CRC[1] = CRCHigh = (byte)((CRCFull >> 8) & 0xFF);
            CRC[0] = CRCLow = (byte)(CRCFull & 0xFF);
        }
        #endregion

        #region Build Message
        public void BuildMessage(byte address, byte type, ushort start, ushort registers, ref byte[] message)
        {
            //Array to receive CRC bytes:
            byte[] CRC = new byte[2];

            message[0] = address;
            message[1] = type;
            message[2] = (byte)(start >> 8);
            message[3] = (byte)start;
            message[4] = (byte)(registers >> 8);
            message[5] = (byte)registers;

            GetCRC(message, ref CRC);
            message[message.Length - 2] = CRC[0];
            message[message.Length - 1] = CRC[1];

            Console.WriteLine($"message = {BitConverter.ToString(message)}");
        }
        #endregion

        #region Check Response
        private bool CheckResponse(byte[] response)
        {
            //Perform a basic CRC check:
            Console.WriteLine($"response {BitConverter.ToString(response)}");
            byte[] CRC = new byte[2];
            GetCRC(response, ref CRC);
            if (CRC[0] == response[response.Length - 2] && CRC[1] == response[response.Length - 1])
                return true;
            else
                return false;
        }
        #endregion

      
    }




}
