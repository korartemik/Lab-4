﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneticAlgo.Core.Models;
using System.Buffers;

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
        private ArrayPool<Point> _listPoints;
        public GenAlgoritm(int maximumValue, int circleCount)
        {
            fMax = maximumValue;
            countBariers = circleCount;
            _listPoints = ArrayPool<Point>.Shared;
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

        public List<Individual> GetBest(int pointCount)
        {
            return population.GetRange(0, pointCount);
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
            for(int i = 0; i< 100000; i++)
            {
                Point[]pointsForNewIndivid = _listPoints.Rent(5);
                pointsForNewIndivid[0] = new Point(0, 0);
                for(int j = 0; j < 4; j++)
                {
                    double f = rand.NextDouble() * fMax;
                    double teta = rand.NextDouble() * Math.PI * 0.5;
                    Point newPoint = new Point(Math.Max(Math.Min(pointsForNewIndivid[j].X + f * Math.Cos(teta), 1), 0), Math.Max(Math.Min(pointsForNewIndivid[j].Y + f * Math.Sin(teta), 1), 0));
                    pointsForNewIndivid[j+1] = newPoint;
                }
                Individual individual = new Individual(pointsForNewIndivid, 0);
                individual.Survival = CountSurvival(individual.Points);
                population.Add(individual);
            }
        }

        public void MergePopulation()
        {
            List<Individual> newPopulation = new List<Individual>(100);
            for(int i = 0; i<25; i++)
            {
                newPopulation.Add(population.ElementAt(i));
            }
            for (int i = 0; i < 25; i++)
            {
                newPopulation.Add(MergeIndividual(population.ElementAt(i), population.ElementAt(i+1)));
                newPopulation.Add(MergeIndividual(population.ElementAt(i), population.ElementAt(i + 25)));
                newPopulation.Add(MergeIndividual(population.ElementAt(i), population.ElementAt(i + 12)));
            }
            foreach(Individual individ in population)
            {
                _listPoints.Return(individ.Points);
            }
            population = newPopulation;
        }

        public Individual MergeIndividual(Individual first, Individual second)
        {
            var listPoints = _listPoints.Rent(5);
            //List<Point> listPoints = new List<Point>(first.Points.Count);
            for(int i = 0; i < first.Points.Length; i++)
            {
                Point point = new Point(0.5 * (first.Points[i].X + second.Points[i].X), 0.5 * (first.Points[i].Y + second.Points[i].Y));
                //listPoints.Add(point);
                listPoints[i] = point;
            }
            Individual individ = new Individual(listPoints, 0);
            individ.Survival = CountSurvival(individ.Points);
            return individ;
        }
        private double CountSurvival(Point[] points)
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

        private double checkBarrier(Point[] points, BarrierCircle barrier)
        {
            double ans = 1;
            double fine = 0.4;
            for(int i = 1; i<points.Length; i++)
            {
                Point pointPref = points[i - 1];
                Point pointPres = points[i];
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
