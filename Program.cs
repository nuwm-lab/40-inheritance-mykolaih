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

    // Rectangle: b1 <= x1 <= a1, b2 <= x2 <= a2
    public class Rectangle
    {
        protected double b1, a1, b2, a2;

        public Rectangle() { }

        public Rectangle(double b1, double a1, double b2, double a2)
        {
            SetCoefficients(b1, a1, b2, a2);
        }

        // set coefficients (ensures b <= a by swapping if necessary)
        public virtual void SetCoefficients(double b1, double a1, double b2, double a2)
        {
            if (b1 <= a1) { this.b1 = b1; this.a1 = a1; }
            else { this.b1 = a1; this.a1 = b1; }

            if (b2 <= a2) { this.b2 = b2; this.a2 = a2; }
            else { this.b2 = a2; this.a2 = b2; }
        }

        public virtual void PrintCoefficients()
        {
            Console.WriteLine("Rectangle bounds");
            Console.WriteLine($"  b1 <= x1 <= a1 : {b1} <= x <= {a1}");
            Console.WriteLine($"  b2 <= x2 <= a2 : {b2} <= y <= {a2}");
        }

        // check if 2D point belongs to rectangle
        public virtual bool Contains(Point2D p)
        {
            return p.X >= b1 && p.X <= a1 && p.Y >= b2 && p.Y <= a2;
        }
    }

    // Parallelepiped: extends Rectangle with b3 <= x3 <= a3
    public class Parallelepiped : Rectangle
    {
        protected double b3, a3;

        public Parallelepiped() : base() { }

        public Parallelepiped(double b1, double a1, double b2, double a2, double b3, double a3)
            : base(b1, a1, b2, a2)
        {
            SetCoefficients(b1, a1, b2, a2, b3, a3);
        }

        // overloaded: set 3D coefficients
        public void SetCoefficients(double b1, double a1, double b2, double a2, double b3, double a3)
        {
            base.SetCoefficients(b1, a1, b2, a2);

            if (b3 <= a3) { this.b3 = b3; this.a3 = a3; }
            else { this.b3 = a3; this.a3 = b3; }
        }

        public override void PrintCoefficients()
        {
            base.PrintCoefficients();
            Console.WriteLine($"  b3 <= x3 <= a3 : {b3} <= z <= {a3}");
        }

        // overloaded: check 3D point membership
        public bool Contains(Point3D p)
        {
            return base.Contains(new Point2D(p.X, p.Y)) && p.Z >= b3 && p.Z <= a3;
        }
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

            Console.WriteLine("\nEnter a 2D point (x y) to check for the rectangle:");
            var p2 = ReadDoubles(2);
            var point2 = new Point2D(p2[0], p2[1]);
            Console.WriteLine(rect.Contains(point2)
                ? "Point belongs to the rectangle."
                : "Point does NOT belong to the rectangle");

            // Parallelepiped input
            Console.WriteLine("\nEnter parallelepiped bounds (b1 a1 b2 a2 b3 a3) separated by spaces:");
            var parVals = ReadDoubles(6);
            var par = new Parallelepiped();
            par.SetCoefficients(parVals[0], parVals[1], parVals[2], parVals[3], parVals[4], parVals[5]);
            par.PrintCoefficients();

            Console.WriteLine("\nEnter a 3D point (x y z) to check for the parallelepiped:");
            var p3 = ReadDoubles(3);
            var point3 = new Point3D(p3[0], p3[1], p3[2]);
            Console.WriteLine(par.Contains(point3)
                ? "Point belongs to the parallelepiped."
                : "Point does NOT belong to the parallelepiped.");

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey(true);
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
