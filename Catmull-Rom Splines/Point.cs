using System.Globalization;

namespace Catmull_Rom_Splines
{
    public struct Point
    {
        public float X;
        public float Y;

        public Point(float X, float Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public new string ToString()
        {
            return $"({X.ToString(CultureInfo.InvariantCulture)}, {Y.ToString(CultureInfo.InvariantCulture)})";
        }
    }
}