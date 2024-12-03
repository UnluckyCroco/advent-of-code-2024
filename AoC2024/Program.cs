using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC2024
{
    internal abstract class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine(Ex1A());
            Console.WriteLine(Ex1B());
            Console.WriteLine(Ex2A());
            Console.WriteLine(Ex2B());
            Console.WriteLine(Ex3A());
            Console.WriteLine(Ex3B());
        }

        private static void PrintList(List<int> list, string separator = " ", string end = "\n")
        {
            foreach (var i in list)
            {
                Console.Write(i + separator);
            }

            Console.Write(end);
        }

        private static void PrintList(List<string> list, string separator = " ", string end = "\n")
        {
            foreach (var i in list)
            {
                Console.Write(i + separator);
            }

            Console.Write(end);
        }

        private static int Ex2A()
        {
            var lines = File.ReadAllLines("./input/2.txt").ToList();

            var newLines = lines.Select(line =>
                line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList());

            var counter = 0;
            foreach (var line in newLines)
            {
                var isAsc = line[0] < line[line.Count - 1] ? -1 : 1;
                counter++;
                for (var i = 0; i < line.Count - 1; i++)
                {
                    if ((line[i] - line[i + 1]) * isAsc > 3 || (line[i] - line[i + 1]) * isAsc < 1)
                    {
                        counter--;
                        break;
                    }

                    if ((isAsc != 1 || line[i] >= line[i + 1]) && (isAsc != -1 || line[i] <= line[i + 1])) continue;
                    counter--;
                    break;
                }
            }

            return counter;
        }

        private static int Ex1A()
        {
            var lines = File.ReadAllLines("./input/1.txt").ToList();

            var firstList = new List<int>();
            var secondList = new List<int>();
            lines.Select(line => line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)).ToList()
                .ForEach(ball =>
                {
                    firstList = firstList.Append(Convert.ToInt32(ball[0])).ToList();
                    secondList = secondList.Append(Convert.ToInt32(ball[1])).ToList();
                });

            firstList.Sort();
            secondList.Sort();

            return firstList.Select((t, i) => Math.Abs(t - secondList[i])).Sum();
        }

        private static int Ex1B()
        {
            var lines = File.ReadAllLines("./input/1.txt").ToList();

            var firstList = new List<int>();
            var secondList = new List<int>();
            lines.Select(line => line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)).ToList()
                .ForEach(ball =>
                {
                    firstList = firstList.Append(Convert.ToInt32(ball[0])).ToList();
                    secondList = secondList.Append(Convert.ToInt32(ball[1])).ToList();
                });

            return firstList.Select(t => secondList.FindAll(e => e == t).Count * t).Sum();
        }
        
        private static int Ex2B()
        {
            var lines = File.ReadAllLines("./input/2.txt").ToList();

            var newLines = lines.Select(line =>
                line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList()).ToList();

            return newLines.Select(Fuck).Count(ret => ret);
        }

        private static bool Fuck(List<int> line)
        {
            var originalLine = new List<int>(line);
            var isAsc = line[0] < line[line.Count - 1] ? -1 : 1;
            var hasErrored = -1;
            var tryOther = false;
            for (var i = 0; i < line.Count - 1; i++)
            {
                if ((line[i] - line[i + 1]) * isAsc > 3 || (line[i] - line[i + 1]) * isAsc < 1)
                {
                    if (hasErrored == -1)
                    {
                        line.RemoveAt(i);
                        hasErrored = i;
                        i = -1;
                        continue;
                    }

                    if (tryOther) return false;

                    tryOther = true;
                    line = originalLine;
                    line.RemoveAt(hasErrored + 1);
                    i = -1;
                    continue;
                }

                if ((isAsc != 1 || line[i] >= line[i + 1]) && (isAsc != -1 || line[i] <= line[i + 1])) continue;
                if (hasErrored == -1)
                {
                    line.RemoveAt(i);
                    hasErrored = i;
                    i = -1;
                    continue;
                }

                if (tryOther) return false;

                tryOther = true;
                line = originalLine;
                line.RemoveAt(hasErrored + 1);
                i = -1;
            }

            return true;
        }
        
        private static int Ex3A()
        {
            var lines = File.ReadAllLines("./input/3.txt").ToList();

            return lines.Select(line => Regex.Matches(line, @"mul\(\d{1,3},\d{1,3}\)", RegexOptions.None).Cast<Match>()
                .Select(m => m.Value.Substring(4).Replace(")", string.Empty).Split(','))
                .Aggregate(0, (total, q) => int.Parse(q[0]) * int.Parse(q[1]) + total)).Sum();
        }

        private static int Ex3B()
        {
            var lines = File.ReadAllLines("./input/3.txt").ToList();

            var ret = lines.Select(line => Regex
                .Matches(line, @"(mul\(\d{1,3},\d{1,3}\))|(don't\(\))|(do\(\))", RegexOptions.None).Cast<Match>()
                .Select(m => m.Value));

            var doMul = true;
            var total = 0;
            foreach (var s in ret.SelectMany(r => r))
            {
                switch (s)
                {
                    case "don't()":
                        doMul = false;
                        break;
                    case "do()":
                        doMul = true;
                        break;
                    default:
                        if (doMul)
                        {
                            var spl = s.Substring(4).Replace(")", string.Empty).Split(',').Select(int.Parse)
                                .Aggregate((curr, next) => curr * next);
                            total += spl;
                        }

                        break;
                }
            }

            return total;
        }
    }
}