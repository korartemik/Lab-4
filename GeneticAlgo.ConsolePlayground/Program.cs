// See https://aka.ms/new-console-template for more information

using GeneticAlgo.Shared;
using GeneticAlgo.Shared.Tools;
using GeneticAlgo.Core;
using GeneticAlgo.Core.Models;
using GeneticAlgo.Shared.Models;
using Serilog;

GenAlgoritm geneticAlgo = new GenAlgoritm("j");
geneticAlgo.Next();
Individual individ = geneticAlgo.GetBest().First();
while (individ.Survival < 0.99)
{
    geneticAlgo.Next();
    individ = geneticAlgo.GetBest().First();
}
foreach(Point point in individ.Points)
{
    Console.WriteLine(point.X + " " + point.Y);
}
Console.WriteLine(individ.Survival);