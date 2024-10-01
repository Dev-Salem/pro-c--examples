//Overloading - implicit & explicit conversion - Extension Methods
using System.Collections;

namespace Ch11;

public class PointClass(int x, int y) : IComparer<PointClass>
{
    public int X { get; set; } = x;
    public int Y { get; set; } = y;

    public static PointClass operator +(PointClass a, PointClass b) => new(a.X + b.X, a.Y + b.Y);

    public static PointClass operator ++(PointClass a) => new(a.X + 1, a.Y + 1);

    public static PointClass operator +(PointClass a, int b) => new(x: a.X + b, y: a.Y + b);

    public override bool Equals(object? o) => o!.ToString() == ToString();

    public override int GetHashCode() => ToString().GetHashCode();

    public override string ToString() => $"Point({X},{Y})";

    public int Compare(PointClass? x, PointClass? y)
    {
        if (x is not null && y is not null)
        {
            if (x.X > y.X && x.Y > y.Y)
            {
                return 1;
            }
            else if (x.X < y.X && x.Y < y.Y)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
        throw new ArgumentNullException();
    }

    public static bool operator ==(PointClass a, PointClass b) => a.Equals(b);

    public static bool operator !=(PointClass a, PointClass b) => !a.Equals(b);

    public static implicit operator PointClass(Vector v) => new(x: v.X, y: v.Y);

    public static explicit operator PointClass(int coordinate) => new(x: coordinate, y: coordinate);

    public static void Example()
    {
        var p1 = new PointClass(x: 1, y: 3);
        var p2 = new PointClass(x: -3, y: 5);
        var p3 = new PointClass(x: 1, y: 3);
        Console.WriteLine(p1 + p2);
        Console.WriteLine(p2 + 3);
        Console.WriteLine(p1 != p2);
        Console.WriteLine(p1 == p3);
        Console.WriteLine(++p1);
        var myAnonType = new
        {
            FirstPoint = p1,
            SecondPoint = p2,
            Sum = p1 + p2,
        };
        myAnonType.Print();
    }
}

//====================================

public class Vector(int x, int y)
{
    public int X { set; get; } = x;
    public int Y { set; get; } = y;

    public int GetProduct() => X * Y;

    public static explicit operator Vector(PointClass p) => new(p.X, p.Y);

    public override string ToString() => $"Vector({X}, {Y})";

    public static void Example()
    {
        Vector vector = new(x: 1, y: 0);
        PointClass point = new(x: 2, y: 3);

        //explicit operator
        PointClass point2 = (PointClass)1;
        //implicit conversion
        PointClass convertedVector = vector;
        //explicit conversion
        Vector convertedPoint = (Vector)point;
        List<PointClass> points = [point, point2, convertedVector];

        vector.Print();
        convertedVector.Print();
        point.Print();
        convertedPoint.Print();
        point2.Print();
        123.ReverseNumber().Print();
        point.ReversePoint();
        ((PointClass)vector).ReversePoint().Print();
        points.Print();
    }

    public static void Example2()
    {
        unsafe
        {
            //working with pointers, huh?
            int myInt;
            int* intPointer = &myInt;
        }
    }
}

static class MyExtensions
{
    public static int ReverseNumber(this int number)
    {
        var arrayNumber = number.ToString().ToCharArray();
        Array.Reverse(arrayNumber);
        string stringNumber = new(arrayNumber);
        return int.Parse(stringNumber);
    }

    public static PointClass ReversePoint(this PointClass point)
    {
        return new PointClass(x: point.Y, y: point.X);
    }

    private static void PrintCollection(this IEnumerable<dynamic> collection) =>
        Console.WriteLine(string.Join(", ", collection));

    public static void Print(this object o)
    {
        if (o is IEnumerable<dynamic> collection)
        {
            collection.PrintCollection();
        }
        else
        {
            Console.WriteLine(o);
        }
    }

    public static void Separate(this int o)
    {
        string s = "*";
        for (int i = 0; i < o; i++)
        {
            s += " * ";
        }
        s.Print();
    }
    // public static IEnumerator GetEnumerator(this PointClass point) => point.GetEnumerator();
}
