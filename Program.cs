public Point3D(double x, double y, double z) { X = x; Y = y; Z = z; }
}

    /// <summary>
    /// Rectangle: b1 <= x1 <= a1, b2 <= x2 <= a2
    /// </summary>
public class Rectangle
{
private double _b1, _a1, _b2, _a2;
@@ -38,14 +40,14 @@ public Rectangle(double b1, double a1, double b2, double a2)
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
@@ -60,50 +62,67 @@ public virtual void SetCoefficients(double b1, double a1, double b2, double a2)
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
@@ -115,22 +134,25 @@ public override void PrintCoefficients()
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
@@ -156,8 +178,7 @@ public static void Main(string[] args)
// Parallelepiped input
Console.WriteLine("\nEnter parallelepiped bounds (b1 a1 b2 a2 b3 a3) separated by spaces:");
var parVals = ReadDoubles(6);
          
            var par = new Parallelepiped(parVals[0], parVals[1], parVals[2], parVals[3], parVals[4], parVals[5]);
par.PrintCoefficients();

Console.WriteLine("\nEnter a 3D point (x1 x2 x3) to check for the parallelepiped:");
@@ -208,4 +229,4 @@ private static double[] ReadDoubles(int count)
return list;
}
}
}
}
