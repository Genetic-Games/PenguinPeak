using PenguinPeak.Enums.Directions;

namespace PenguinPeak.Models
{
    public class Direction
    {
        public Vertical Vertical { get; set; } = Vertical.None;

        public Horizontal Horizontal { get; set; } = Horizontal.None;

        public Relative Relative { get; set; } = Relative.None;
    }
}
