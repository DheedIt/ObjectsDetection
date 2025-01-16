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
           
           
                int res = Solution.ObjectsCount("C:\\Users\\zhula\\source\\repos\\RomansTask\\RomansTask\\Pictures\\img101.png", "C:\\Users\\zhula\\source\\repos\\RomansTask\\RomansTask\\Pictures\\img200.png",150);
                Console.WriteLine(res);
            
        }
    }

}


