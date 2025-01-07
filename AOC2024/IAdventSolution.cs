using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2024
{
    internal interface IAdventSolution
    {
         public (long, long) Execute(string[] lines);
    }
}
