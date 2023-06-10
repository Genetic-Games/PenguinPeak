using PenguinPeak.Extensions;
using PenguinPeak.Generators;
using UnityEngine;

namespace PenguinPeak.Controllers
{
    public class MountainController : MonoBehaviour
    {
        public int MapSize;
        public double MapRoughnessDelta;

        public Terrain MountainTerrain;

        private float[,] HeightMap;

        private readonly HeightMapGenerator _heightMapGenerator = new();

        // Initialization
        public void Start()
        {
            Debug.Assert(MountainTerrain != default, $"{nameof(MountainTerrain)} is not properly set.");
            Debug.Assert(MapSize != default, $"{nameof(MapSize)} is not properly set.");
            Debug.Assert(MapRoughnessDelta != default, $"{nameof(MapRoughnessDelta)} is not properly set.");

            GenerateAndApplyHeightMapToTerrain();
        }

        // Game Loop - Executed Once Per Frame
        public void Update()
        {

        }

        public void GenerateAndApplyHeightMapToTerrain()
        {
            GenerateHeightMap();
            ApplyHeightMapToTerrain(HeightMap, MountainTerrain);
        }

        public void GenerateHeightMap()
        {
            var heightMap = _heightMapGenerator.GenerateHeightMap(mapSize: MapSize, roughnessDelta: MapRoughnessDelta);
            HeightMap = heightMap.Transpose(); // TODO - FIGURE OUT IF WE REALLY NEED / WANT TO TRANSPOSE HERE BASED ON THIS - https://docs.unity3d.com/ScriptReference/TerrainData.SetHeights.html
        }

        public void ApplyHeightMapToTerrain(float[,] heightMap, Terrain terrain)
        {
            terrain.terrainData.SetHeights(xBase: 0, yBase: 0, heights: heightMap);
        }
    }
}
