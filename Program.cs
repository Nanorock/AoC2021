﻿
using System.Diagnostics;
using System.Reflection;

namespace AdventOfCode_2021
{
    class Program
    {
        static void Main(string[] args)
        {
            RunLatest(false);
            //WatchRunAll();
            while(true)
                Console.ReadLine();
        }

        static void WatchRunAll()
        {
            double totalTime = 0;
            for (int i = 1; i <= AdventOfCode.LatestDay(); i++)
                totalTime+=WatchRun(i);
            ColorConsole.RainbowLine($"########################");
            ColorConsole.RainbowLine($"###TOTAL TIME:{totalTime:F2}ms###");
            ColorConsole.RainbowLine($"########################");
        }
        static double WatchRun(int day)
        {
            ColorConsole.PrintLine($"<Blue>Running {AdventOfCode.GetName(day)}");
            var sw = Stopwatch.StartNew();
            Run(day);
            sw.Stop();
            ColorConsole.PrintLine($"<Blue>Ran in <Red>{sw.Elapsed.TotalMilliseconds}ms");
            Console.WriteLine();
            return sw.Elapsed.TotalMilliseconds;
        }
        
        static void RunLatest(bool example = false, ERunPart runPart = ERunPart.Both) { Run(AdventOfCode.LatestDay(), example, runPart); }
        static void Run(int day, bool example = false, ERunPart runPart = ERunPart.Both)
        {
            var aoc = AdventOfCode.Create(day, example);
            if (aoc == null) {
                Console.WriteLine($"No aoc class for day {day}");
                return;
            }
            Run(aoc, runPart);
        }
        static void Run(AdventOfCode aoc, ERunPart runPart = ERunPart.Both)
        {
            aoc.Init();
            if (runPart != ERunPart.Part2)
                aoc.Run1();
            if (runPart != ERunPart.Part1)
                aoc.Run2();
        }
    }
    enum ERunPart { Both, Part1, Part2 }
    abstract class AdventOfCode
    {
        static Dictionary<int, string> _aocNames;
        static Dictionary<int, AdventOfCode> _aocTypes;
        static Dictionary<int, AdventOfCode> AoCs
        {
            get
            {
                if (_aocTypes == null)
                {
                    _aocTypes = new Dictionary<int, AdventOfCode>();
                    _aocNames = new Dictionary<int, string>();
                    var types = Assembly.GetCallingAssembly().GetTypes();
                    for (int i = 0; i < types.Length; i++)
                    {
                        if (types[i].BaseType != typeof(AdventOfCode)) continue;
                        var day = int.Parse(new string(types[i].Name.Where(char.IsDigit).ToArray()));
                        AoCs[day] = Activator.CreateInstance(types[i]) as AdventOfCode;
                        _aocNames[day] = types[i].Name;
                    }
                }
                return _aocTypes;
            }
        }
        public static int LatestDay() => AoCs.Select(kvp => kvp.Key).Max();
        public static string GetName(int day) => AoCs != null && _aocNames.TryGetValue(day, out var name) ? name : "Unknown";
        public static AdventOfCode Create(int aocDay, bool example)
        {
            if (AoCs.TryGetValue(aocDay, out var aoc))
            {
                aoc.SetInput(example);
                return aoc;
            }
            return null;
        }
        
        protected string[] inputFile;
        void SetInput(bool example = false)
        {
            string number = "";
            string name = GetType().Name;
            for (int i = 0; i < name.Length; i++)
                if (char.IsDigit(name[i]))
                    number += name[i];

            string file = $"C:/Users/Nanorock/source/repos/AoC2021/INPUT/aoc_{number}";
            if (example) file += "_ex";
            inputFile = File.ReadAllLines($"{file}.txt");
        }


        public virtual void Init() { }
        public virtual void Run1() { }
        public virtual void Run2() { }

        protected static void WriteLine(string line) => Console.WriteLine(line);
    }
}
