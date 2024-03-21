using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Terrain))]
public class TerrainManipulator : MonoBehaviour
{

    [Header("Noise settings")]
    public float scale = 100;
    public Vector3 size = new(100, 30, 100);

    [Range(1, 8)]
    public int octaves = 3;


    const int SAND = 0;
    const int GRASS = 1;
    const int ROCK = 2;
    const int ICE = 3;

    Terrain terrain;
    TerrainData terrainData;
    float[,] heightMap;
    float[,,] textureMap;

    void Start()
    {
        terrain = GetComponent<Terrain>();
        terrainData = terrain.terrainData;
        Generate();
        Color();
    }

   
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) 
        {
            Generate();
            Color();
        }
    }


    void Generate()
    {
        terrainData.size = size;
        var resolution = terrainData.heightmapResolution;
        heightMap = new float[resolution, resolution];
        
        var seed = Random.Range(-10000, 10000);
        print($"Seed: {seed} ");

        for (int z = 0; z < resolution; z++)
        {
            for (int x = 0; x < resolution; x++)
            {
                for (int o = 0; o < octaves; o++)
                {
                    var px = (x + seed) / scale * Mathf.Pow(2, o);
                    var pz = (z + seed) / scale * Mathf.Pow(2, o);
                    var value = (noise.snoise(new float2(px, pz)) + 1) / 2 / Mathf.Pow(2, o);

                    var sign = 0 % 2 == 0 ? 1 : -1;

                    heightMap[z, x] += Mathf.Pow(value, 2) * sign;
                }
                
            }
        }

        terrainData.SetHeights(0, 0, heightMap);
        terrain.Flush();
    }

    void Color()
    {
        textureMap = terrainData.GetAlphamaps(0, 0, terrainData.alphamapWidth, terrainData.alphamapHeight);

        for (int y = 0; y < terrainData.alphamapHeight; y++)
        {
            for (int x = 0; x < terrainData.alphamapWidth; x++)
            {
                textureMap[x, y, SAND] = 0;
                textureMap[x, y, GRASS] = 0;
                textureMap[x, y, ROCK] = 0;
                textureMap[x, y, ICE] = 0;

                if (heightMap[x, y] < 0.2f)
                {
                    textureMap[x, y, SAND] = 1;
                }
                else if(heightMap[x, y] < 0.4f)
                {
                    textureMap[x, y, GRASS] = 1;
                }
                else if (heightMap[x, y] < 0.7f)
                {
                    textureMap[x, y, ROCK] = 1;
                }
                else
                {
                    textureMap[x, y, ICE] = 1;
                }

            }
        }

        terrainData.SetAlphamaps(0, 0, textureMap);
        terrain.Flush();
    }
}
