using System;
using System.Drawing;
using System.Globalization;
using System.Numerics;
using OpenCvSharp;
using OpenCvSharp.Internal.Vectors;
using RomansTask;

namespace RomansTask
{
    class Program
    {
        static void Main(string[] args) // Source - Template
        {
           
            Solution cl = new Solution();
                int res = cl.ObjectsCount("C:\\Users\\zhula\\source\\repos\\RomansTask\\RomansTask\\Pictures\\img2.png", "C:\\Users\\zhula\\source\\repos\\RomansTask\\RomansTask\\Pictures\\tmp3.png",150);
                Console.WriteLine(res);
            
        }
    }

}


