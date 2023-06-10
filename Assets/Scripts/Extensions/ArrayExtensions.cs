using System.Text;

namespace PenguinPeak.Extensions
{
    public static class ArrayExtensions
    {
        public static string ConvertToString<T>(this T[,] matrix, string delimiter = "\t")
        {
            var stringBuilder = new StringBuilder();
            var rows = matrix.GetLength(dimension: 0);
            var columns = matrix.GetLength(dimension: 1);

            stringBuilder.AppendLine($"Dimensions = [{rows},{columns}]:");

            for (var x = 0; x < rows; x++)
            {
                stringBuilder.Append("[").Append(delimiter);

                for (var y = 0; y < columns; y++)
                {
                    stringBuilder
                        .Append(matrix.GetValue(x, y))
                        .Append(delimiter);
                }

                stringBuilder.AppendLine("]");
            }

            return stringBuilder.ToString();
        }

        public static T[,] Transpose<T>(this T[,] matrix)
        {
            var rows = matrix.GetLength(dimension: 0);
            var columns = matrix.GetLength(dimension: 1);

            var result = new T[columns, rows];

            for (var y = 0; y < columns; y++)
            {
                for (var x = 0; x < rows; x++)
                {
                    result[y, x] = matrix[x, y];
                }
            }
            return result;
        }
    }
}
