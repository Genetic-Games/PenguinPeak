using PenguinPeak.Generators;
using System;
using UnityEngine;

namespace PenguinPeak.Controllers
{
    public class GameController : MonoBehaviour
    {
        public int MapSize;
        public double MapRoughnessDelta;

        public Terrain MountainTerrain;

        private HeightMapGenerator _heightMapGenerator;

        // Initialization
        void Start()
        {
            Debug.Assert(MountainTerrain != null, "The mountain terrain object is not properly set.");

            _heightMapGenerator = new HeightMapGenerator();

            var heightMap = _heightMapGenerator.GenerateHeightMap(mapSize: MapSize, roughnessDelta: MapRoughnessDelta);
            Debug.Log(heightMap); // TODO - FIX ME TO DUMP REAL VALUES

            MountainTerrain.terrainData.SetHeights(xBase: 0, yBase: 0, heights: heightMap);
        }

        // Game Loop - Executed Once Per Frame
        void Update()
        {
        
        }
    }
}