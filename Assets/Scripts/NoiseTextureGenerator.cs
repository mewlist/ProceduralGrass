using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Remoting.Messaging;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class NoiseTextureGenerator : MonoBehaviour
{
    public enum GeneratorType
    {
        WhiteNoise,
        HaltonNoise,
        BlueNoise
    }

    [SerializeField] private Material material;
    [SerializeField] private bool update;
    [SerializeField] [Range(0, 16384)] private int sampleCount = 100;
    [SerializeField] private GeneratorType generatorType;

    private const int TextureSize = 128;

    private Texture2D texture;

    private void OnValidate()
    {
        UpdateTexture();
    }

    private void Start()
    {
        CreateTexture();
    }

    private void Update()
    {
        if (update)
        {
            UpdateTexture();
            update = false;
        }
    }

    private void CreateTexture()
    {
        texture = new Texture2D(TextureSize, TextureSize, TextureFormat.RGBA32, false);
    }

    private void UpdateTexture()
    {
        if (texture == null) CreateTexture();

        // Clear
        var pixels = texture.GetPixels32();
        for (var i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.black;
        }

        texture.SetPixels32(pixels);

        // Sample
        material.mainTexture = texture;

        INoiseGenerator noiseGenerator;
        switch (generatorType)
        {
            case GeneratorType.WhiteNoise:
                noiseGenerator = new WhiteNoiseGenerator(TextureSize);
                break;
            case GeneratorType.HaltonNoise:
                noiseGenerator = new HaltonNoiseGenerator(TextureSize);
                break;
            case GeneratorType.BlueNoise:
                noiseGenerator = new BlueNoiseGenerator(TextureSize);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        for (var i = 0; i < sampleCount; i++)
        {
            var point = noiseGenerator.Next();
            texture.SetPixel((int)point.x, (int)point.y, Color.white);
        }

        texture.Apply();
    }
}