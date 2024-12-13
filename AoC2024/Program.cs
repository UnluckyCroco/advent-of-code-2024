using System;
using System.Collections;
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
            // Console.WriteLine(Ex5A());
            // Console.WriteLine(Ex5B());
            // Console.WriteLine(Ex6A());
            // Console.WriteLine(Ex6B());
            // Console.WriteLine(Ex7A());
            // Console.WriteLine(Ex7B());
            // Console.WriteLine(Ex8A());
            // Console.WriteLine(Ex8B());
            // Console.WriteLine(Ex9A());
            // Console.WriteLine(Ex9B());
            // Console.WriteLine(Ex10A());
            // Console.WriteLine(Ex10B());
            // Console.WriteLine(Ex11A());
            // Console.WriteLine(Ex11B());
            // Console.WriteLine(Ex12A());
            Console.WriteLine(Ex12B());
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

            var possibilities = new List<string> { "MMSS", "MSMS", "SSMM", "SMSM" };
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
                else dict[spl[1]] = new List<int> { spl[0] };
            });

            var updates = lines.GetRange(cut + 1, lines.Count - 1 - cut);

            return updates.Sum(Check);

            int Check(string update)
            {
                var spl = update.Split(',').Select(int.Parse).ToList();

                foreach (var s in spl.Select((value, i) => new { i, value }))
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
                else dict[spl[1]] = new List<int> { spl[0] };
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

        private static int Ex6A()
        {
            var lines = File.ReadAllLines("./input/6.txt").Select(s => s.ToCharArray().ToList()).ToList();

            var x = 0;
            var y = 0;
            for (var i = 0; i < lines.Count; i++)
            {
                if ((x = lines[i].IndexOf('^')) <= 0) continue;
                y = i;
                break;
            }

            var yLen = lines.Count - 1;
            var xLen = lines[0].Count - 1;

            var looping = true;
            var rotator = 0;
            var direction = new List<int> { (int)Math.Sin(-1 / (2 / Math.PI)), (int)Math.Sin(0 / (2 / Math.PI)) };

            // {1, 0} -> {0, 1} -> {-1, 0} -> {0, -1} -> {1, 0}
            Console.WriteLine($"({y}, {x})");

            while (looping)
            {
                Console.WriteLine(direction[0] + " " + direction[1]);
                var newY = y + direction[0];
                var newX = x + direction[1];
                if (newY > yLen || newY < 0) looping = false;
                else if (newX > xLen || newX < 0) looping = false;
                else if (lines[newY][newX] == '#')
                {
                    rotator++;
                    direction = new List<int>
                        { (int)Math.Sin((rotator - 1) / (2 / Math.PI)), (int)Math.Sin(rotator / (2 / Math.PI)) };
                }

                lines[y][x] = 'X';
                y += direction[0];
                x += direction[1];
            }


            foreach (var chars in lines)
            {
                foreach (var c in chars)
                {
                    Console.Write(c);
                }

                Console.WriteLine();
            }

            return lines.Select(n => n.Count(m => m == 'X')).Sum();
        }

        private static List<List<char>> Ex6ACopy()
        {
            var lines = File.ReadAllLines("./input/6.txt").Select(s => s.ToCharArray().ToList()).ToList();

            var x = 0;
            var y = 0;
            for (var i = 0; i < lines.Count; i++)
            {
                if ((x = lines[i].IndexOf('^')) <= 0) continue;
                y = i;
                break;
            }

            var yLen = lines.Count - 1;
            var xLen = lines[0].Count - 1;

            var looping = true;
            var rotator = 0;
            var direction = new List<int> { (int)Math.Sin(-1 / (2 / Math.PI)), (int)Math.Sin(0 / (2 / Math.PI)) };

            while (looping)
            {
                var newY = y + direction[0];
                var newX = x + direction[1];
                if (newY > yLen || newY < 0) looping = false;
                else if (newX > xLen || newX < 0) looping = false;
                else if (lines[newY][newX] == '#')
                {
                    rotator++;
                    direction = new List<int>
                        { (int)Math.Sin((rotator - 1) / (2 / Math.PI)), (int)Math.Sin(rotator / (2 / Math.PI)) };
                }

                lines[y][x] = 'X';
                y += direction[0];
                x += direction[1];
            }

            return lines;
        }

        private static int Ex6B()
        {
            var lines = Ex6ACopy();

            var x = 86;
            var y = 55;

            var yLen = lines.Count - 1;
            var xLen = lines[0].Count - 1;


            var count = 0;

            for (var i = 0; i < lines.Count; i++)
            {
                for (var i1 = 0; i1 < lines[i].Count; i1++)
                {
                    if (lines[i][i1] != 'X') continue;
                    if (i == y && i1 == x) continue;
                    var newField = new List<List<char>>();
                    foreach (var chars in lines)
                    {
                        newField.Add(new List<char>());
                        foreach (var c in chars)
                        {
                            newField.Last().Add(c);
                        }
                    }

                    newField[i][i1] = '#';
                    var loops = Loops(y, x, newField, yLen, xLen);
                    if (loops) count++;
                }
            }

            return count;
        }

        private static bool Loops(int y, int x, List<List<char>> field, int yLen, int xLen)
        {
            (int y, int x) direction = ((int)Math.Sin(-1 / (2 / Math.PI)), (int)Math.Sin(0 / (2 / Math.PI)));
            var passedCoords = new List<(int y, int x, (int y, int x) direction)> { (y, x, direction) };
            var rotator = 0;
            while (true)
            {
                var newY = y + direction.y;
                var newX = x + direction.x;
                if (newY > yLen || newY < 0) return false;
                if (newX > xLen || newX < 0) return false;
                (int y, int x, (int y, int x) direction) newCoord;

                while (field[newY][newX] == '#')
                {
                    rotator++;
                    direction = ((int)Math.Sin((rotator - 1) / (2 / Math.PI)), (int)Math.Sin(rotator / (2 / Math.PI)));
                    newY = y + direction.y;
                    newX = x + direction.x;
                    newCoord = (y, x, direction);
                    if (passedCoords.Contains(newCoord)) return true;
                    passedCoords.Add(newCoord);
                }

                y += direction.y;
                x += direction.x;
                field[y][x] = '0';

                newCoord = (y, x, direction);
                if (passedCoords.Contains(newCoord)) return true;
                passedCoords.Add(newCoord);
            }
        }

        private static long Ex7A()
        {
            var lines = File.ReadAllLines("./input/7.txt").ToList();

            var thing = lines.Select(line =>
            {
                var values = line.Split(':');
                (long total, List<long> list) d = (long.Parse(values[0]),
                    values[1].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList());
                return d;
            }).ToList();

            // 82 236 356 10
            // 1 1 1 => 7
            // * * *
            // 1 1 0 => 6
            // * * +
            // 1 0 1 => 5
            // * + *
            // 1 0 0 => 4
            // * + +
            // 0 1 1 => 3
            // + * *
            // 0 1 0 => 2
            // + * +
            // 0 0 1 => 1
            // + + *
            // 0 0 0 => 0
            // + + +

            return thing.Select((t) =>
            {
                var numbers = t.list.Count - 1;

                var poss = Convert.ToInt32(Math.Pow(2, numbers));
                for (var i = 0; i < poss; i++)
                {
                    var b = new BitArray(new[] { i })
                    {
                        Length = numbers
                    };

                    var res = t.list.Select((c, index) => (c, index)).Aggregate((curr, next) =>
                    {
                        return b[curr.index] ? (curr.c * next.c, next.index) : (curr.c + next.c, next.index);
                    });

                    if (res.c == t.total)
                    {
                        return t.total;
                    }
                }

                return 0;
            }).Sum();
        }

        static void PrintBits(BitArray list)
        {
            foreach (bool o in list)
            {
                Console.Write(o ? 1 : 0);
            }

            Console.WriteLine();
        }

        private static List<(long total, List<long> list)> Ex7ACopy()
        {
            var lines = File.ReadAllLines("./input/7.txt").ToList();

            var thing = lines.Select(line =>
            {
                var values = line.Split(':');
                (long total, List<long> list) d = (long.Parse(values[0]),
                    values[1].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList());
                return d;
            }).ToList();

            return thing.Where(t =>
            {
                var numbers = t.list.Count - 1;

                var poss = Convert.ToInt32(Math.Pow(2, numbers));
                for (var i = 0; i < poss; i++)
                {
                    var b = new BitArray(new[] { i })
                    {
                        Length = numbers
                    };

                    var res = t.list.Select((c, index) => (c, index)).Aggregate((curr, next) =>
                    {
                        return b[curr.index] ? (curr.c * next.c, next.index) : (curr.c + next.c, next.index);
                    });

                    if (res.c == t.total)
                    {
                        return false;
                    }
                }

                return true;
            }).ToList();
        }

        private static long Ex7B()
        {
            var lines = Ex7ACopy();

            // 82 236 356
            // 2 = ||
            // 1 = *
            // 0 = +
            // 2 2
            // 2 1
            // 2 0
            // 1 2
            // 0 2
            // 1 1 -
            // 0 0 -
            // 1 0 -
            // 0 1 -

            return lines.Select((t, j) =>
            {
                var numbers = t.list.Count - 1;
                var poss = Convert.ToInt32(Math.Pow(3, numbers));
                for (var i = 0; i < poss; i++)
                {
                    var b = Base3(i, numbers);
                    if (!b.Contains(2)) continue;
                    var res = t.list.Select((c, index) => (c, index)).Aggregate((curr, next) =>
                    {
                        switch (b[curr.index])
                        {
                            case 2:
                                return (long.Parse(curr.c.ToString() + next.c), next.index);
                            case 1:
                                return (curr.c * next.c, next.index);
                            default:
                                return (curr.c + next.c, next.index);
                        }
                    });

                    if (res.c != t.total) continue;
                    return t.total;
                }

                return 0;
            }).Sum() + Ex7A();
        }

        private static List<int> Base3(int b, int len)
        {
            var l = new List<int>();
            // len = 3
            // [0,0,0] = 0
            // [0,0,2] = 2
            // [0,2,0] = 6
            // [0,2,2] = 8
            // [1,0,0] = 9
            // [2,0,0] = 18
            // [2,2,0] = 24
            // [2,2,2] = 26
            // b = 3 -> [0,1,0]
            // b / 9
            for (var i = 0; i < len; i++)
            {
                var divisor = Math.Pow(3, len - i - 1);
                var fits = (int)Math.Floor(b / divisor);
                b = (int)(b % divisor);
                l.Add(fits);
            }

            return l;
        }

        private static void PrintBase3(List<int> base3List)
        {
            foreach (var o in base3List)
            {
                Console.Write(o);
            }

            Console.WriteLine();
        }

        private static int Ex8A()
        {
            var lines = File.ReadAllLines("./input/8.txt").ToList();
            var dict = new Dictionary<char, List<(int y, int x)>>();

            var yLen = lines.Count;
            var xLen = lines[0].Length;

            for (var i = 0; i < yLen; i++)
            {
                for (var j = 0; j < xLen; j++)
                {
                    var curr = lines[i][j];
                    if (curr == '.') continue;
                    if (dict.ContainsKey(curr)) dict[curr].Add((i, j));
                    else dict[curr] = new List<(int y, int x)> { (i, j) };
                }
            }

            return dict.Keys.Select(p =>
                    (from value1 in dict[p] from value2 in dict[p] where value1 != value2 select (value1, value2))
                    .Select(
                        q => (q.value1.y + (q.value1.y - q.value2.y), q.value1.x + (q.value1.x - q.value2.x))))
                .SelectMany(y => y)
                .Where(o => o.Item1 < yLen && o.Item2 < xLen && o.Item1 >= 0 && o.Item2 >= 0)
                .Distinct()
                .Count();
        }

        private static int Ex8B()
        {
            var lines = File.ReadAllLines("./input/8.txt").ToList();
            var dict = new Dictionary<char, List<(int y, int x)>>();

            var yLen = lines.Count;
            var xLen = lines[0].Length;

            for (var i = 0; i < yLen; i++)
            {
                for (var j = 0; j < xLen; j++)
                {
                    var curr = lines[i][j];
                    if (curr == '.') continue;
                    if (dict.ContainsKey(curr)) dict[curr].Add((i, j));
                    else dict[curr] = new List<(int y, int x)> { (i, j) };
                }
            }

            return dict.Keys.Select(p =>
                    (from value1 in dict[p] from value2 in dict[p] where value1 != value2 select (value1, value2))
                    .Select(
                        q =>
                        {
                            var list = new List<(int y, int x)> { q.value1, q.value2 };
                            var yInc = q.value1.y - q.value2.y;
                            var xInc = q.value1.x - q.value2.x;
                            var y = q.value1.y + yInc;
                            var x = q.value1.x + xInc;
                            while (y < yLen && y >= 0 && x < xLen && x >= 0)
                            {
                                list.Add((y, x));
                                y += yInc;
                                x += xInc;
                            }

                            return list;
                        }
                    ).ToList()
                    .SelectMany(l => l)
                )
                .SelectMany(y => y)
                .Distinct().Count();
        }
        
        private static long Ex9A()
        {
            var line = File.ReadAllLines("./input/9.txt").ToList()[0];
            
            var backIndex = line.Length - 1;
            var backCount = line[backIndex] - '0';
            var sum = 0L;
            var freeToggle = false;
            var fileId = 0;
            var filePosition = 0;

            for (var i = 0; i < line.Length; i++)
            {
                var curr = line[i] - '0';

                if (!freeToggle)
                {
                    for (var newIndex = filePosition; newIndex < curr + filePosition; newIndex++)
                    {
                        sum += newIndex * fileId;
                    }
                    filePosition += curr;
                    fileId++;
                }
                else
                {
                    for (var j = 0; j < curr; j++)
                    {
                        if (backIndex / 2 == fileId)
                        {
                            while (backCount > 0)
                            {
                                sum += filePosition * (backIndex / 2);
                                filePosition++;
                                backCount--;
                            }
                            return sum;
                        }
                        if (backCount == 0)
                        {
                            backIndex -= 2;
                            backCount = line[backIndex] - '0';
                        }
                        sum += filePosition * (backIndex / 2);

                        filePosition++;
                        backCount--;
                    }
                }
                freeToggle = !freeToggle;
            }
            return sum;
        }

        private class FileSize
        {
            public int Size { get; private set; }
            public int FreeSpace { get; set; }
            public int FileIndex { get; private set; }
            
            public FileSize(int size, int freeSpace, int fileIndex)
            {
                Size = size;
                FreeSpace = freeSpace;
                FileIndex = fileIndex;
            }

            public new string ToString()
            {
                return $"FIndex: {FileIndex}, Size: {Size}, FreeSpace: {FreeSpace}";
            }
        }
        
        private static long Ex9B()
        {
            var line = File.ReadAllLines("./input/9.txt").ToList()[0].Select(x => x - '0').ToList();

            var files = line.Where((i, index) => index % 2 == 0).ToList();
            var free = line.Where((i, index) => index % 2 != 0).ToList();
            var fileSizes = files.Select((x, index) => (x, index))
                .Zip(free, (fi, fr) => new FileSize(fi.x, fr, fi.index)).ToList();
            
            fileSizes.Add(new FileSize(files.Last(), 0, files.Count - 1));

            for (var i = fileSizes.Count - 1; i >= 0; i--)
            {
                var curr = fileSizes[i];

                var fitsIndex = fileSizes.FindIndex(x => x.FreeSpace >= curr.Size);
                if (fitsIndex == -1 || fitsIndex >= i) continue;

                var fits = fileSizes[fitsIndex];
                var insertFile = new FileSize(curr.Size, fits.FreeSpace - curr.Size, curr.FileIndex);
                fits.FreeSpace = 0;

                fileSizes.Remove(curr);
                fileSizes.Insert(i, new FileSize(0, curr.Size + curr.FreeSpace, 0));
                
                fileSizes.Insert(fitsIndex + 1, insertFile);
                i++;
            }
            
            var sum = 0L;
            var storageIndex = 0;
            
            foreach (var fileSize in fileSizes)
            {
                for (var i = 0; i < fileSize.Size; i++)
                {
                    sum += fileSize.FileIndex * storageIndex;
                    storageIndex++;
                }

                storageIndex += fileSize.FreeSpace;
            }
            
            return sum;
        }

        private static int Ex10A()
        {
            var lines = File.ReadAllLines("./input/10.txt").ToList();

            var yLen = lines.Count;
            var xLen = lines[0].Length;
            var trails = new List<(int y, int x)>();
            
            for (var i = 0; i < lines.Count; i++)
            {
                for (var i1 = 0; i1 < lines[i].Length; i1++)
                {
                    if (lines[i][i1] == '0')
                    {
                        trails.AddRange(TrailFinder((i, i1), 0).Distinct());
                    }
                }
            }

            return trails.Count;
            
            List<(int y, int x)> TrailFinder((int y, int x) trailStart, int height)
            {
                var testTrails = new List<(int y, int x)>();
                
                if (height == 9)
                {
                    return new List<(int y, int x)>{trailStart};
                }
                
                for (var y = -1; y < 2; y++)
                {
                    if (y + trailStart.y >= yLen || y + trailStart.y < 0) continue;
                    for (var x = -1; x < 2; x++)
                    {
                        if (x + trailStart.x >= xLen || x + trailStart.x < 0 || Math.Abs(x + y) % 2 != 1) continue;
                        if (lines[y + trailStart.y][x + trailStart.x] - '0' == height + 1)
                        {
                            testTrails.AddRange(TrailFinder((y + trailStart.y, x + trailStart.x), height + 1));
                        }
                    }
                }

                return testTrails;
            }
        }
        
        private static int Ex10B()
        {
            var lines = File.ReadAllLines("./input/10.txt").ToList();

            var yLen = lines.Count;
            var xLen = lines[0].Length;
            var trails = new List<(int y, int x)>();
            
            for (var i = 0; i < lines.Count; i++)
            {
                for (var i1 = 0; i1 < lines[i].Length; i1++)
                {
                    if (lines[i][i1] == '0')
                    {
                        trails.AddRange(TrailFinder((i, i1), 0));
                    }
                }
            }

            return trails.Count;
            
            List<(int y, int x)> TrailFinder((int y, int x) trailStart, int height)
            {
                var testTrails = new List<(int y, int x)>();
                
                if (height == 9)
                {
                    return new List<(int y, int x)>{trailStart};
                }
                
                for (var y = -1; y < 2; y++)
                {
                    if (y + trailStart.y >= yLen || y + trailStart.y < 0) continue;
                    for (var x = -1; x < 2; x++)
                    {
                        if (x + trailStart.x >= xLen || x + trailStart.x < 0 || Math.Abs(x + y) % 2 != 1) continue;
                        if (lines[y + trailStart.y][x + trailStart.x] - '0' == height + 1)
                        {
                            testTrails.AddRange(TrailFinder((y + trailStart.y, x + trailStart.x), height + 1));
                        }
                    }
                }

                return testTrails;
            }
        }

        private class Stone
        {
            public long Value { get; private set; }
            public int Depth { get; private set; }
            public List<Stone> Children = new List<Stone>();

            public Stone(long value, int depth)
            {
                Value = value;
                Depth = depth;
            }
        }
        
        private static int Ex11A()
        {
            var line = File.ReadAllLines("./input/11.txt").ToList()[0];

            var l = new List<Stone>();
            l.AddRange(line.Split(' ').Select(v => new Stone(int.Parse(v), 0)));

            var tempChildren = new List<Stone>(l);

            for (var i = 0; i < 25; i++)
            {
                foreach (var i1 in tempChildren)
                {
                    var str = i1.Value.ToString();
                    var len = str.Length;
                    if (len % 2 == 0)
                    {
                        i1.Children.Add(new Stone(int.Parse(str.Substring(0, len / 2)), i));
                        i1.Children.Add(new Stone(int.Parse(str.Substring(len / 2)), i));
                        continue;
                    }

                    if (str == "0")
                    {
                        i1.Children.Add(new Stone(1, i));
                        continue;
                    }
                    
                    i1.Children.Add(new Stone(i1.Value * 2024, i));
                }
                tempChildren = tempChildren.SelectMany(b => b.Children).ToList();
            }

            return tempChildren.Count;
        }
        
        private static long Ex11B()
        {
            var line = File.ReadAllLines("./input/11.txt").ToList()[0];

            var dict = new Dictionary<long, List<long>>();
            var stones = line.Split(' ').ToDictionary(long.Parse, se => 1L);

            for (var i = 0; i < 75; i++)
            {
                var stonesCopy = new Dictionary<long, long>(stones);
                
                foreach (var stonesKey in stonesCopy.Keys)
                {
                    if (stonesCopy[stonesKey] == 0) continue;
                    var v = stonesKey;
                    var amount = stonesCopy[stonesKey];
                    
                    if (dict.TryGetValue(v, out var existing))
                    {
                        foreach (var l in existing)
                        {
                            stones[l] += amount;
                        }
                        stones[v] -= amount;

                        continue;
                    }
                
                    var str = v.ToString();
                    var len = str.Length;
                    if (len % 2 == 0)
                    {
                        var first = long.Parse(str.Substring(0, len / 2));
                        var second = long.Parse(str.Substring(len / 2));
                        dict.Add(v, new List<long> { first, second });
                        if (stones.ContainsKey(first)) stones[first] += amount;
                        else stones.Add(first, amount);
                        if (stones.ContainsKey(second)) stones[second] += amount;
                        else stones.Add(second, amount);
                    }
                    else if (str == "0")
                    {
                        dict.Add(v, new List<long> { 1 });
                        stones.Add(1, amount);
                    }
                    else
                    {
                        var newValue = v * 2024;
                        dict.Add(v, new List<long> { newValue });
                        if (stones.ContainsKey(newValue)) stones[newValue] += amount;
                        else stones.Add(newValue, amount);
                    }
                    stones[v] -= amount;
                }
            }

            return stones.Sum(kvp => kvp.Value);
        }
        
        private static long Ex12A()
        {
            var lines = File.ReadAllLines("./input/12.txt").ToList();

            var passed = new List<(int y, int x)>();
            var nodes = new Dictionary<(int y, int x), int>();
            var total = 0;
            var yLen = lines.Count;
            var xLen = lines[0].Length;
            
            for (var i = 0; i < lines.Count; i++)
            {
                for (var i1 = 0; i1 < lines[i].Length; i1++)
                {
                    if (passed.Contains((i, i1))) continue;
                    var c = lines[i][i1];
                    
                    nodes.Clear();
                    nodes.Add((i, i1), 4);
                    ScanArea(c, i, i1);
                    total += nodes.Select(z => z.Value).Sum() * nodes.Count;
                    passed.AddRange(nodes.Keys);
                }
            }

            return total;
            
            void ScanArea(char f, int y, int x)
            {
                for (var u = -1; u < 2; u++)
                {
                    var uy = u + y;
                    if (uy >= yLen || uy < 0) continue;
                    for (var v = -1; v < 2; v++)
                    {
                        var vx = v + x;
                        if (Math.Abs(u + v) % 2 != 1) continue;
                        if (vx >= xLen || vx < 0) continue;
                        if (nodes.ContainsKey((uy, vx))) continue;
                        if (lines[uy][vx] != f) continue;

                        var newNodeTouch = 4;
                        for (var o = -1; o < 2; o++)
                        {
                            var ouy = o + uy;
                            for (var p = -1; p < 2; p++)
                            {
                                var pvx = p + vx;
                                if (Math.Abs(o + p) % 2 != 1) continue;

                                if (! nodes.ContainsKey((ouy, pvx))) continue;
                                
                                newNodeTouch--;
                                nodes[(ouy, pvx)]--;
                            }
                        }
                        
                        nodes.Add((uy, vx), newNodeTouch);

                        ScanArea(f, uy, vx);
                    }
                }
            }
        }
        
        private static long Ex12B()
        {
            var lines = File.ReadAllLines("./input/12.txt").ToList();

            var passed = new List<(int y, int x)>();
            var nodes = new Dictionary<(int y, int x), int>();
            var total = 0;
            var yLen = lines.Count;
            var xLen = lines[0].Length;
            
            for (var i = 0; i < lines.Count; i++)
            {
                for (var i1 = 0; i1 < lines[i].Length; i1++)
                {
                    if (passed.Contains((i, i1))) continue;
                    var c = lines[i][i1];
                    
                    nodes.Clear();
                    nodes.Add((i, i1), CheckCorners(c, i, i1));
                    ScanArea(c, i, i1);
                    total += nodes.Select(z => z.Value).Sum() * nodes.Count;
                    passed.AddRange(nodes.Keys);
                }
            }

            return total;
            
            void ScanArea(char f, int y, int x)
            {
                for (var u = -1; u < 2; u++)
                {
                    var uy = u + y;
                    if (uy >= yLen || uy < 0) continue;
                    for (var v = -1; v < 2; v++)
                    {
                        var vx = v + x;
                        if (Math.Abs(u + v) % 2 != 1) continue;
                        if (vx >= xLen || vx < 0) continue;
                        if (nodes.ContainsKey((uy, vx))) continue;
                        if (lines[uy][vx] != f) continue;
                        
                        nodes.Add((uy, vx), CheckCorners(f, uy, vx));

                        ScanArea(f, uy, vx);
                    }
                }
            }

            int CheckCorners(char f, int y, int x)
            {
                (int y, int x) direction = (-1, 0);
                var count = 0;
                
                for (var i = 0; i < 4; i++)
                {
                    var sideA = true;
                    var sideB = true;
                    var sideD = true;
                    if (direction.y + y >= yLen || direction.y + y < 0 || direction.x + x >= xLen || direction.x + x < 0) sideA = false;
                    else if (lines[y + direction.y][x + direction.x] != f) sideA = false;
                    
                    (int y, int x) diagonal = (direction.y + direction.x, direction.x - direction.y);
                    
                    if (diagonal.y + y >= yLen || diagonal.y + y < 0 || diagonal.x + x >= xLen || diagonal.x + x < 0) sideD = false;
                    else if (lines[y + diagonal.y][x + diagonal.x] != f) sideD = false;
                    
                    direction = (direction.x, direction.y * -1);

                    if (direction.y + y >= yLen || direction.y + y < 0 || direction.x + x >= xLen || direction.x + x < 0) sideB = false;
                    else if (lines[y + direction.y][x + direction.x] != f) sideB = false;

                    if ((!sideA && !sideB) || (sideA && sideB && !sideD)) count++;
                }

                return count;
            }
        }
        
        
        private static long Template()
        {
            var lines = File.ReadAllLines("./input/[x].txt").ToList();

            return 0;
        }
    }
}
