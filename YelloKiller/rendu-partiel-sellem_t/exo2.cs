using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace partiel2
{
    class exo2
    {
        static void Main(string[] args)
        {
            string sh = "SHmelano> ";
            bool on = true;

            while (on)
            {
                Console.Write(sh);

                string get = "";
                get += Console.ReadLine();
                string[] array = new string[100];
                array = get.Split(' ');               

                if (array[0] == "quit")
                    on = false;
                else if (array[0] == "cat") { cat(array); }
                else if (array[0] == "echo") { echo(array); }
                else if (array[0] == "rotx") { rotx(array); }
                else if (array[0] == "derotx") { derotx(array); }
                else if (array[0] == "cksum") { cksum(array);}              
                else
                    Console.WriteLine("SHmelano : command not found");

            }
        }

        static void cat(string[] args)
        {
            string filename = args[1];
            try
            {
               
                StreamReader file = new StreamReader(filename);
                string str = "";
                while (!file.EndOfStream)
                    str += file.ReadLine() + "\n";
                Console.WriteLine(str);
            }
            catch (FileNotFoundException)
            { Console.WriteLine("This file : " + filename + " does not exist, please write an existing file"); }

        }

        static void echo(string[] args)
        {
            string message = "";
            for (int i = 1; i < args.Count(); i++)
                message += args[i] + " ";
            Console.WriteLine(message);
        }

        static void rotx(string[] args)
        {
            try
            {
                string str = args[1];
                int x = Convert.ToInt32(args[2]);

                string T = "";
                for (int i = 0; i < str.Length; i++)
                {
                    int s = (int)str[i];
                    if (s < 65 || (s > 90 && s < 97) || s > 122)
                    { break; }
                    T += (char)(s + x);
                }

                if (T.Length == str.Length)
                    Console.WriteLine(T);
                else
                    Console.WriteLine("Error , string contains char different of a to z");
            }
            catch
            { Console.WriteLine("Please write : derotx string int"); }
        }

        static void derotx(string[] args)
        {
            try
            {
                string str = args[1];
                int x = Convert.ToInt32(args[2]);

                string T = "";
                for (int i = 0; i < str.Length; i++)
                {
                    int s = (int)str[i];
                    if (s < 65 || (s > 90 && s < 97) || s > 122)
                    { break; }
                    T += (char)(s - x);
                }

                if (T.Length == str.Length)
                    Console.WriteLine(T);
                else
                    Console.WriteLine("Error , string contains char different of a to z");
            }
            catch
            { Console.WriteLine("Please write : derotx string int"); }
        }

        static void cksum(string[] args)
        {
            string message = "";
            for (int i = 1; i < args.Count(); i++)
                message += args[i] + " ";

            int ascii = 0;
            for (int i = 0; i < message.Length; i++)
            {
                int s = (int)message[i];
                ascii += s;                
            }
            ascii = ascii % 256;

            Console.WriteLine(ascii);
        }
    }
}
