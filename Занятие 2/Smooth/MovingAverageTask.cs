using System;
using System.Collections.Generic;
using System.Linq;

namespace yield
{
    public static class MovingMaxTask
    {
    }

    class Program
    {
        static void Main(string[] args)
        {
            var data = new List<int> { 2, 6, 2, 1, 3, 2, 5, 8, 1 };
            var windowWidth = 5;

            var movingMax = data.MovingMax(windowWidth);

            foreach (var max in movingMax)
            {
                Console.WriteLine(max);
            }
        }
    }
}