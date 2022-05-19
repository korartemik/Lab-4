using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneticAlgo.Shared.Models;

namespace GeneticAlgo.Core.Models
{
    public class Individual
    {

        public Individual(List<Point> points)
        {
            Points = points;
        }

        public List<Point> Points { get; private set; }
        public double Survival { get; set; }
    }
}
