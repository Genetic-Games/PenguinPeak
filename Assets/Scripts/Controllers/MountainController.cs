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

        private readonly HeightMapGenerator _heightMapGenerator = new();

        // Initialization
        void Start()
        {
            Debug.Assert(MountainTerrain != null, "The mountain terrain object is not properly set.");

            var heightMap = _heightMapGenerator.GenerateHeightMap(mapSize: MapSize, roughnessDelta: MapRoughnessDelta);
            Debug.Log(heightMap.TwoDimensionalArrayToString());

            MountainTerrain.terrainData.SetHeights(xBase: 0, yBase: 0, heights: heightMap);
        }

        // Game Loop - Executed Once Per Frame
        void Update()
        {

        }
    }
}
