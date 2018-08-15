using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlueNoiseGenerator : INoiseGenerator
{
    private const int CandidateCount = 100;

    private float Range { get; set; }
    private WhiteNoiseGenerator WhiteNoiseGenerator { get; set; }

    readonly List<Vector2> samples = new List<Vector2>();
    private QuadTree quadTree;

    readonly List<Vector2> allSamples = new List<Vector2>();
    private int allSamplesCount;

    public BlueNoiseGenerator(float range)
    {
        int intRange = (int) range;
        allSamplesCount = intRange * intRange;
        allSamples = Enumerable.Range(0, allSamplesCount)
            .Select(x => new Vector2(x / intRange, x % intRange)).ToList();
        Range = range;
        WhiteNoiseGenerator = new WhiteNoiseGenerator(range);

        var first = WhiteNoiseGenerator.Next();
        samples.Add(first);
        
        quadTree = new QuadTree(first);
    }

    public Vector2 Next()
    {
        return FilledBlueNoise();
    }
    
    private Vector2 BlueNoise()
    {
        var candidates = Enumerable.Range(0, CandidateCount).Select(_ => WhiteNoiseGenerator.Next()).ToList();
        var sample = samples.Last();
        var best = new DistanceAndPoint(0, sample);
        for (var j = 0; j < CandidateCount; j++)
        {
            var candidate = candidates[j];
            var nearest = quadTree.NearestFrom(candidate, new DistanceAndPoint(int.MaxValue, sample));
            if (nearest.Distance > best.Distance)
            {
                best = new DistanceAndPoint(nearest.Distance, candidate);
            }
        }
        quadTree.Insert(best.Point);
        samples.Add(best.Point);
        return best.Point;
    }

    private Vector2 FilledBlueNoise()
    {
        if (allSamplesCount == 0) return Vector2.zero;

        var candidateIndexes = Enumerable.Range(0, CandidateCount)
            .Select(_ => Random.Range(0, allSamplesCount));
        var candidates = candidateIndexes.Select(x =>
            new
            {
                index = x,
                point = allSamples[x]
            }).ToList();

        var sample = samples.Last();
        var best = new DistanceAndPoint(0, sample);
        var bestIndex = 0;
        for (var j = 0; j < CandidateCount; j++)
        {
            var candidate = candidates[j];
            var nearest = quadTree.NearestFrom(candidate.point, new DistanceAndPoint(int.MaxValue, sample));
            if (nearest.Distance > best.Distance)
            {
                best = new DistanceAndPoint(nearest.Distance, candidate.point);
                bestIndex = candidate.index;
            }
        }
        quadTree.Insert(best.Point);
        samples.Add(best.Point);
        allSamples[bestIndex] = allSamples[allSamplesCount - 1];
        allSamplesCount--;
        return best.Point;
    }
}
