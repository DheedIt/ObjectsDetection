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
        static int Main(string[] args){
           return Solution.ObjectsCount("C:\\Users\\zhula\\source\\repos\\RomansTask\\RomansTask\\Pictures\\img101.png", "C:\\Users\\zhula\\source\\repos\\RomansTask\\RomansTask\\Pictures\\img200.png", 30);
                        // Solution.ObjectsCount("Way-to-image", "Way-to-template-to-be-found", 0);

        }
        

    }

}


