using PenguinPeak.Generators;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinPeak.Controllers
{
    public class MountainController : MonoBehaviour
    {
        public int MapSize;
        public float MapRoughnessDelta;

        public Terrain MountainTerrain;
        public Slider MapRoughnessSlider;

        private float[,] HeightMap;

        private readonly HeightMapGenerator _heightMapGenerator = new();

        // Initialization
        public void Start()
        {
            Debug.Assert(MountainTerrain != default, $"{nameof(MountainTerrain)} is not properly set.");
            Debug.Assert(MapRoughnessSlider != default, $"{nameof(MapRoughnessSlider)} is not properly set.");

            Debug.Assert(MapSize != default, $"{nameof(MapSize)} is not properly set.");
            Debug.Assert(MapRoughnessDelta != default, $"{nameof(MapRoughnessDelta)} is not properly set.");

            SetMapRoughnessOnSlider(MapRoughnessDelta);
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
            HeightMap = _heightMapGenerator.GenerateHeightMap(mapSize: MapSize, roughnessDelta: MapRoughnessDelta);
        }

        public void ApplyHeightMapToTerrain(float[,] heightMap, Terrain terrain)
        {
            terrain.terrainData.SetHeights(xBase: 0, yBase: 0, heights: heightMap);
        }

        public void SetMapRoughnessOnSlider(float value)
        {
            MapRoughnessSlider.value = value;
        }

        public void UpdateMapRoughnessFromSlider()
        {
            var sliderValue = MapRoughnessSlider.value;
            MapRoughnessDelta = sliderValue;
        }
    }
}
