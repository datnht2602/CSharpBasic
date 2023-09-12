using System;
using System.Collections.Generic;
using Contract;
using AlexDataAnalyser;
using System.IO;
using static System.Environment;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpBasic
{
    class Program
    {
        static void Main(string[] args)
        {
            string currentPath = Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
            string folderPath = Path.Combine(currentPath, "Data");
        
            Console.WriteLine(folderPath);
            List<IDataAnalyser> Analysers = new List<IDataAnalyser>();
           
            Analysers.Add(new AlexAnalyser(folderPath));
            Analysers.Add(new DerekAnalyser(folderPath));

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            foreach (IDataAnalyser analyser in Analysers)
            {
                Console.WriteLine($"Author is {analyser.Author} ");
                foreach (string str in analyser.GetTopTenStrings(analyser.Path))
                {
                    Console.WriteLine("String: " + str);
                }
            }
            stopwatch.Stop();

            // Get the elapsed time
            TimeSpan elapsed = stopwatch.Elapsed;

            Console.WriteLine("Taken time: " + elapsed.TotalMilliseconds + " milliseconds");
            Console.WriteLine("Press any ken to exit.");
            Console.ReadKey();
        }
    }
}
