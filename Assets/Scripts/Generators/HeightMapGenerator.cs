using PenguinPeak.Enums.Directions;
using PenguinPeak.Models;
using System;
using System.Collections.Generic;

namespace PenguinPeak.Generators
{
    public class HeightMapGenerator
    {
        private readonly Random _random = new();

        private static readonly Direction _left = new() { Horizontal = Horizontal.Left };
        private static readonly Direction _right = new() { Horizontal = Horizontal.Right };
        private static readonly Direction _up = new() { Vertical = Vertical.Up };
        private static readonly Direction _down = new() { Vertical = Vertical.Down };

        private static readonly Direction _upAndLeft = new() { Vertical = Vertical.Up, Horizontal = Horizontal.Left };
        private static readonly Direction _upAndRight = new() { Vertical = Vertical.Up, Horizontal = Horizontal.Right };
        private static readonly Direction _downAndLeft = new() { Vertical = Vertical.Down, Horizontal = Horizontal.Left };
        private static readonly Direction _downAndRight = new() { Vertical = Vertical.Down, Horizontal = Horizontal.Right };

        private readonly List<Direction> _diagonalDirections = new() { _upAndLeft, _upAndRight, _downAndLeft, _downAndRight };
        private readonly List<Direction> _horizontalDirections = new() { _left, _right };
        private readonly List<Direction> _verticalDirections = new() { _up, _down };

        /// <summary>
        /// Generate a 2D height map given a specific map size and roughness delta utilizing the Diamond-Square algorithm.
        /// Details - https://en.wikipedia.org/wiki/Diamond-square_algorithm 
        /// </summary>
        /// <param name="mapSize">The size of one dimension of the height map to be generated (not the height dimension)</param>
        /// <param name="roughnessDelta">The </param>
        /// <returns>Returns a mapSize by mapSize 2D array of height values</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public float[,] GenerateHeightMap(int mapSize, double roughnessDelta)
        {
            if (mapSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(mapSize), mapSize, "Value must be a positive integer.");
            }

            if (Math.Log(mapSize - 1, newBase: 2) % 1 != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(mapSize), mapSize, "Value must be equal to 2^(k) + 1 for any positive integer value of k.");
            }

            if (roughnessDelta <= 0.0d || roughnessDelta >= 1.0d)
            {
                throw new ArgumentOutOfRangeException(nameof(roughnessDelta), roughnessDelta, "Value must be between 0.0 and 1.0 (exclusive)");
            }

            var map = new float[mapSize, mapSize];

            var distanceToSimilarCells = mapSize - 1;
            var roughness = 1.0d;

            while (distanceToSimilarCells > 1)
            {
                var distanceToNextCell = (int)Math.Floor(distanceToSimilarCells / 2.0d);

                map = DiamondStep(map, mapSize, distanceToSimilarCells, distanceToNextCell, roughness);
                map = SquareHorizontalStep(map, mapSize, distanceToSimilarCells, distanceToNextCell, roughness);
                map = SquareVerticalStep(map, mapSize, distanceToSimilarCells, distanceToNextCell, roughness);

                distanceToSimilarCells = (int)Math.Floor(distanceToSimilarCells / 2.0d);
                roughness *= roughnessDelta;
            }

            return map;
        }

        private float[,] DiamondStep(float[,] map, int mapSize, int distanceToSimilarCells, int distanceToNextCell, double roughness)
        {
            for (var x = distanceToNextCell; x < mapSize; x += distanceToSimilarCells)
            {
                for (var y = distanceToNextCell; y < mapSize; y += distanceToSimilarCells)
                {
                    // Check in all related directions and take the average height of all values in the map
                    var averageHeight = GetAverageHeight(map, mapSize, distanceToNextCell, x, y, _diagonalDirections);

                    // Add some randomized "roughness" to the height to be less formulaic
                    var heightRoughness = GetHeightRoughness(roughness);

                    // Finally, set the height value in the height map
                    map[x, y] = averageHeight + heightRoughness;
                }
            }

            return map;
        }

        private float[,] SquareHorizontalStep(float[,] map, int mapSize, int distanceToSimilarCells, int distanceToNextCell, double roughness)
        {
            for (var x = distanceToNextCell; x < mapSize; x += distanceToSimilarCells)
            {
                for (var y = 0; y < mapSize; y += distanceToSimilarCells)
                {
                    // Check in all related directions and take the average height of all values in the map
                    var averageHeight = GetAverageHeight(map, mapSize, distanceToNextCell, x, y, _horizontalDirections);

                    // Add some randomized "roughness" to the height to be less formulaic
                    var heightRoughness = GetHeightRoughness(roughness);

                    // Finally, set the height value in the height map
                    map[x, y] = averageHeight + heightRoughness;
                }
            }

            return map;
        }

        private float[,] SquareVerticalStep(float[,] map, int mapSize, int distanceToSimilarCells, int distanceToNextCell, double roughness)
        {
            for (var x = 0; x < mapSize; x += distanceToSimilarCells)
            {
                for (var y = distanceToNextCell; y < mapSize; y += distanceToSimilarCells)
                {
                    // Check in all related directions and take the average height of all values in the map
                    var averageHeight = GetAverageHeight(map, mapSize, distanceToNextCell, x, y, _verticalDirections);

                    // Add some randomized "roughness" to the height to be less formulaic
                    var heightRoughness = GetHeightRoughness(roughness);

                    // Finally, set the height value in the height map
                    map[x, y] = averageHeight + heightRoughness;
                }
            }

            return map;
        }

        private float GetAverageHeight(float[,] map, int mapSize, int distanceToNextCell, int x, int y, List<Direction> mapDirections)
        {
            var sumOfHeights = 0.0f;
            var numberOfHeights = 0;

            foreach (var direction in mapDirections)
            {
                var xReferenceIndex = x + ((int)direction.Horizontal * distanceToNextCell);
                var yReferenceIndex = y + ((int)direction.Vertical * distanceToNextCell);

                // Make sure the point we have computed is within the bounds of the map
                if (0 <= xReferenceIndex && xReferenceIndex < mapSize && 0 <= yReferenceIndex && yReferenceIndex < mapSize)
                {
                    sumOfHeights += map[xReferenceIndex, yReferenceIndex];
                    numberOfHeights++;
                }
            }

            return sumOfHeights / numberOfHeights;
        }

        /// <summary>
        /// The Random class does not allow you to generate a random double in a specified range
        /// In order to do that manually, this function:
        ///   1. Generates a double in the range [0, 1)
        ///   2. Widens the range to [0, 2 * roughness) with multiplacation
        ///   3. Shifts the range to [-roughness, +roughness) with subtraction
        ///
        /// Details - https://learn.microsoft.com/en-us/dotnet/api/system.random?view=net-7.0#retrieve-integers-in-a-specified-range
        /// </summary>
        /// <returns>Height roughness</returns>
        private float GetHeightRoughness(double roughness)
        {
            var randomValue = _random.NextDouble();
            var heightRoughness = (randomValue * 2.0f * roughness) - roughness;
            
            // Single (precision) is synonymous with float and has half of the precision of a double
            return Convert.ToSingle(heightRoughness);
        }
    }
}
