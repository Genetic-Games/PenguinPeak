using System.Text;

namespace PenguinPeak.Extensions
{
    public static class ArrayExtensions
    {
        public static string TwoDimensionalArrayToString<T>(this T[,] matrix, string delimiter = "\t")
        {
            var stringBuilder = new StringBuilder();
            var width = matrix.GetLength(dimension: 0);
            var length = matrix.GetLength(dimension: 1);

            stringBuilder.AppendLine($"Dimensions = [{width},{length}]:");

            for (var x = 0; x < width; x++)
            {
                stringBuilder.Append("[").Append(delimiter);

                for (var y = 0; y < length; y++)
                {
                    stringBuilder
                        .Append(matrix.GetValue(x, y))
                        .Append(delimiter);
                }

                stringBuilder.AppendLine("]");
            }

            return stringBuilder.ToString();
        }

    }
}
