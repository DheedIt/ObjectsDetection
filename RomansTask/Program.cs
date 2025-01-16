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
        static void Main(string[] args,int limiter) =>
                Console.WriteLine(Solution.ObjectsCount(args[0], args[1],limiter));
        static void Main(string[] args, int limiter, bool showDiff) =>
               Console.WriteLine(Solution.ObjectsCount(args[0], args[1], limiter, showDiff));
        static void Main(string[] args) =>
               Console.WriteLine(Solution.ObjectsCount(args[0], args[1]));

    }

}


