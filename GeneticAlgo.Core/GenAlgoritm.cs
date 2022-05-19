using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneticAlgo.Shared.Models;
using GeneticAlgo.Core.Models;

namespace GeneticAlgo.Core
{
    public class GenAlgoritm
    {
        private string _path;
        private double fMax = 0;
        private double countBariers = 0;
        private double dt = 0;
        private int iteration = 0;
        public List<Individual> population;
        public GenAlgoritm(string path)
        {
            _path = path;
            GetData();
        }

        public List<BarrierCircle> BarrierCircles { get; private set; }

        /*public void GetData(string path)
        {

        }*/

        public void GetData()
        {
            dt = 0.001;
            fMax = 1;
            countBariers = 4;
            BarrierCircles = new List<BarrierCircle>();
            BarrierCircles.Add(new BarrierCircle(new Point(0.33218833804130554, 0.15921106934547424), 0.23818166553974152));
            BarrierCircles.Add(new BarrierCircle(new Point(0.9211785793304443, 0.21001200377941132), 0.24298787117004395));
            BarrierCircles.Add(new BarrierCircle(new Point(0.6558014154434204, 0.7025460004806519), 0.21127113699913025));
            BarrierCircles.Add(new BarrierCircle(new Point(0.05513463541865349, 0.7919896245002747), 0.20693418383598328));

        }

        public List<Individual> GetBest()
        {
            return population.GetRange(0, 2);
        }

        public void Next()
        {
            if (iteration == 0)
            {
                CreatePopulation();
            }
            else
            {
                MergePopulation();
            }
            population = population.OrderByDescending(x => x.Survival).ToList();
            iteration++;
        }

        public void CreatePopulation()
        {
            population = new List<Individual>();
            Random rand = new Random();
            for(int i = 0; i< 1000; i++)
            {
                List<Point> pointsForNewIndivid = new List<Point>();
                pointsForNewIndivid.Add(new Point(0, 0));
                for(int j = 0; j < Math.Sqrt(2)/fMax + 2; j++)
                {
                    double f = rand.NextDouble() * fMax;
                    double teta = rand.NextDouble() * Math.PI * 0.5;
                    Point newPoint = new Point(Math.Max(Math.Min(pointsForNewIndivid.Last().X + f * Math.Cos(teta), 1), 0), Math.Max(Math.Min(pointsForNewIndivid.Last().Y + f * Math.Sin(teta), 1), 0));
                    //Point newPoint = new Point(pointsForNewIndivid.Last().X + f * Math.Cos(teta), pointsForNewIndivid.Last().Y + f * Math.Sin(teta));
                    pointsForNewIndivid.Add(newPoint);
                }
                Individual individual = new Individual(pointsForNewIndivid);
                individual.Survival = CountSurvival(individual.Points);
                population.Add(individual);
            }
        }

        public void MergePopulation()
        {
            List<Individual> newPopulation = new List<Individual>();
            for(int i = 0; i<250; i++)
            {
                newPopulation.Add(population.ElementAt(i));
            }
            for (int i = 0; i < 250; i++)
            {
                newPopulation.Add(MergeIndividual(population.ElementAt(i), population.ElementAt(i+1)));
                newPopulation.Add(MergeIndividual(population.ElementAt(i), population.ElementAt(i + 250)));
                newPopulation.Add(MergeIndividual(population.ElementAt(i), population.ElementAt(i + 125)));
            }
            population = newPopulation;
        }

        public Individual MergeIndividual(Individual first, Individual second)
        {
            List<Point> listPoints = new List<Point>();
            for(int i = 0; i < first.Points.Count; i++)
            {
                Point point = new Point(0.5 * (first.Points.ElementAt(i).X + second.Points.ElementAt(i).X), 0.5 * (first.Points.ElementAt(i).Y + second.Points.ElementAt(i).Y));
                listPoints.Add(point);
            }
            Individual individ = new Individual(listPoints);
            individ.Survival = CountSurvival(individ.Points);
            return individ;
        }
        private double CountSurvival(List<Point> points)
        {
            double surv = 1;
            surv = surv * distance(points.Last());
            //Console.WriteLine(distance(points.Last()) + " " + points.Last().X + " " + points.Last().Y);
            foreach(BarrierCircle barrierCircle in BarrierCircles)
            {
                surv *= checkBarrier(points, barrierCircle);
            }

            foreach(Point point in points)
            {
                surv *= CheckCoordinate(point);
            }
            return surv;
        }

        private double distance(Point point)
        {
            return (Math.Sqrt(2) - Math.Sqrt((1 - point.X) * (1 - point.X) + (1 - point.Y) * (1 - point.Y))) / Math.Sqrt(2);
        }

        private double checkBarrier(List<Point> points, BarrierCircle barrier)
        {
            double ans = 1;
            double fine = 0.4;
            for(int i = 1; i<points.Count; i++)
            {
                Point pointPref = points.ElementAt(i - 1);
                Point pointPres = points.ElementAt(i);
                double x1 = pointPref.X;
                double y1 = pointPref.Y;
                double x2 = pointPres.X;
                double y2 = pointPres.Y;
                for(double x = x1; x < x2; x = x + 0.001)
                {
                    double y = x * (y1 - y2) / (x1 - x2) + y1 - x1 * ((y1 - y2) / (x1 - x2));
                    if ((x - barrier.Center.X) * (x - barrier.Center.X) + (y - barrier.Center.Y) * (y - barrier.Center.Y) <= barrier.Radius * barrier.Radius)
                    {
                        ans = fine * ans;
                        break;
                    }
                }
                
            }
            if ((points.Last().X - barrier.Center.X) * (points.Last().X - barrier.Center.X) + (points.Last().Y - barrier.Center.Y) * (points.Last().Y - barrier.Center.Y) <= barrier.Radius * barrier.Radius)
            {
                ans = fine * ans;
            }
            return ans;
        }

        private double CheckCoordinate(Point point)
        {
            if(point.X > 1 | point.X < 0 | point.Y < 0 | point.Y > 1)
            {
                return 0.1;
            }
            return 1;
        }
    }
}
