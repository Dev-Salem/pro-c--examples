//LINQ to Objects

using System.Collections;
using Ch11;

namespace Ch13;

public class LinqExamples()
{
    public static void Example1()
    {
        string[] books =
        [
            "1 hello",
            "can you hear",
            "3 me",
            "2 I have",
            "been",
            "5 missing",
            "1 you",
        ];
        IEnumerable<string> queryExpression =
            from book in books
            where book.Contains('1')
            orderby book
            select book;

        var queryExtension = books.Where(x => x.Contains('9')).OrderBy((x) => x[1]);
        foreach (var item in queryExtension.DefaultIfEmpty("default"))
        {
            Console.WriteLine(item);
        }
    }

    public static void Example2()
    {
        List<(string name, int age)> people =
        [
            (name: "John", age: 23),
            (name: "Cory", age: 4),
            (name: "Becky", age: 88),
            (name: "Trevor", age: 39),
        ];
        List<(string name, int age)> people2 =
        [
            (name: "Cory", age: 21),
            (name: "Ed", age: 4),
            (name: "Eddy", age: 33),
            (name: "Edward", age: 21),
        ];
        var distinctBy = people.Concat(people2).DistinctBy(x => x.age);
        var aggregation = (from p in people select p).Max(x => x.age);
        double averageAge = (from p in people select p).Average(x => x.age);
        averageAge.Print();
    }
}

public record Car
{
    public static List<Car> myCars =
    [
        new()
        {
            PetName = "Henry",
            Color = "Silver",
            Speed = 100,
            Make = "BMW",
        },
        new()
        {
            PetName = "Daisy",
            Color = "Tan",
            Speed = 90,
            Make = "BMW",
        },
        new()
        {
            PetName = "Mary",
            Color = "Black",
            Speed = 55,
            Make = "VW",
        },
        new()
        {
            PetName = "Clunker",
            Color = "Rust",
            Speed = 5,
            Make = "Yugo",
        },
        new()
        {
            PetName = "Melvin",
            Color = "White",
            Speed = 43,
            Make = "Ford",
        },
    ];
    public string PetName { get; set; } = "";
    public string Color { get; set; } = "";
    public int Speed { get; set; }
    public string Make { get; set; } = "";

    public static void Example2()
    {
        var q0 = from c in myCars select c;
        var query1 = from c in myCars where c.Speed > 80 select c.PetName;
        var query2 = (from c in myCars select c).Take(2);
        var q3 = (from c in myCars orderby c.Speed descending select c).TakeWhile(x =>
            x.Speed > 60
        );
        var q4 = (from c in myCars select c).Take(..3);
        var q5 = (from c in myCars select c).Chunk(2);
        var q6 = from c in myCars select (c.PetName, c.Make);
        var q7 = from c in myCars select new { name = c.PetName, speed = c.Speed };
        var q8 = (from c in myCars where c.Speed > 50 select c.PetName).Concat(
            from c in myCars
            where c.Speed >= 80
            select c.PetName
        );
        var q9 = q0.UnionBy((from c in myCars where c.Speed > 50 select c), x => x.Speed > 80);
        // q8.Print();
        // query1.Print();
        // query2.Print();
        // q3.Print();
        // q4.Print();
        // q5.Print();
        // q5.Last().Print();
        // q6.Print();
        // q7.ToList().Print();
        bool result = q3.TryGetNonEnumeratedCount(out int count);
        if (result)
        {
            count.Print();
        }
        q9.Print();
    }

    public static void Example()
    {
        var query = from car in myCars where car.Speed > 90 && car.Make == "BMW" select car;
        ArrayList myCarsNonGe =
        [
            new Car
            {
                PetName = "Henry",
                Color = "Silver",
                Speed = 100,
                Make = "BMW",
            },
            new Car
            {
                PetName = "Daisy",
                Color = "Tan",
                Speed = 90,
                Make = "BMW",
            },
            new Car
            {
                PetName = "Mary",
                Color = "Black",
                Speed = 55,
                Make = "VW",
            },
            new Car
            {
                PetName = "Clunker",
                Color = "Rust",
                Speed = 5,
                Make = "Yugo",
            },
            new Car
            {
                PetName = "Melvin",
                Color = "White",
                Speed = 43,
                Make = "Ford",
            },
        ];

        var myCarToGen = myCarsNonGe.OfType<Car>();
        var trySome = from c in myCarToGen select c;
    }
}
