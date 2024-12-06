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
            // Console.WriteLine(Ex1A());
            // Console.WriteLine(Ex1B());
            // Console.WriteLine(Ex2A());
            // Console.WriteLine(Ex2B());
            // Console.WriteLine(Ex3A());
            // Console.WriteLine(Ex3B());
            // Console.WriteLine(Ex4A());
            // Console.WriteLine(Ex4B());
            Console.WriteLine(Ex5A());
            Console.WriteLine(Ex5B());
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
        
        private static int Ex4A()
        {
            var lines = File.ReadAllLines("./input/4.txt").ToList();

            var newLines = new List<string>(from number in Enumerable.Range(0, lines[0].Length) select "");
            var strings = new List<string>();
            for (var i = 0; i < lines.Count; i++)
            {
                for (var j = 0; j < lines[i].Length; j++)
                {
                    newLines[j] += lines[i][j];
                    if (j < lines[i].Length - 3)
                    {
                        strings.Add($"{lines[i][j]}{lines[i][j + 1]}{lines[i][j + 2]}{lines[i][j + 3]}");
                    }

                    if (i >= lines.Count - 3) continue;
                    strings.Add($"{lines[i][j]}{lines[i + 1][j]}{lines[i + 2][j]}{lines[i + 3][j]}");

                    if (j >= 3)
                    {
                        strings.Add($"{lines[i][j]}{lines[i + 1][j - 1]}{lines[i + 2][j - 2]}{lines[i + 3][j - 3]}");
                    }
                    if (j < lines[i].Length - 3)
                    {
                        strings.Add($"{lines[i][j]}{lines[i + 1][j + 1]}{lines[i + 2][j + 2]}{lines[i + 3][j + 3]}");
                    }
                }
            }

            return strings.Count(s => s == "XMAS" || s == "SAMX");
        }
        
        private static int Ex4B()
        {
            var lines = File.ReadAllLines("./input/4.txt").ToList();

            var possibilities = new List<string> {"MMSS", "MSMS", "SSMM", "SMSM"};
            var count = 0;
            for (var i = 0; i < lines.Count; i++)
            {
                if (i == 0 || i == lines.Count - 1) continue;

                for (var j = 0; j < lines[i].Length; j++)
                {
                    if (j == 0 || j == lines[i].Length - 1 || lines[i][j] != 'A') continue;

                    var xmas = "";
                    for (var k = -1; k < 2; k++)
                    {
                        for (var l = -1; l < 2; l++)
                        {
                            if (Math.Abs(k + l) == 1 || (k == 0 && l == 0)) continue;
                            xmas += lines[i + k][j + l];
                        }
                    }

                    if (possibilities.Contains(xmas)) count++;
                }
            }
            
            return count;
        }
        
        private static int Ex5A()
        {
            var lines = File.ReadAllLines("./input/5.txt").ToList();

            var cut = lines.IndexOf("");
            
            var dict = new Dictionary<int, List<int>>();
            var rules = lines.GetRange(0, cut);
            rules.ForEach(rule =>
            {
                var spl = rule.Split('|').Select(int.Parse).ToList();
                
                if (dict.ContainsKey(spl[1])) dict[spl[1]].Add(spl[0]);
                else dict[spl[1]] = new List<int>{ spl[0]};
            });
            
            var updates = lines.GetRange(cut + 1, lines.Count - 1 - cut);

            return updates.Sum(Check);

            int Check(string update)
            {
                var spl = update.Split(',').Select(int.Parse).ToList();
                
                foreach (var s in spl.Select((value, i) => new {i, value}))
                {
                    for (var j = s.i + 1; j < spl.Count; j++)
                    {
                        var j1 = j;
                        var tr = dict[s.value].FirstOrDefault(t => t == spl[j1]);
                        if (tr == 0) continue;
                        return 0;
                    }
                }

                return spl[spl.Count / 2];
            }
        }
        
        private static int Ex5B()
        {
            var lines = File.ReadAllLines("./input/5.txt").ToList();
        
            var cut = lines.IndexOf("");
            
            var dict = new Dictionary<int, List<int>>();
            var rules = lines.GetRange(0, cut);
            rules.ForEach(rule =>
            {
                var spl = rule.Split('|').Select(int.Parse).ToList();
                
                if (dict.ContainsKey(spl[1])) dict[spl[1]].Add(spl[0]);
                else dict[spl[1]] = new List<int>{ spl[0] };
            });
            
            var updates = lines.GetRange(cut + 1, lines.Count - 1 - cut);
            return updates.Select(Check).Sum();
        
            int Check(string update)
            {
                var spl = update.Split(',').Select(int.Parse).ToList();
        
                var hasFaulted = false;
                for (var i = 0; i < spl.Count; i++)
                {
                    var curr = spl[i];

                    for (var j = i + 1; j < spl.Count; j++)
                    {
                        var j1 = j;
                        var tr = dict[curr].FirstOrDefault(t => t == spl[j1]);
                        if (tr == 0) continue;
                        spl[i] = spl[j];
                        spl[j] = curr;
                        hasFaulted = true;
                        i--;
                        break;
                    }
                }
                
                return hasFaulted ? spl[spl.Count / 2] : 0;
            }
        }
    }
}