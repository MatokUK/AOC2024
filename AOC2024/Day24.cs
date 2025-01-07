using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC2024
{
    internal class Day24 : IAdventSolution
    {
        class Gate
        {
            private string op;
            public Gate(string op) 
            { 
                this.op = op;
            }

            public bool Execute(Wire a, Wire b)
            {
                return op switch
                {
                    "XOR" => a.SafeValue ^ b.SafeValue,
                    "AND" => a.SafeValue && b.SafeValue,
                    "OR" => a.SafeValue || b.SafeValue,
                    _ => throw new NotImplementedException(),
                };                
            }

            public override string ToString()
            {
                return op;
            }
        }

        class Wire : IComparable<Wire>
        {
            private string name;
            private bool? value;
            public bool IsOutput { get; init; }

            public bool SafeValue
            {
                get
                {
                    if (value == null)
                    {
                        throw new InvalidOperationException("Value is not defined");
                    }

                    return (bool)value;
                    
                }
            }

            public Wire(string name, bool? value = null) 
            { 
                this.name = name;
                this.value = value;
                IsOutput = name.StartsWith("z");
            }

            public void SetValue(bool  value)
            {
                if (this.value != null)
                {
                    throw new InvalidOperationException("Values is already defined!");
                }
                this.value = value;
            }

            public bool HasValue()
            {
                return this.value != null;
            }

            public override string ToString()
            {
                return $"{name} [{(value != null ? value.ToString() : '?')}]";
            }

            public int CompareTo(Wire? other)
            {
                return other.name.CompareTo(this.name);
            }
        }

        public (long, long) Execute(string[] lines)
        {
            var wires = ReadWires(lines);
            var connextions = ReadConnections(lines);

            do
            {
                foreach (var kvp in connextions)
                {
                    var w1 = kvp.Key.Item1;
                    var w2 = kvp.Key.Item2;
                    var w3 = kvp.Key.Item3;

                    if (wires[w1].HasValue() && wires[w2].HasValue() && !wires[w3].HasValue())
                    {
                        wires[w3].SetValue(kvp.Value.Execute(wires[w1], wires[w2]));
                    }
                }

            } while (!wires.All(x => x.Value.HasValue()));


            var wiresOutput = wires.Values.Where(w => w.IsOutput).ToList();

            wiresOutput.Sort((a, b) => a.CompareTo(b));

            var binaryString = string.Join("", wiresOutput.Select(w => Convert.ToInt32(w.SafeValue).ToString()));

            return (Convert.ToInt64(binaryString, 2), 0);
        }

        private Dictionary<string, Wire> ReadWires(string[] lines)
        {
            Dictionary<string, Wire> result = new();
            foreach (var line in lines)
            {
                if (line.Contains(": "))
                {
                    var parts = line.Split(": ");
                    result[parts[0]] = (new Wire(parts[0], parts[1] == "1" ? true : false));
                }

                if (line.Contains("->"))
                {
                    var matches = Regex.Matches(line, @"([a-z0-9]+) (OR|XOR|AND) ([a-z0-9]+) -> ([a-z0-9]+)");

                    var w1 = matches[0].Groups[1].ToString();
                    var w2 = matches[0].Groups[3].ToString();
                    var w3 = matches[0].Groups[4].ToString();

                    if (!result.ContainsKey(w1))
                    {
                        result[w1] = new Wire(w1);
                    }

                    if (!result.ContainsKey(w2))
                    {
                        result[w2] = new Wire(w2);
                    }

                    if (!result.ContainsKey(w3))
                    {
                        result[w3] = new Wire(w3);
                    }
                }
            }

            return result;
        }

        private Dictionary<(string, string, string), Gate> ReadConnections(string[] lines)
        {
            Dictionary<(string, string, string), Gate> result = new();
            foreach (var line in lines)
            {              
                if (line.Contains("->"))
                {
                    var matches = Regex.Matches(line, @"([a-z0-9]+) (OR|XOR|AND) ([a-z0-9]+) -> ([a-z0-9]+)");

                    var w1 = matches[0].Groups[1].ToString();
                    var w2 = matches[0].Groups[3].ToString();
                    var w3 = matches[0].Groups[4].ToString();

                    result[(w1, w2, w3)] = new Gate(matches[0].Groups[2].ToString());
                }
            }

            return result;
        }
    }
}
