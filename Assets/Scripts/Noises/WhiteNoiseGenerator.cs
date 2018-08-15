using UnityEngine;

public class WhiteNoiseGenerator : INoiseGenerator
{
    private float Range { get; set; }

    public WhiteNoiseGenerator(float range)
    {
        Random.seed = 10;
        Range = range;
    }

    public Vector2 Next()
    {
        return new Vector2(Random.Range(0, Range), Random.Range(0, Range));
    }
}
