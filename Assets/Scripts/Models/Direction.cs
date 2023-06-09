using PenguinPeak.Enums.Directions;
using System;

namespace PenguinPeak.Models
{
    public class Direction
    {
        public Vertical Vertical { get; set; } = Vertical.None;

        public Horizontal Horizontal { get; set; } = Horizontal.None;

        public Relative Relative { get; set; } = Relative.None;

        public static Direction Left => new() { Horizontal = Horizontal.Left };

        public static Direction Right => new() { Horizontal = Horizontal.Right };

        public static Direction Up => new() { Vertical = Vertical.Up };

        public static Direction Down => new() { Vertical = Vertical.Down };

        public static Direction Forward => new() { Relative = Relative.Forward };

        public static Direction Backward => new() { Relative = Relative.Backward };

        public Direction Combine(Direction other)
        {
            var combinedDirection = new Direction();

            if (this.Vertical == Vertical.None)
            {
                combinedDirection.Vertical = other.Vertical;
            }
            else if (other.Vertical == Vertical.None)
            {
                combinedDirection.Vertical = this.Vertical;
            }
            else if (this.Vertical == other.Vertical)
            {
                combinedDirection.Vertical = other.Vertical;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(Vertical), "Unable to combine opposing vertical directions");
            }

            if (this.Horizontal == Horizontal.None)
            {
                combinedDirection.Horizontal = other.Horizontal;
            }
            else if (other.Horizontal == Horizontal.None)
            {
                combinedDirection.Horizontal = this.Horizontal;
            }
            else if (this.Horizontal == other.Horizontal)
            {
                combinedDirection.Horizontal = other.Horizontal;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(Horizontal), "Unable to combine opposing horizontal directions");
            }

            if (this.Relative == Relative.None)
            {
                combinedDirection.Relative = other.Relative;
            }
            else if (other.Relative == Relative.None)
            {
                combinedDirection.Relative = this.Relative;
            }
            else if (this.Relative == other.Relative)
            {
                combinedDirection.Relative = other.Relative;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(Relative), "Unable to combine opposing relative directions");
            }

            return combinedDirection;
        }
    }
}
