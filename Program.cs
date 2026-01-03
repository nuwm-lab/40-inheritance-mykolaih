using System;
using System.Globalization;

namespace Task08_RectangleParallelepiped
{
    public struct Point2D
    {
        public double X { get; }
        public double Y { get; }

        public Point2D(double x, double y) { X = x; Y = y; }
    }

    public struct Point3D
    {
        public double X { get; }
        public double Y { get; }
        public double Z { get; }

        public Point3D(double x, double y, double z) { X = x; Y = y; Z = z; }
    }

    /// <summary>
    /// Rectangle: MinX1 <= x1 <= MaxX1, MinX2 <= x2 <= MaxX2
    /// </summary>
    public class Rectangle
    {
        private double _minX1, _maxX1, _minX2, _maxX2;

        // protected properties allow derived classes to read bounds but not modify directly
        protected double MinX1 => _minX1;
        protected double MaxX1 => _maxX1;
        protected double MinX2 => _minX2;
        protected double MaxX2 => _maxX2;

        public Rectangle() { }

        public Rectangle(double b1, double a1, double b2, double a2)
        {
            SetCoefficients(b1, a1, b2, a2);
        }

        /// <summary>Validate numeric value (finite)</summary>
        protected static void ValidateNumber(double v, string name)
        {
            if (double.IsNaN(v) || double.IsInfinity(v))
                throw new ArgumentException($"{name} must be a finite number.", name);
        }

        /// <summary>Set rectangle coefficients (ensures b <= a)</summary>
        public virtual void SetCoefficients(double b1, double a1, double b2, double a2)
        {
            ValidateNumber(b1, nameof(b1));
            ValidateNumber(a1, nameof(a1));
            ValidateNumber(b2, nameof(b2));
            ValidateNumber(a2, nameof(a2));

            if (b1 <= a1) { _b1 = b1; _a1 = a1; }
            else { _b1 = a1; _a1 = b1; }

            if (b2 <= a2) { _b2 = b2; _a2 = a2; }
            else { _b2 = a2; _a2 = b2; }
        }

        /// <summary>Print rectangle bounds</summary>
        public virtual void PrintCoefficients()
        {
            Console.WriteLine("Rectangle bounds:");
            Console.WriteLine($"  b1 <= x1 <= a1 : {B1} <= x1 <= {A1}");
            Console.WriteLine($"  b2 <= x2 <= a2 : {B2} <= x2 <= {A2}");
        }

        /// <summary>
        /// Protected helper for containment checks using coordinate array.
        /// Keep as protected to avoid loose, untyped public API.
        /// Expects at least two coordinates [x1, x2].
        /// </summary>
        protected virtual bool ContainsCoords(params double[] coords)
        {
            if (coords == null || coords.Length < 2) return false;
            var x1 = coords[0];
            var x2 = coords[1];
            return x1 >= B1 && x1 <= A1 && x2 >= B2 && x2 <= A2;
        }

        /// <summary>Public, type-safe containment check for 2D points</summary>
        public virtual bool Contains(Point2D p) => ContainsCoords(p.X, p.Y);
    }

    /// <summary>
    /// Parallelepiped: extends Rectangle with b3 <= x3 <= a3.
    /// </summary>
    public class Parallelepiped : Rectangle
    {
        private double _b3, _a3;
        protected double B3 => _b3;
        protected double A3 => _a3;

        public Parallelepiped() { }

        // Call base to initialize first two dimensions only, then set third dimension directly.
        public Parallelepiped(double b1, double a1, double b2, double a2, double b3, double a3)
            : base(b1, a1, b2, a2)
        {
            if (double.IsNaN(b3) || double.IsInfinity(b3))
                throw new ArgumentException($"{nameof(b3)} must be a finite number.", nameof(b3));
            if (double.IsNaN(a3) || double.IsInfinity(a3))
                throw new ArgumentException($"{nameof(a3)} must be a finite number.", nameof(a3));

            if (b3 <= a3) { _b3 = b3; _a3 = a3; }
            else { _b3 = a3; _a3 = b3; }
        }

        /// <summary>
        /// Overload to set all 3D coefficients; reuses base logic for first two.
        /// </summary>
        public void SetCoefficients(double b1, double a1, double b2, double a2, double b3, double a3)
        {
            // reuse base validation and normalization for first two dimensions
            base.SetCoefficients(b1, a1, b2, a2);

            if (double.IsNaN(b3) || double.IsInfinity(b3))
                throw new ArgumentException($"{nameof(b3)} must be a finite number.", nameof(b3));
            if (double.IsNaN(a3) || double.IsInfinity(a3))
                throw new ArgumentException($"{nameof(a3)} must be a finite number.", nameof(a3));

            if (b3 <= a3) { _b3 = b3; _a3 = a3; }
            else { _b3 = a3; _a3 = b3; }
        }

        public override void PrintCoefficients()
        {
            base.PrintCoefficients();
            Console.WriteLine($"  b3 <= x3 <= a3 : {B3} <= x3 <= {A3}");
        }

        /// <summary>
        /// Polymorphic containment override.
        /// If only 2 coords provided, check base projection; with 3 coords, check full 3D containment.
        /// </summary>
        protected override bool ContainsCoords(params double[] coords)
        {
            if (coords == null || coords.Length < 2) return false;
            if (coords.Length == 2) return base.ContainsCoords(coords);
            var x1 = coords[0];
            var x2 = coords[1];
            var x3 = coords[2];
            return base.ContainsCoords(x1, x2) && x3 >= B3 && x3 <= A3;
        }

        /// <summary>Public, type-safe containment for 3D points</summary>
        public bool Contains(Point3D p) => ContainsCoords(p.X, p.Y, p.Z);

        /// <summary>Projective 2D containment uses base behavior</summary>
        public override bool Contains(Point2D p) => ContainsCoords(p.X, p.Y);
    }

    public static class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Rectangle and Parallelepiped demo\n");

            // Rectangle input
            Console.WriteLine("Enter rectangle bounds (b1 a1 b2 a2) separated by spaces:");
            var rectVals = ReadDoubles(4);
            var rect = new Rectangle();
            rect.SetCoefficients(rectVals[0], rectVals[1], rectVals[2], rectVals[3]);
            rect.PrintCoefficients();

            Console.WriteLine("\nEnter a 2D point (x1 x2) to check for the rectangle:");
            var p2 = ReadDoubles(2);
            var point2 = new Point2D(p2[0], p2[1]);
            Console.WriteLine(rect.Contains(point2)
                ? "Point belongs to the rectangle."
                : "Point does NOT belong to the rectangle");

            // Parallelepiped input
            Console.WriteLine("\nEnter parallelepiped bounds (b1 a1 b2 a2 b3 a3) separated by spaces:");
            var parVals = ReadDoubles(6);
            var par = new Parallelepiped(parVals[0], parVals[1], parVals[2], parVals[3], parVals[4], parVals[5]);
            par.PrintCoefficients();

            Console.WriteLine("\nEnter a 3D point (x1 x2 x3) to check for the parallelepiped:");
            var p3 = ReadDoubles(3);
            var point3 = new Point3D(p3[0], p3[1], p3[2]);
            Console.WriteLine(par.Contains(point3)
                ? "Point belongs to the parallelepiped."
                : "Point does NOT belong to the parallelepiped");
        }

        // helper to read exactly n doubles from one line or multiple lines; uses InvariantCulture
        private static double[] ReadDoubles(int count)
        {
            var list = new double[count];
            int read = 0;
            while (read < count)
            {
                string line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                {
                    Console.WriteLine("Input empty. Please enter numbers:");
                    continue;
                }

                var parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var p in parts)
                {
                    if (read >= count) break;
                    if (double.TryParse(p, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out double v))
                    {
                        list[read++] = v;
                    }
                    else if (double.TryParse(p, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.CurrentCulture, out v))
                    {
                        list[read++] = v;
                    }
                    else
                    {
                        Console.WriteLine($"Could not parse '{p}'. Enter a valid number.");
                    }
                }

                if (read < count)
                {
                    Console.WriteLine($"Need {count - read} more number(s)...");
                }
            }
            return list;
        }
    }
}
