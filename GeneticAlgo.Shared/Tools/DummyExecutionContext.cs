using GeneticAlgo.Shared.Models;

namespace GeneticAlgo.Shared.Tools;

public class DummyExecutionContext : IExecutionContext
{
    private readonly int _circleCount;
    private readonly int _pointCount;
    private readonly int _maximumValue;
    private readonly Random _random;
    private GenAlgoritm _geneticAlgo;

    public DummyExecutionContext(int pointCount, int maximumValue, int circleCount)
    {
        _pointCount = pointCount;
        _maximumValue = maximumValue;
        _circleCount = circleCount;
        _geneticAlgo = new GenAlgoritm(maximumValue, circleCount);
        _random = Random.Shared;
    }
    
    private double Next => _random.NextDouble() * _random.Next(_maximumValue);

    public void Reset() { }

    public Task<IterationResult> ExecuteIterationAsync()
    {
        return Task.FromResult(IterationResult.IterationFinished);
    }

    public void ReportStatistics(IStatisticsConsumer statisticsConsumer)
    {
        _geneticAlgo.Next();
        Individual points = _geneticAlgo.GetBest(1).First();
        Statistic[] statistics = Enumerable.Range(0, points.Points.Count)
            .Select(i => new Statistic(i, points.Points.ElementAt(i), points.Survival))
            .ToArray();

        List<BarrierCircle> barrierCircles = _geneticAlgo.BarrierCircles;
        BarrierCircle[] circles = barrierCircles.ToArray();
        
        statisticsConsumer.Consume(statistics, circles);
    }
}