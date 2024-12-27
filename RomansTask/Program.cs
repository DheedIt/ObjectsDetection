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
            var res = cl.ObjectsCount(args[0], args[1]);
            Console.WriteLine(res);
        }
    }

}


