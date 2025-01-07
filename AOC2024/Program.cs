// See https://aka.ms/new-console-template for more information


using AOC2024;
using System.Reflection;


string basePath = AppDomain.CurrentDomain.BaseDirectory;


string inputFilePath = @"../../../data/day17.txt";

//string inputFilePath =Path.Combine(basePath, "data", "day06.txt");
string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), inputFilePath);
string[] lines = File.ReadAllLines(path);



IAdventSolution day = new Day17();


(long part1, long part2) = day.Execute(lines);

Console.WriteLine("Part I: " + part1);
Console.WriteLine("Part II: " + part2);
