using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace partiel2
{
    class exo1
    {
        public static void WriteInfFile(string filename, string s)
        {
            StreamWriter str = new StreamWriter(filename);
            str.WriteLine(s);
            str.Close();
        }

        public static string MyReplace(string s, string replace, string value)
        {
            string T = "";
            int begin = 0;
            int end = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (replace[0] == s[i])
                    for (int j = 0; j < replace.Length; j++)
                        if (s[j + i] == replace[j])
                        {
                            begin = i;
                            T += replace[j];
                        }
                if (replace == T)
                {
                    end = i;
                    break;
                }
            }
            T = "";

            for (int i = 0; i < begin; i++)
                T += s[i];
            for (int i = 0; i < value.Length; i++)
                T += value[i];
            for (int i = 0; i < end; i++)
                T += s[begin + replace.Length + i];
            for (int i = 0; i + end + replace.Length < s.Length; i++)
                T += s[replace.Length + end + i];

            return T;
        }
    }

    class Point
    {
        int x, y;
        string color;

        public Point(int x, int y, string color)
        {
            this.x = x;
            this.y = y;
            this.color = color;
        }

        public override string ToString()
        { return ("Point(" + x + ", " + y + ", " + color + ")"); }

    }
}
