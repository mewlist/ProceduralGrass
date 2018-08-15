using UnityEngine;

public class HaltonNoiseGenerator : INoiseGenerator
{
    private float Range { get; set; }

    private uint index = 0;
    public HaltonNoiseGenerator(float range)
    {
        Range = range;
    }

    public Vector2 Next()
    {
        index++;
        return new Vector2(
            Range * HaltonSequence.Base2(index),
            Range * HaltonSequence.Base3(index));
    }
}
