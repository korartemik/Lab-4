// See https://aka.ms/new-console-template for more information
using GeneticAlgo.Core;
using GeneticAlgo.Core.Models;
using Serilog;

GenAlgoritm geneticAlgo = new GenAlgoritm(5,5);
geneticAlgo.Next();
Individual individ = geneticAlgo.GetBest(5).First();
for(int i = 0; i<1000; i++)
{
    geneticAlgo.Next();
    individ = geneticAlgo.GetBest(5).First();
}
foreach(Point point in individ.Points)
{
    Console.WriteLine(point.X + " " + point.Y);
}
Console.WriteLine(individ.Survival);