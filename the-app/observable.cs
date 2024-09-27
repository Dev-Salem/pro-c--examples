using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;

namespace ObservableExample
{
    public class GenericPoint<T>
        where T : new()
    {
        public T? X { get; set; }
        public T? Y { get; set; }

        public GenericPoint()
        {
            X = default;
            Y = default;
        }

        public GenericPoint(T x, T y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }

    public interface IVector<T>
    {
        public abstract double GetVectorResult(T x, T y);
    }

    class Person
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public int Age { get; set; }

        public static ObservableCollection<Person> people =
        [
            new Person
            {
                FirstName = "Peter",
                LastName = "Murphy",
                Age = 52,
            },
            new Person
            {
                FirstName = "Kevin",
                LastName = "Key",
                Age = 48,
            },
        ];

        public override string ToString()
        {
            return $"({FirstName}, {LastName})";
        }

        public Person this[int index]
        {
            get => people[index];
            set => people.Insert(index, value);
        }

        public Person(string fname, string lname, int age)
        {
            FirstName = fname;
            LastName = lname;
            Age = age;
        }

        public int Count => people.Count;

        public Person this[string key]
        {
            get => people.Single(p => p.FirstName == key);
            set => people.Add(new Person(fname: key));
        }

        public Person(string fname) => FirstName = fname;

        public Person() { }

        public static void Exe()
        {
            people.CollectionChanged += people_CollectionChanged;

            static void people_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
            {
                Console.WriteLine("Action is {0}", e.Action);
                Console.WriteLine("Old value is {0}", string.Join(", ", e.OldItems));
                Console.WriteLine("new value is {0}", string.Join(", ", e.NewItems));
            }
        }
    }
}
